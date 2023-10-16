using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiteJogos.Core.Migrations
{
    public partial class JogoDaMemoriaMidia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JogoDaMemoriaMidia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JogoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MidiaPrimariaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MidiaSecundariaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Excluido = table.Column<bool>(type: "bit", nullable: false),
                    Inclusao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Alteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioInclusaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioAlteracaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JogoDaMemoriaMidia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JogoDaMemoriaMidia_Jogo_JogoId",
                        column: x => x.JogoId,
                        principalTable: "Jogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JogoDaMemoriaMidia_Midia_MidiaPrimariaId",
                        column: x => x.MidiaPrimariaId,
                        principalTable: "Midia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JogoDaMemoriaMidia_Midia_MidiaSecundariaId",
                        column: x => x.MidiaSecundariaId,
                        principalTable: "Midia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JogoDaMemoriaMidia_Usuario_UsuarioAlteracaoId",
                        column: x => x.UsuarioAlteracaoId,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JogoDaMemoriaMidia_Usuario_UsuarioInclusaoId",
                        column: x => x.UsuarioInclusaoId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JogoDaMemoriaMidia_JogoId",
                table: "JogoDaMemoriaMidia",
                column: "JogoId");

            migrationBuilder.CreateIndex(
                name: "IX_JogoDaMemoriaMidia_MidiaPrimariaId",
                table: "JogoDaMemoriaMidia",
                column: "MidiaPrimariaId");

            migrationBuilder.CreateIndex(
                name: "IX_JogoDaMemoriaMidia_MidiaSecundariaId",
                table: "JogoDaMemoriaMidia",
                column: "MidiaSecundariaId");

            migrationBuilder.CreateIndex(
                name: "IX_JogoDaMemoriaMidia_UsuarioAlteracaoId",
                table: "JogoDaMemoriaMidia",
                column: "UsuarioAlteracaoId");

            migrationBuilder.CreateIndex(
                name: "IX_JogoDaMemoriaMidia_UsuarioInclusaoId",
                table: "JogoDaMemoriaMidia",
                column: "UsuarioInclusaoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JogoDaMemoriaMidia");
        }
    }
}
