using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiteJogos.Core.Migrations
{
    public partial class FotoPerfil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MidiaId",
                table: "Usuario",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MidiaId",
                table: "Usuario");
        }
    }
}
