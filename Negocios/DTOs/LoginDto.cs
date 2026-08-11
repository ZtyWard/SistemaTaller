using System.ComponentModel.DataAnnotations;

namespace Negocios.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "El usuario o correo es obligatorio.")]
    [Display(Name = "Usuario o correo")]
    public string Usuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Recordarme")]
    public bool Recordarme { get; set; }
}