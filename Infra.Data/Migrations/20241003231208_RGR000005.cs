using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class RGR000005 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoriaCNH",
                table: "Veiculo");

            migrationBuilder.DropColumn(
                name: "TipoVeiculo",
                table: "Veiculo");

            migrationBuilder.CreateTable(
                name: "Localidade",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Cep = table.Column<string>(type: "text", nullable: false),
                    Uf = table.Column<string>(type: "text", nullable: false),
                    Cidade = table.Column<string>(type: "text", nullable: false),
                    Bairro = table.Column<string>(type: "text", nullable: false),
                    Logradouro = table.Column<string>(type: "text", nullable: false)
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
                    CNH = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motorista", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Passageiro",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    CPF = table.Column<string>(type: "text", nullable: false),
                    Telefone = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Sexo = table.Column<int>(type: "integer", nullable: false),
                    LocalidadeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passageiro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Passageiro_Localidade_LocalidadeId",
                        column: x => x.LocalidadeId,
                        principalTable: "Localidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GatilhoViagem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    VeiculoId = table.Column<long>(type: "bigint", nullable: false),
                    MotoristaId = table.Column<long>(type: "bigint", nullable: false),
                    OrigemId = table.Column<long>(type: "bigint", nullable: false),
                    DestinoId = table.Column<long>(type: "bigint", nullable: false),
                    HorarioSaida = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HorarioChegada = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Distancia = table.Column<decimal>(type: "numeric", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GatilhoViagem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GatilhoViagem_Localidade_DestinoId",
                        column: x => x.DestinoId,
                        principalTable: "Localidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GatilhoViagem_Localidade_OrigemId",
                        column: x => x.OrigemId,
                        principalTable: "Localidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GatilhoViagem_Motorista_MotoristaId",
                        column: x => x.MotoristaId,
                        principalTable: "Motorista",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GatilhoViagem_Veiculo_VeiculoId",
                        column: x => x.VeiculoId,
                        principalTable: "Veiculo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Viagem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoViagem = table.Column<string>(type: "text", nullable: false),
                    DataViagem = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VeiculoId = table.Column<long>(type: "bigint", nullable: false),
                    MotoristaId = table.Column<long>(type: "bigint", nullable: false),
                    OrigemId = table.Column<long>(type: "bigint", nullable: false),
                    DestinoId = table.Column<long>(type: "bigint", nullable: false),
                    GatinhoViagemId = table.Column<long>(type: "bigint", nullable: true),
                    HorarioSaida = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HorarioChegada = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Situacao = table.Column<int>(type: "integer", nullable: false),
                    Distancia = table.Column<decimal>(type: "numeric", nullable: false),
                    NumeroPassageiros = table.Column<int>(type: "integer", nullable: false),
                    Lotado = table.Column<bool>(type: "boolean", nullable: false),
                    Excesso = table.Column<bool>(type: "boolean", nullable: false),
                    GatilhoViagemId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Viagem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Viagem_GatilhoViagem_GatilhoViagemId",
                        column: x => x.GatilhoViagemId,
                        principalTable: "GatilhoViagem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Viagem_GatilhoViagem_GatinhoViagemId",
                        column: x => x.GatinhoViagemId,
                        principalTable: "GatilhoViagem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Viagem_Localidade_DestinoId",
                        column: x => x.DestinoId,
                        principalTable: "Localidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Viagem_Localidade_OrigemId",
                        column: x => x.OrigemId,
                        principalTable: "Localidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Viagem_Motorista_MotoristaId",
                        column: x => x.MotoristaId,
                        principalTable: "Motorista",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Viagem_Veiculo_VeiculoId",
                        column: x => x.VeiculoId,
                        principalTable: "Veiculo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ViagemPassageiro",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ViagemId = table.Column<long>(type: "bigint", nullable: false),
                    PassageiroId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViagemPassageiro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ViagemPassageiro_Passageiro_PassageiroId",
                        column: x => x.PassageiroId,
                        principalTable: "Passageiro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ViagemPassageiro_Viagem_ViagemId",
                        column: x => x.ViagemId,
                        principalTable: "Viagem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ViagemPosicao",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ViagemId = table.Column<long>(type: "bigint", nullable: false),
                    DataHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Latitude = table.Column<string>(type: "text", nullable: false),
                    Longitude = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViagemPosicao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ViagemPosicao_Viagem_ViagemId",
                        column: x => x.ViagemId,
                        principalTable: "Viagem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GatilhoViagem_DestinoId",
                table: "GatilhoViagem",
                column: "DestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_GatilhoViagem_MotoristaId",
                table: "GatilhoViagem",
                column: "MotoristaId");

            migrationBuilder.CreateIndex(
                name: "IX_GatilhoViagem_OrigemId",
                table: "GatilhoViagem",
                column: "OrigemId");

            migrationBuilder.CreateIndex(
                name: "IX_GatilhoViagem_VeiculoId",
                table: "GatilhoViagem",
                column: "VeiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_Passageiro_LocalidadeId",
                table: "Passageiro",
                column: "LocalidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Viagem_DestinoId",
                table: "Viagem",
                column: "DestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_Viagem_GatilhoViagemId",
                table: "Viagem",
                column: "GatilhoViagemId");

            migrationBuilder.CreateIndex(
                name: "IX_Viagem_GatinhoViagemId",
                table: "Viagem",
                column: "GatinhoViagemId");

            migrationBuilder.CreateIndex(
                name: "IX_Viagem_MotoristaId",
                table: "Viagem",
                column: "MotoristaId");

            migrationBuilder.CreateIndex(
                name: "IX_Viagem_OrigemId",
                table: "Viagem",
                column: "OrigemId");

            migrationBuilder.CreateIndex(
                name: "IX_Viagem_VeiculoId",
                table: "Viagem",
                column: "VeiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_ViagemPassageiro_PassageiroId",
                table: "ViagemPassageiro",
                column: "PassageiroId");

            migrationBuilder.CreateIndex(
                name: "IX_ViagemPassageiro_ViagemId",
                table: "ViagemPassageiro",
                column: "ViagemId");

            migrationBuilder.CreateIndex(
                name: "IX_ViagemPosicao_ViagemId",
                table: "ViagemPosicao",
                column: "ViagemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ViagemPassageiro");

            migrationBuilder.DropTable(
                name: "ViagemPosicao");

            migrationBuilder.DropTable(
                name: "Passageiro");

            migrationBuilder.DropTable(
                name: "Viagem");

            migrationBuilder.DropTable(
                name: "GatilhoViagem");

            migrationBuilder.DropTable(
                name: "Localidade");

            migrationBuilder.DropTable(
                name: "Motorista");

            migrationBuilder.AddColumn<int>(
                name: "CategoriaCNH",
                table: "Veiculo",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TipoVeiculo",
                table: "Veiculo",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
