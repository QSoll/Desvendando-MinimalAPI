using System;


namespace DesvendandoMinimalAPI.Domain
{
    public class Administrador
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Perfil { get; set; } = "Editor"; // ou "Adm"
    }
}
