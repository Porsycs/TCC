using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiteJogos.Core.Migrations
{
    public partial class JogoRanking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JogoRanking",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JogoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Jogador = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pontuacao = table.Column<int>(type: "int", nullable: false),
                    Excluido = table.Column<bool>(type: "bit", nullable: false),
                    Inclusao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Alteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioInclusaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioAlteracaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JogoRanking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JogoRanking_Jogo_JogoId",
                        column: x => x.JogoId,
                        principalTable: "Jogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JogoRanking_Usuario_UsuarioAlteracaoId",
                        column: x => x.UsuarioAlteracaoId,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JogoRanking_Usuario_UsuarioInclusaoId",
                        column: x => x.UsuarioInclusaoId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JogoRanking_JogoId",
                table: "JogoRanking",
                column: "JogoId");

            migrationBuilder.CreateIndex(
                name: "IX_JogoRanking_UsuarioAlteracaoId",
                table: "JogoRanking",
                column: "UsuarioAlteracaoId");

            migrationBuilder.CreateIndex(
                name: "IX_JogoRanking_UsuarioInclusaoId",
                table: "JogoRanking",
                column: "UsuarioInclusaoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JogoRanking");
        }
    }
}
