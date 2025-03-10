
# Prueba Técnica - Backend (.NET + EF Core)

Este es el backend desarrollado en .NET con Entity Framework Core para la prueba técnica. Se encarga de obtener información del sitio de la CENACE mediante web scraping con Selenium.

## Instalación y Configuración

### Generar una Nueva Migración

Para generar una nueva migración en Entity Framework Core, ejecuta el siguiente comando en la raíz del proyecto:

```sh
dotnet ef migrations add Inicial -p .\PruebatecnicaBack.Infrastructure\ -s .\PruebatecnicaBack.Api\
```

### Actualizar la Base de Datos

Después de crear la migración, aplica los cambios a la base de datos con:

```sh
dotnet ef database update -p .\PruebatecnicaBack.Infrastructure\ -s .\PruebatecnicaBack.Api\
```

Recomiendo usar PowerShell con permisos de administrador

![Terminal](/terminal.png)

Modificar los archivos appSettings.json para la conexión a la base de datos

```sh
"defaultConnection": "Host=localhost;Database=prueba_tecnica;Username=magadan;Password=magadan"
```

Modificar el archivo del servicio del scrapper el sleep, si se presenta algún error, esto depende de la velocidad de internet del cliente donde se ejecuta el backend, para poder obtener los datos de la CENACE

```sh
driver.Navigate().GoToUrl("https://www.cenace.gob.mx/Paginas/SIM/CapacidadDemandadaRAP.aspx");
Thread.Sleep(500);

var yearDropdown = new SelectElement(driver.FindElement(By.Id("ContentPlaceHolder1_DrpAnio")));
yearDropdown.SelectByValue(year.ToString());
Thread.Sleep(500);
```

## Endpoints Disponibles

### Scraper (Obtener Archivos Excel)

Este endpoint descarga los 4 archivos `.xls` de cada página por año:

```sh
GET https://localhost:7070/api/v1/scraper?year=2024&update=false
```

**Parámetros:**
- `year`: Año de los datos a obtener.
- `update`: (Opcional) `true` para actualizar los datos, `false` para obtener los actuales.

### Obtener Datos Filtrados

Este endpoint permite buscar y filtrar datos:

```sh
GET https://localhost:7070/api/v1/zones?searchTerm=Participante&sortColumn=Name&sortOrder=asc&year=2024&minCapacity=0&maxCapacity=1000000&page=1&pageSize=10
```

**Parámetros:**
- `searchTerm`: Término de búsqueda (por ejemplo, un nombre de participante).
- `sortColumn`: Columna por la cual ordenar (por ejemplo, `Name`).
- `sortOrder`: Dirección del orden (`asc` o `desc`).
- `year`: Año de los datos.
- `minCapacity`: Capacidad mínima.
- `maxCapacity`: Capacidad máxima.
- `page`: Número de página.
- `pageSize`: Cantidad de resultados por página.

## Ejecución

Ejecuta primero este backend antes de iniciar el frontend para asegurar el correcto funcionamiento de la aplicación.


## Arquitectura y Tecnologías Utilizadas

El proyecto cuenta con fragmentos de código que he ido mejorando a lo largo del tiempo y está estructurado siguiendo el patrón **CQRS** con **MediatR**, **FluentValidation** y **Bulk Inserts** para la carga masiva de datos. También utilizo algunas plantillas reutilizables para mejorar la eficiencia en el desarrollo.

## Autenticación y Seguridad

- El sistema cuenta con **autenticación de usuarios con roles** y manejo de **Refresh Token**.
- El **Access Token** tiene una duración de **60 minutos**.
- El **Refresh Token** tiene una duración de **7 días**.
- Los endpoints están protegidos y requieren autenticación.

## Base de Datos y Persistencia

- Los datos obtenidos del scraping se almacenan en la base de datos en la tabla **Zones**.
- Cada vez que se ejecuta el scraping, se agregan nuevos registros.
- Si es necesario, los datos pueden ser eliminados manualmente desde la base de datos para realizar más pruebas.
- En la capa de **Infrastructure**, dentro de **Persistence**, existen **Seeders** para la creación de usuarios y roles.

## Usuarios de Prueba

El sistema incluye dos usuarios predefinidos para pruebas:

1. **Administrador:**
   - **Correo:** `dany_magadan@hotmail.com`
   - **Contraseña:** `Heros2022#`

2. **Usuario Regular:**
   - **Correo:** `juan_perez@hotmail.com`
   - **Contraseña:** `Heros2022#`

## Pantalla de Swagger
Se muestra la pantalla del funcionamiento del swagger
![Pantalla de swagger](/swagger.png)

## Prueba de concepto usando Quartz
¿Cuándo usar un Background Task con Quartz?

Quartz es ideal cuando: 

   - Necesitas ejecutar tareas recurrentes en intervalos específicos (cada minuto, hora, día, etc.).
   - Requieres programar tareas con flexibilidad (cron jobs, ejecuciones diferidas).
   - Necesitas manejar múltiples tareas en paralelo con diferentes programaciones.
   - Quieres reintentos automáticos en caso de fallas.

¿Quartz.NET consume muchos recursos?
- Quartz en sí es liviano porque usa un Scheduler, lo que significa que solo consume recursos cuando un Job está en ejecución.
- Si tu Job es pesado (como scraping con muchas peticiones web), el consumo dependerá más de la lógica dentro del Job que de Quartz en sí.
- Para optimizarlo, puedes:

Ejecutar Jobs en horarios de baja carga.
Limitar la concurrencia de Jobs.
Configurar un ThreadPool para evitar el consumo excesivo de hilos.