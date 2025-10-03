#nullable enable

using DesvendandoMinimalAPI.Domain;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;



namespace DesvendandoMinimalAPI.Repositories
{
    public class AdministradorRepository
    {
        private readonly List<Administrador> _administradores = new();

        public void Adicionar(Administrador adm) => _administradores.Add(adm);

        public IEnumerable<Administrador> Listar() => _administradores;

        public Administrador? BuscarPorEmailSenha(string email, string senha) =>
            _administradores.FirstOrDefault(a =>
                a.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                a.Senha == senha);
    }
}
