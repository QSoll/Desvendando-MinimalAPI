namespace DesvendandoMinimalAPI.DTOs;

public class AdministradorCreateDTO
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Perfil { get; set; } = "Editor"; // "Adm" ou "Editor"
}
