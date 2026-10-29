var builder = WebApplication.CreateBuilder(args);

// CORS : autorise le client Blazor (port 5167 en local, port 5000 sous Docker) à demander un token
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("http://localhost:5167", "http://localhost:5000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// HttpClientFactory : client nommé pour appeler l'API de gestion (recherche d'un utilisateur par login), via son nom de service Docker
builder.Services.AddHttpClient("GestionApi", client =>
{
    client.BaseAddress = new Uri("http://gestionordinateurs-api:5140/api/");
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
