namespace GestionOrdinateurs.Shared;

// Ligne de l'historique des réparations, renvoyée par l'API et affichée par le client.
public record HistoriqueReparationRecord(
    int OrdinateurId,
    string NomOrdinateur,
    string Technicien,
    string TypeReparation,
    DateTime DateIntervention
);
