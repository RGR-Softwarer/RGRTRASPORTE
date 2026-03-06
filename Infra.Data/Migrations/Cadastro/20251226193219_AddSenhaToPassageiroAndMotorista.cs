using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Migrations.Cadastro
{
    /// <inheritdoc />
    public partial class AddSenhaToPassageiroAndMotorista : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PAS_SENHA",
                table: "T_PASSAGEIRO",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MOT_SENHA",
                table: "T_MOTORISTA",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PAS_SENHA",
                table: "T_PASSAGEIRO");

            migrationBuilder.DropColumn(
                name: "MOT_SENHA",
                table: "T_MOTORISTA");
        }
    }
}
