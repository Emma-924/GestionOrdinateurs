namespace GestionOrdinateurs.Shared;

// Collection spécialisée d'ordinateurs, accessible avec des crochets par id ou par nom.
public class InventaireOrdinateurs
{
    public List<Ordinateur> Ordinateurs { get; set; } = new();

    // Indexeur par id : inventaire[ordinateurId] renvoie l'ordinateur qui a cet id, ou null.
    public Ordinateur? this[int index]
    {
        get { return Ordinateurs.FirstOrDefault(o => o.Id == index); }
    }

    // Indexeur par nom : inventaire["Dell XPS"] renvoie l'ordinateur qui a ce nom, ou null.
    public Ordinateur? this[string reference]
    {
        get { return Ordinateurs.FirstOrDefault(o => o.Nom.ToLower() == reference.ToLower()); }
    }
}