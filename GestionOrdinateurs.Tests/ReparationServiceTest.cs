using GestionOrdinateurs.API;
using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;

namespace GestionOrdinateurs.Tests;

// Tests unitaires du service des réparations.
public class ReparationServiceTest
{
    // Crée un contexte branché sur une base de données en mémoire, différente pour chaque test.
    private ParcInformatiqueContext CreerContexte()
    {
        var options =
            new DbContextOptionsBuilder<ParcInformatiqueContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        return new ParcInformatiqueContext(options);
    }

    // Vérifie que la simulation crée bien une réparation "Terminée" liée à l'ordinateur.
    [Fact]
    public async Task SimulerReparation_CreeUneReparationTerminee()
    {
        var context = CreerContexte();
        var service = new ReparationService(context, new JournalService());
        var ordinateur = new Ordinateur
        {
            Nom = "PC-TEST",
            Etat = EtatOrdinateur.EnPanne,
            Stock = 1,
            Prix = 999,
            Caracteristiques = new Caracteristiques(16, 512, TypeOrdinateur.Portable, "Asus", "Intel Core i7")
        };
        context.Ordinateurs.Add(ordinateur);
        await context.SaveChangesAsync();

        await service.SimulerReparationAsync(ordinateur);

        var reparation = Assert.Single(context.Reparations.ToList());
        Assert.Equal(ordinateur.Id, reparation.OrdinateurId);
        Assert.Equal("Terminée", reparation.Statut);
    }
}