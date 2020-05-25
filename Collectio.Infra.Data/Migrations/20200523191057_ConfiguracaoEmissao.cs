using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Collectio.Infra.Data.Migrations
{
    public partial class ConfiguracaoEmissao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfiguracaoEmissao",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DataCriacao = table.Column<DateTime>(nullable: false),
                    DataAtualizacao = table.Column<DateTime>(nullable: false),
                    OwnerId = table.Column<Guid>(nullable: false),
                    NomeEmpresa = table.Column<string>(nullable: false),
                    CpfCnpj = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Telefone_Ddd = table.Column<string>(nullable: true),
                    Telefone_Numero = table.Column<string>(nullable: true),
                    AgenciaConta_Conta = table.Column<string>(nullable: true),
                    AgenciaConta_Agencia = table.Column<string>(nullable: true),
                    Status_Status = table.Column<int>(nullable: true),
                    Status_MensagemErro = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracaoEmissao", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracaoEmissao_OwnerId",
                table: "ConfiguracaoEmissao",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracaoEmissao");
        }
    }
}
