using Microsoft.EntityFrameworkCore.Migrations;

namespace back_end.Migrations
{
    public partial class Votaciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Votaciones",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Puntuacion = table.Column<int>(type: "int", nullable: false),
                    PeliculaID = table.Column<int>(type: "int", nullable: false),
                    UsuarioID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votaciones", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Votaciones_AspNetUsers_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Votaciones_Peliculas_PeliculaID",
                        column: x => x.PeliculaID,
                        principalTable: "Peliculas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Votaciones_PeliculaID",
                table: "Votaciones",
                column: "PeliculaID");

            migrationBuilder.CreateIndex(
                name: "IX_Votaciones_UsuarioID",
                table: "Votaciones",
                column: "UsuarioID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Votaciones");
        }
    }
}
