using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API;

// Contrôleur qui gère les réparations et le stock de SSD (accessible uniquement aux utilisateurs connectés).
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReparationController : ControllerBase
{
    private ParcInformatiqueContext _context;
    private ReparationService _reparService;
    private StockService _stockService;
    private ILogger<ReparationController> _logger;

    // Crée le contrôleur avec le contexte, les services et le logger.
    public ReparationController(ParcInformatiqueContext context, ReparationService reparService,
        StockService stockService, ILogger<ReparationController> logger)
    {
        _context = context;
        _reparService = reparService;
        _stockService = stockService;
        _logger = logger;
    }

    // Simule 3 réparations en parallèle sur les 3 premiers ordinateurs (réservé aux Admin).
    [Authorize(Roles = "Admin")]
    [HttpPost("SimulerReparation")]
    public async Task<IActionResult> SimulerReparation()
    {
        List<Ordinateur> listOrdinateurs = await _context.Ordinateurs.ToListAsync();

        await Task.WhenAll(
            _reparService.SimulerReparationAsync(listOrdinateurs[0]),
            _reparService.SimulerReparationAsync(listOrdinateurs[1]),
            _reparService.SimulerReparationAsync(listOrdinateurs[2])
        );

        return Ok("Simulation terminée");
    }

    // Retourne la liste des réparations avec les informations de leur ordinateur.
    [HttpGet]
    public async Task<ActionResult<List<Reparation>>> ObtenirReparations()
    {
        var reparations = await _context.Reparations
            .Include(r => r.UnOrdinateur)
            .ToListAsync();

        return Ok(reparations);
    }

    // Retourne l'historique des réparations sous forme de HistoriqueReparationRecord.
    [HttpGet("historique")]
    public async Task<ActionResult<List<HistoriqueReparationRecord>>> ObtenirHistorique()
    {
        return Ok(await _reparService.ObtenirHistorique());
    }

    // Lance 10 tâches en parallèle qui consomment chacune 1 SSD, puis renvoie le stock restant.
    [HttpGet("GetStockSSD")]
    public IActionResult GetStockSSD()
    {
        Parallel.For(
            0,
            10,
            i => _stockService.ConsommerSSD());

        _logger.LogInformation("Stock SSD restant : {Stock}", _stockService.StockSSD);

        return Ok(_stockService.StockSSD);
    }
}