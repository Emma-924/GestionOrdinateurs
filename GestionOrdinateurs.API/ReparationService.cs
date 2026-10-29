using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;

namespace GestionOrdinateurs.API;

// Service qui simule des réparations d'ordinateurs et fournit leur historique.
public class ReparationService
{
    private ParcInformatiqueContext _context;
    private JournalService _journal;

    // Crée le service avec le contexte de la base de données et le service de journal.
    public ReparationService(ParcInformatiqueContext context, JournalService journal)
    {
        _context = context;
        _journal = journal;
    }

    // Crée une réparation "En cours" en base pour l'ordinateur donné (verrou car le contexte est partagé entre les tâches).
    private Reparation CreerReparation(Ordinateur ordinateur)
    {
        var reparation = new Reparation
        {
            UnOrdinateur = ordinateur,
            TypeReparation = "Remplacement SSD",
            DateDebut = DateTime.Now,
            Statut = "En cours",
            Technicien = "Technicien Atelier"
        };

        lock (_context)
        {
            _context.Reparations.Add(reparation);
            _context.SaveChanges();
        }

        return reparation;
    }

    // Simule la réparation d'un ordinateur pendant 2 secondes, la passe à "Terminée" et l'écrit dans les logs.
    public async Task SimulerReparationAsync(Ordinateur ordinateur)
    {
        var reparation = CreerReparation(ordinateur);

        await Task.Delay(2000);

        lock (_context)
        {
            reparation.Statut = "Terminée";
            _context.SaveChanges();
        }

        Console.WriteLine($"{ordinateur.Nom} réparé");
        _journal.Ecrire("Logs/Reparations_Logs", $"{ordinateur.Nom} réparé");
    }

    // Retourne l'historique de toutes les réparations sous forme de HistoriqueReparationRecord.
    public async Task<List<HistoriqueReparationRecord>> ObtenirHistorique()
    {
        return await _context.Reparations
            .Select(r => new HistoriqueReparationRecord(
                r.OrdinateurId,
                r.UnOrdinateur.Nom,
                r.Technicien,
                r.TypeReparation,
                r.DateDebut))
            .ToListAsync();
    }
}