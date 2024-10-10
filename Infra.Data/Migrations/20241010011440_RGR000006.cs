using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class RGR000006 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataFimViagem",
                table: "Viagem",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataInicioViagem",
                table: "Viagem",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescricaoViagem",
                table: "Viagem",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DistanciaRealizada",
                table: "Viagem",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LatitudeFimViagem",
                table: "Viagem",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LatitudeInicioViagem",
                table: "Viagem",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LongitudeFimViagem",
                table: "Viagem",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LongitudeInicioViagem",
                table: "Viagem",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoProblema",
                table: "Viagem",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PolilinhaRota",
                table: "Viagem",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PolilinhaRotaRealizada",
                table: "Viagem",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LocalidadeDesembarqueId",
                table: "Passageiro",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "LocalidadeEmbarqueId",
                table: "Passageiro",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Observacao",
                table: "Passageiro",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Situacao",
                table: "Passageiro",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CPF",
                table: "Motorista",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoriaCNH",
                table: "Motorista",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Motorista",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Motorista",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observacao",
                table: "Motorista",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Situacao",
                table: "Motorista",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Telefone",
                table: "Motorista",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidadeCNH",
                table: "Motorista",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "Localidade",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "Localidade",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                table: "Localidade",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "Localidade",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescricaoViagem",
                table: "GatilhoViagem",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "DiasSemana",
                table: "GatilhoViagem",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PolilinhaRota",
                table: "GatilhoViagem",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Passageiro_LocalidadeDesembarqueId",
                table: "Passageiro",
                column: "LocalidadeDesembarqueId");

            migrationBuilder.CreateIndex(
                name: "IX_Passageiro_LocalidadeEmbarqueId",
                table: "Passageiro",
                column: "LocalidadeEmbarqueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Passageiro_Localidade_LocalidadeDesembarqueId",
                table: "Passageiro",
                column: "LocalidadeDesembarqueId",
                principalTable: "Localidade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Passageiro_Localidade_LocalidadeEmbarqueId",
                table: "Passageiro",
                column: "LocalidadeEmbarqueId",
                principalTable: "Localidade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passageiro_Localidade_LocalidadeDesembarqueId",
                table: "Passageiro");

            migrationBuilder.DropForeignKey(
                name: "FK_Passageiro_Localidade_LocalidadeEmbarqueId",
                table: "Passageiro");

            migrationBuilder.DropIndex(
                name: "IX_Passageiro_LocalidadeDesembarqueId",
                table: "Passageiro");

            migrationBuilder.DropIndex(
                name: "IX_Passageiro_LocalidadeEmbarqueId",
                table: "Passageiro");

            migrationBuilder.DropColumn(
                name: "DataFimViagem",
                table: "Viagem");

            migrationBuilder.DropColumn(
                name: "DataInicioViagem",
                table: "Viagem");

            migrationBuilder.DropColumn(
                name: "DescricaoViagem",
                table: "Viagem");

            migrationBuilder.DropColumn(
                name: "DistanciaRealizada",
                table: "Viagem");

            migrationBuilder.DropColumn(
                name: "LatitudeFimViagem",
                table: "Viagem");

            migrationBuilder.DropColumn(
                name: "LatitudeInicioViagem",
                table: "Viagem");

            migrationBuilder.DropColumn(
                name: "LongitudeFimViagem",
                table: "Viagem");

            migrationBuilder.DropColumn(
                name: "LongitudeInicioViagem",
                table: "Viagem");

            migrationBuilder.DropColumn(
                name: "MotivoProblema",
                table: "Viagem");

            migrationBuilder.DropColumn(
                name: "PolilinhaRota",
                table: "Viagem");

            migrationBuilder.DropColumn(
                name: "PolilinhaRotaRealizada",
                table: "Viagem");

            migrationBuilder.DropColumn(
                name: "LocalidadeDesembarqueId",
                table: "Passageiro");

            migrationBuilder.DropColumn(
                name: "LocalidadeEmbarqueId",
                table: "Passageiro");

            migrationBuilder.DropColumn(
                name: "Observacao",
                table: "Passageiro");

            migrationBuilder.DropColumn(
                name: "Situacao",
                table: "Passageiro");

            migrationBuilder.DropColumn(
                name: "CPF",
                table: "Motorista");

            migrationBuilder.DropColumn(
                name: "CategoriaCNH",
                table: "Motorista");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Motorista");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Motorista");

            migrationBuilder.DropColumn(
                name: "Observacao",
                table: "Motorista");

            migrationBuilder.DropColumn(
                name: "Situacao",
                table: "Motorista");

            migrationBuilder.DropColumn(
                name: "Telefone",
                table: "Motorista");

            migrationBuilder.DropColumn(
                name: "ValidadeCNH",
                table: "Motorista");

            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "Localidade");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Localidade");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Localidade");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Localidade");

            migrationBuilder.DropColumn(
                name: "DescricaoViagem",
                table: "GatilhoViagem");

            migrationBuilder.DropColumn(
                name: "DiasSemana",
                table: "GatilhoViagem");

            migrationBuilder.DropColumn(
                name: "PolilinhaRota",
                table: "GatilhoViagem");
        }
    }
}
