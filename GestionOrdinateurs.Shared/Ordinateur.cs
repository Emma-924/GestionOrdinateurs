using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestionOrdinateurs.Shared;

// Représente un ordinateur du parc informatique.
[Table("Ordinateur")]
public class Ordinateur
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public EtatOrdinateur Etat { get; set; }

    [DefaultValue(0)]
    public int Stock { get; set; }

    [DefaultValue(0)]
    public decimal Prix { get; set; }

    public bool Vendu { get; set; } = false;

    public Caracteristiques Caracteristiques { get; set; }

    [JsonIgnore]
    public List<Reparation> Reparations { get; set; } = new();
}
