---
name: solid-isp-csharp
description: Auditar Interface Segregation Principle en C#. Usar cuando consumidores dependen de miembros que no usan, implementaciones lanzan excepciones o mocks requieren configuración irrelevante; evaluar interfaces desde clientes reales, no por cantidad de métodos.
---

# ISP en C#

1. Construir una matriz cliente → miembros usados para cada interfaz candidata.
2. Detectar implementaciones vacías, miembros sin sentido y cambios que recompilan clientes no relacionados.
3. Separar capacidades por rol del consumidor solo cuando existan grupos de uso estables.
4. Preservar una interfaz cohesiva aunque sea amplia si sus clientes requieren el contrato completo.
5. Considerar interfaces de solo lectura/escritura, command/query y adaptadores; evitar interfaces de un método sin beneficio real.
6. Revisar impacto en DI, mocks, API pública, versionado y descubribilidad.

Confirmar con clientes concretos y proponer la segregación mínima compatible.
