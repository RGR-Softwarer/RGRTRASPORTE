using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Data.Migrations.Transportador
{
    /// <inheritdoc />
    public partial class T000001_Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    MOV_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MOV_CREATED_BY = table.Column<string>(type: "text", nullable: false),
                    MOV_UPDATED_BY = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MODELO_VEICULAR", x => x.MOV_ID);
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
                    VEI_COR = table.Column<string>(type: "text", nullable: false),
                    VEI_RENAVAM = table.Column<string>(type: "text", nullable: false),
                    VEI_VENCIMENTO_LICENCIAMENTO = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VEI_TIPO_COMBUSTIVEL = table.Column<int>(type: "integer", nullable: false),
                    VEI_STATUS = table.Column<int>(type: "integer", nullable: false),
                    VEI_OBSERVACAO = table.Column<string>(type: "text", nullable: false),
                    MOV_ID = table.Column<long>(type: "bigint", nullable: true),
                    VEI_SITUACAO = table.Column<int>(type: "integer", nullable: false),
                    VEI_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VEI_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VEI_CREATED_BY = table.Column<string>(type: "text", nullable: false),
                    VEI_UPDATED_BY = table.Column<string>(type: "text", nullable: false)
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
                    MOT_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOC_ORIGEM_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOC_DESTINO_ID = table.Column<long>(type: "bigint", nullable: false),
                    GAV_HORARIO_SAIDA = table.Column<TimeSpan>(type: "interval", nullable: false),
                    GAV_HORARIO_CHEGADA = table.Column<TimeSpan>(type: "interval", nullable: false),
                    GAV_VALOR_PASSAGEM = table.Column<decimal>(type: "numeric", nullable: false),
                    GAV_QUANTIDADE_VAGAS = table.Column<int>(type: "integer", nullable: false),
                    GAV_DISTANCIA = table.Column<decimal>(type: "numeric", nullable: false),
                    GAV_DESCRICAO_VIAGEM = table.Column<string>(type: "text", nullable: false),
                    GAV_POLILINHA_ROTA = table.Column<string>(type: "text", nullable: false),
                    GAV_ATIVO = table.Column<bool>(type: "boolean", nullable: false),
                    GAV_DIAS_SEMANA = table.Column<string>(type: "text", nullable: false),
                    GAV_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GAV_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GAV_CREATED_BY = table.Column<string>(type: "text", nullable: false),
                    GAV_UPDATED_BY = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_GATILHO_VIAGEM", x => x.GAV_ID);
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
                    VIA_DISTANCIA = table.Column<decimal>(type: "numeric", nullable: false),
                    VIA_POLILINHA_ROTA = table.Column<string>(type: "text", nullable: false),
                    VEI_ID = table.Column<long>(type: "bigint", nullable: false),
                    MOT_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOC_ORIGEM_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOC_DESTINO_ID = table.Column<long>(type: "bigint", nullable: false),
                    VIA_SITUACAO = table.Column<int>(type: "integer", nullable: false),
                    VIA_LOTADO = table.Column<bool>(type: "boolean", nullable: false),
                    VIA_DATA_INICIO = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VIA_DATA_FIM = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VIA_QUANTIDADE_VAGAS = table.Column<int>(type: "integer", nullable: false),
                    VIA_VAGAS_DISPONIVEIS = table.Column<int>(type: "integer", nullable: false),
                    VIA_DESCRICAO = table.Column<string>(type: "text", nullable: false),
                    VIA_ATIVO = table.Column<bool>(type: "boolean", nullable: false),
                    GAV_ID = table.Column<long>(type: "bigint", nullable: true),
                    VIA_DATA = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VIA_HORA_CHEGADA = table.Column<TimeSpan>(type: "interval", nullable: false),
                    VIA_HORA_SAIDA = table.Column<TimeSpan>(type: "interval", nullable: false),
                    VIA_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VIA_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VIA_CREATED_BY = table.Column<string>(type: "text", nullable: false),
                    VIA_UPDATED_BY = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_VIAGEM", x => x.VIA_ID);
                    table.ForeignKey(
                        name: "FK_T_VIAGEM_T_GATILHO_VIAGEM_GAV_ID",
                        column: x => x.GAV_ID,
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
                    VIA_ID = table.Column<long>(type: "bigint", nullable: false),
                    PAS_ID = table.Column<long>(type: "bigint", nullable: false),
                    VIP_DATA_RESERVA = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VIP_CONFIRMADO = table.Column<bool>(type: "boolean", nullable: false),
                    VIP_DATA_CONFIRMACAO = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VIP_OBSERVACAO = table.Column<string>(type: "text", nullable: true),
                    VIP_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VIP_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VIP_CREATED_BY = table.Column<string>(type: "text", nullable: false),
                    VIP_UPDATED_BY = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_VIAGEM_PASSAGEIRO", x => x.VIP_ID);
                    table.ForeignKey(
                        name: "FK_T_VIAGEM_PASSAGEIRO_T_VIAGEM_VIA_ID",
                        column: x => x.VIA_ID,
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
                    VPO_LATITUDE = table.Column<decimal>(type: "numeric", nullable: false),
                    VPO_LONGITUDE = table.Column<decimal>(type: "numeric", nullable: false),
                    VPO_DATA_HORA = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VPO_CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VPO_UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VPO_CREATED_BY = table.Column<string>(type: "text", nullable: false),
                    VPO_UPDATED_BY = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_VIAGEM_POSICAO", x => x.VPO_ID);
                    table.ForeignKey(
                        name: "FK_T_VIAGEM_POSICAO_T_VIAGEM_VIA_ID",
                        column: x => x.VIA_ID,
                        principalTable: "T_VIAGEM",
                        principalColumn: "VIA_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_GATILHO_VIAGEM_GAV_CREATED_AT",
                table: "T_GATILHO_VIAGEM",
                column: "GAV_CREATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_GATILHO_VIAGEM_GAV_CREATED_BY",
                table: "T_GATILHO_VIAGEM",
                column: "GAV_CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_T_GATILHO_VIAGEM_GAV_UPDATED_AT",
                table: "T_GATILHO_VIAGEM",
                column: "GAV_UPDATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_GATILHO_VIAGEM_GAV_UPDATED_BY",
                table: "T_GATILHO_VIAGEM",
                column: "GAV_UPDATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_T_GATILHO_VIAGEM_VEI_ID",
                table: "T_GATILHO_VIAGEM",
                column: "VEI_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_MODELO_VEICULAR_MOV_CREATED_AT",
                table: "T_MODELO_VEICULAR",
                column: "MOV_CREATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_MODELO_VEICULAR_MOV_CREATED_BY",
                table: "T_MODELO_VEICULAR",
                column: "MOV_CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_T_MODELO_VEICULAR_MOV_UPDATED_AT",
                table: "T_MODELO_VEICULAR",
                column: "MOV_UPDATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_MODELO_VEICULAR_MOV_UPDATED_BY",
                table: "T_MODELO_VEICULAR",
                column: "MOV_UPDATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_T_VEICULO_MOV_ID",
                table: "T_VEICULO",
                column: "MOV_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VEICULO_VEI_CREATED_AT",
                table: "T_VEICULO",
                column: "VEI_CREATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_VEICULO_VEI_CREATED_BY",
                table: "T_VEICULO",
                column: "VEI_CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_T_VEICULO_VEI_UPDATED_AT",
                table: "T_VEICULO",
                column: "VEI_UPDATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_VEICULO_VEI_UPDATED_BY",
                table: "T_VEICULO",
                column: "VEI_UPDATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_GAV_ID",
                table: "T_VIAGEM",
                column: "GAV_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_VEI_ID",
                table: "T_VIAGEM",
                column: "VEI_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_VIA_CREATED_AT",
                table: "T_VIAGEM",
                column: "VIA_CREATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_VIA_CREATED_BY",
                table: "T_VIAGEM",
                column: "VIA_CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_VIA_UPDATED_AT",
                table: "T_VIAGEM",
                column: "VIA_UPDATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_VIA_UPDATED_BY",
                table: "T_VIAGEM",
                column: "VIA_UPDATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_PASSAGEIRO_VIA_ID",
                table: "T_VIAGEM_PASSAGEIRO",
                column: "VIA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_PASSAGEIRO_VIP_CREATED_AT",
                table: "T_VIAGEM_PASSAGEIRO",
                column: "VIP_CREATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_PASSAGEIRO_VIP_CREATED_BY",
                table: "T_VIAGEM_PASSAGEIRO",
                column: "VIP_CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_PASSAGEIRO_VIP_UPDATED_AT",
                table: "T_VIAGEM_PASSAGEIRO",
                column: "VIP_UPDATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_PASSAGEIRO_VIP_UPDATED_BY",
                table: "T_VIAGEM_PASSAGEIRO",
                column: "VIP_UPDATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_POSICAO_VIA_ID",
                table: "T_VIAGEM_POSICAO",
                column: "VIA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_POSICAO_VPO_CREATED_AT",
                table: "T_VIAGEM_POSICAO",
                column: "VPO_CREATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_POSICAO_VPO_CREATED_BY",
                table: "T_VIAGEM_POSICAO",
                column: "VPO_CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_POSICAO_VPO_UPDATED_AT",
                table: "T_VIAGEM_POSICAO",
                column: "VPO_UPDATED_AT");

            migrationBuilder.CreateIndex(
                name: "IX_T_VIAGEM_POSICAO_VPO_UPDATED_BY",
                table: "T_VIAGEM_POSICAO",
                column: "VPO_UPDATED_BY");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_VIAGEM_PASSAGEIRO");

            migrationBuilder.DropTable(
                name: "T_VIAGEM_POSICAO");

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
