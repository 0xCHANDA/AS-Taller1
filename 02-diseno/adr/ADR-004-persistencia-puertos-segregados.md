# ADR-004 — Persistencia con puertos segregados vs concreto directo

**Estado:** ACEPTADO (retrospectivo)
**Fecha:** 2026-08-10
**Evidencia:** `IPersistencia.cs:6-36`, `PersistenciaService.cs:13-34`, `Program.cs:71-77`, `02-diseno/DISENO-TO-BE.md` (§8–9)

## Contexto

OLD expone persistencia como `PersistenciaService` concreto: servicios MVC dependen directamente de la clase que mezcla archivos TXT, validación, Castle y HTTP. Cambiar formato de archivo, ruta, delimitador o framework de logging obliga a modificar una clase usada por todos los servicios.

La rúbrica exige DIP demostrable y un composition root explícito. El dolor #1 priorizado es precisamente persistencia.

## Alternativas

| ID | Descripción | Evaluación |
|---|---|---|
| A | Mantener `PersistenciaService` como dependencia concreta directa en cada servicio | **Descartada.** Viola DIP; todo servicio conoce formato de archivo y delimitadores. |
| B | Un repositorio genérico `IRepository<T>` | **Descartada.** Oculta operaciones específicas (`CargarPotrerosConReses`, `GuardarVacuna` con firma no uniforme). ISP: clientes usarían solo un subconjunto. |
| C | Cinco interfaces segregadas por cliente: `IPersistenciaPotreros`, `IPersistenciaReses`, `IPersistenciaVacunas`, `IPersistenciaVentas`, `IPersistenciaUsuarios` | **Elegida.** Cada cliente depende exactamente de las capacidades que usa. |

## Decisión

Alternativa C. `Bib_Hacienda.Interfaces/IPersistencia.cs` declara cinco interfaces en el namespace del dominio, usando tipos del dominio (`Potrero`, `Res`, `Vacuna`, `Venta`, `Usuario`). `PersistenciaService` en la capa MVC implementa todas. `Program.cs:71-77` registra una instancia concreta detrás de los cinco puertos. El composition root es el único lugar que conoce el adaptador concreto.

## Consecuencias

- **Positivo:** `PotreroService` solo recibe `IPersistenciaPotreros` + `IPersistenciaReses`; no conoce `IPersistenciaVacunas`.
- **Positivo:** Cambiar formato TXT a JSON o base de datos solo afecta a `PersistenciaService`, no a los servicios de aplicación.
- **Negativo:** `PersistenciaService` implementa cinco puertos y concentra múltiples razones de cambio internas (formatos, rutas, encoding). Es deuda técnica consciente.
- **Negativo:** No existe un segundo adaptador real; los puertos están justificados por dirección arquitectónica y clientes distintos, no por variedad de implementaciones.

## Principios SOLID

- **DIP:** `PotreroService` → `IPersistenciaPotreros` ← `PersistenciaService`.
- **ISP:** Cinco contratos en vez de un monolito de persistencia.
- **SRP (deuda):** `PersistenciaService` sigue siendo grande internamente; no se dividió sin presión de cambio en formato.

## Verificación

- `02-diseno/DISENO-TO-BE.md` (§8–9): mapa completo de inversiones.
- `HaciendaNEW.Verification/Program.cs`: `VerificarPuertosPersistencia` comprueba que `PersistenciaService` implementa los cinco puertos.
- `Bib_Hacienda.csproj:1-12`: sin referencias a Castle, ASP.NET ni acceso a archivos.
