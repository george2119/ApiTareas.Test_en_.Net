# API Tareas - .NET 9 & Docker

Este proyecto es una API REST profesional para la gestión de tareas, diseñada para demostrar habilidades en arquitectura backend, contenedorización y documentación.

## Tecnologías utilizadas
* **Framework:** .NET 9 (C#)
* **Base de Datos:** MySQL
* **ORM:** Entity Framework Core
* **Contenedores:** Docker & Docker Compose
* **Documentación:** Swagger (OpenAPI)

## Cómo ejecutar el proyecto
La forma más rápida es utilizando Docker:

1. Clona el repositorio.
2. Ejecuta `docker compose up --build` en la raíz.
3. Accede a Swagger en: `http://localhost:8080/swagger`

## Características
* Autenticación básica implementada.
* Validaciones robustas en controladores (manejo de conflictos de ID y 404).
* Base de datos persistente mediante volúmenes de Docker.
