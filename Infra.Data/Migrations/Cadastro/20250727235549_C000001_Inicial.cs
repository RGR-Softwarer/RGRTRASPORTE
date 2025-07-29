using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Data.Migrations.Cadastro
{
    /// <inheritdoc />
    public partial class C000001_Inicial : Migration
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
                    LOC_LATITUDE = table.Column<decimal>(type: "numeric", nullable: false),
                    LOC_LONGITUDE = table.Column<decimal>(type: "numeric", nullable: false),
                    LOC_ATIVO = table.Column<bool>(type: "boolean", nullable: false),
                    LOC_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LOC_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LOC_CREATED_BY = table.Column<string>(type: "text", nullable: false),
                    LOC_UPDATED_BY = table.Column<string>(type: "text", nullable: false)
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
                    MOT_RG = table.Column<string>(type: "text", nullable: false),
                    MOT_CNH = table.Column<string>(type: "text", nullable: false),
                    MOT_CATEGORIA_CNH = table.Column<int>(type: "integer", nullable: false),
                    MOT_VALIDADE_CNH = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MOT_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MOT_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MOT_CREATED_BY = table.Column<string>(type: "text", nullable: false),
                    MOT_UPDATED_BY = table.Column<string>(type: "text", nullable: false),
                    MOT_NOME = table.Column<string>(type: "text", nullable: false),
                    MOT_CPF = table.Column<string>(type: "text", nullable: false),
                    MOT_TELEFONE = table.Column<string>(type: "text", nullable: false),
                    MOT_EMAIL = table.Column<string>(type: "text", nullable: false),
                    MOT_SEXO = table.Column<int>(type: "integer", nullable: false),
                    LOC_ID = table.Column<long>(type: "bigint", nullable: false),
                    MOT_SITUACAO = table.Column<bool>(type: "boolean", nullable: false),
                    MOT_OBSERVACAO = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MOTORISTA", x => x.MOT_ID);
                });

            migrationBuilder.CreateTable(
                name: "T_REGISTRO_AUDITORIA",
                columns: table => new
                {
                    RAU_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RAU_NOME_ENTIDADE = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RAU_ENTIDADE_ID = table.Column<long>(type: "bigint", nullable: false),
                    RAU_ACAO = table.Column<int>(type: "integer", nullable: false),
                    RAU_DADOS = table.Column<string>(type: "TEXT", nullable: false),
                    RAU_USUARIO = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RAU_IP = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
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

            migrationBuilder.CreateTable(
                name: "T_PASSAGEIRO",
                columns: table => new
                {
                    PAS_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LOC_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOC_EMBARQUE_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOC_DESEMBARQUE_ID = table.Column<long>(type: "bigint", nullable: false),
                    PAS_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PAS_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PAS_CREATED_BY = table.Column<string>(type: "text", nullable: false),
                    PAS_UPDATED_BY = table.Column<string>(type: "text", nullable: false),
                    PAS_NOME = table.Column<string>(type: "text", nullable: false),
                    PAS_CPF = table.Column<string>(type: "text", nullable: false),
                    PAS_TELEFONE = table.Column<string>(type: "text", nullable: false),
                    PAS_EMAIL = table.Column<string>(type: "text", nullable: false),
                    PAS_SEXO = table.Column<int>(type: "integer", nullable: false),
                    PAS_SITUACAO = table.Column<bool>(type: "boolean", nullable: false),
                    PAS_OBSERVACAO = table.Column<string>(type: "text", nullable: false)
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
                name: "IX_T_LOCALIDADE_LOC_CREATED_AT",
                table: "T_LOCALIDADE",
                column: "LOC_CREATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_LOCALIDADE_LOC_CREATED_BY",
                table: "T_LOCALIDADE",
                column: "LOC_CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_T_LOCALIDADE_LOC_UPDATED_AT",
                table: "T_LOCALIDADE",
                column: "LOC_UPDATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_LOCALIDADE_LOC_UPDATED_BY",
                table: "T_LOCALIDADE",
                column: "LOC_UPDATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_T_MOTORISTA_MOT_CREATED_AT",
                table: "T_MOTORISTA",
                column: "MOT_CREATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_MOTORISTA_MOT_CREATED_BY",
                table: "T_MOTORISTA",
                column: "MOT_CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_T_MOTORISTA_MOT_UPDATED_AT",
                table: "T_MOTORISTA",
                column: "MOT_UPDATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_MOTORISTA_MOT_UPDATED_BY",
                table: "T_MOTORISTA",
                column: "MOT_UPDATED_BY");

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

            migrationBuilder.CreateIndex(
                name: "IX_T_PASSAGEIRO_PAS_CREATED_AT",
                table: "T_PASSAGEIRO",
                column: "PAS_CREATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_PASSAGEIRO_PAS_CREATED_BY",
                table: "T_PASSAGEIRO",
                column: "PAS_CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_T_PASSAGEIRO_PAS_UPDATED_AT",
                table: "T_PASSAGEIRO",
                column: "PAS_UPDATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_PASSAGEIRO_PAS_UPDATED_BY",
                table: "T_PASSAGEIRO",
                column: "PAS_UPDATED_BY");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_MOTORISTA");

            migrationBuilder.DropTable(
                name: "T_PASSAGEIRO");

            migrationBuilder.DropTable(
                name: "T_REGISTRO_AUDITORIA");

            migrationBuilder.DropTable(
                name: "T_LOCALIDADE");
        }
    }
}
