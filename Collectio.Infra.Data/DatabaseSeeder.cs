using Collectio.Domain.CartaoCreditoAggregate;
using Collectio.Domain.ConfiguracaoEmissaoAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Collectio.Domain.Base.ValueObjects;

namespace Collectio.Infra.Data
{
    public static class DatabaseSeeder
    {
        private static Guid OwnerId = Guid.Parse("ef2ccc70-c4f5-4f32-bb24-f9316013901c");
        private static Guid ConfiguracaoEmissaoId = Guid.Parse("5a18db9b-90bb-42a6-9bb2-f3602a90dbb4");
        private static Guid CartaoCreditoId = Guid.Parse("5dd3e230-85af-4b98-aa11-14da9026e6be");
        private static DateTime DataCriacao = DateTime.Today;

        public static void Seed(this EntityTypeBuilder<CartaoCredito> cartaoCreditoBuilder)
        {
            cartaoCreditoBuilder.HasData(new
            {
                Id = CartaoCreditoId,
                CpfCnpjProprietario = "71536108049",
                Numero = "5211********2071",
                Nome = "Proprietario C. Credito",
                DataCriacao,
                OwnerId
            });
        }

        public static void Seed(this EntityTypeBuilder<ConfiguracaoEmissao> configuracaoEmissaoBuilder)
        {
            configuracaoEmissaoBuilder.HasData(new
            {
                Id = ConfiguracaoEmissaoId,
                NomeEmpresa = "Empresa Teste",
                CpfCnpj = "71536108049",
                Email = "email@email.com",
                DataCriacao,
                OwnerId
            });
        }

        public static void Seed(this OwnedNavigationBuilder<ConfiguracaoEmissao, AgenciaConta> agenciaContaBuilder)
        {
            agenciaContaBuilder.HasData(new
            {
                Agencia = "3238",
                Conta = "52841",
                ConfiguracaoEmissaoId
            });
        }

        public static void Seed(this OwnedNavigationBuilder<ConfiguracaoEmissao, StatusConfiguracaoEmissaoValueObject> statusConfiguracaoEmissaoBuilder)
        {
            statusConfiguracaoEmissaoBuilder.HasData(new
            {
                Status = StatusConfiguracaoEmissao.Processado,
                ConfiguracaoEmissaoId
            });
        }

        public static void Seed(this OwnedNavigationBuilder<ConfiguracaoEmissao, Telefone> telefoneBuilder)
        {
            telefoneBuilder.HasData(new
            {
                Ddd = "85",
                Numero = "996422022",
                ConfiguracaoEmissaoId
            });
        }

        public static void Seed(this OwnedNavigationBuilder<CartaoCredito, StatusCartaoValueObject> statusCartaoCreditoBuilder)
        {
            statusCartaoCreditoBuilder.HasData(new
            {
                Status = StatusCartao.Processado,
                CartaoCreditoId
            });
        }
    }
}