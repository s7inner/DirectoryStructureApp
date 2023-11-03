using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DirectoryStructureApp.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MyCatalogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MyCatalogId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyCatalogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyCatalogs_MyCatalogs_MyCatalogId",
                        column: x => x.MyCatalogId,
                        principalTable: "MyCatalogs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MyCatalogs_MyCatalogId",
                table: "MyCatalogs",
                column: "MyCatalogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyCatalogs");
        }
    }
}
