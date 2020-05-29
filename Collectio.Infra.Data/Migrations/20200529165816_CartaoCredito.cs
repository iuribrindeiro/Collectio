using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Collectio.Infra.Data.Migrations
{
    public partial class CartaoCredito : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CartaoCredito_TenantId",
                table: "ClienteCobranca",
                newName: "CartaoCreditoCobranca_TenantId");

            migrationBuilder.RenameColumn(
                name: "CartaoCredito_Numero",
                table: "ClienteCobranca",
                newName: "CartaoCreditoCobranca_Numero");

            migrationBuilder.RenameColumn(
                name: "CartaoCredito_Nome",
                table: "ClienteCobranca",
                newName: "CartaoCreditoCobranca_Nome");

            migrationBuilder.CreateTable(
                name: "CartaoCredito",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DataCriacao = table.Column<DateTime>(nullable: false),
                    DataAtualizacao = table.Column<DateTime>(nullable: false),
                    OwnerId = table.Column<Guid>(nullable: false),
                    CpfCnpjProprietario = table.Column<string>(nullable: false),
                    Status_Status = table.Column<int>(nullable: true),
                    Status_MensagemErro = table.Column<string>(nullable: true),
                    Numero = table.Column<string>(nullable: true),
                    Nome = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartaoCredito", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transacao",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DataCriacao = table.Column<DateTime>(nullable: false),
                    DataAtualizacao = table.Column<DateTime>(nullable: false),
                    OwnerId = table.Column<Guid>(nullable: false),
                    CobrancaId = table.Column<string>(nullable: false),
                    Valor = table.Column<decimal>(nullable: false),
                    CartaoId = table.Column<Guid>(nullable: false),
                    Status_MensagemErro = table.Column<string>(nullable: true),
                    Status_Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transacao_CartaoCredito_CartaoId",
                        column: x => x.CartaoId,
                        principalTable: "CartaoCredito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartaoCredito_CpfCnpjProprietario",
                table: "CartaoCredito",
                column: "CpfCnpjProprietario");

            migrationBuilder.CreateIndex(
                name: "IX_CartaoCredito_OwnerId",
                table: "CartaoCredito",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacao_CartaoId",
                table: "Transacao",
                column: "CartaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacao_CobrancaId",
                table: "Transacao",
                column: "CobrancaId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacao_OwnerId",
                table: "Transacao",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transacao");

            migrationBuilder.DropTable(
                name: "CartaoCredito");

            migrationBuilder.RenameColumn(
                name: "CartaoCreditoCobranca_TenantId",
                table: "ClienteCobranca",
                newName: "CartaoCredito_TenantId");

            migrationBuilder.RenameColumn(
                name: "CartaoCreditoCobranca_Numero",
                table: "ClienteCobranca",
                newName: "CartaoCredito_Numero");

            migrationBuilder.RenameColumn(
                name: "CartaoCreditoCobranca_Nome",
                table: "ClienteCobranca",
                newName: "CartaoCredito_Nome");
        }
    }
}
