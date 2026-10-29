using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API;

// Données transmises avec l'événement d'ajout d'un ordinateur.
public class OrdinateurAjouteEventArgs : EventArgs
{
    public Ordinateur Ordinateur { get; }

    // Crée les données de l'événement pour l'ordinateur ajouté.
    public OrdinateurAjouteEventArgs(Ordinateur ordinateur)
    {
        Ordinateur = ordinateur;
    }
}
