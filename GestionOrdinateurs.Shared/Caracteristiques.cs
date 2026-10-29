namespace GestionOrdinateurs.Shared;

// Regroupe les caractéristiques techniques d'un ordinateur.
public record Caracteristiques
{
    public int RamGo { get; set; }
    public TypeOrdinateur TypeOrdi { get; set; }
    public int StockageGo { get; set; }
    public string Marque { get; set; }
    public string Processeur { get; set; }

    // Crée les caractéristiques avec toutes leurs valeurs.
    public Caracteristiques(int ramGo, int stockageGo, TypeOrdinateur typeOrdi, string marque, string processeur)
    {
        RamGo = ramGo;
        StockageGo = stockageGo;
        TypeOrdi = typeOrdi;
        Marque = marque;
        Processeur = processeur;
    }
}