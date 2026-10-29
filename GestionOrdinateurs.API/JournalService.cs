namespace GestionOrdinateurs.API;

// Service qui écrit des messages datés dans des fichiers de logs.
public class JournalService
{
    private readonly object _lockObject = new();

    // Crée le dossier si besoin, puis ajoute "date du jour - message" à la fin du fichier journal.log.
    public void Ecrire(string dossier, string message)
    {
        lock (_lockObject)
        {
            Directory.CreateDirectory(dossier);

            using var writer =
                new StreamWriter(
                    dossier + "/journal.log",
                    append: true);

            writer.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy")} - {message}");
        }
    }
}