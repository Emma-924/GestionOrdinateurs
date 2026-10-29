var builder = WebApplication.CreateBuilder(args);

// CORS : autorise le client Blazor (port 5167) à demander un token
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("http://localhost:5167")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// HttpClientFactory : client nommé pour appeler l'API de gestion (recherche d'un utilisateur par login)
builder.Services.AddHttpClient("GestionApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5140/api/");
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Activer CORS
app.UseCors("AllowBlazorClient");

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
