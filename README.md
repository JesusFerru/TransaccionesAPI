# Sistema de Transacciones Bancarias API

Este proyecto implementa una API RESTful utilizando .NET 8 para la gestión de cuentas y transacciones bancarias. Está diseñado siguiendo principios de arquitectura limpia y mejores prácticas de desarrollo.

## Requisitos Previos

*   **SDK de .NET 8**: Asegúrate de tener instalado el SDK de .NET 8.0 o superior.
*   **Base de Datos**: El proyecto utiliza PostgreSQL. Para este entorno de pruebas, se ha configurado una base de datos en la nube utilizando **Neon**.

## Instrucciones de Configuración

Por motivos de seguridad, el archivo de configuración `appsettings.json` que contiene la cadena de conexión a la base de datos (Neon) y la `ApiKey` para la autenticación **se enviará por correo privado**.

1.  Una vez recibido el archivo `appsettings.json` o los secretos correspondientes, colócalos en la raíz del proyecto `Transacciones.API`.
2.  El archivo debe tener una estructura similar a esta:

```json
{
  "Logging": { ... },
  "ConnectionStrings": {
    "DefaultConnection": "CADENA_DE_CONEXION_A_NEON"
  },
  "Security": {
    "ApiKey": "API_KEY_SECRETA"
  }
}
```

## Base de Datos (Neon)

La base de datos se encuentra alojada en **Neon** (Serverless Postgres) para facilitar las pruebas y la accesibilidad.

### Actualización de la Base de Datos

Si necesitas aplicar migraciones o actualizar la base de datos manualmente, puedes ejecutar el siguiente comando desde la carpeta `Transacciones.Infrastructure`:

```bash
cd Transacciones.Infrastructure
dotnet ef database update --startup-project ..\Transacciones.API\
```

## Ejecución del Proyecto

Para ejecutar la API localmente:

1.  Navega a la carpeta del proyecto API:
    ```bash
    cd Transacciones.API
    ```
2.  Ejecuta el proyecto:
    ```bash
    dotnet run
    ```
3.  La API estará disponible en `http://localhost:5169` (o el puerto configurado).
4.  Puedes acceder a la documentación Swagger en: `http://localhost:5169/swagger`

### Autenticación en Pruebas (Postman/Insomnia)
Para consumir los endpoints, debes incluir el header `Authorization` con el valor `Bearer <TU_API_KEY>`.
Ejemplo: `Authorization: Bearer ey5ApiKeySecreta...oc`

## Ejecución de Tests

El proyecto incluye pruebas unitarias para validar la lógica de negocio.

Para ejecutar las pruebas:

1.  Navega a la raíz de la solución o a la carpeta de tests:
    ```bash
    cd Transacciones.Tests
    ```
2.  Ejecuta los tests:
    ```bash
    dotnet test
    ```

## Decisiones Técnicas

El proyecto ha sido construido siguiendo **Clean Architecture** para asegurar la separación de responsabilidades y la mantenibilidad.

*   **Arquitectura**:
    *   **Core**: Contiene las entidades del dominio, interfaces y lógica de negocio pura. No tiene dependencias externas.
    *   **Infrastructure**: Implementa las interfaces del Core (Repositorios, Servicios externos, Acceso a Datos con EF Core).
    *   **API**: La capa de presentación, implementada con Endpoints.

*   **Patrones de Diseño**:
    *   **REPR (Request-Endpoint-Response)**: Se utilizó `Ardalis.ApiEndpoints` en lugar de Controladores tradicionales. Esto permite tener cada endpoint en su propia clase, siguiendo el Principio de Responsabilidad Única (SRP) y facilitando la organización (Vertical Slices).
    *   **Repository Pattern**: Abstracción del acceso a datos.
    *   **Unit of Work**: Para manejar transacciones y asegurar la integridad de los datos en operaciones que involucran múltiples pasos.
    *   **Specification Pattern**: Utilizado para encapsular consultas complejas (ej. historial de transacciones).

*   **Librerías y Herramientas**:
    *   **Entity Framework Core**: ORM para acceso a datos.
    *   **FluentValidation**: Para la validación robusta de los modelos de entrada (Requests).
    *   **AutoMapper**: Para el mapeo entre Entidades y DTOs.
    *   **Serilog**: Para el logging estructurado.
    *   **Swagger/OpenAPI**: Para documentación y pruebas de la API.

*   **Seguridad**:
    *   Implementación de un filtro de autorización personalizado (`ApiKeyAuthorizationFilter`) que valida un `ApiKey` en el header `Authorization` (Bearer token).

## Estrategia de Concurrencia

Para garantizar la integridad de los datos, especialmente en operaciones críticas como **Abonos** y **Retiros**, se ha implementado una estrategia basada en **Transacciones de Base de Datos**.

*   Se utiliza el patrón **Unit of Work** para gestionar el ciclo de vida de la transacción.
*   En cada operación de transacción (Abono/Retiro):
    1.  Se inicia una transacción explícita (`BeginTransactionAsync`).
    2.  Se realizan las validaciones de negocio (ej. verificación de saldo).
    3.  Se actualiza el saldo de la cuenta.
    4.  Se registra la transacción en el historial.
    5.  Si todo es correcto, se confirman los cambios (`CommitAsync`).
    6.  Si ocurre algún error, se revierten todos los cambios (`RollbackAsync`).

Esto asegura la **Atomicidad** de las operaciones: o se completan todos los cambios o no se aplica ninguno, evitando estados inconsistentes en el saldo de las cuentas.
