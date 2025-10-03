using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using DesvendandoMinimalAPI.Veiculos;
using DesvendandoMinimalAPI.Domain;



namespace DesvendandoMinimalAPI.Veiculos
{

    public static class VeiculoValidator
    {
        public static List<string> Validar(Veiculo veiculo)
        {
            var erros = new List<string>();

            if (string.IsNullOrWhiteSpace(veiculo.Modelo))
                erros.Add("Modelo é obrigatório.");

            if (string.IsNullOrWhiteSpace(veiculo.Placa))
                erros.Add("Placa é obrigatória.");

            if (veiculo.Ano < 1900 || veiculo.Ano > DateTime.Now.Year)
                erros.Add("Ano inválido.");

            return erros;
        }
    }
}
