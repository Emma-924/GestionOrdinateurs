using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using GestionOrdinateurs.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace AuthentificationApi.Controllers;

// Contrôleur qui génère les tokens JWT des utilisateurs.
[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private IConfiguration _configuration;
    private IHttpClientFactory _httpClientFactory;

    // Crée le contrôleur avec la configuration (clé JWT) et la fabrique de HttpClient (appel de l'API de gestion).
    public AuthenticationController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    // Récupère l'utilisateur via l'API de gestion, puis renvoie son token JWT valable 15 minutes.
    // Renvoie 404 si l'utilisateur n'existe pas, 503 si l'API de gestion est injoignable, 502 pour une autre erreur.
    [HttpPost("login")]
    public async Task<IActionResult> Login(string login)
    {
        var client = _httpClientFactory.CreateClient("GestionApi");

        HttpResponseMessage reponse;
        try
        {
            reponse = await client.GetAsync($"Users/{Uri.EscapeDataString(login)}");
        }
        catch (HttpRequestException)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable,
                "L'API de gestion est injoignable.");
        }

        if (reponse.StatusCode == HttpStatusCode.NotFound)
        {
            return NotFound("L'utilisateur n'existe pas.");
        }

        if (!reponse.IsSuccessStatusCode)
        {
            return StatusCode(StatusCodes.Status502BadGateway,
                "Erreur lors de la récupération de l'utilisateur.");
        }

        var user = await reponse.Content.ReadFromJsonAsync<User>();

        var jwtKey = _configuration["Jwt:Key"] ?? "";
        var key = Encoding.ASCII.GetBytes(jwtKey);

        return Ok(GenTokenJWT(key, user!.Role, user.Login));
    }

    // Construit le token JWT avec le login et le rôle dans les claims, puis le signe avec la clé.
    private string GenTokenJWT(byte[] key, RoleEnum role, string login)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, login),
            new Claim(ClaimTypes.Role, role.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    // Envoie le login et le mot de passe à Keycloak, puis renvoie le token d'accès obtenu.
    // Renvoie 401 si les identifiants sont refusés, 503 si Keycloak est injoignable.
    [HttpPost("keycloak-login")]
    public async Task<IActionResult> LoginKeycloak(LoginRequest request)
    {
        var client = _httpClientFactory.CreateClient();

        var parametres = new Dictionary<string, string>
    {
        { "grant_type", "password" },
        { "client_id", _configuration["Keycloak:ClientId"] ?? "" },
        { "client_secret", _configuration["Keycloak:ClientSecret"] ?? "" },
        { "username", request.Username },
        { "password", request.Password }
    };

        HttpResponseMessage reponse;
        try
        {
            reponse = await client.PostAsync(_configuration["Keycloak:TokenUrl"], new FormUrlEncodedContent(parametres));
        }
        catch (HttpRequestException)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Keycloak est injoignable.");
        }

        if (!reponse.IsSuccessStatusCode)
        {
            return Unauthorized("Login ou mot de passe incorrect.");
        }

        var json = await reponse.Content.ReadFromJsonAsync<JsonElement>();
        return Ok(json.GetProperty("access_token").GetString());
    }
}
