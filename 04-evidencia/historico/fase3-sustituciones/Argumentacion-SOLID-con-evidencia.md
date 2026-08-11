# Argumentación SOLID con evidencia — Fase 3

**Sistema evaluado:** `03-src/redisenado/HaciendaNEW`  
**Corte:** 2026-08-10  
**Estado:** código de producción congelado tras el cierre de `BLOCKER-API-001`. Este documento no propone ni autoriza refactors adicionales.

## Criterio

Una aplicación de SOLID se considera respaldada solo cuando existe un cliente, contrato o presión de cambio concreta. La mera presencia de clases largas, condicionales, herencia o inyección de dependencias no se usa como prueba.

## Trazabilidad consolidada

| Principio | Hallazgo y presión concreta | Cambio materializado | Evidencia exacta | Riesgo reducido | Verificación disponible | Estado / trade-off |
|---|---|---|---|---|---|---|
| **SRP** | Los validadores heredaban cuatro operaciones y cada clase lanzaba `NotImplementedException` en tres; persistencia también construía aspectos técnicos. | Se eliminó la jerarquía de validación ancha. Cada validador implementa una capacidad concreta y la composición del interceptor quedó en MVC. `RegistroVenta` conserva el historial separado de la operación genérica de venta. | `Interfaces/IValidadorRes.cs:5-8`; equivalentes para potrero, vacuna y venta; `Clases/Validaciones/ValidarRes.cs:11-20`; `p_mvcHacienda/Program.cs:29-69`; `Clases/Hacienda.cs:20,30,157-178`. | Evita métodos falsos y separa reglas del dominio de la composición Castle/HTTP. | `HaciendaNEW.Verification/Program.cs:152-267` comprueba validadores y ausencia de métodos ajenos. | **Confirmado como mejora local.** `PersistenciaService` todavía concentra formatos de cinco agregados; se mantiene como deuda, no como blocker del cierre. |
| **OCP** | SC-1 exige vender reses, lácteos, pieles y futuras variantes sin agregar un método de venta por tipo. | `Producto` define el elemento vendible; `IInventarioVendible<T>` define consulta/retiro; una sola implementación `Hacienda.vender<T>` procesa cualquier inventario conforme. | `Producto.cs:9-30`; `IInventarioVendible.cs:8-12`; `Hacienda.cs:145-169`; `Lacteo.cs:9-13`; `Piel.cs:9-13`. | Una variante nueva puede agregarse mediante un `Producto` y un inventario, sin modificar el algoritmo de venta. | `Program.cs:474-579` del verifier cubre res, lácteo, piel, producto definido en el verifier y rechazo atómico de ausente/null. | **Confirmado para el eje “tipo de producto vendible”.** No se afirma que todo condicional del sistema viole OCP. |
| **LSP** | `Res : Producto` tenía estado `Nombre` duplicado y la edad podía aceptar valores en `Res` que setters sobrescritos rechazaban según el subtipo. | `Res` inicializa `Producto` con `base(nombre)` y usa un único `Nombre`. `Edad` es de solo lectura; cada subtipo valida su invariante antes de delegar al constructor base. | `Res.cs:12,23-35`; `Ternero.cs:9-19`; `Cebon.cs:9-19`; `Novillo.cs:9-19`. | Una res observada como producto conserva identidad única; ya no existe un setter virtual que fortalezca precondiciones durante la vida del objeto. | `Program.cs:72-102,590-650` del verifier comprueba identidad como `Producto`, rangos y ausencia de setter público de edad. | **Confirmado como cierre del defecto identificado.** Las categorías etarias continúan modeladas como tipos; cambiar de categoría no forma parte del comportamiento actual. |
| **ISP** | El cliente de venta solo necesita `contiene` y `retirar`, no `agregar`; los consumidores de persistencia no usan todas las operaciones de un puerto monolítico. | Se separó `IInventarioVendible<T>` de `IInventario<T>` y la persistencia se expone mediante cinco interfaces orientadas a capacidades/clientes. Los validadores también tienen contratos específicos. | `IInventario.cs:8-11`; `IInventarioVendible.cs:8-12`; `IPersistencia.cs:6-36`; constructores de `PotreroService`, `ResService`, `VacunaService` y `UsuarioService`. | Los clientes no quedan forzados a depender de operaciones no utilizadas. | `Program.cs:270-290,330-346` del verifier comprueba puertos y contrato vendible estrecho. | **Confirmado.** La sobrecarga histórica con `IInventario<T>` se conserva deliberadamente como fachada de compatibilidad de API. |
| **DIP** | MVC compilaba contra una DLL por `HintPath`; servicios de aplicación dependían de `PersistenciaService`; el dominio dependía de Castle/ASP.NET. | MVC usa `ProjectReference`; `Bib_Hacienda` no referencia paquetes técnicos; servicios dependen de puertos de persistencia y `Program` enlaza interfaces con `PersistenciaService` e interceptores MVC. | `p_mvcHacienda.csproj:14-16`; `Bib_Hacienda.csproj:1-12`; `IPersistencia.cs:6-36`; `Program.cs:29-77,118-129`; `PersistenciaService.cs:13-34`. | El código fuente real participa en el grafo de build; la política no necesita construir Castle ni conocer el adaptador concreto. | `Program.cs:26,270-327` del verifier inspecciona assembly, puertos, referencias técnicas y controladores. | **Confirmado con límites explícitos.** `Program` puede conocer detalles porque es la raíz de composición; la carga inicial aún usa el adaptador concreto y conserva riesgo de hidratación parcial. |

## Cadena de evidencia de SC-1

1. **Presión:** vender productos derivados sin crear `vender_lacteo`, `vender_piel`, etc.
2. **Contrato estable:** `Producto` + `IInventarioVendible<T>`.
3. **Implementaciones actuales:** `Potrero`, `InventarioLacteos` e `InventarioPieles`.
4. **Política única:** `Hacienda.vender<T>(IInventarioVendible<T>, T, uint)`.
5. **Compatibilidad:** se restauró `vender<T>(IInventario<T>, T, uint)` como sobrecarga delegante en `Hacienda.cs:138-142`.
6. **Resultado final focalizado:** Test-Guardian emitió **PASS** y Adversarial-Reviewer declaró `BLOCKER-API-001 CLOSED`, con **0 blockers nuevos**.

## Decisiones conservadoras

- No se dividió `Hacienda` solo por tamaño: sigue siendo el agregado/fachada central mientras no exista evidencia suficiente para otra partición.
- No se creó una interfaz para cada servicio MVC.
- No se introdujo un reloj abstracto sin un requisito temporal demostrado.
- No se reescribió `PersistenciaService`; se invirtió primero la dependencia visible a sus clientes.
- No se eliminó la API histórica de venta: la compatibilidad binaria prevalece sobre la pureza del contrato estrecho.

## Deuda diferida e incertidumbre

- Falta una prueba automatizada con un consumidor binario precompilado contra la firma histórica de `vender`.
- El verifier selecciona por reflexión la sobrecarga estrecha, pero no protege explícitamente la sobrecarga histórica.
- `PersistenciaService` conserva varias razones de cambio relacionadas con formatos y usa contexto HTTP dentro de la capa MVC.
- La hidratación en `Program.cs:80-116` puede publicar estado parcialmente cargado después de una excepción.
- La coexistencia de dos sobrecargas `vender` puede ser ambigua para clientes reflexivos que busquen solo por nombre.
- Estas observaciones no fueron clasificadas como blockers nuevos en la revisión focalizada final.
