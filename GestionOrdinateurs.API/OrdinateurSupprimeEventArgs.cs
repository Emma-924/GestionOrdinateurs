using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API;

// Données transmises avec l'événement de suppression d'un ordinateur.
public class OrdinateurSupprimeEventArgs : EventArgs
{
    public Ordinateur Ordinateur { get; }

    // Crée les données de l'événement pour l'ordinateur supprimé.
    public OrdinateurSupprimeEventArgs(Ordinateur ordinateur)
    {
        Ordinateur = ordinateur;
    }
}
