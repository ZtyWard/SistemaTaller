

AXIS es un sistema de gestión para un taller automotriz desarrollado en:

- ASP.NET Core MVC
- .NET 8
- Entity Framework Core 8
- SQL Server
- ASP.NET Identity

La solución utiliza una arquitectura de 3 proyectos:

- Datos
- Negocios
- SistemaTaller

La dependencia debe mantenerse:

SistemaTaller
    ↓
Negocios
    ↓
Datos
    ↓
SQL Server

No invertir ni romper estas dependencias.



REGLA PRINCIPAL

NO inventar propiedades, columnas, relaciones, tablas,
métodos ni comportamientos que no existan en el proyecto.

Antes de modificar o crear código:

1. Revisar el modelo correspondiente en Datos/Models.
2. Revisar su Configuration en Datos/Configurations.
3. Revisar interfaces existentes.
4. Revisar repositories existentes.
5. Revisar services existentes.
6. Revisar DTOs existentes.
7. Revisar Program.cs.
8. Seguir exactamente los patrones ya utilizados.

Si una propiedad no existe en el modelo, NO utilizarla.

Ejemplo:

Si CategoriaProducto no tiene Descripcion,
no crear código que utilice CategoriaProducto.Descripcion.



ESTILO DE PROGRAMACIÓN

Mantener el estilo de programación existente en AXIS.

No introducir una arquitectura diferente.

No reemplazar Repository Pattern.

No reemplazar Services.

No mover lógica de Negocios hacia Controllers.

No colocar consultas EF Core directamente en Controllers.

No colocar lógica de negocio en Views.

Mantener:

Controller
    ↓
Service
    ↓
Repository
    ↓
DbContext
    ↓
Database



 MODELOS

Los modelos existentes son la fuente de verdad
para nombres de propiedades y relaciones.

NO modificar modelos existentes solamente para hacer
que código nuevo compile.

Si falta una propiedad para una funcionalidad,
detenerse y reportarlo antes de modificar el modelo.

REPOSITORIES

Los repositories deben utilizar:

- Interfaces en Datos/Interfaces
- Implementaciones en Datos/Repositories

Seguir el patrón Repository existente.

Utilizar Entity Framework Core.

Usar AsNoTracking() en consultas de solo lectura
cuando corresponda.

No colocar lógica de negocio compleja en repositories.

 SERVICES

Los Services deben contener la lógica de negocio.

Los Services deben:

- Validar datos.
- Validar relaciones.
- Aplicar reglas de negocio.
- Coordinar repositories.
- Mapear entidades a DTOs.

No colocar lógica de negocio importante
directamente en Controllers.


DTOs

Mantener DTOs separados de los Models.

Seguir los nombres y estructura existentes:

[Entidad]Dto
[Entidad]GuardarDto

No agregar propiedades que no sean necesarias
o que no correspondan al modelo real.

PROGRAM.CS

Cada vez que se agregue un Repository o Service,
registrarlo en Program.cs.

NO duplicar registros.

Cuando se modifique Program.cs:

ENTREGAR SIEMPRE EL ARCHIVO COMPLETO ACTUALIZADO.

Mantener la organización por secciones:

- Base de datos
- Identity
- Repositories
- Services
- MVC
- Pipeline HTTP
- Ruta principal

 IDENTITY

ASP.NET Identity ya está configurado.

ApplicationUser es el usuario principal del sistema.

No reemplazar Identity por otro sistema de autenticación.

No eliminar la configuración existente.

BASE DE DATOS

SQL Server es la base de datos utilizada.

Entity Framework Core es el ORM.

No cambiar de proveedor de base de datos.

No crear tablas nuevas sin verificar primero
si existen en el diseño actual.

CATÁLOGOS

Los catálogos deben implementarse siguiendo
el mismo patrón Repository + Service + DTO.

Antes de implementar cada catálogo:

- revisar su Model
- revisar su Configuration
- revisar relaciones

NO asumir que todos los catálogos tienen
las mismas propiedades.

Por ejemplo:

NO asumir que todos tienen:
- Activo
- Descripcion
- Codigo

Solo utilizar propiedades que realmente existan.


 CAMBIOS

NO hacer refactors generales.

NO cambiar nombres de clases existentes.

NO cambiar namespaces existentes.

NO cambiar paquetes NuGet.

NO cambiar .NET 8.

NO cambiar la arquitectura.

NO eliminar archivos existentes.

NO reescribir módulos que ya funcionan.

Realizar cambios mínimos y específicos.

 COMPILACIÓN

Después de realizar cambios:

1. Compilar toda la solución.
2. Revisar errores.
3. Corregir solamente errores causados por los cambios.
4. Volver a compilar.
5. Confirmar resultado.

No declarar que el trabajo está terminado
si la solución no compila correctamente.



ANTES DE MODIFICAR

Primero inspeccionar.

Después planificar.

Después modificar.

No asumir.

No generar archivos basándose únicamente
en nombres de clases.





Estamos terminando los catálogos.

Ya están implementados:

- CategoriaProducto

Pendientes:

- Marca
- Modelo
- TipoVehiculo
- TipoCombustible
- Puesto
- Especialidad

Implementar estos módulos respetando exactamente
los modelos y configuraciones existentes.

NO avanzar todavía hacia:

- Controllers
- UI
- Auditoría
- Login
- Roles

hasta terminar los catálogos y comprobar compilación.



 REGLA DE SEGURIDAD DEL PROYECTO

Si existe duda sobre una propiedad,
relación, tabla o comportamiento:

NO INVENTAR.

Inspeccionar primero el código existente.

Si después de inspeccionarlo sigue sin estar claro,
informar el problema y pedir confirmación.



 RESULTADO ESPERADO


El código generado debe parecer escrito por
el mismo desarrollador que construyó el resto de AXIS.

Priorizar consistencia sobre introducir
patrones nuevos o "mejoras" innecesarias.