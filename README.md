# API de Horóscopo en .NET 6

Esta es una API construida en .NET 6 para ofrecer servicios relacionados con horóscopos, estadísticas de signos zodiacales y el historial de consultas de los usuarios. Los endpoints permiten obtener el horóscopo según la fecha de nacimiento, obtener estadísticas de los signos y consultar el historial de consultas.

# Requisitos 

- .NET 6 SDK
- Herramientas para ejecutar APIs (como Postman o Insomnia)
- Base de datos configurada.

# Instalación

1. Clonar el repositorio.
2. Restaurar los paquetes. Navega a la carpeta del proyecto y ejecuta el siguiente comando para restaurar los paquetes NuGet necesarios:
    dotnet restore
3. Ejecutar la API.

# Endpoints

1. Obtener Horóscopo
Método: POST
Ruta: /horoscopo
Descripción: Obtiene el horóscopo para el signo zodiacal basado en la fecha de nacimiento proporcionada.
```json
{
  "Nombre": "Juan Pérez",
  "Email": "juan@gmail.com",
  "Fecha_nacimiento": "1990-03-15"
}
```

2. Obtener Estadísticas de Signos
Método: GET
Ruta: /estadisticas-signo
Descripción: Devuelve las estadisticas del signo más buscado.

```json
  {
    "Signo": "Aries",
    "CantidadConsultas": 120
  }
```

3. Obtener Historial de Consultas
Método: GET
Ruta: /historial-consultas
Descripción: Devuelve el historial de todas las consultas realizadas, incluyendo el nombre del usuario, signo y fecha de la consulta.

 ```json  
 [
  {
    "Nombre": "Juan Pérez",
    "Signo": "Piscis",
    "FechaConsulta": "2024-03-15T08:30:00"
  },
  {
    "Nombre": "Ana García",
    "Signo": "Leo",
    "FechaConsulta": "2024-03-14T09:15:00"
  }
]
 ```