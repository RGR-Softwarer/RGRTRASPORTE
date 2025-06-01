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
                name: "Localidade",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "text", nullable: true),
                    Cidade = table.Column<string>(type: "text", nullable: true),
                    Cep = table.Column<string>(type: "text", nullable: true),
                    Bairro = table.Column<string>(type: "text", nullable: true),
                    Logradouro = table.Column<string>(type: "text", nullable: true),
                    Numero = table.Column<string>(type: "text", nullable: true),
                    Complemento = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: false),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localidade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Motorista",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RG = table.Column<string>(type: "text", nullable: true),
                    CNH = table.Column<string>(type: "text", nullable: true),
                    CategoriaCNH = table.Column<int>(type: "integer", nullable: false),
                    ValidadeCNH = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: true),
                    CPF = table.Column<string>(type: "text", nullable: true),
                    Telefone = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Sexo = table.Column<int>(type: "integer", nullable: false),
                    LocalidadeId = table.Column<long>(type: "bigint", nullable: false),
                    LocalidadeEmbarqueId = table.Column<long>(type: "bigint", nullable: false),
                    LocalidadeDesembarqueId = table.Column<long>(type: "bigint", nullable: false),
                    Situacao = table.Column<bool>(type: "boolean", nullable: false),
                    Observacao = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motorista", x => x.Id);
                });

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
                    MOV_DESCRICAO_MODELO = table.Column<string>(type: "text", nullable: false),
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
                name: "Passageiro",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LocalidadeId = table.Column<long>(type: "bigint", nullable: false),
                    LocalidadeEmbarqueId = table.Column<long>(type: "bigint", nullable: true),
                    LocalidadeDesembarqueId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: true),
                    CPF = table.Column<string>(type: "text", nullable: true),
                    Telefone = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Sexo = table.Column<int>(type: "integer", nullable: false),
                    Situacao = table.Column<bool>(type: "boolean", nullable: false),
                    Observacao = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passageiro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Passageiro_Localidade_LocalidadeDesembarqueId",
                        column: x => x.LocalidadeDesembarqueId,
                        principalTable: "Localidade",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Passageiro_Localidade_LocalidadeEmbarqueId",
                        column: x => x.LocalidadeEmbarqueId,
                        principalTable: "Localidade",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Passageiro_Localidade_LocalidadeId",
                        column: x => x.LocalidadeId,
                        principalTable: "Localidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    GAV_HORARIO_SAIDA = table.Column<TimeSpan>(type: "interval", nullable: false),
                    GAV_HORARIO_CHEGADA = table.Column<TimeSpan>(type: "interval", nullable: false),
                    GAV_VALOR_PASSAGEM = table.Column<decimal>(type: "numeric", nullable: false),
                    GAV_QUANTIDADE_VAGAS = table.Column<int>(type: "integer", nullable: false),
                    GAV_DISTANCIA = table.Column<decimal>(type: "numeric", nullable: false),
                    GAV_DESCRICAO_VIAGEM = table.Column<string>(type: "text", nullable: true),
                    GAV_POLILINHA_ROTA = table.Column<string>(type: "text", nullable: true),
                    GAV_ATIVO = table.Column<bool>(type: "boolean", nullable: false),
                    GAV_DIAS_SEMANA = table.Column<string>(type: "text", nullable: true),
                    GAV_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GAV_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_GATILHO_VIAGEM", x => x.GAV_ID);
                    table.ForeignKey(
                        name: "FK_T_GATILHO_VIAGEM_Localidade_LOC_DESTINO_ID",
                        column: x => x.LOC_DESTINO_ID,
                        principalTable: "Localidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_GATILHO_VIAGEM_Localidade_LOC_ORIGEM_ID",
                        column: x => x.LOC_ORIGEM_ID,
                        principalTable: "Localidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_GATILHO_VIAGEM_T_VEICULO_VEI_ID",
                        column: x => x.VEI_ID,
                        principalTable: "T_VEICULO",
                        principalColumn: "VEI_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_VIAGEM",
                columns: table => new
                {
                    VIA_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VIA_CODIGO = table.Column<string>(type: "text", nullable: false),
                    VIA_DATA = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VIA_HORA_SAIDA = table.Column<TimeSpan>(type: "interval", nullable: false),
                    VIA_HORA_CHEGADA = table.Column<TimeSpan>(type: "interval", nullable: false),
                    VEI_ID = table.Column<long>(type: "bigint", nullable: false),
                    MOT_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOC_ORIGEM_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOC_DESTINO_ID = table.Column<long>(type: "bigint", nullable: false),
                    GAT_ID = table.Column<long>(type: "bigint", nullable: true),
                    VIA_VALOR_PASSAGEM = table.Column<decimal>(type: "numeric", nullable: false),
                    VIA_QUANTIDADE_VAGAS = table.Column<int>(type: "integer", nullable: false),
                    VIA_VAGAS_DISPONIVEIS = table.Column<int>(type: "integer", nullable: false),
                    VIA_DISTANCIA = table.Column<decimal>(type: "numeric", nullable: false),
                    VIA_NUMERO_PASSAGEIROS = table.Column<int>(type: "integer", nullable: false),
                    VIA_LOTADO = table.Column<bool>(type: "boolean", nullable: false),
                    VIA_EXCESSO = table.Column<bool>(type: "boolean", nullable: false),
                    VIA_MOTIVO_PROBLEMA = table.Column<string>(type: "text", nullable: false),
                    VIA_DESCRICAO = table.Column<string>(type: "text", nullable: false),
                    VIA_LATITUDE_FIM = table.Column<decimal>(type: "numeric", nullable: false),
                    VIA_LATITUDE_INICIO = table.Column<decimal>(type: "numeric", nullable: false),
                    VIA_DISTANCIA_REALIZADA = table.Column<decimal>(type: "numeric", nullable: false),
                    VIA_POLILINHA_ROTA = table.Column<string>(type: "text", nullable: false),
                    VIA_POLILINHA_ROTA_REALIZADA = table.Column<string>(type: "text", nullable: false),
                    VIA_SITUACAO = table.Column<int>(type: "integer", nullable: false),
                    VIA_MOTIVO_CANCELAMENTO = table.Column<string>(type: "text", nullable: false),
                    VIA_ATIVO = table.Column<bool>(type: "boolean", nullable: false),
                    VIA_DATA_INICIO = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VIA_DATA_FIM = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VIA_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VIA_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_VIAGEM", x => x.VIA_ID);
                    table.ForeignKey(
                        name: "FK_T_VIAGEM_Localidade_LOC_DESTINO_ID",
                        column: x => x.LOC_DESTINO_ID,
                        principalTable: "Localidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_VIAGEM_Localidade_LOC_ORIGEM_ID",
                        column: x => x.LOC_ORIGEM_ID,
                        principalTable: "Localidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_VIAGEM_Motorista_MOT_ID",
                        column: x => x.MOT_ID,
                        principalTable: "Motorista",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_VIAGEM_T_GATILHO_VIAGEM_GAT_ID",
                        column: x => x.GAT_ID,
                        principalTable: "T_GATILHO_VIAGEM",
                        principalColumn: "GAV_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_VIAGEM_T_VEICULO_VEI_ID",
                        column: x => x.VEI_ID,
                        principalTable: "T_VEICULO",
                        principalColumn: "VEI_ID",
                        onDelete: ReferentialAction.Restrict);
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
                        name: "FK_T_VIAGEM_PASSAGEIRO_Passageiro_PAS_PASSAGEIRO_ID",
                        column: x => x.PAS_PASSAGEIRO_ID,
                        principalTable: "Passageiro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_VIAGEM_PASSAGEIRO_T_VIAGEM_VIA_VIAGEM_ID",
                        column: x => x.VIA_VIAGEM_ID,
                        principalTable: "T_VIAGEM",
                        principalColumn: "VIA_ID",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_Passageiro_LocalidadeDesembarqueId",
                table: "Passageiro",
                column: "LocalidadeDesembarqueId");

            migrationBuilder.CreateIndex(
                name: "IX_Passageiro_LocalidadeEmbarqueId",
                table: "Passageiro",
                column: "LocalidadeEmbarqueId");

            migrationBuilder.CreateIndex(
                name: "IX_Passageiro_LocalidadeId",
                table: "Passageiro",
                column: "LocalidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_T_GATILHO_VIAGEM_GAV_CREATED_AT",
                table: "T_GATILHO_VIAGEM",
                column: "GAV_CREATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_GATILHO_VIAGEM_GAV_UPDATED_AT",
                table: "T_GATILHO_VIAGEM",
                column: "GAV_UPDATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_GATILHO_VIAGEM_LOC_DESTINO_ID",
                table: "T_GATILHO_VIAGEM",
                column: "LOC_DESTINO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_GATILHO_VIAGEM_LOC_ORIGEM_ID",
                table: "T_GATILHO_VIAGEM",
                column: "LOC_ORIGEM_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_GATILHO_VIAGEM_VEI_ID",
                table: "T_GATILHO_VIAGEM",
                column: "VEI_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_HISTORICO_OBJETO_HOB_CREATED_AT",
                table: "T_HISTORICO_OBJETO",
                column: "HOB_CREATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_HISTORICO_OBJETO_HOB_UPDATED_AT",
                table: "T_HISTORICO_OBJETO",
                column: "HOB_UPDATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_HISTORICO_PROPRIEDADE_HOB_ID",
                table: "T_HISTORICO_PROPRIEDADE",
                column: "HOB_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_HISTORICO_PROPRIEDADE_HPR_CREATED_AT",
                table: "T_HISTORICO_PROPRIEDADE",
                column: "HPR_CREATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_HISTORICO_PROPRIEDADE_HPR_UPDATED_AT",
                table: "T_HISTORICO_PROPRIEDADE",
                column: "HPR_UPDATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_MODELO_VEICULAR_MOV_CREATED_AT",
                table: "T_MODELO_VEICULAR",
                column: "MOV_CREATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_MODELO_VEICULAR_MOV_UPDATED_AT",
                table: "T_MODELO_VEICULAR",
                column: "MOV_UPDATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_VEICULO_MOV_ID",
                table: "T_VEICULO",
                column: "MOV_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VEICULO_VEI_CREATED_AT",
                table: "T_VEICULO",
                column: "VEI_CREATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_VEICULO_VEI_UPDATED_AT",
                table: "T_VEICULO",
                column: "VEI_UPDATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_GAT_ID",
                table: "T_VIAGEM",
                column: "GAT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_LOC_DESTINO_ID",
                table: "T_VIAGEM",
                column: "LOC_DESTINO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_LOC_ORIGEM_ID",
                table: "T_VIAGEM",
                column: "LOC_ORIGEM_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_MOT_ID",
                table: "T_VIAGEM",
                column: "MOT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_VEI_ID",
                table: "T_VIAGEM",
                column: "VEI_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_VIA_CREATED_AT",
                table: "T_VIAGEM",
                column: "VIA_CREATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_VIA_UPDATED_AT",
                table: "T_VIAGEM",
                column: "VIA_UPDATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_PASSAGEIRO_PAS_PASSAGEIRO_ID",
                table: "T_VIAGEM_PASSAGEIRO",
                column: "PAS_PASSAGEIRO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_PASSAGEIRO_VIA_VIAGEM_ID",
                table: "T_VIAGEM_PASSAGEIRO",
                column: "VIA_VIAGEM_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_PASSAGEIRO_VIP_CREATED_AT",
                table: "T_VIAGEM_PASSAGEIRO",
                column: "VIP_CREATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_PASSAGEIRO_VIP_UPDATED_AT",
                table: "T_VIAGEM_PASSAGEIRO",
                column: "VIP_UPDATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_POSICAO_VIA_ID",
                table: "T_VIAGEM_POSICAO",
                column: "VIA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_POSICAO_VPO_CREATED_AT",
                table: "T_VIAGEM_POSICAO",
                column: "VPO_CREATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_POSICAO_VPO_UPDATED_AT",
                table: "T_VIAGEM_POSICAO",
                column: "VPO_UPDATED_AT");
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
                name: "Passageiro");

            migrationBuilder.DropTable(
                name: "T_VIAGEM");

            migrationBuilder.DropTable(
                name: "Motorista");

            migrationBuilder.DropTable(
                name: "T_GATILHO_VIAGEM");

            migrationBuilder.DropTable(
                name: "Localidade");

            migrationBuilder.DropTable(
                name: "T_VEICULO");

            migrationBuilder.DropTable(
                name: "T_MODELO_VEICULAR");
        }
    }
}
