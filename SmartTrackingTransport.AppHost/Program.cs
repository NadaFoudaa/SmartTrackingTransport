var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.SmartTrackingTransport>("smarttrackingtransport");

builder.Build().Run();
