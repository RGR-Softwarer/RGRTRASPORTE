using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Data.Migrations.Cadastro
{
    /// <inheritdoc />
    public partial class C000001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_LOCALIDADE",
                columns: table => new
                {
                    LOC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LOC_NOME = table.Column<string>(type: "text", nullable: false),
                    LOC_CEP = table.Column<string>(type: "text", nullable: false),
                    LOC_UF = table.Column<string>(type: "text", nullable: false),
                    LOC_CIDADE = table.Column<string>(type: "text", nullable: false),
                    LOC_BAIRRO = table.Column<string>(type: "text", nullable: false),
                    LOC_LOGRADOURO = table.Column<string>(type: "text", nullable: false),
                    LOC_COMPLEMENTO = table.Column<string>(type: "text", nullable: false),
                    LOC_NUMERO = table.Column<string>(type: "text", nullable: false),
                    LOC_LATITUDE = table.Column<string>(type: "text", nullable: false),
                    LOC_LONGITUDE = table.Column<string>(type: "text", nullable: false),
                    LOC_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LOC_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_LOCALIDADE", x => x.LOC_ID);
                });

            migrationBuilder.CreateTable(
                name: "T_MOTORISTA",
                columns: table => new
                {
                    MOT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MOT_NOME = table.Column<string>(type: "text", nullable: false),
                    MOT_SITUACAO = table.Column<bool>(type: "boolean", nullable: false),
                    MOT_CPF = table.Column<string>(type: "text", nullable: false),
                    MOT_RG = table.Column<string>(type: "text", nullable: true),
                    MOT_TELEFONE = table.Column<string>(type: "text", nullable: false),
                    MOT_EMAIL = table.Column<string>(type: "text", nullable: false),
                    MOT_CNH = table.Column<string>(type: "text", nullable: false),
                    MOT_CATEGORIA_CNH = table.Column<int>(type: "integer", nullable: false),
                    MOT_VALIDADE_CNH = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MOT_OBSERVACAO = table.Column<string>(type: "text", nullable: true),
                    MOT_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MOT_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MOTORISTA", x => x.MOT_ID);
                });

            migrationBuilder.CreateTable(
                name: "T_PASSAGEIRO",
                columns: table => new
                {
                    PAS_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PAS_NOME = table.Column<string>(type: "text", nullable: false),
                    PAS_SITUACAO = table.Column<bool>(type: "boolean", nullable: false),
                    PAS_CPF = table.Column<string>(type: "text", nullable: false),
                    PAS_TELEFONE = table.Column<string>(type: "text", nullable: false),
                    PAS_EMAIL = table.Column<string>(type: "text", nullable: false),
                    PAS_SEXO = table.Column<int>(type: "integer", nullable: false),
                    LOC_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOC_EMBARQUE_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOC_DESEMBARQUE_ID = table.Column<long>(type: "bigint", nullable: false),
                    PAS_OBSERVACAO = table.Column<string>(type: "text", nullable: false),
                    PAS_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PAS_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_PASSAGEIRO", x => x.PAS_ID);
                    table.ForeignKey(
                        name: "FK_T_PASSAGEIRO_T_LOCALIDADE_LOC_DESEMBARQUE_ID",
                        column: x => x.LOC_DESEMBARQUE_ID,
                        principalTable: "T_LOCALIDADE",
                        principalColumn: "LOC_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_PASSAGEIRO_T_LOCALIDADE_LOC_EMBARQUE_ID",
                        column: x => x.LOC_EMBARQUE_ID,
                        principalTable: "T_LOCALIDADE",
                        principalColumn: "LOC_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_PASSAGEIRO_T_LOCALIDADE_LOC_ID",
                        column: x => x.LOC_ID,
                        principalTable: "T_LOCALIDADE",
                        principalColumn: "LOC_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_PASSAGEIRO_LOC_DESEMBARQUE_ID",
                table: "T_PASSAGEIRO",
                column: "LOC_DESEMBARQUE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_PASSAGEIRO_LOC_EMBARQUE_ID",
                table: "T_PASSAGEIRO",
                column: "LOC_EMBARQUE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_PASSAGEIRO_LOC_ID",
                table: "T_PASSAGEIRO",
                column: "LOC_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_MOTORISTA");

            migrationBuilder.DropTable(
                name: "T_PASSAGEIRO");

            migrationBuilder.DropTable(
                name: "T_LOCALIDADE");
        }
    }
}
