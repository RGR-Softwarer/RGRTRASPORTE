using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class RGR000004 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Capacidade",
                table: "Veiculo",
                newName: "NumeroChassi");

            migrationBuilder.RenameColumn(
                name: "Ano",
                table: "Veiculo",
                newName: "AnoModelo");

            migrationBuilder.AlterColumn<string>(
                name: "Modelo",
                table: "Veiculo",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Marca",
                table: "Veiculo",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnoFabricacao",
                table: "Veiculo",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "ModeloVeiculoId",
                table: "Veiculo",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VencimentoLicenciamento",
                table: "Veiculo",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ModeloVeicular",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Situacao = table.Column<bool>(type: "boolean", nullable: false),
                    DescricaoModelo = table.Column<bool>(type: "boolean", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    QuantidadeAssento = table.Column<int>(type: "integer", nullable: false),
                    QuantidadeEixo = table.Column<int>(type: "integer", nullable: false),
                    CapacidadeMaxima = table.Column<int>(type: "integer", nullable: false),
                    PassageirosEmPe = table.Column<int>(type: "integer", nullable: false),
                    PossuiBanheiro = table.Column<bool>(type: "boolean", nullable: false),
                    PossuiClimatizador = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeloVeicular", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Veiculo_ModeloVeiculoId",
                table: "Veiculo",
                column: "ModeloVeiculoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Veiculo_ModeloVeicular_ModeloVeiculoId",
                table: "Veiculo",
                column: "ModeloVeiculoId",
                principalTable: "ModeloVeicular",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Veiculo_ModeloVeicular_ModeloVeiculoId",
                table: "Veiculo");

            migrationBuilder.DropTable(
                name: "ModeloVeicular");

            migrationBuilder.DropIndex(
                name: "IX_Veiculo_ModeloVeiculoId",
                table: "Veiculo");

            migrationBuilder.DropColumn(
                name: "AnoFabricacao",
                table: "Veiculo");

            migrationBuilder.DropColumn(
                name: "ModeloVeiculoId",
                table: "Veiculo");

            migrationBuilder.DropColumn(
                name: "VencimentoLicenciamento",
                table: "Veiculo");

            migrationBuilder.RenameColumn(
                name: "NumeroChassi",
                table: "Veiculo",
                newName: "Capacidade");

            migrationBuilder.RenameColumn(
                name: "AnoModelo",
                table: "Veiculo",
                newName: "Ano");

            migrationBuilder.AlterColumn<string>(
                name: "Modelo",
                table: "Veiculo",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Marca",
                table: "Veiculo",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
