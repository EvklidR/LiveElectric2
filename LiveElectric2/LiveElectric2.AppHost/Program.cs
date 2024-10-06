var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.LiveElectric2_Server>("liveelectric2-server");

builder.Build().Run();
