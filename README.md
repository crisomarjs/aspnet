# Tienda de Libros - ASP.NET 9 MVC

## Descripción
Este es un proyecto de una tienda de libros desarrollado con **ASP.NET Core 9** utilizando el patrón **MVC (Model-View-Controller)**.
La aplicación permite gestionar un catálogo de libros, realizar búsquedas. Como base de datos se utiliza **SQLite** para un almacenamiento ligero y eficiente.

## Tecnologías Utilizadas
- **ASP.NET Core 9**
- **MVC (Model-View-Controller)**
- **Entity Framework Core**
- **SQLite**
- **Razor Views**

## Requisitos Previos
Antes de ejecutar el proyecto, asegúrate de tener instalado lo siguiente:

- .NET 9 SDK ([Descargar aquí](https://dotnet.microsoft.com/download/dotnet/9.0))
- Visual Studio 2022 o superior (opcional pero recomendado)

## Estructura del Proyecto
📦 AppStore (Directorio principal)  
 ┣ 📂 Controllers/        # Controladores para la lógica del negocio  
 ┣ 📂 Migrations/         # Migraciones de la base de datos SQLite  
 ┣ 📂 Models/             # Modelos de datos de la aplicación  
 ┣ 📂 Properties/         # Configuración del proyecto  
 ┣ 📂 Repositories/       # Repositorios para acceso a la base de datos  
 ┣ 📂 Views/              # Vistas de la aplicación (HTML + Razor)  
 ┣ 📂 wwwroot/            # Archivos estáticos (CSS, JS, imágenes)  
 ┣ 📜 AppStore.csproj     # Archivo del proyecto ASP.NET  
 ┣ 📜 LocalDataBase.db    # Archivo de la base de datos SQLite  
 ┣ 📜 appsettings.json    # Configuración principal de la aplicación  
 ┣ 📜 Program.cs          # Archivo de inicio del proyecto  


## Funcionalidades
✅ Administración de libros (CRUD: Crear, Leer, Actualizar, Eliminar).  
✅ Búsqueda y filtrado de libros.  
✅ Autenticación y autorización de usuarios.  
✅ Base de datos SQLite para almacenamiento de información.  
