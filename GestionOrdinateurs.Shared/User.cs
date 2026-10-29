using System.ComponentModel.DataAnnotations;

namespace GestionOrdinateurs.Shared;

// Représente un utilisateur de l'application (identifié par son login unique).
public class User
{
    public int Id { get; set; }

    [Required]
    public string Login { get; set; } = string.Empty;

    public RoleEnum Role { get; set; }
}
