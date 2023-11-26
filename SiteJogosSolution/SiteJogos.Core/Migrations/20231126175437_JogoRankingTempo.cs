using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiteJogos.Core.Migrations
{
    public partial class JogoRankingTempo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Tempo",
                table: "JogoRanking",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tempo",
                table: "JogoRanking");
        }
    }
}
