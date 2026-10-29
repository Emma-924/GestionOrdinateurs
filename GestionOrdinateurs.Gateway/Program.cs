var builder = WebApplication.CreateBuilder(args);

// YARP : les routes et les destinations sont lues dans la section "ReverseProxy" de appsettings.json
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.MapReverseProxy();

app.Run();
