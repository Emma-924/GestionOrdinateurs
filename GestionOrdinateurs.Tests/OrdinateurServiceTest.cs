using GestionOrdinateurs.API;
using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;

namespace GestionOrdinateurs.Tests;

// Tests unitaires du service des ordinateurs.
public class OrdinateurServiceTest
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

    // Vérifie que l'ajout d'un ordinateur augmente le nombre d'ordinateurs de 1.
    [Fact]
    public async Task Ajouter_UnOrdinateur_AugmenteLeNombre()
    {
        var service = new OrdinateurService(CreerContexte());
        var nombreAvant = (await service.ListerOrdinateurs()).Count;
        var request = new OrdinateurRequest("PC-TEST", EtatOrdinateur.EnService, 5, 999,
            new Caracteristiques(16, 512, TypeOrdinateur.Portable, "Asus", "Intel Core i7"));

        await service.AjouterOrdinateur(request);

        Assert.Equal(nombreAvant + 1, (await service.ListerOrdinateurs()).Count);
    }

    // Vérifie que la suppression d'un ordinateur existant retourne true.
    [Fact]
    public async Task Supprimer_OrdinateurExistant_RetourneTrue()
    {
        var service = new OrdinateurService(CreerContexte());
        var request = new OrdinateurRequest("PC-TEST", EtatOrdinateur.EnService, 5, 999,
            new Caracteristiques(16, 512, TypeOrdinateur.Portable, "Asus", "Intel Core i7"));
        var ordinateur = await service.AjouterOrdinateur(request);

        var resultat = await service.SupprimerOrdinateur(ordinateur.Id);

        Assert.True(resultat);
        Assert.Null(await service.ObtenirParId(ordinateur.Id));
    }

    // Vérifie que la suppression d'un ordinateur inexistant retourne false.
    [Fact]
    public async Task Supprimer_OrdinateurInexistant_RetourneFalse()
    {
        var service = new OrdinateurService(CreerContexte());

        var resultat = await service.SupprimerOrdinateur(9999);

        Assert.False(resultat);
    }

    // Vérifie que la recherche par marque retourne l'ordinateur de cette marque.
    [Fact]
    public async Task RechercherParMarque_MarqueExistante_RetourneLOrdinateur()
    {
        var service = new OrdinateurService(CreerContexte());
        var request = new OrdinateurRequest("PC-TEST", EtatOrdinateur.EnService, 5, 999,
            new Caracteristiques(16, 512, TypeOrdinateur.Portable, "MarqueTest", "Intel Core i7"));
        await service.AjouterOrdinateur(request);

        var resultat = await service.RechercherParMarque("marquetest");

        Assert.Single(resultat);
    }
}