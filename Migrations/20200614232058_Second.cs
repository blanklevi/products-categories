using Microsoft.EntityFrameworkCore.Migrations;

namespace ProdsCats.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Associations_Associations_AssociationId1",
                table: "Associations");

            migrationBuilder.DropIndex(
                name: "IX_Associations_AssociationId1",
                table: "Associations");

            migrationBuilder.DropColumn(
                name: "AssociationId1",
                table: "Associations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssociationId1",
                table: "Associations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Associations_AssociationId1",
                table: "Associations",
                column: "AssociationId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Associations_Associations_AssociationId1",
                table: "Associations",
                column: "AssociationId1",
                principalTable: "Associations",
                principalColumn: "AssociationId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
