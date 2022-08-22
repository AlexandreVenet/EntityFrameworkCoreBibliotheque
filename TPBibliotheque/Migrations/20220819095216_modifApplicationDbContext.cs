using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPBibliotheque.Migrations
{
    public partial class modifApplicationDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livres_Editeur_EditeurId",
                table: "Livres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Editeur",
                table: "Editeur");

            migrationBuilder.RenameTable(
                name: "Editeur",
                newName: "Editeurs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Editeurs",
                table: "Editeurs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Livres_Editeurs_EditeurId",
                table: "Livres",
                column: "EditeurId",
                principalTable: "Editeurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livres_Editeurs_EditeurId",
                table: "Livres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Editeurs",
                table: "Editeurs");

            migrationBuilder.RenameTable(
                name: "Editeurs",
                newName: "Editeur");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Editeur",
                table: "Editeur",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Livres_Editeur_EditeurId",
                table: "Livres",
                column: "EditeurId",
                principalTable: "Editeur",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
