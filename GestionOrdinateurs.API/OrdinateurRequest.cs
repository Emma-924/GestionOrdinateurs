using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API;

// Données envoyées par le client pour créer ou modifier un ordinateur.
public class OrdinateurRequest
{
    public string Nom { get; set; }
    public EtatOrdinateur Etat { get; set; }
    public int Stock { get; set; }
    public decimal Prix { get; set; }
    public Caracteristiques Caracteristiques { get; set; }

    // Crée une requête avec les informations d'un ordinateur.
    public OrdinateurRequest(string nom, EtatOrdinateur etat, int stock, decimal prix, Caracteristiques caracteristiques)
    {
        Nom = nom;
        Etat = etat;
        Stock = stock;
        Prix = prix;
        Caracteristiques = caracteristiques;
    }
}