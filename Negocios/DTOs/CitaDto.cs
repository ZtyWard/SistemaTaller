using System.ComponentModel.DataAnnotations;

namespace Negocios.DTOs;

public class CitaDto
{
    public int IdCita { get; set; }

    public string NumeroCita { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Cliente")]
    public int IdCliente { get; set; }

    [Required]
    [Display(Name = "Vehículo")]
    public int IdVehiculo { get; set; }

    [Required]
    [Display(Name = "Servicio")]
    public int IdServicio { get; set; }

    [Display(Name = "Mecánico")]
    public int? IdEmpleado { get; set; }

    [StringLength(100)]
    [Display(Name = "Área")]
    public string? Area { get; set; }

    [Required]
    [Display(Name = "Inicio")]
    public DateTime FechaInicio { get; set; }

    [Required]
    [Display(Name = "Fin")]
    public DateTime FechaFin { get; set; }

    [Required]
    [StringLength(30)]
    public string Estado { get; set; } = "Programada";

    [StringLength(500)]
    public string? Observaciones { get; set; }

    public string ClienteNombre { get; set; } = string.Empty;

    public string VehiculoPlaca { get; set; } = string.Empty;

    public string ServicioNombre { get; set; } = string.Empty;

    public string EmpleadoNombre { get; set; } = string.Empty;
}

public class CitaFormularioDto
{
    [Required(ErrorMessage = "Seleccione un cliente.")]
    [Display(Name = "Cliente")]
    public int IdCliente { get; set; }

    [Required(ErrorMessage = "Seleccione un vehículo.")]
    [Display(Name = "Vehículo")]
    public int IdVehiculo { get; set; }

    [Required(ErrorMessage = "Seleccione un servicio.")]
    [Display(Name = "Servicio")]
    public int IdServicio { get; set; }

    [Display(Name = "Mecánico")]
    public int? IdEmpleado { get; set; }

    [StringLength(100)]
    [Display(Name = "Área")]
    public string? Area { get; set; }

    [Required(ErrorMessage = "Indique la fecha y hora de inicio.")]
    [Display(Name = "Inicio")]
    public DateTime FechaInicio { get; set; }

    [Display(Name = "Fin")]
    public DateTime FechaFin { get; set; }

    [Required]
    [StringLength(30)]
    public string Estado { get; set; } = "Programada";

    [StringLength(500)]
    public string? Observaciones { get; set; }
}