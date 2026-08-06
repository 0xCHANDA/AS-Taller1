---
name: csharp-codebase-map
description: Cartografiar soluciones C#/.NET antes de una auditoría o refactorización. Usar para descubrir proyectos, frameworks, entry points, composición, capas, dependencias, contratos públicos, persistencia, controladores, servicios y cobertura de pruebas.
---

# Cartografía de código C#

1. Localizar `.sln`, `.csproj`, target frameworks, paquetes y proyectos de prueba.
2. Identificar entry points y composition roots (`Program.cs`, `Startup`, configuración DI).
3. Mapear namespaces, referencias de proyecto y dirección de dependencias.
4. Localizar UI/controladores, aplicación/servicios, dominio/modelos e infraestructura/persistencia.
5. Registrar interfaces, bases públicas, DTOs, eventos y formatos persistidos como contratos sensibles.
6. Buscar reflexión, serialización, archivos, base de datos, red, tiempo y estáticos globales.
7. Ejecutar solo comprobaciones apropiadas y registrar baseline, fallos preexistentes y desconocidos.

Entregar un mapa factual; no convertir nombres de carpetas en arquitectura asumida ni diagnosticar SOLID todavía.
