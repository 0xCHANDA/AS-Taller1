---
name: solid-srp-csharp
description: Auditar Single Responsibility Principle en C#. Usar cuando clases, servicios o controladores mezclan actores, políticas, persistencia, formato, validación, comunicación o cambian por razones independientes; no usar la longitud como prueba suficiente.
---

# SRP en C#

1. Identificar actores o fuentes de cambio reales: negocio, almacenamiento, presentación, integración, seguridad u operación.
2. Agrupar métodos/campos por invariantes y datos que deben cambiar juntos.
3. Rastrear commits, llamadas o escenarios que demuestren cambios independientes cuando estén disponibles.
4. Rechazar falsos positivos: clase larga pero cohesiva, facade coordinadora o DTO estable.
5. Confirmar solo si responsabilidades independientes fuerzan cambios o pruebas no relacionadas en el mismo tipo.
6. Proponer extracción mínima hacia un límite con nombre de dominio; evitar clases `Manager/Helper` sin contrato claro.

En ASP.NET, revisar especialmente controladores con lógica de negocio/persistencia y servicios que mezclan reglas con archivos, correo o formato. Reportar con el esquema común y sin editar.
