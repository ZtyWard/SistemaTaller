using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Datos.Repositories;

public class OrdenTrabajoRepository
    : Repository<OrdenTrabajo>, IOrdenTrabajoRepository
{
    public OrdenTrabajoRepository(
        SistemaTallerDbContext context)
        : base(context)
    {
    }

    // =====================================================
    // OBTENER ORDEN COMPLETA
    // =====================================================

    public async Task<OrdenTrabajo?> ObtenerCompletaAsync(
        int id)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Cotizacion)
                .ThenInclude(x => x!.Diagnostico)
                    .ThenInclude(x => x!.Recepcion)
                        .ThenInclude(x => x!.Vehiculo)
                            .ThenInclude(x => x!.Cliente)
            .FirstOrDefaultAsync(
                x => x.IdOrdenTrabajo == id);
    }

    // =====================================================
    // OBTENER POR ESTADO
    // =====================================================

    public async Task<IEnumerable<OrdenTrabajo>>
        ObtenerPorEstadoAsync(string estado)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Cotizacion)
                .ThenInclude(x => x!.Diagnostico)
                    .ThenInclude(x => x!.Recepcion)
                        .ThenInclude(x => x!.Vehiculo)
            .Where(x => x.Estado == estado)
            .OrderByDescending(x => x.FechaInicio)
            .ToListAsync();
    }

    // =====================================================
    // OBTENER ABIERTAS
    // =====================================================

    public async Task<IEnumerable<OrdenTrabajo>>
        ObtenerAbiertasAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Cotizacion)
                .ThenInclude(x => x!.Diagnostico)
                    .ThenInclude(x => x!.Recepcion)
                        .ThenInclude(x => x!.Vehiculo)
                            .ThenInclude(x => x!.Cliente)
            .Where(x => x.Estado != "Finalizada")
            .OrderByDescending(x => x.FechaInicio)
            .ToListAsync();
    }

    // =====================================================
    // CREAR CON USUARIO
    // =====================================================

    public async Task CrearConUsuarioAsync(
        OrdenTrabajo orden,
        string usuarioId)
    {
        var conexion =
            _context.Database.GetDbConnection();

        var debeCerrarConexion =
            conexion.State != ConnectionState.Open;

        if (debeCerrarConexion)
        {
            await _context.Database.OpenConnectionAsync();
        }

        Exception? errorOperacion = null;
        var contextoEstablecido = false;

        try
        {
            // Guardamos el usuario autenticado en la sesión SQL.
            // El trigger de auditoría puede utilizar este valor.
            await _context.Database
                .ExecuteSqlInterpolatedAsync($"""
                    EXEC sys.sp_set_session_context
                        @key = N'UsuarioId',
                        @value = {usuarioId};
                    """);

            contextoEstablecido = true;

            // Agregamos la nueva orden.
            _dbSet.Add(orden);

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            errorOperacion = ex;
            throw;
        }
        finally
        {
            try
            {
                if (contextoEstablecido)
                {
                    await _context.Database
                        .ExecuteSqlRawAsync(
                            "EXEC sys.sp_set_session_context " +
                            "@key = N'UsuarioId', @value = NULL;");
                }
            }
            catch when (errorOperacion != null)
            {
                // Conservamos el error original.
            }
            finally
            {
                if (debeCerrarConexion)
                {
                    await _context.Database
                        .CloseConnectionAsync();
                }
            }
        }
    }

    // =====================================================
    // ACTUALIZAR CON USUARIO
    // =====================================================

    public async Task<bool> ActualizarConUsuarioAsync(
        int idOrdenTrabajo,
        int idCotizacion,
        string? observaciones,
        string usuarioId)
    {
        var orden = await _dbSet
            .FirstOrDefaultAsync(
                x => x.IdOrdenTrabajo == idOrdenTrabajo);

        if (orden == null)
            return false;

        var conexion =
            _context.Database.GetDbConnection();

        var debeCerrarConexion =
            conexion.State != ConnectionState.Open;

        if (debeCerrarConexion)
        {
            await _context.Database.OpenConnectionAsync();
        }

        Exception? errorOperacion = null;
        var contextoEstablecido = false;

        try
        {
            // Guardamos el usuario autenticado en la sesión SQL.
            await _context.Database
                .ExecuteSqlInterpolatedAsync($"""
                    EXEC sys.sp_set_session_context
                        @key = N'UsuarioId',
                        @value = {usuarioId};
                    """);

            contextoEstablecido = true;

            orden.IdCotizacion = idCotizacion;
            orden.Observaciones = observaciones;

            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            errorOperacion = ex;
            throw;
        }
        finally
        {
            try
            {
                if (contextoEstablecido)
                {
                    await _context.Database
                        .ExecuteSqlRawAsync(
                            "EXEC sys.sp_set_session_context " +
                            "@key = N'UsuarioId', @value = NULL;");
                }
            }
            catch when (errorOperacion != null)
            {
                // Conservamos el error original.
            }
            finally
            {
                if (debeCerrarConexion)
                {
                    await _context.Database
                        .CloseConnectionAsync();
                }
            }
        }
    }

    // =====================================================
    // CAMBIAR ESTADO
    // =====================================================

    public async Task CambiarEstadoAsync(
        int idOrdenTrabajo,
        string nuevoEstado,
        string usuarioId,
        string? observaciones)
    {
        var conexion =
            _context.Database.GetDbConnection();

        var debeCerrarConexion =
            conexion.State != ConnectionState.Open;

        if (debeCerrarConexion)
        {
            await _context.Database.OpenConnectionAsync();
        }

        Exception? errorOperacion = null;
        var contextoEstablecido = false;

        try
        {
            // Establecemos el usuario en la sesión SQL.
            await _context.Database
                .ExecuteSqlInterpolatedAsync($"""
                    EXEC sys.sp_set_session_context
                        @key = N'UsuarioId',
                        @value = {usuarioId};
                    """);

            contextoEstablecido = true;

            // Ejecutamos el Stored Procedure.
            await _context.Database
                .ExecuteSqlInterpolatedAsync($"""
                    EXEC dbo.sp_CambiarEstadoOrden
                        @IdOrdenTrabajo = {idOrdenTrabajo},
                        @NuevoEstado = {nuevoEstado},
                        @UsuarioId = {usuarioId},
                        @Observaciones = {observaciones};
                    """);
        }
        catch (Exception ex)
        {
            errorOperacion = ex;
            throw;
        }
        finally
        {
            try
            {
                if (contextoEstablecido)
                {
                    await _context.Database
                        .ExecuteSqlRawAsync(
                            "EXEC sys.sp_set_session_context " +
                            "@key = N'UsuarioId', @value = NULL;");
                }
            }
            catch when (errorOperacion != null)
            {
                // Se conserva el error original.
            }
            finally
            {
                if (debeCerrarConexion)
                {
                    await _context.Database
                        .CloseConnectionAsync();
                }
            }
        }
    }
}