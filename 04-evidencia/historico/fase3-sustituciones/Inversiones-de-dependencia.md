# Inversiones de dependencia y raíz de composición — Fase 3

**Sistema:** `03-src/redisenado/HaciendaNEW`  
**Corte:** 2026-08-10  
**Formato:** documentación textual, sin UML. El código de producción permanece congelado.

## Regla de diseño

Los módulos que expresan casos de uso dependen de contratos formulados según las capacidades que consumen. Los detalles de archivos, ASP.NET y Castle implementan o decoran esos contratos y se seleccionan en `p_mvcHacienda/Program.cs`, la raíz de composición.

La inyección por constructor no se considera DIP por sí sola; la evidencia relevante es la dirección de cada dependencia.

## Mapa de inversiones materializadas

| Política / cliente de alto nivel | Detalle de bajo nivel | Abstracción usada por el cliente | Implementación actual | Composición y evidencia |
|---|---|---|---|---|
| `PotreroService` | Archivos de potreros y reses | `IPersistenciaPotreros`, `IPersistenciaReses` | `PersistenciaService` | Constructor en `Servicios/PotreroService.cs:13`; bindings en `Program.cs:71-77`. |
| `ResService` | Archivos de reses y ventas | `IPersistenciaReses`, `IPersistenciaVentas` | `PersistenciaService` | Constructor en `Servicios/ResService.cs:12`; bindings en `Program.cs:71-77`. |
| `VacunaService` | Archivos de vacunas, potreros y reses | `IPersistenciaVacunas`, `IPersistenciaPotreros`, `IPersistenciaReses` | `PersistenciaService` | Constructor en `Servicios/VacunaService.cs:14-19`; bindings en `Program.cs:71-77`. |
| `UsuarioService` | Archivo de usuarios | `IPersistenciaUsuarios` | `PersistenciaService` | `UsuarioService.cs:11-15`; fábrica en `Program.cs:123-129`. |
| Política genérica de venta en `Hacienda` | Inventarios concretos | `IInventarioVendible<T>` | `Potrero`, `InventarioLacteos`, `InventarioPieles` y futuras implementaciones conformes | Contrato en `IInventarioVendible.cs:8-12`; política en `Hacienda.cs:145-169`. |
| Persistencia que valida objetos antes de guardar | Validadores concretos y mensajería HTTP | `IValidadorPotrero`, `IValidadorRes`, `IValidadorVacuna`, `IValidadorVenta` | `Validador*` decorados por `InterceptorValidarInformacion` | Dependencias en `PersistenciaService.cs:23-50`; proxies en `Program.cs:29-69`. |
| MVC respecto del dominio compilado | DLL manual potencialmente obsoleta | Referencia de proyecto/fuente | `Bib_Hacienda.csproj` | `p_mvcHacienda.csproj:14-16` usa `ProjectReference`; ya no existe `HintPath`. |

## Propiedad y ubicación de los contratos

Los puertos de persistencia están en `Bib_Hacienda.Interfaces/IPersistencia.cs`. Sus firmas usan conceptos del dominio (`Potrero`, `Res`, `Vacuna`, `Venta`, `Usuario`) y no exponen rutas, archivos, delimitadores, `IWebHostEnvironment` ni `HttpContext`. El adaptador `p_mvcHacienda.Servicios.PersistenciaService` depende de esos contratos y aporta los detalles técnicos.

Esta dirección permite que los servicios de aplicación conozcan únicamente las capacidades necesarias. No se creó un repositorio por entidad ni una interfaz por servicio; las cinco interfaces corresponden a grupos de operaciones que tienen clientes observables distintos.

## Raíz de composición

`p_mvcHacienda/Program.cs` es el único lugar donde se seleccionan y enlazan los detalles principales:

1. registra validadores concretos (`29-33`);
2. registra el interceptor Castle de la capa MVC (`35-36`);
3. crea proxies de las interfaces de validación (`38-69`);
4. registra una instancia de `PersistenciaService` detrás de cinco puertos (`71-77`);
5. construye e hidrata el singleton `Hacienda` (`79-116`);
6. registra los servicios de aplicación (`118-129`).

Que `Program` conozca implementaciones concretas no viola DIP: esa es precisamente la responsabilidad de una raíz de composición. Los controladores no deben asumir esa responsabilidad ni construir persistencia, proxies o dominio.

## Frontera dominio–infraestructura

`Bib_Hacienda/Bib_Hacienda.csproj` es un proyecto SDK `net8.0` sin referencias de paquetes. Castle, `IHttpContextAccessor`, `IWebHostEnvironment` y acceso a archivos permanecen en el proyecto MVC. El verifier contiene controles de que:

- `Bib_Hacienda` no referencia Castle ni ASP.NET (`VerificarBibHaciendaSinDependenciasTecnicas`);
- `PersistenciaService` implementa todos los puertos (`VerificarPuertosPersistencia`);
- los controladores no dependen directamente de `Hacienda` ni de `PersistenciaService` (`VerificarControladoresNoDependenDeDominioPersistencia`).

## Compatibilidad de API frente a pureza del puerto

La política nueva usa el contrato estrecho `IInventarioVendible<T>`. Sin embargo, se conserva además la firma pública histórica:

```csharp
vender<T>(IInventario<T> inventario, T producto, uint monto)
```

Esta sobrecarga delega en la política estrecha. Es un adaptador de compatibilidad dentro de la fachada pública, no una segunda implementación. El cambio mínimo cerró `BLOCKER-API-001`; Test-Guardian emitió **PASS** y Adversarial-Reviewer confirmó **0 blockers** y **0 blockers nuevos**.

## Límites, incertidumbre y deuda diferida

- La hidratación inicial solicita `PersistenciaService` concreto en `Program.cs:83`. Es aceptable dentro de la raíz, pero el bloque `try/catch` puede devolver una `Hacienda` parcialmente cargada; atomicidad de startup queda diferida.
- `PersistenciaService` implementa cinco puertos en una sola clase. Esto reduce acoplamiento de clientes, pero no elimina sus múltiples razones internas de cambio.
- La adaptación de mensajes de validación todavía usa `IHttpContextAccessor` dentro del adaptador MVC. El dominio permanece limpio, aunque persistencia y presentación siguen acopladas en infraestructura.
- No existe un segundo adaptador real; los puertos están justificados por dirección arquitectónica y clientes distintos, no por variedad artificial de implementaciones.
- No se probó un consumidor binario externo precompilado de la API de venta. La firma histórica exacta fue restaurada y la revisión focalizada la declaró cerrada.
- No se autorizan nuevas abstracciones o refactors después del freeze salvo un blocker confirmado, reproducible y respaldado por evidencia concreta.
