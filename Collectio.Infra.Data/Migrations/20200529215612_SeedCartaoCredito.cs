using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Collectio.Infra.Data.Migrations
{
    public partial class SeedCartaoCredito : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Numero",
                table: "CartaoCredito",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "CartaoCredito",
                columns: new[] { "Id", "CpfCnpjProprietario", "DataAtualizacao", "DataCriacao", "Nome", "Numero", "OwnerId", "Status_MensagemErro", "Status_Status" },
                values: new object[] { new Guid("658b8bd5-de12-4c94-9bc9-c81bd1c3ac89"), "71536108049", new DateTime(2020, 5, 29, 18, 56, 12, 386, DateTimeKind.Local).AddTicks(230), new DateTime(2020, 5, 29, 18, 56, 12, 385, DateTimeKind.Local).AddTicks(1093), "Proprietario C. Credito", "5211********2071", new Guid("ef2ccc70-c4f5-4f32-bb24-f9316013901c"), null, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CartaoCredito",
                keyColumn: "Id",
                keyValue: new Guid("658b8bd5-de12-4c94-9bc9-c81bd1c3ac89"));

            migrationBuilder.AlterColumn<string>(
                name: "Numero",
                table: "CartaoCredito",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
