IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

var grpcApp = builder.AddProject<Projects.IdGen_Grpc>("idgen-grpc");

builder.AddProject<Projects.IdGen_Api>("idgen-api")
    .WithReference(grpcApp)
    .WaitFor(grpcApp);

await builder.Build().RunAsync();
