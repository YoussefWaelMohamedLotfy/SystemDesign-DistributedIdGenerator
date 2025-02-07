using IdGen.DependencyInjection;
using IdGen.Grpc.Services;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
if (!int.TryParse(builder.Configuration["IdGenerator:GeneratorId"], out int genId))
{
    throw new Exception("Generator ID not valid");
}

builder.Services.AddIdGen(genId);
builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Distributed ID Generator", Version = "v1" });

    var filePath = Path.Combine(AppContext.BaseDirectory, "IdGen.Grpc.xml");
    c.IncludeXmlComments(filePath);
    c.IncludeGrpcXmlComments(filePath, includeControllerXmlComments: true);
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My gRPC API V1"));

// Configure the HTTP request pipeline.
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.MapGrpcService<IdGeneratorService>();

await app.RunAsync();
