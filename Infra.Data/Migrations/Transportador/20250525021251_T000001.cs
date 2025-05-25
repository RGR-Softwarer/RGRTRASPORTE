using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Data.Migrations.Transportador
{
    /// <inheritdoc />
    public partial class T000001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_HISTORICO_OBJETO",
                columns: table => new
                {
                    HOB_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HOB_OBJETO = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    HOB_CODIGO_OBJETO = table.Column<long>(type: "bigint", nullable: false),
                    HOB_DESCRICAO_OBJETO = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    HOB_ACAO = table.Column<int>(type: "integer", nullable: false),
                    HOB_TIPO_AUDITADO = table.Column<int>(type: "integer", nullable: false),
                    HOB_ORIGEM_AUDITADO = table.Column<int>(type: "integer", nullable: false),
                    HOB_DESCRICAO_ACAO = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    HOB_DATA = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HOB_IP = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    HOB_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HOB_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_HISTORICO_OBJETO", x => x.HOB_ID);
                });

            migrationBuilder.CreateTable(
                name: "T_MODELO_VEICULAR",
                columns: table => new
                {
                    MOV_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MOV_SITUACAO = table.Column<bool>(type: "boolean", nullable: false),
                    MOV_DESCRICAO_MODELO = table.Column<bool>(type: "boolean", nullable: false),
                    MOV_TIPO = table.Column<int>(type: "integer", nullable: false),
                    MOV_QUANTIDADE_ASSENTO = table.Column<int>(type: "integer", nullable: false),
                    MOV_QUANTIDADE_EIXO = table.Column<int>(type: "integer", nullable: false),
                    MOV_CAPACIDADE_MAXIMA = table.Column<int>(type: "integer", nullable: false),
                    MOV_PASSAGEIROS_EM_PE = table.Column<int>(type: "integer", nullable: false),
                    MOV_POSSUI_BANHEIRO = table.Column<bool>(type: "boolean", nullable: false),
                    MOV_POSSUI_CLIMATIZADOR = table.Column<bool>(type: "boolean", nullable: false),
                    MOV_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MOV_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MODELO_VEICULAR", x => x.MOV_ID);
                });

            migrationBuilder.CreateTable(
                name: "T_HISTORICO_PROPRIEDADE",
                columns: table => new
                {
                    HPR_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPR_PROPRIEDADE = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    HPR_DE = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    HPR_PARA = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    HOB_ID = table.Column<long>(type: "bigint", nullable: false),
                    HPR_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HPR_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_HISTORICO_PROPRIEDADE", x => x.HPR_ID);
                    table.ForeignKey(
                        name: "FK_T_HISTORICO_PROPRIEDADE_T_HISTORICO_OBJETO_HOB_ID",
                        column: x => x.HOB_ID,
                        principalTable: "T_HISTORICO_OBJETO",
                        principalColumn: "HOB_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_VEICULO",
                columns: table => new
                {
                    VEI_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VEI_PLACA = table.Column<string>(type: "text", nullable: false),
                    VEI_MODELO = table.Column<string>(type: "text", nullable: false),
                    VEI_MARCA = table.Column<string>(type: "text", nullable: false),
                    VEI_NUMERO_CHASSI = table.Column<string>(type: "text", nullable: false),
                    VEI_ANO_MODELO = table.Column<int>(type: "integer", nullable: false),
                    VEI_ANO_FABRICACAO = table.Column<int>(type: "integer", nullable: false),
                    VEI_COR = table.Column<string>(type: "text", nullable: true),
                    VEI_RENAVAM = table.Column<string>(type: "text", nullable: false),
                    VEI_VENCIMENTO_LICENCIAMENTO = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VEI_TIPO_COMBUSTIVEL = table.Column<int>(type: "integer", nullable: false),
                    VEI_STATUS = table.Column<int>(type: "integer", nullable: false),
                    VEI_OBSERVACAO = table.Column<string>(type: "text", nullable: true),
                    MOV_ID = table.Column<long>(type: "bigint", nullable: true),
                    VEI_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VEI_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_VEICULO", x => x.VEI_ID);
                    table.ForeignKey(
                        name: "FK_T_VEICULO_T_MODELO_VEICULAR_MOV_ID",
                        column: x => x.MOV_ID,
                        principalTable: "T_MODELO_VEICULAR",
                        principalColumn: "MOV_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_GATILHO_VIAGEM",
                columns: table => new
                {
                    GAV_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GAV_DESCRICAO = table.Column<string>(type: "text", nullable: false),
                    VEI_ID = table.Column<long>(type: "bigint", nullable: false),
                    MOT_MOTORISTA_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOC_ORIGEM_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOC_DESTINO_ID = table.Column<long>(type: "bigint", nullable: false),
                    GAV_HORARIO_SAIDA = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GAV_HORARIO_CHEGADA = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GAV_DESCRICAO_VIAGEM = table.Column<string>(type: "text", nullable: true),
                    GAV_DISTANCIA = table.Column<decimal>(type: "numeric", nullable: false),
                    GAV_POLILHINA_ROTA = table.Column<string>(type: "text", nullable: true),
                    GAV_ATIVO = table.Column<bool>(type: "boolean", nullable: false),
                    GAV_DIAS_SEMANA = table.Column<string>(type: "text", nullable: true),
                    GAV_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GAV_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_GATILHO_VIAGEM", x => x.GAV_ID);
                    table.ForeignKey(
                        name: "FK_T_GATILHO_VIAGEM_T_VEICULO_VEI_ID",
                        column: x => x.VEI_ID,
                        principalTable: "T_VEICULO",
                        principalColumn: "VEI_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_VIAGEM",
                columns: table => new
                {
                    VIA_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VIA_CODIGO_VIAGEM = table.Column<string>(type: "text", nullable: false),
                    VIA_DATA_VIAGEM = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VEI_ID = table.Column<long>(type: "bigint", nullable: false),
                    MOT_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOC_ORIGEM_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOC_DESTINO_ID = table.Column<long>(type: "bigint", nullable: false),
                    GAV_ID = table.Column<long>(type: "bigint", nullable: false),
                    VIA_HORARIO_SAIDA = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VIA_HORARIO_CHEGADA = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VIA_SITUACAO = table.Column<int>(type: "integer", nullable: false),
                    VIA_MOTIVO_PROBLEMA = table.Column<string>(type: "text", nullable: false),
                    VIA_DESCRICAO_VIAGEM = table.Column<string>(type: "text", nullable: false),
                    DataInicioViagem = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VIA_LATITUDE_INICIO_VIAGEM = table.Column<decimal>(type: "numeric", nullable: false),
                    LongitudeInicioViagem = table.Column<decimal>(type: "numeric", nullable: true),
                    DataFimViagem = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VIA_LATITUDE_FIM_VIAGEM = table.Column<decimal>(type: "numeric", nullable: false),
                    LongitudeFimViagem = table.Column<decimal>(type: "numeric", nullable: true),
                    VIA_DISTANCIA = table.Column<decimal>(type: "numeric", nullable: false),
                    VIA_DISTANCIA_REALIZADA = table.Column<decimal>(type: "numeric", nullable: false),
                    VIA_POLILINHA_ROTA = table.Column<string>(type: "text", nullable: false),
                    VIA_POLILINHA_ROTA_REALIZADA = table.Column<string>(type: "text", nullable: false),
                    VIA_NUMERO_PASSAGEIROS = table.Column<int>(type: "integer", nullable: false),
                    VIA_LOTADO = table.Column<bool>(type: "boolean", nullable: false),
                    VIA_EXCESSO = table.Column<bool>(type: "boolean", nullable: false),
                    VIA_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VIA_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_VIAGEM", x => x.VIA_ID);
                    table.ForeignKey(
                        name: "FK_T_VIAGEM_T_GATILHO_VIAGEM_GAV_ID",
                        column: x => x.GAV_ID,
                        principalTable: "T_GATILHO_VIAGEM",
                        principalColumn: "GAV_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_VIAGEM_T_VEICULO_VEI_ID",
                        column: x => x.VEI_ID,
                        principalTable: "T_VEICULO",
                        principalColumn: "VEI_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_VIAGEM_PASSAGEIRO",
                columns: table => new
                {
                    VIP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VIA_VIAGEM_ID = table.Column<long>(type: "bigint", nullable: false),
                    PAS_PASSAGEIRO_ID = table.Column<long>(type: "bigint", nullable: false),
                    VIP_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VIP_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_VIAGEM_PASSAGEIRO", x => x.VIP_ID);
                    table.ForeignKey(
                        name: "FK_T_VIAGEM_PASSAGEIRO_T_VIAGEM_VIA_VIAGEM_ID",
                        column: x => x.VIA_VIAGEM_ID,
                        principalTable: "T_VIAGEM",
                        principalColumn: "VIA_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_VIAGEM_POSICAO",
                columns: table => new
                {
                    VPO_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VIA_ID = table.Column<long>(type: "bigint", nullable: false),
                    VPO_DATA_HORA = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VPO_LATITUDE = table.Column<string>(type: "text", nullable: false),
                    VPO_LONGITUDE = table.Column<string>(type: "text", nullable: false),
                    VPO_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VPO_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_VIAGEM_POSICAO", x => x.VPO_ID);
                    table.ForeignKey(
                        name: "FK_T_VIAGEM_POSICAO_T_VIAGEM_VIA_ID",
                        column: x => x.VIA_ID,
                        principalTable: "T_VIAGEM",
                        principalColumn: "VIA_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_GATILHO_VIAGEM_VEI_ID",
                table: "T_GATILHO_VIAGEM",
                column: "VEI_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_HISTORICO_PROPRIEDADE_HOB_ID",
                table: "T_HISTORICO_PROPRIEDADE",
                column: "HOB_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VEICULO_MOV_ID",
                table: "T_VEICULO",
                column: "MOV_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_GAV_ID",
                table: "T_VIAGEM",
                column: "GAV_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_VEI_ID",
                table: "T_VIAGEM",
                column: "VEI_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_PASSAGEIRO_VIA_VIAGEM_ID",
                table: "T_VIAGEM_PASSAGEIRO",
                column: "VIA_VIAGEM_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_POSICAO_VIA_ID",
                table: "T_VIAGEM_POSICAO",
                column: "VIA_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_HISTORICO_PROPRIEDADE");

            migrationBuilder.DropTable(
                name: "T_VIAGEM_PASSAGEIRO");

            migrationBuilder.DropTable(
                name: "T_VIAGEM_POSICAO");

            migrationBuilder.DropTable(
                name: "T_HISTORICO_OBJETO");

            migrationBuilder.DropTable(
                name: "T_VIAGEM");

            migrationBuilder.DropTable(
                name: "T_GATILHO_VIAGEM");

            migrationBuilder.DropTable(
                name: "T_VEICULO");

            migrationBuilder.DropTable(
                name: "T_MODELO_VEICULAR");
        }
    }
}
