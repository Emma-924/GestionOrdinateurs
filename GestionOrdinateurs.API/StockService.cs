namespace GestionOrdinateurs.API;

// Service qui gère le stock de SSD, partagé par plusieurs threads.
public class StockService
{
    public int StockSSD = 10;

    private readonly object _lockObject = new();

    // Consomme un SSD : un seul thread à la fois peut lire, attendre 10 ms et enregistrer le stock diminué de 1.
    public void ConsommerSSD()
    {
        lock (_lockObject)
        {
            int stockActuel = StockSSD;
            Task.Delay(10).Wait();
            StockSSD = stockActuel - 1;
        }
    }
}