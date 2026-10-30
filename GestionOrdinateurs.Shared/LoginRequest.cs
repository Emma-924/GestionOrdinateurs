namespace GestionOrdinateurs.Shared;

// Identifiants saisis sur la page de connexion (login et mot de passe Keycloak).
public record LoginRequest(string Username, string Password);