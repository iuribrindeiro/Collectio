using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Collectio.Infra.Data.Migrations
{
    public partial class InitialMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DataCriacao = table.Column<DateTime>(nullable: false),
                    DataAtualizacao = table.Column<DateTime>(nullable: false),
                    OwnerId = table.Column<Guid>(nullable: false),
                    Nome = table.Column<string>(nullable: false),
                    CpfCnpj = table.Column<string>(nullable: true),
                    CartaoCreditoPadraoId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cobranca",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DataCriacao = table.Column<DateTime>(nullable: false),
                    DataAtualizacao = table.Column<DateTime>(nullable: false),
                    OwnerId = table.Column<Guid>(nullable: false),
                    Descricao = table.Column<string>(nullable: false),
                    Valor = table.Column<decimal>(nullable: false),
                    Vencimento = table.Column<DateTime>(nullable: false),
                    Transacao_FormaPagamento = table.Column<int>(nullable: true),
                    Transacao_Status = table.Column<int>(nullable: true),
                    ConfiguracaoEmissaoId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cobranca", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClienteCobranca",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DataCriacao = table.Column<DateTime>(nullable: false),
                    DataAtualizacao = table.Column<DateTime>(nullable: false),
                    OwnerId = table.Column<Guid>(nullable: false),
                    Nome = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    CpfCnpj = table.Column<string>(nullable: false),
                    Endereco_Rua = table.Column<string>(nullable: true),
                    Endereco_Numero = table.Column<string>(nullable: true),
                    Endereco_Bairro = table.Column<string>(nullable: true),
                    Endereco_Cep = table.Column<string>(nullable: true),
                    Endereco_Cidade = table.Column<string>(nullable: true),
                    Endereco_Uf = table.Column<string>(nullable: true),
                    CartaoCredito_Numero = table.Column<string>(nullable: true),
                    CartaoCredito_Nome = table.Column<string>(nullable: true),
                    CartaoCredito_TenantId = table.Column<string>(nullable: true),
                    Telefone_Ddd = table.Column<string>(nullable: true),
                    Telefone_Numero = table.Column<string>(nullable: true),
                    TenantId = table.Column<string>(nullable: false),
                    CobrancaId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClienteCobranca", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClienteCobranca_Cobranca_CobrancaId",
                        column: x => x.CobrancaId,
                        principalTable: "Cobranca",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pagamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DataCriacao = table.Column<DateTime>(nullable: false),
                    DataAtualizacao = table.Column<DateTime>(nullable: false),
                    OwnerId = table.Column<Guid>(nullable: false),
                    CobrancaId = table.Column<Guid>(nullable: false),
                    Valor = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagamento_Cobranca_CobrancaId",
                        column: x => x.CobrancaId,
                        principalTable: "Cobranca",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_CartaoCreditoPadraoId",
                table: "Cliente",
                column: "CartaoCreditoPadraoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_OwnerId",
                table: "Cliente",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ClienteCobranca_CobrancaId",
                table: "ClienteCobranca",
                column: "CobrancaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClienteCobranca_OwnerId",
                table: "ClienteCobranca",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ClienteCobranca_TenantId",
                table: "ClienteCobranca",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Cobranca_ConfiguracaoEmissaoId",
                table: "Cobranca",
                column: "ConfiguracaoEmissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cobranca_OwnerId",
                table: "Cobranca",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamento_CobrancaId",
                table: "Pagamento",
                column: "CobrancaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pagamento_OwnerId",
                table: "Pagamento",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "ClienteCobranca");

            migrationBuilder.DropTable(
                name: "Pagamento");

            migrationBuilder.DropTable(
                name: "Cobranca");
        }
    }
}
