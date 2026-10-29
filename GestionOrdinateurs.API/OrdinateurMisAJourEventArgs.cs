using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API;

// Données transmises avec l'événement de mise à jour d'un ordinateur.
public class OrdinateurMisAJourEventArgs : EventArgs
{
    public Ordinateur Ordinateur { get; }

    // Crée les données de l'événement pour l'ordinateur mis à jour.
    public OrdinateurMisAJourEventArgs(Ordinateur ordinateur)
    {
        Ordinateur = ordinateur;
    }
}
