using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API;

// Contrôleur qui gère les ordinateurs du parc informatique (accessible uniquement aux utilisateurs connectés).
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrdinateursController : ControllerBase
{
    private IOrdinateurService _ordinateurService;
    private ILogger<OrdinateursController> _logger;
    private IConfiguration _configuration;
    private JournalService _journal;

    // Crée le contrôleur et l'abonne aux événements du service.
    public OrdinateursController(IOrdinateurService ordinateurService, ILogger<OrdinateursController> logger,
        IConfiguration configuration, JournalService journal)
    {
        _ordinateurService = ordinateurService;
        _logger = logger;
        _configuration = configuration;
        _journal = journal;
        _ordinateurService.OrdinateurAjoute += OrdinateurEvenements.AfficherMessageAjout;
        _ordinateurService.OrdinateurMisAJour += OrdinateurEvenements.AfficherMessageMiseAJour;
        _ordinateurService.OrdinateurSupprime += OrdinateurEvenements.AfficherMessageSuppression;
    }

    // Retourne la liste de tous les ordinateurs.
    [HttpGet]
    public async Task<ActionResult<List<Ordinateur>>> ObtenirTous()
    {
        _logger.LogInformation("GET api/ordinateurs : liste demandée");
        return Ok(await _ordinateurService.ListerOrdinateurs());
    }

    // Retourne uniquement les ordinateurs disponibles (stock > 0).
    [HttpGet("disponibles")]
    public async Task<ActionResult<List<Ordinateur>>> ObtenirDisponibles()
    {
        _logger.LogInformation("GET api/ordinateurs/disponibles : ordinateurs disponibles demandés");
        return Ok(await _ordinateurService.ListerDisponibles());
    }

    // Retourne les ordinateurs du plus cher au moins cher.
    [HttpGet("prix-decroissant")]
    public async Task<ActionResult<List<Ordinateur>>> ObtenirParPrixDecroissant()
    {
        _logger.LogInformation("GET api/ordinateurs/prix-decroissant : tri par prix demandé");
        return Ok(await _ordinateurService.ListerParPrixDecroissant());
    }

    // Retourne un ordinateur par son id, ou 404 s'il n'existe pas.
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Ordinateur>> ObtenirParId(int id)
    {
        _logger.LogInformation("GET api/ordinateurs/{Id} : ordinateur demandé", id);
        var ordinateur = await _ordinateurService.ObtenirParId(id);
        return ordinateur is null ? NotFound() : Ok(ordinateur);
    }

    // Retourne le diagnostic d'un ordinateur, ou 404 s'il n'existe pas.
    [HttpGet("{id:int}/diagnostic")]
    public async Task<IActionResult> ObtenirDiagnostic(int id)
    {
        _logger.LogInformation("GET api/ordinateurs/{Id}/diagnostic : diagnostic demandé", id);
        var ordinateur = await _ordinateurService.ObtenirParId(id);

        if (ordinateur == null)
        {
            return NotFound();
        }

        return Ok(_ordinateurService.Diagnostiquer(ordinateur));
    }

    // Retourne les ordinateurs d'une marque donnée.
    [HttpGet("marque/{marque}")]
    public async Task<ActionResult<List<Ordinateur>>> ObtenirParMarque(string marque)
    {
        _logger.LogInformation("GET api/ordinateurs/marque/{Marque} : recherche par marque", marque);
        return Ok(await _ordinateurService.RechercherParMarque(marque));
    }

    // Crée un ordinateur, l'écrit dans les logs et retourne 201 avec l'ordinateur créé (réservé aux Admin).
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Ordinateur>> Creer(OrdinateurRequest request)
    {
        _logger.LogInformation("POST api/ordinateurs : création demandée");
        var ordinateur = await _ordinateurService.AjouterOrdinateur(request);
        _journal.Ecrire("Logs/Ordinateurs", $"{ordinateur.Nom} ajouté");
        return CreatedAtAction(nameof(ObtenirParId), new { id = ordinateur.Id }, ordinateur);
    }

    // Modifie un ordinateur et l'écrit dans les logs (200), ou retourne 404 s'il n'existe pas (réservé aux Admin).
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Modifier(int id, OrdinateurRequest request)
    {
        _logger.LogInformation("PUT api/ordinateurs/{Id} : modification demandée", id);
        if (!await _ordinateurService.ModifierOrdinateur(id, request))
        {
            return NotFound();
        }

        _journal.Ecrire("Logs/Ordinateurs", $"{request.Nom} modifié");
        return Ok(await _ordinateurService.ObtenirParId(id));
    }

    // Supprime un ordinateur et l'écrit dans les logs (204), ou retourne 404 s'il n'existe pas (réservé aux Admin).
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Supprimer(int id)
    {
        _logger.LogInformation("DELETE api/ordinateurs/{Id} : suppression demandée", id);
        if (!await _ordinateurService.SupprimerOrdinateur(id))
        {
            return NotFound();
        }

        _journal.Ecrire("Logs/Ordinateurs", $"Ordinateur {id} supprimé");
        return NoContent();
    }

    // Retourne le nom et la version de l'application.
    [HttpGet("info")]
    public IActionResult ObtenirInfo()
    {
        _logger.LogInformation("GET api/ordinateurs/info : informations de l'application demandées");
        string nom = _configuration["Application:Name"] ?? "Inconnu";
        string version = _configuration["Application:Version"] ?? "Inconnue";
        return Ok(new { Nom = nom, Version = version });
    }

    // Retourne les statistiques du parc en déconstruisant le tuple renvoyé par le service.
    [HttpGet("statistiques")]
    public IActionResult ObtenirStatistiques()
    {
        _logger.LogInformation("GET api/ordinateurs/statistiques : statistiques demandées");

        var (total, enStock, valeurStock, nbReparations)
            = _ordinateurService.CalculerStatistiques();

        return Ok(new
        {
            TotalOrdinateur = total,
            OrdiDisponibles = enStock,
            ValeurStock = valeurStock,
            NbReparations = nbReparations
        });
    }

    // Cherche un ordinateur par son nom grâce à l'indexeur de InventaireOrdinateurs, ou 404 s'il n'existe pas.
    [HttpGet("inventaire/{nom}")]
    public async Task<IActionResult> Chercher(string nom)
    {
        _logger.LogInformation("GET api/ordinateurs/inventaire/{Nom} : recherche dans l'inventaire", nom);

        var inventaire = new InventaireOrdinateurs();
        inventaire.Ordinateurs = await _ordinateurService.ListerOrdinateurs();

        var ordinateur = inventaire[nom];

        return ordinateur is null ? NotFound() : Ok(ordinateur);
    }
}