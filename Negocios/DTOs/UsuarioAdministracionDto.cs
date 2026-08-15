using System.ComponentModel.DataAnnotations;

namespace Negocios.DTOs;

public class UsuarioAdministracionDto
{
    public string Id { get; set; } = string.Empty;

    [Display(Name = "Usuario")]
    public string Usuario { get; set; } = string.Empty;

    [Display(Name = "Correo electrónico")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Nombre completo")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Display(Name = "Activo")]
    public bool Activo { get; set; }

    public List<string> Roles { get; set; } = new();
}

public class UsuarioGuardarDto
{
    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    [StringLength(50, ErrorMessage = "El usuario no puede superar los 50 caracteres.")]
    [Display(Name = "Usuario")]
    public string Usuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido.")]
    [StringLength(150, ErrorMessage = "El correo no puede superar los 150 caracteres.")]
    [Display(Name = "Correo electrónico")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [StringLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres.")]
    [Display(Name = "Nombre completo")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Display(Name = "Contraseña")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    public string? Password { get; set; }

    [Display(Name = "Confirmar contraseña")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden.")]
    public string? ConfirmarPassword { get; set; }

    [Display(Name = "Activo")]
    public bool Activo { get; set; } = true;

    public List<string> Roles { get; set; } = new();
}