using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Data.Migrations.Transportador
{
    /// <inheritdoc />
    public partial class UpdateToNet9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "VPO_LONGITUDE",
                table: "T_VIAGEM_POSICAO",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "VPO_LATITUDE",
                table: "T_VIAGEM_POSICAO",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<DateTime>(
                name: "VIP_DATA_LIMITE_CONFIRMACAO",
                table: "T_VIAGEM_PASSAGEIRO",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VIP_PASSAGEIRO_FIXO",
                table: "T_VIAGEM_PASSAGEIRO",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "VIP_STATUS_CONFIRMACAO",
                table: "T_VIAGEM_PASSAGEIRO",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VIA_TIPO_TRECHO",
                table: "T_VIAGEM",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "VIA_VIAGEM_PAR_ID",
                table: "T_VIAGEM",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VEI_OBSERVACAO",
                table: "T_VEICULO",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "VEI_COR",
                table: "T_VEICULO",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "GAV_POLILINHA_ROTA",
                table: "T_GATILHO_VIAGEM",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "GAV_DIAS_SEMANA",
                table: "T_GATILHO_VIAGEM",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "GAV_DESCRICAO_VIAGEM",
                table: "T_GATILHO_VIAGEM",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "T_REGISTRO_AUDITORIA",
                columns: table => new
                {
                    RAU_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RAU_NOME_ENTIDADE = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RAU_ENTIDADE_ID = table.Column<long>(type: "bigint", nullable: false),
                    RAU_ACAO = table.Column<int>(type: "integer", nullable: false),
                    RAU_DADOS = table.Column<string>(type: "TEXT", nullable: true),
                    RAU_USUARIO = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RAU_IP = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    RAU_DATA_OCORRENCIA = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RAU_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RAU_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RAU_CREATED_BY = table.Column<string>(type: "text", nullable: false),
                    RAU_UPDATED_BY = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_REGISTRO_AUDITORIA", x => x.RAU_ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_VIA_VIAGEM_PAR_ID",
                table: "T_VIAGEM",
                column: "VIA_VIAGEM_PAR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_REGISTRO_AUDITORIA_RAU_CREATED_AT",
                table: "T_REGISTRO_AUDITORIA",
                column: "RAU_CREATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_REGISTRO_AUDITORIA_RAU_CREATED_BY",
                table: "T_REGISTRO_AUDITORIA",
                column: "RAU_CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_T_REGISTRO_AUDITORIA_RAU_UPDATED_AT",
                table: "T_REGISTRO_AUDITORIA",
                column: "RAU_UPDATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_REGISTRO_AUDITORIA_RAU_UPDATED_BY",
                table: "T_REGISTRO_AUDITORIA",
                column: "RAU_UPDATED_BY");

            migrationBuilder.AddForeignKey(
                name: "FK_T_VIAGEM_T_VIAGEM_VIA_VIAGEM_PAR_ID",
                table: "T_VIAGEM",
                column: "VIA_VIAGEM_PAR_ID",
                principalTable: "T_VIAGEM",
                principalColumn: "VIA_ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_VIAGEM_T_VIAGEM_VIA_VIAGEM_PAR_ID",
                table: "T_VIAGEM");

            migrationBuilder.DropTable(
                name: "T_REGISTRO_AUDITORIA");

            migrationBuilder.DropIndex(
                name: "IX_T_VIAGEM_VIA_VIAGEM_PAR_ID",
                table: "T_VIAGEM");

            migrationBuilder.DropColumn(
                name: "VIP_DATA_LIMITE_CONFIRMACAO",
                table: "T_VIAGEM_PASSAGEIRO");

            migrationBuilder.DropColumn(
                name: "VIP_PASSAGEIRO_FIXO",
                table: "T_VIAGEM_PASSAGEIRO");

            migrationBuilder.DropColumn(
                name: "VIP_STATUS_CONFIRMACAO",
                table: "T_VIAGEM_PASSAGEIRO");

            migrationBuilder.DropColumn(
                name: "VIA_TIPO_TRECHO",
                table: "T_VIAGEM");

            migrationBuilder.DropColumn(
                name: "VIA_VIAGEM_PAR_ID",
                table: "T_VIAGEM");

            migrationBuilder.AlterColumn<decimal>(
                name: "VPO_LONGITUDE",
                table: "T_VIAGEM_POSICAO",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "VPO_LATITUDE",
                table: "T_VIAGEM_POSICAO",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VEI_OBSERVACAO",
                table: "T_VEICULO",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VEI_COR",
                table: "T_VEICULO",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GAV_POLILINHA_ROTA",
                table: "T_GATILHO_VIAGEM",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GAV_DIAS_SEMANA",
                table: "T_GATILHO_VIAGEM",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GAV_DESCRICAO_VIAGEM",
                table: "T_GATILHO_VIAGEM",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
