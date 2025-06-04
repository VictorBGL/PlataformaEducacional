using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlataformaEducacional.Financeiro.Data.Migrations
{
    /// <inheritdoc />
    public partial class PrimeiroMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pagamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AlunoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CursoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Valor = table.Column<decimal>(type: "TEXT", nullable: false),
                    Data = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DadosCartao_NumeroMascarado = table.Column<string>(type: "varchar(100)", nullable: false),
                    DadosCartao_NomeTitular = table.Column<string>(type: "varchar(100)", nullable: false),
                    DadosCartao_Validade = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DadosCartao_CvvCartao = table.Column<string>(type: "varchar(100)", nullable: false),
                    Status_Status = table.Column<string>(type: "varchar(100)", nullable: false),
                    Status_MotivoRejeicao = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamento", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pagamento");
        }
    }
}
