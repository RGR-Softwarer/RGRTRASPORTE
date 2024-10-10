using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class RGR000007 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Viagem_GatilhoViagem_GatilhoViagemId",
                table: "Viagem");

            migrationBuilder.DropIndex(
                name: "IX_Viagem_GatilhoViagemId",
                table: "Viagem");

            migrationBuilder.DropColumn(
                name: "GatilhoViagemId",
                table: "Viagem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "GatilhoViagemId",
                table: "Viagem",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Viagem_GatilhoViagemId",
                table: "Viagem",
                column: "GatilhoViagemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Viagem_GatilhoViagem_GatilhoViagemId",
                table: "Viagem",
                column: "GatilhoViagemId",
                principalTable: "GatilhoViagem",
                principalColumn: "Id");
        }
    }
}
