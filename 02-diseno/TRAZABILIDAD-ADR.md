# TRAZABILIDAD-ADR — Hallazgos → ADR → Elemento TO-BE

**Sistema:** `03-src/redisenado/HaciendaNEW`
**Corte:** 2026-08-10
**Origen:** diagnósticos AS-IS documentados en `01-diagnostico/inventario de hallazgos.docx` y `01-diagnostico/puntos de dolor priorizados.docx`.

## Tabla de trazabilidad

| Hallazgo | Problema | ADR | Elemento TO-BE | SOLID | Beneficio |
|---|---|---|---|---|---|
| H-01 | `IValidarInformacion` monolítica con `NotImplementedException` en cada validador. | ADR-004 | `IValidadorRes`, `IValidadorPotrero`, `IValidadorVacuna`, `IValidadorVenta` (cuatro interfaces segregadas). | ISP | Clientes forzados solo a capacidades reales. |
| H-02 | `PersistenciaService` monolítico: servicios MVC dependen de un solo concreto. | ADR-004 | Cinco interfaces segregadas (`IPersistenciaPotreros`, `IPersistenciaReses`, `IPersistenciaVacunas`, `IPersistenciaVentas`, `IPersistenciaUsuarios`). | DIP, ISP | Una instancia, cinco contratos; dirección de dependencia. |
| H-03 | `Res.Edad` setter virtual fortalecía precondiciones. | ADR-005 | `Edad` solo lectura; validación en constructor de cada subtipo. | LSP | Sustituibilidad Res -> Ternero/Cebon/Novillo. |
| H-04 | `Res : Producto` con `Nombre` duplicado. | ADR-005 | Constructor invoca `base(nombre)`; una sola identidad observable. | LSP | Identidad compartida. |
| H-05 | SC-1: vender productos derivados sin método por tipo. | ADR-002 | `Producto` + `IInventarioVendible<T>` + `Hacienda.vender<T>`. | OCP, ISP | Una política, muchas implementaciones. |
| H-06 | `Potrero.agregar` no-op silencioso. | ADR-005 | `agregar` valida tipo/edad/duplicado/capacidad. | LSP | Comportamiento contractual observable. |
| H-07 | `PersistenciaService` con archivos TXT inline. | ADR-004 | Puertos en `Bib_Hacienda.Interfaces`; implementación en MVC. | DIP | Inversión real. |
| H-08 | `Hacienda` con cuatro `crear_vacuna` (~200 líneas). | ADR-006 | `FabricadorVacunas` con métodos `Crear` y `CrearLote`. | SRP | Una responsabilidad por clase. |
| H-09 | `Hacienda` con `new RegistroVenta()` inline. | ADR-006 | Constructor `Hacienda(RegistroVenta, FabricadorVacunas)`. | DI (no DIP) | Construcción externalizada en `Program.cs`. |

## Cadena de evidencia

- **H-01..H-07:** documentados en `02-diseno/adr/ADR-001..005.md` (originales conservados).
- **H-08..H-09:** incorporados como consecuencia del refactor 2026-08-10. Documentados en `04-evidencia/bitacora-ia/BITACORA-IA.md` (entradas 17-18) y `04-evidencia/metricas/SC1-METRICA-OCP.md` (sección "Refactor 2026-08-10").

## Estado de trazabilidad

- **Trazabilidad Hallazgo → ADR → elemento TO-BE:** COMPLETA.
- **Correspondencia TO-BE ↔ código:** verificada contra `04-evidencia/characterization/CHARACTERIZATION-MATRIX.md` y `04-evidencia/trazabilidad/TOBE-CODE-MATRIX.md`.
- **Caracterización conductual:** 19 MATCH, 1 DELIBERATE_STRUCTURAL (C20), 0 BEHAVIORAL_MISMATCH. Ver `04-evidencia/characterization/CHARACTERIZATION-MATRIX.md`.
