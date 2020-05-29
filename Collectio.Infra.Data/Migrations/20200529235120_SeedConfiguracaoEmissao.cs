using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Collectio.Infra.Data.Migrations
{
    public partial class SeedConfiguracaoEmissao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CartaoCredito",
                keyColumn: "Id",
                keyValue: new Guid("658b8bd5-de12-4c94-9bc9-c81bd1c3ac89"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAtualizacao",
                table: "Transacao",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAtualizacao",
                table: "Pagamento",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAtualizacao",
                table: "ConfiguracaoEmissao",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAtualizacao",
                table: "Cobranca",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAtualizacao",
                table: "ClienteCobranca",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAtualizacao",
                table: "Cliente",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAtualizacao",
                table: "CartaoCredito",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "CartaoCredito",
                columns: new[] { "Id", "CpfCnpjProprietario", "DataAtualizacao", "DataCriacao", "Nome", "Numero", "OwnerId", "Status_MensagemErro", "Status_Status" },
                values: new object[] { new Guid("5dd3e230-85af-4b98-aa11-14da9026e6be"), "71536108049", null, new DateTime(2020, 5, 29, 0, 0, 0, 0, DateTimeKind.Local), "Proprietario C. Credito", "5211********2071", new Guid("ef2ccc70-c4f5-4f32-bb24-f9316013901c"), null, 1 });

            migrationBuilder.InsertData(
                table: "ConfiguracaoEmissao",
                columns: new[] { "Id", "CpfCnpj", "DataAtualizacao", "DataCriacao", "Email", "NomeEmpresa", "OwnerId", "AgenciaConta_Agencia", "AgenciaConta_Conta", "Telefone_Ddd", "Telefone_Numero", "Status_MensagemErro", "Status_Status" },
                values: new object[] { new Guid("5a18db9b-90bb-42a6-9bb2-f3602a90dbb4"), "71536108049", null, new DateTime(2020, 5, 29, 0, 0, 0, 0, DateTimeKind.Local), "email@email.com", "Empresa Teste", new Guid("ef2ccc70-c4f5-4f32-bb24-f9316013901c"), "3238", "52841", "85", "996422022", null, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CartaoCredito",
                keyColumn: "Id",
                keyValue: new Guid("5dd3e230-85af-4b98-aa11-14da9026e6be"));

            migrationBuilder.DeleteData(
                table: "ConfiguracaoEmissao",
                keyColumn: "Id",
                keyValue: new Guid("5a18db9b-90bb-42a6-9bb2-f3602a90dbb4"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAtualizacao",
                table: "Transacao",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAtualizacao",
                table: "Pagamento",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAtualizacao",
                table: "ConfiguracaoEmissao",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAtualizacao",
                table: "Cobranca",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAtualizacao",
                table: "ClienteCobranca",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAtualizacao",
                table: "Cliente",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAtualizacao",
                table: "CartaoCredito",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "CartaoCredito",
                columns: new[] { "Id", "CpfCnpjProprietario", "DataAtualizacao", "DataCriacao", "Nome", "Numero", "OwnerId", "Status_MensagemErro", "Status_Status" },
                values: new object[] { new Guid("658b8bd5-de12-4c94-9bc9-c81bd1c3ac89"), "71536108049", new DateTime(2020, 5, 29, 18, 56, 12, 386, DateTimeKind.Local).AddTicks(230), new DateTime(2020, 5, 29, 18, 56, 12, 385, DateTimeKind.Local).AddTicks(1093), "Proprietario C. Credito", "5211********2071", new Guid("ef2ccc70-c4f5-4f32-bb24-f9316013901c"), null, 1 });
        }
    }
}
