using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionOrdinateurs.API.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringOrdinateur : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reparations_Ordinateurs_OrdinateurId",
                table: "Reparations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ordinateurs",
                table: "Ordinateurs");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Ordinateurs");

            migrationBuilder.DropColumn(
                name: "ModeEconomieEnergie",
                table: "Ordinateurs");

            migrationBuilder.DropColumn(
                name: "NombreEcrans",
                table: "Ordinateurs");

            migrationBuilder.RenameTable(
                name: "Ordinateurs",
                newName: "Ordinateur");

            migrationBuilder.RenameColumn(
                name: "Processeur",
                table: "Ordinateur",
                newName: "Caracteristiques_Processeur");

            migrationBuilder.RenameColumn(
                name: "Marque",
                table: "Ordinateur",
                newName: "Caracteristiques_Marque");

            migrationBuilder.RenameColumn(
                name: "Ram",
                table: "Ordinateur",
                newName: "Caracteristiques_TypeOrdi");

            migrationBuilder.AlterColumn<decimal>(
                name: "Prix",
                table: "Ordinateur",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AddColumn<int>(
                name: "Caracteristiques_RamGo",
                table: "Ordinateur",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Caracteristiques_StockageGo",
                table: "Ordinateur",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ordinateur",
                table: "Ordinateur",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reparations_Ordinateur_OrdinateurId",
                table: "Reparations",
                column: "OrdinateurId",
                principalTable: "Ordinateur",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reparations_Ordinateur_OrdinateurId",
                table: "Reparations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ordinateur",
                table: "Ordinateur");

            migrationBuilder.DropColumn(
                name: "Caracteristiques_RamGo",
                table: "Ordinateur");

            migrationBuilder.DropColumn(
                name: "Caracteristiques_StockageGo",
                table: "Ordinateur");

            migrationBuilder.RenameTable(
                name: "Ordinateur",
                newName: "Ordinateurs");

            migrationBuilder.RenameColumn(
                name: "Caracteristiques_Processeur",
                table: "Ordinateurs",
                newName: "Processeur");

            migrationBuilder.RenameColumn(
                name: "Caracteristiques_Marque",
                table: "Ordinateurs",
                newName: "Marque");

            migrationBuilder.RenameColumn(
                name: "Caracteristiques_TypeOrdi",
                table: "Ordinateurs",
                newName: "Ram");

            migrationBuilder.AlterColumn<double>(
                name: "Prix",
                table: "Ordinateurs",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Ordinateurs",
                type: "TEXT",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ModeEconomieEnergie",
                table: "Ordinateurs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NombreEcrans",
                table: "Ordinateurs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ordinateurs",
                table: "Ordinateurs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reparations_Ordinateurs_OrdinateurId",
                table: "Reparations",
                column: "OrdinateurId",
                principalTable: "Ordinateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
