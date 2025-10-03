using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using DesvendandoMinimalAPI.Context;
using DesvendandoMinimalAPI.Domain;
using DesvendandoMinimalAPI.Veiculos;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; // necessário para [FromServices]

namespace DesvendandoMinimalAPI.Endpoints
{
    public static class VeiculoEndpoints
    {
        public static void MapVeiculoEndpoints(this WebApplication app)
        {
            app.MapGet("/veiculos", [Authorize] async ([FromServices] AppDbContext db) =>
                await db.Veiculos.ToListAsync());

            app.MapGet("/veiculos/{id}", [Authorize] async (int id, [FromServices] AppDbContext db) =>
                await db.Veiculos.FindAsync(id) is Veiculo veiculo
                    ? Results.Ok(veiculo)
                    : Results.NotFound());

            app.MapPost("/veiculos", [Authorize] async (Veiculo veiculo, [FromServices] AppDbContext db) =>
            {
                var erros = VeiculoValidator.Validar(veiculo);
                if (erros.Any()) return Results.BadRequest(erros);

                db.Veiculos.Add(veiculo);
                await db.SaveChangesAsync();
                return Results.Created($"/veiculos/{veiculo.Id}", veiculo);
            });

            app.MapPut("/veiculos/{id}", [Authorize] async (int id, Veiculo input, [FromServices] AppDbContext db) =>
            {
                var veiculo = await db.Veiculos.FindAsync(id);
                if (veiculo is null) return Results.NotFound();

                var erros = VeiculoValidator.Validar(input);
                if (erros.Any()) return Results.BadRequest(erros);

                veiculo.Modelo = input.Modelo;
                veiculo.Placa = input.Placa;
                veiculo.Ano = input.Ano;
                veiculo.Cor = input.Cor;

                await db.SaveChangesAsync();
                return Results.Ok(veiculo);
            });

            app.MapDelete("/veiculos/{id}", [Authorize] async (int id, [FromServices] AppDbContext db) =>
            {
                var veiculo = await db.Veiculos.FindAsync(id);
                if (veiculo is null) return Results.NotFound();

                db.Veiculos.Remove(veiculo);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
        }
    }
}
