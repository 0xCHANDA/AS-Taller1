# Bitácora de decisiones sobre propuestas de IA

Solo se registran decisiones visibles en artefactos de este repositorio; no se inventan consultas humanas.

| Propuesta de IA | Decisión del equipo | Argumento técnico |
|---|---|---|
| Test Guardian propuso PASS aunque faltaban las caracterizaciones obligatorias. | REJECTED | Build y verifier no sustituyen ejecución OLD<->NEW. El rechazo histórico está en el reporte previo y motivó los 11 casos pareados. |
| Agentes de evidencia trataron la restricción de herramientas como imposibilidad de caracterizar. | CORRECTED | El usuario autorizó explícitamente `03-src/phase4/Characterization` como infraestructura no productiva; los runners se ejecutan con el wrapper seguro. |
| Usar `Producto` + `IInventarioVendible<T>` y una venta genérica para SC-1. | ACCEPTED | Existe un eje real (Lacteo, Carne, Piel y variantes externas); Carne se añadió sin tocar la política central. |
| Hacer `Producto.Nombre` inmutable como única salida para SOLID-LSP-001. | CORRECTED | Rompía el setter público observado. La solución menor registra el objeto retornado por `retirar`, garantizando que venta y retiro tienen la misma identidad aun con búsqueda por nombre. |
| Crear una interfaz por cada servicio o implementar todas las cajas del UML aspiracional. | REJECTED | No había cliente ni variación que justificara esas abstracciones; se conservan puertos por capacidades reales y se reemplaza el TO-BE obsoleto por el diseño materializado. |
| Considerar cualquier `switch` una violación OCP. | REJECTED | Los switches etarios y de hidratación no prueban por sí solos presión repetida; OCP se mide únicamente en el eje SC-1. |
| Cambiar mensajes y reglas legacy para "mejorarlos". | REJECTED | OLD es autoridad conductual. C07 y C10 se corrigieron en sentido inverso: NEW volvió a las salidas OLD. |
| Implementar SC-3 (historia clínica) como SC principal en lugar de SC-1. | REJECTED | SC-1 ofrece un eje OCP más aislable (producto vendible) y menor acoplamiento con reglas de dominio complejas. SC-3 se analizó pero no se implementó (`SC3-ANALIZADA-NO-IMPLEMENTADA.md`). |
| Generar PNG del TO-BE-FINAL con herramienta externa. | REJECTED (bloqueo técnico) | No hay PlantUML instalado en el host ni permiso para instalar software. El `.puml` editable es normativo. Ver `TO-BE-FINAL-ENRICHED.puml`. |
| Marcar archivos `02-diseno/` como SUPERSEDED. | ACCEPTED (parcialmente bloqueado) | Los archivos en `02-diseno/` están fuera del perímetro de escritura del evidence engineer. Se documenta en `TRAZABILIDAD-FINAL.md` y se crean versiones canónicas en `04-evidencia/`. |
| Extraer `FabricadorVacunas` desde Hacienda para mejorar SRP (cuatro `crear_vacuna` con ~200 líneas de duplicación). | ACEPTADO | Responsabilidad única: la fachada Hacienda delega creación de vacunas a una clase concreta pequeña; sigue acoplada a Bacteriana/Viva (deuda consciente: solo dos tipos). No se introdujo factory registry, builder ni reflection. |
| Crear `Hacienda(RegistroVenta, FabricadorVacunas)` para mostrar DIP por constructor. | ACEPTADO | Demostración pedagógica de DIP en `Program.cs`. Se conserva el constructor sin argumentos para no romper consumidores legacy (Verificación, Characterization, Demo y MVC). |
| Convertir `l_potreros`/`l_vacunas` en `readonly` aprovechando la nueva extracción. | CORREGIDO | El `private set` no es `init` y C# no permite asignar a `readonly` desde un setter no-init. Se conservó la semántica no-readonly original (esos campos no se reasignan en producción, pero el setter privado asegura la API observable). |
| Crear una `IVacunaFactory` con reflection/registry para centralizar Bacteriana/Viva. | RECHAZADO | Solo existen dos tipos de vacuna. Una jerarquía completa con reflection añade complejidad sin cliente ni variación real. La clase concreta `FabricadorVacunas` documenta la deuda y sigue siendo legible. |
| Crear una interfaz `IFabricadorVacunas` por simetría DIP. | RECHAZADO | No hay cliente distinto de Hacienda que requiera la abstracción. La inyección por constructor ya cumple DIP sin crear un puerto sin uso. |
| Mover la `Hacienda.alimentar_res` completa a un servicio separado. | RECHAZADO | La operación coordina Potrero, Res y eventos; extraerla multiplica actores sin reducir el cambio presión. La fachada la conserva como una sola responsabilidad: alimentar una res. |
