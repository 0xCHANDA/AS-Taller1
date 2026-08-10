# Bitácora de decisiones sobre propuestas de IA

Solo se registran decisiones visibles en artefactos de este repositorio; no se inventan consultas humanas.

| Propuesta de IA | Decisión del equipo | Argumento técnico |
|---|---|---|
| Test Guardian propuso PASS aunque faltaban las caracterizaciones obligatorias. | REJECTED | Build y verifier no sustituyen ejecución OLD↔NEW. El rechazo histórico está en el reporte previo y motivó los 11 casos pareados. |
| Agentes de evidencia trataron la restricción de herramientas como imposibilidad de caracterizar. | CORRECTED | El usuario autorizó explícitamente `03-src/phase4/Characterization` como infraestructura no productiva; los runners se ejecutan con el wrapper seguro. |
| Usar `Producto` + `IInventarioVendible<T>` y una venta genérica para SC-1. | ACCEPTED | Existe un eje real (Lacteo, Carne, Piel y variantes externas); Carne se añadió sin tocar la política central. |
| Hacer `Producto.Nombre` inmutable como única salida para SOLID-LSP-001. | CORRECTED | Rompía el setter público observado. La solución menor registra el objeto retornado por `retirar`, garantizando que venta y retiro tienen la misma identidad aun con búsqueda por nombre. |
| Crear una interfaz por cada servicio o implementar todas las cajas del UML aspiracional. | REJECTED | No había cliente ni variación que justificara esas abstracciones; se conservan puertos por capacidades reales y se reemplaza el TO-BE obsoleto por el diseño materializado. |
| Considerar cualquier `switch` una violación OCP. | REJECTED | Los switches etarios y de hidratación no prueban por sí solos presión repetida; OCP se mide únicamente en el eje SC-1. |
| Cambiar mensajes y reglas legacy para “mejorarlos”. | REJECTED | OLD es autoridad conductual. C07 y C10 se corrigieron en sentido inverso: NEW volvió a las salidas OLD. |
