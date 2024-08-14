using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class RGR000002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Ano",
                table: "Veiculo",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Veiculo",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateTable(
                name: "HistoricoObjeto",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Objeto = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CodigoObjeto = table.Column<long>(type: "bigint", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Acao = table.Column<int>(type: "integer", nullable: false),
                    TipoAuditado = table.Column<int>(type: "integer", nullable: false),
                    OrigemAuditado = table.Column<int>(type: "integer", nullable: false),
                    DescricaoAcao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Data = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IP = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoObjeto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HistoricoPropriedade",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Propriedade = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    De = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Para = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    HistoricoObjetoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoPropriedade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoPropriedade_HistoricoObjeto_HistoricoObjetoId",
                        column: x => x.HistoricoObjetoId,
                        principalTable: "HistoricoObjeto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoPropriedade_HistoricoObjetoId",
                table: "HistoricoPropriedade",
                column: "HistoricoObjetoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricoPropriedade");

            migrationBuilder.DropTable(
                name: "HistoricoObjeto");

            migrationBuilder.AlterColumn<int>(
                name: "Ano",
                table: "Veiculo",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Veiculo",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
