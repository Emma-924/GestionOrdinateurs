using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API;

// Contrat du service qui gère les ordinateurs du parc informatique.
public interface IOrdinateurService
{
    // Retourne tous les ordinateurs.
    Task<List<Ordinateur>> ListerOrdinateurs();

    // Retourne l'ordinateur correspondant à l'id, ou null s'il n'existe pas.
    Task<Ordinateur?> ObtenirParId(int id);

    // Ajoute un nouvel ordinateur et retourne l'ordinateur créé.
    Task<Ordinateur> AjouterOrdinateur(OrdinateurRequest request);

    // Modifie un ordinateur et retourne false s'il n'existe pas.
    Task<bool> ModifierOrdinateur(int id, OrdinateurRequest request);

    // Supprime un ordinateur et retourne false s'il n'existe pas.
    Task<bool> SupprimerOrdinateur(int id);

    // Retourne les ordinateurs d'une marque donnée.
    Task<List<Ordinateur>> RechercherParMarque(string marque);

    // Retourne uniquement les ordinateurs disponibles (stock supérieur à 0).
    Task<List<Ordinateur>> ListerDisponibles();

    // Retourne les ordinateurs triés du plus cher au moins cher.
    Task<List<Ordinateur>> ListerParPrixDecroissant();

    // Retourne l'action à faire sur un ordinateur selon son état et sa RAM.
    string Diagnostiquer(Ordinateur ordinateur);

    // Calcule les statistiques du parc et les renvoie dans un tuple nommé.
    (int TotalOrdinateur, int OrdiDisponibles, decimal ValeurStock, int NbReparations) CalculerStatistiques();

    // Événement déclenché quand un ordinateur est ajouté.
    event EventHandler<OrdinateurAjouteEventArgs>? OrdinateurAjoute;

    // Événement déclenché quand un ordinateur est supprimé.
    event EventHandler<OrdinateurSupprimeEventArgs>? OrdinateurSupprime;

    // Événement déclenché quand un ordinateur est mis à jour.
    event EventHandler<OrdinateurMisAJourEventArgs>? OrdinateurMisAJour;
}
