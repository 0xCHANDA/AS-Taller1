---
name: architecture-csharp
description: Analizar arquitectura de soluciones C# con MVC, capas, Clean Architecture y puertos/adaptadores. Usar para evaluar límites, responsabilidades de componentes, dirección de dependencias, integración, persistencia y riesgos estructurales antes de refactorizar.
---

# Arquitectura C#

1. Identificar componentes, relaciones, principios rectores y vistas relevantes.
2. Comparar estructura declarada con dependencias reales entre proyectos/namespaces.
3. En MVC, separar HTTP/presentación, coordinación de caso de uso, reglas y persistencia.
4. En Clean Architecture, comprobar que dependencias apunten hacia políticas y dominio.
5. En hexagonal, nombrar puertos por necesidades del núcleo y adaptadores por tecnología.
6. Evaluar atributos de calidad afectados: mantenibilidad, pruebas, escalabilidad, integración, seguridad y operación.
7. No recomendar una arquitectura objetivo completa para corregir un problema local.

Entregar límites confirmados, ciclos, fugas de infraestructura, costos de cambio y el movimiento mínimo de dependencias.
