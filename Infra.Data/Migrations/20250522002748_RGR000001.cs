using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class RGR000001 : Migration
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
                name: "T_HISTORICO_PROPRIEDADE",
                columns: table => new
                {
                    HPR_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPR_PROPRIEDADE = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    HPR_DE = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    HPR_PARA = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    HPR_HISTORICO_OBJETO_ID = table.Column<long>(type: "bigint", nullable: false),
                    HPR_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HPR_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_HISTORICO_PROPRIEDADE", x => x.HPR_ID);
                    table.ForeignKey(
                        name: "FK_T_HISTORICO_PROPRIEDADE_T_HISTORICO_OBJETO_HPR_HISTORICO_OB~",
                        column: x => x.HPR_HISTORICO_OBJETO_ID,
                        principalTable: "T_HISTORICO_OBJETO",
                        principalColumn: "HOB_ID",
                        onDelete: ReferentialAction.Cascade);
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
                    PAS_LOCALIDADE_ID = table.Column<long>(type: "bigint", nullable: false),
                    LocalidadeEmbarqueId = table.Column<long>(type: "bigint", nullable: false),
                    LocalidadeDesembarqueId = table.Column<long>(type: "bigint", nullable: false),
                    Observacao = table.Column<string>(type: "text", nullable: true),
                    PAS_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PAS_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_PASSAGEIRO", x => x.PAS_ID);
                    table.ForeignKey(
                        name: "FK_T_PASSAGEIRO_T_LOCALIDADE_LocalidadeDesembarqueId",
                        column: x => x.LocalidadeDesembarqueId,
                        principalTable: "T_LOCALIDADE",
                        principalColumn: "LOC_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_PASSAGEIRO_T_LOCALIDADE_LocalidadeEmbarqueId",
                        column: x => x.LocalidadeEmbarqueId,
                        principalTable: "T_LOCALIDADE",
                        principalColumn: "LOC_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_PASSAGEIRO_T_LOCALIDADE_PAS_LOCALIDADE_ID",
                        column: x => x.PAS_LOCALIDADE_ID,
                        principalTable: "T_LOCALIDADE",
                        principalColumn: "LOC_ID",
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
                    VEI_MODELO_VEICULO_ID = table.Column<long>(type: "bigint", nullable: true),
                    VEI_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VEI_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_VEICULO", x => x.VEI_ID);
                    table.ForeignKey(
                        name: "FK_T_VEICULO_T_MODELO_VEICULAR_VEI_MODELO_VEICULO_ID",
                        column: x => x.VEI_MODELO_VEICULO_ID,
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
                    GAV_VEICULO_ID = table.Column<long>(type: "bigint", nullable: false),
                    GAV_MOTORISTA_ID = table.Column<long>(type: "bigint", nullable: false),
                    GAV_ORIGEM_ID = table.Column<long>(type: "bigint", nullable: false),
                    GAV_DESTINO_ID = table.Column<long>(type: "bigint", nullable: false),
                    GAV_HORARIO_SAIDA = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GAV_HORARIO_CHEGADA = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GAV_DESCRICAO_VIAGEM = table.Column<string>(type: "text", nullable: true),
                    GAV_DISTANCIA = table.Column<decimal>(type: "numeric", nullable: false),
                    GAV_POLILHINA_ROTA = table.Column<string>(type: "text", nullable: true),
                    GAV_ATIVO = table.Column<bool>(type: "boolean", nullable: false),
                    DiasSemana = table.Column<int[]>(type: "integer[]", nullable: true),
                    GAV_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GAV_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_GATILHO_VIAGEM", x => x.GAV_ID);
                    table.ForeignKey(
                        name: "FK_T_GATILHO_VIAGEM_T_LOCALIDADE_GAV_DESTINO_ID",
                        column: x => x.GAV_DESTINO_ID,
                        principalTable: "T_LOCALIDADE",
                        principalColumn: "LOC_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_GATILHO_VIAGEM_T_LOCALIDADE_GAV_ORIGEM_ID",
                        column: x => x.GAV_ORIGEM_ID,
                        principalTable: "T_LOCALIDADE",
                        principalColumn: "LOC_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_GATILHO_VIAGEM_T_MOTORISTA_GAV_MOTORISTA_ID",
                        column: x => x.GAV_MOTORISTA_ID,
                        principalTable: "T_MOTORISTA",
                        principalColumn: "MOT_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_GATILHO_VIAGEM_T_VEICULO_GAV_VEICULO_ID",
                        column: x => x.GAV_VEICULO_ID,
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
                    VIA_VEICULO_ID = table.Column<long>(type: "bigint", nullable: false),
                    VIA_MOTORISTA_ID = table.Column<long>(type: "bigint", nullable: false),
                    VIA_ORIGEM_ID = table.Column<long>(type: "bigint", nullable: false),
                    VIA_DESTINO_ID = table.Column<long>(type: "bigint", nullable: false),
                    GatilhoViagemId = table.Column<long>(type: "bigint", nullable: true),
                    VIA_HORARIO_SAIDA = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VIA_HORARIO_CHEGADA = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VIA_SITUACAO = table.Column<int>(type: "integer", nullable: false),
                    MotivoProblema = table.Column<string>(type: "text", nullable: true),
                    DescricaoViagem = table.Column<string>(type: "text", nullable: true),
                    DataInicioViagem = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LatitudeInicioViagem = table.Column<decimal>(type: "numeric", nullable: true),
                    LongitudeInicioViagem = table.Column<decimal>(type: "numeric", nullable: true),
                    DataFimViagem = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LatitudeFimViagem = table.Column<decimal>(type: "numeric", nullable: true),
                    LongitudeFimViagem = table.Column<decimal>(type: "numeric", nullable: true),
                    VIA_DISTANCIA = table.Column<decimal>(type: "numeric", nullable: false),
                    DistanciaRealizada = table.Column<decimal>(type: "numeric", nullable: true),
                    PolilinhaRota = table.Column<string>(type: "text", nullable: true),
                    PolilinhaRotaRealizada = table.Column<string>(type: "text", nullable: true),
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
                        name: "FK_T_VIAGEM_T_GATILHO_VIAGEM_GatilhoViagemId",
                        column: x => x.GatilhoViagemId,
                        principalTable: "T_GATILHO_VIAGEM",
                        principalColumn: "GAV_ID");
                    table.ForeignKey(
                        name: "FK_T_VIAGEM_T_LOCALIDADE_VIA_DESTINO_ID",
                        column: x => x.VIA_DESTINO_ID,
                        principalTable: "T_LOCALIDADE",
                        principalColumn: "LOC_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_VIAGEM_T_LOCALIDADE_VIA_ORIGEM_ID",
                        column: x => x.VIA_ORIGEM_ID,
                        principalTable: "T_LOCALIDADE",
                        principalColumn: "LOC_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_VIAGEM_T_MOTORISTA_VIA_MOTORISTA_ID",
                        column: x => x.VIA_MOTORISTA_ID,
                        principalTable: "T_MOTORISTA",
                        principalColumn: "MOT_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_VIAGEM_T_VEICULO_VIA_VEICULO_ID",
                        column: x => x.VIA_VEICULO_ID,
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
                    VIP_VIAGEM_ID = table.Column<long>(type: "bigint", nullable: false),
                    VIP_PASSAGEIRO_ID = table.Column<long>(type: "bigint", nullable: false),
                    VIP_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VIP_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_VIAGEM_PASSAGEIRO", x => x.VIP_ID);
                    table.ForeignKey(
                        name: "FK_T_VIAGEM_PASSAGEIRO_T_PASSAGEIRO_VIP_PASSAGEIRO_ID",
                        column: x => x.VIP_PASSAGEIRO_ID,
                        principalTable: "T_PASSAGEIRO",
                        principalColumn: "PAS_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_VIAGEM_PASSAGEIRO_T_VIAGEM_VIP_VIAGEM_ID",
                        column: x => x.VIP_VIAGEM_ID,
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
                    VIA_VIAGEM_ID = table.Column<long>(type: "bigint", nullable: false),
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
                        name: "FK_T_VIAGEM_POSICAO_T_VIAGEM_VIA_VIAGEM_ID",
                        column: x => x.VIA_VIAGEM_ID,
                        principalTable: "T_VIAGEM",
                        principalColumn: "VIA_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_GATILHO_VIAGEM_GAV_DESTINO_ID",
                table: "T_GATILHO_VIAGEM",
                column: "GAV_DESTINO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_GATILHO_VIAGEM_GAV_MOTORISTA_ID",
                table: "T_GATILHO_VIAGEM",
                column: "GAV_MOTORISTA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_GATILHO_VIAGEM_GAV_ORIGEM_ID",
                table: "T_GATILHO_VIAGEM",
                column: "GAV_ORIGEM_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_GATILHO_VIAGEM_GAV_VEICULO_ID",
                table: "T_GATILHO_VIAGEM",
                column: "GAV_VEICULO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_HISTORICO_PROPRIEDADE_HPR_HISTORICO_OBJETO_ID",
                table: "T_HISTORICO_PROPRIEDADE",
                column: "HPR_HISTORICO_OBJETO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_PASSAGEIRO_LocalidadeDesembarqueId",
                table: "T_PASSAGEIRO",
                column: "LocalidadeDesembarqueId");

            migrationBuilder.CreateIndex(
                name: "IX_T_PASSAGEIRO_LocalidadeEmbarqueId",
                table: "T_PASSAGEIRO",
                column: "LocalidadeEmbarqueId");

            migrationBuilder.CreateIndex(
                name: "IX_T_PASSAGEIRO_PAS_LOCALIDADE_ID",
                table: "T_PASSAGEIRO",
                column: "PAS_LOCALIDADE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VEICULO_VEI_MODELO_VEICULO_ID",
                table: "T_VEICULO",
                column: "VEI_MODELO_VEICULO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_GatilhoViagemId",
                table: "T_VIAGEM",
                column: "GatilhoViagemId");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_VIA_DESTINO_ID",
                table: "T_VIAGEM",
                column: "VIA_DESTINO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_VIA_MOTORISTA_ID",
                table: "T_VIAGEM",
                column: "VIA_MOTORISTA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_VIA_ORIGEM_ID",
                table: "T_VIAGEM",
                column: "VIA_ORIGEM_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_VIA_VEICULO_ID",
                table: "T_VIAGEM",
                column: "VIA_VEICULO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_PASSAGEIRO_VIP_PASSAGEIRO_ID",
                table: "T_VIAGEM_PASSAGEIRO",
                column: "VIP_PASSAGEIRO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_PASSAGEIRO_VIP_VIAGEM_ID",
                table: "T_VIAGEM_PASSAGEIRO",
                column: "VIP_VIAGEM_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_POSICAO_VIA_VIAGEM_ID",
                table: "T_VIAGEM_POSICAO",
                column: "VIA_VIAGEM_ID");
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
                name: "T_PASSAGEIRO");

            migrationBuilder.DropTable(
                name: "T_VIAGEM");

            migrationBuilder.DropTable(
                name: "T_GATILHO_VIAGEM");

            migrationBuilder.DropTable(
                name: "T_LOCALIDADE");

            migrationBuilder.DropTable(
                name: "T_MOTORISTA");

            migrationBuilder.DropTable(
                name: "T_VEICULO");

            migrationBuilder.DropTable(
                name: "T_MODELO_VEICULAR");
        }
    }
}
