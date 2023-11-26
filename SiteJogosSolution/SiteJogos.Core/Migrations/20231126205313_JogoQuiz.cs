using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiteJogos.Core.Migrations
{
    public partial class JogoQuiz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Aleatorio",
                table: "Jogo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LimiteTempo",
                table: "Jogo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JogoQuiz",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JogoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    Pergunta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Resposta1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Resposta2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Resposta3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Resposta4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RespostaCerta = table.Column<int>(type: "int", nullable: false),
                    Excluido = table.Column<bool>(type: "bit", nullable: false),
                    Inclusao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Alteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioInclusaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioAlteracaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JogoQuiz", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JogoQuiz_Jogo_JogoId",
                        column: x => x.JogoId,
                        principalTable: "Jogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JogoQuiz_Usuario_UsuarioAlteracaoId",
                        column: x => x.UsuarioAlteracaoId,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JogoQuiz_Usuario_UsuarioInclusaoId",
                        column: x => x.UsuarioInclusaoId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JogoQuiz_JogoId",
                table: "JogoQuiz",
                column: "JogoId");

            migrationBuilder.CreateIndex(
                name: "IX_JogoQuiz_UsuarioAlteracaoId",
                table: "JogoQuiz",
                column: "UsuarioAlteracaoId");

            migrationBuilder.CreateIndex(
                name: "IX_JogoQuiz_UsuarioInclusaoId",
                table: "JogoQuiz",
                column: "UsuarioInclusaoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JogoQuiz");

            migrationBuilder.DropColumn(
                name: "Aleatorio",
                table: "Jogo");

            migrationBuilder.DropColumn(
                name: "LimiteTempo",
                table: "Jogo");
        }
    }
}
