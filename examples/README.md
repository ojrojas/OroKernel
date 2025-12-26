# OroKernel Examples

Este directorio contiene ejemplos de proyectos de consola que demuestran cómo integrar y utilizar la librería OroKernel Shared con Entity Framework Core.

## Proyectos de Ejemplo

### 1. UserManagement

**Uso de `BaseEntity` (con identificador Guid) con EF Core**

Este proyecto demuestra cómo crear y gestionar usuarios utilizando la clase base `BaseEntity` que proporciona identificadores Guid automáticos, integrado con Entity Framework Core.

#### Características demostradas:
- Configuración de Entity Framework Core con base de datos en memoria
- Inyección de dependencias con HostBuilder
- Operaciones CRUD completas (Create, Read, Update, Delete)
- Auditoría automática de cambios en la base de datos
- Gestión del estado de usuarios (activar/desactivar)
- Actualización de información de usuarios
- Herencia de funcionalidades de BaseEntity
- Patrón Repository para acceso a datos

#### Ejecutar el ejemplo:
```bash
cd examples/UserManagement
dotnet run
```

### 2. IdentityManagement

**Uso de `BaseEntity<T, TId>` con EF Core**

Este proyecto demuestra cómo crear y gestionar tipos de identificación utilizando la clase base genérica `BaseEntity<IdentificationType, int>` donde:
- `T` = `IdentificationType` (el tipo de entidad)
- `TId` = `int` (el tipo del identificador)

#### Características demostradas:
- Configuración de Entity Framework Core con base de datos en memoria
- Inyección de dependencias con HostBuilder
- Operaciones CRUD completas con tipos de ID personalizados
- Validación de números de identificación usando expresiones regulares
- Gestión del estado de tipos de identificación
- Actualización de información y patrones de validación
- Herencia de funcionalidades de BaseEntity<T, TId>
- Auditoría automática de todas las operaciones
- Patrón Repository para acceso a datos

#### Ejecutar el ejemplo:
```bash
cd examples/IdentityManagement
dotnet run
```

## Tecnologías Utilizadas

Ambos proyectos demuestran la integración completa con:

- **Entity Framework Core**: ORM para acceso a datos
- **In-Memory Database**: Base de datos en memoria para demostración
- **Dependency Injection**: Configuración de servicios con HostBuilder
- **Repository Pattern**: Patrón de repositorio para acceso a datos
- **Auditing**: Auditoría automática de cambios
- **Domain Events**: Soporte para eventos de dominio

## Diferencias entre BaseEntity y BaseEntity<T, TId>

### BaseEntity
- Utiliza `Guid` como tipo de identificador
- ID auto-generado con `Guid.CreateVersion7()`
- Ideal para entidades con IDs únicos globales
- Ejemplo: Gestión de usuarios, productos, etc.

### BaseEntity<T, TId>
- Utiliza un tipo de identificador personalizado (`TId`)
- `TId` debe ser un `struct` que implemente `IEquatable<TId>`
- Permite IDs de tipos como `int`, `long`, `Guid`, etc.
- Útil cuando necesitas control total sobre el tipo de ID
- Ejemplo: Tipos de identificación, categorías, estados, etc.

## Requisitos

- .NET 10.0 o superior
- Acceso a la librería OroKernel Shared

## Estructura de los Proyectos

```
examples/
├── UserManagement/
│   ├── UserManagement.csproj
│   ├── Data/
│   │   └── UserManagementDbContext.cs    # DbContext heredando de AuditableDbContext
│   ├── User.cs                           # Entidad User heredando de BaseEntity
│   └── Program.cs                        # Programa principal con DI y EF Core
└── IdentityManagement/
    ├── IdentityManagement.csproj
    ├── Data/
    │   └── IdentityManagementDbContext.cs # DbContext heredando de AuditableDbContext
    ├── IdentificationType.cs             # Entidad IdentificationType heredando de BaseEntity<T, TId>
    └── Program.cs                        # Programa principal con DI y EF Core
```

## Notas Importantes

- `BaseEntity<TId>` requiere que `TId` sea un tipo de valor (`struct`) que implemente `IEquatable<TId>`
- Para usar `string` como ID, utiliza `BaseEntity` normal que usa `Guid`
- Ambos tipos de entidades heredan automáticamente de `WithDomainEventBase` para soporte de eventos de dominio
- La auditoría automática registra todas las operaciones CRUD en la tabla `AuditEntries`
- Los ejemplos usan base de datos en memoria para facilitar la ejecución, pero pueden adaptarse fácilmente a SQL Server, PostgreSQL, etc.