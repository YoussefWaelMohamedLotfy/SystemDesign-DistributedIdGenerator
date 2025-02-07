using Google.Protobuf.WellKnownTypes;

using IdGen.Api;

using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddGrpcClient<IdGenerator.IdGeneratorClient>(o =>
{
    o.Address = new Uri("https://idgen-grpc");
});
builder.Services.AddOpenApi();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.MapDefaultEndpoints();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

app.MapGet("/snowflake", async (IdGenerator.IdGeneratorClient client, CancellationToken ct) =>
{
    var call = await client.GetSnowflakeIdAsync(new Empty(), cancellationToken: ct);
    return Results.Ok(call.Id);
})
.WithName("GetSnowflakeID");

await app.RunAsync();
