namespace GestionOrdinateurs.API;

// Méthodes appelées quand les événements du service des ordinateurs sont déclenchés.
public static class OrdinateurEvenements
{
    // Affiche un message dans la console quand un ordinateur est ajouté.
    public static void AfficherMessageAjout(object? sender, OrdinateurAjouteEventArgs e)
    {
        Console.WriteLine("Ordinateur ajouté : " + e.Ordinateur.Nom + " (Id " + e.Ordinateur.Id + ")");
    }

    // Affiche un message dans la console quand un ordinateur est mis à jour.
    public static void AfficherMessageMiseAJour(object? sender, OrdinateurMisAJourEventArgs e)
    {
        Console.WriteLine("Ordinateur mis à jour : " + e.Ordinateur.Nom + " (Id " + e.Ordinateur.Id + ")");
    }

    // Affiche un message dans la console quand un ordinateur est supprimé.
    public static void AfficherMessageSuppression(object? sender, OrdinateurSupprimeEventArgs e)
    {
        Console.WriteLine("Ordinateur supprimé : " + e.Ordinateur.Nom + " (Id " + e.Ordinateur.Id + ")");
    }
}
