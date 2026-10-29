using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API;

// Contrôleur qui permet de consulter les utilisateurs de l'application.
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private ParcInformatiqueContext _context;

    // Crée le contrôleur avec le contexte de la base de données.
    public UsersController(ParcInformatiqueContext context)
    {
        _context = context;
    }

    // Retourne la liste de tous les utilisateurs.
    [HttpGet]
    public async Task<ActionResult<List<User>>> ObtenirTous()
    {
        return Ok(await _context.Users.ToListAsync());
    }

    // Retourne l'utilisateur correspondant au login, ou 404 s'il n'existe pas.
    [HttpGet("{login}")]
    public async Task<ActionResult<User>> ObtenirParLogin(string login)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        return user is null ? NotFound() : Ok(user);
    }
}
