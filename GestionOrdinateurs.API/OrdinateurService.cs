using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;

namespace GestionOrdinateurs.API;

// Service qui gère les ordinateurs du parc informatique.
public class OrdinateurService : IOrdinateurService
{
    private ParcInformatiqueContext _context;

    public event EventHandler<OrdinateurAjouteEventArgs>? OrdinateurAjoute;
    public event EventHandler<OrdinateurSupprimeEventArgs>? OrdinateurSupprime;
    public event EventHandler<OrdinateurMisAJourEventArgs>? OrdinateurMisAJour;

    // Crée le service avec le contexte de la base de données.
    public OrdinateurService(ParcInformatiqueContext context)
    {
        _context = context;
    }

    // Retourne tous les ordinateurs.
    public async Task<List<Ordinateur>> ListerOrdinateurs()
    {
        return await _context.Ordinateurs.ToListAsync();
    }

    // Retourne l'ordinateur correspondant à l'id, ou null s'il n'existe pas.
    public async Task<Ordinateur?> ObtenirParId(int id)
    {
        return await _context.Ordinateurs.FirstOrDefaultAsync(o => o.Id == id);
    }

    // Retourne uniquement les ordinateurs disponibles (stock supérieur à 0).
    public async Task<List<Ordinateur>> ListerDisponibles()
    {
        return await _context.Ordinateurs
            .Where(o => o.Stock > 0)
            .ToListAsync();
    }

    // Retourne les ordinateurs triés du plus cher au moins cher.
    public async Task<List<Ordinateur>> ListerParPrixDecroissant()
    {
        return await _context.Ordinateurs
            .OrderByDescending(o => o.Prix)
            .ToListAsync();
    }

    // Ajoute un nouvel ordinateur et retourne l'ordinateur créé.
    public async Task<Ordinateur> AjouterOrdinateur(OrdinateurRequest request)
    {
        Ordinateur nouvelOrdinateur = new Ordinateur
        {
            Nom = request.Nom,
            Etat = request.Etat,
            Stock = request.Stock,
            Prix = request.Prix,
            Caracteristiques = request.Caracteristiques
        };

        _context.Ordinateurs.Add(nouvelOrdinateur);
        await _context.SaveChangesAsync();

        OrdinateurAjoute?.Invoke(this, new OrdinateurAjouteEventArgs(nouvelOrdinateur));

        return nouvelOrdinateur;
    }

    // Modifie un ordinateur et retourne false s'il n'existe pas.
    public async Task<bool> ModifierOrdinateur(int id, OrdinateurRequest request)
    {
        Ordinateur? ordinateur = await ObtenirParId(id);

        if (ordinateur == null)
        {
            return false;
        }

        ordinateur.Nom = request.Nom;
        ordinateur.Etat = request.Etat;
        ordinateur.Stock = request.Stock;
        ordinateur.Prix = request.Prix;
        ordinateur.Caracteristiques = request.Caracteristiques;

        await _context.SaveChangesAsync();

        OrdinateurMisAJour?.Invoke(this, new OrdinateurMisAJourEventArgs(ordinateur));

        return true;
    }

    // Supprime un ordinateur et retourne false s'il n'existe pas.
    public async Task<bool> SupprimerOrdinateur(int id)
    {
        Ordinateur? ordinateur = await ObtenirParId(id);

        if (ordinateur == null)
        {
            return false;
        }

        _context.Ordinateurs.Remove(ordinateur);
        await _context.SaveChangesAsync();

        OrdinateurSupprime?.Invoke(this, new OrdinateurSupprimeEventArgs(ordinateur));

        return true;
    }

    // Retourne les ordinateurs d'une marque donnée.
    public async Task<List<Ordinateur>> RechercherParMarque(string marque)
    {
        return await _context.Ordinateurs
            .Where(o => o.Caracteristiques.Marque.ToLower() == marque.ToLower())
            .ToListAsync();
    }

    // Retourne l'action à faire sur un ordinateur selon son état et sa RAM (pattern matching).
    public string Diagnostiquer(Ordinateur ordinateur)
    {
        return ordinateur switch
        {
            {
                Etat: EtatOrdinateur.HorsService
            }
                => "Remplacement",

            {
                Etat: EtatOrdinateur.Lent,
                Caracteristiques:
                {
                    RamGo: < 8
                }
            }
                => "Ajouter RAM",

            _
                => "Aucune action"
        };
    }

    // Calcule les statistiques du parc et les renvoie dans un tuple nommé.
    public (int TotalOrdinateur, int OrdiDisponibles, decimal ValeurStock, int NbReparations)
        CalculerStatistiques()
    {
        List<Ordinateur> ordinateurs = _context.Ordinateurs.ToList();

        int total = ordinateurs.Count;
        int disponibles = ordinateurs.Count(o => o.Stock > 0);
        decimal valeurStock = ordinateurs.Sum(o => o.Prix * o.Stock);
        int nbReparations = _context.Reparations.Count();

        return (total, disponibles, valeurStock, nbReparations);
    }

}