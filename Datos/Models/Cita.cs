namespace Datos.Models;

public class Cita
{
    public int IdCita { get; set; }

    public string NumeroCita { get; set; } = string.Empty;

    public int IdCliente { get; set; }

    public int IdVehiculo { get; set; }

    public int IdServicio { get; set; }

    public int? IdEmpleado { get; set; }

    public string? Area { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public string Estado { get; set; } = "Programada";

    public string? Observaciones { get; set; }

    public Cliente? Cliente { get; set; }

    public Vehiculo? Vehiculo { get; set; }

    public Servicio? Servicio { get; set; }

    public Empleado? Empleado { get; set; }
}