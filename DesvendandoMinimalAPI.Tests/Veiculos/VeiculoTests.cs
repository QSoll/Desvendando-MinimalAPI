using Xunit;
using DesvendandoMinimalAPI.Domain;
using DesvendandoMinimalAPI.Veiculos;
using System.Collections.Generic;


namespace DesvendandoMinimalAPI.Tests.Veiculos
{
    public class VeiculoTests
    {
        [Fact]
        public void DeveValidarVeiculoValido()
        {
            var veiculo = new Veiculo
            {
                Modelo = "Civic",
                Placa = "ABC1234",
                Ano = 2022,
                Cor = "Prata"
            };

            var erros = VeiculoValidator.Validar(veiculo);
            Assert.Empty(erros);
        }

        [Fact]
        public void DeveDetectarErrosEmVeiculoInvalido()
        {
            var veiculo = new Veiculo
            {
                Modelo = "",
                Placa = "",
                Ano = 1800,
                Cor = "Verde"
            };

            var erros = VeiculoValidator.Validar(veiculo);
            Assert.Contains("Modelo é obrigatório.", erros);
            Assert.Contains("Placa é obrigatória.", erros);
            Assert.Contains("Ano inválido.", erros);
        }
    }
}
