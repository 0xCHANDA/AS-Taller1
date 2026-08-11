# Fase 3 — Diseño de la nueva arquitectura TO-BE

**Sistema:** `03-src/redisenado/HaciendaNEW`
**Corte:** 2026-08-10
**Estado:** código de producción congelado tras cierre de Fase 4. Documento consolidado único de Fase 3.

> Este documento describe el código FINAL. No es una aspiración. Cada elemento citado en UML, SOLID, herencias, DIP y ADR existe o se ha verificado contra el código fuente en `03-src/redisenado/HaciendaNEW`.

---

## 1. Objetivo

Documentar la arquitectura TO-BE materializada de HaciendaNEW, los argumentos SOLID que la sostienen, las herencias justificadas, las inversiones de dependencia y la trazabilidad hacia los ADR. El documento cumple los requisitos del enunciado:

1. Diagrama UML TO-BE en notación extendida.
2. Convención de color (negro = conservado; color = intervenido; principio SOLID).
3. Cinco SOLID aplicados y argumentados.
4. Herencias justificadas con verificación LSP (precondiciones, postcondiciones, invariantes, excepciones).
5. Inversiones de dependencia con alto nivel, bajo nivel, abstracción, construcción y composition root.
6. Mínimo cinco ADR.

## 2. Del AS-IS al TO-BE

Cadena de intervenciones que sobrevivieron a Fase 4:

| Hallazgo | Problema | Cambio | Beneficio | SOLID |
|---|---|---|---|---|
| H-01 `IValidarInformacion` monolítico | Métodos `NotImplementedException` en validadores | Cuatro interfaces segregadas (`IValidadorRes`, `IValidadorPotrero`, `IValidadorVacuna`, `IValidadorVenta`) | Clientes obligados solo a depender de capacidades reales | ISP |
| H-02 `PersistenciaService` monolítico | Servicios MVC dependen de un solo concreto | Cinco puertos segregados (`IPersistenciaPotreros`, etc.) | Una instancia, cinco contratos; dirección de dependencia | DIP, ISP |
| H-03 `Res.Edad` con setter virtual | Override fortalecía precondiciones (LSP) | `Edad` solo lectura; validación en constructor de cada subtipo | Sustituibilidad Res -> Ternero/Cebon/Novillo sin rechazos polimórficos | LSP |
| H-04 `Res : Producto` con `Nombre` duplicado | Identidad divergente | Constructor invoca `base(nombre)` | Una sola identidad observable | LSP |
| H-06 `Potrero` con `agregar` no-op silencioso | Inventario sin semántica real | `agregar` valida tipo/edad/duplicado/capacidad | Comportamiento contractual observable | LSP |
| H-07 `PersistenciaService` con archivos TXT inline | Dominio conoce infraestructura | Puertos en `Bib_Hacienda.Interfaces`, implementación en MVC | Inversión real de dependencias | DIP |
| H-08 `Hacienda` con cuatro `crear_vacuna` (~200 líneas) | Fachada concentrando creación de vacunas | Extracción de `FabricadorVacunas` (encabezado 2026-08-10) | Una responsabilidad por clase | SRP |
| H-09 `Hacienda` con `new RegistroVenta()` inline | Construcción acoplada dentro de la fachada | Constructor `Hacienda(RegistroVenta, FabricadorVacunas)` | Construcción externalizada en `Program.cs` | DI (no DIP) |

> La métrica SC-1 contractual se documenta en §13 (Métrica SC-1). Para añadir la terna de productos (Lacteo + Carne + Piel) completa, en OLD habría sido necesario modificar las clases existentes del modelo y la política de venta; en NEW solo se agregaron las clases derivadas.

## 3. Diagrama TO-BE

### 3.1 Fuente canónica

- **PlantUML editable:** `02-diseno/diagramas/TO-BE.puml`
- **Render PNG:** `02-diseno/diagramas/TO-BE.png`.

### 3.2 Convención de color

| Color | Significado |
|---|---|
| Blanco/negro | Conservado del AS-IS sin intervención estructural |
| Verde (#E8F5E9) | OCP — punto de extensión |
| Azul (#E3F2FD) | ISP — interfaz segregada por capacidad |
| Rosa (#FCE4EC) | DIP — puerto de persistencia |
| Violeta (#F3E5F5) | LSP — jerarquía validada |
| Naranja (#FFF3E0) | SRP — responsabilidad separada |
| gris (#ECEFF1) | Infraestructura / composition root |
| amarillo (#FFFDE7) | SC-1 variante nueva (Carne) |

### 3.3 Elementos del modelo

Para cada clase/interfaz relevante, el PUML documenta:

- Nombre.
- Atributos con tipo y visibilidad.
- Operaciones con parámetros, tipos y retorno.
- Visibilidad (`+`, `-`, `#`).
- Relaciones: herencia, realización, asociación, agregación, composición, dependencia.
- Multiplicidades relevantes.
- Notas: SRP/OCP/LSP/ISP/DIP.

## 4. SRP — Single Responsibility Principle

### 4.1 ¿Qué problemática concreta había en AS-IS?

`Hacienda` concentraba:

- Coordinación de potreros y reses.
- Venta (genérica y legacy).
- Creación de vacunas (cuatro overloads de `crear_vacuna`).
- Aplicación de vacunas.
- Disparo de eventos de peso y vacunación.
- Gestión de inventario de vacunas.

La creación de vacunas por sí sola ocupaba ~180 líneas con cuatro copias casi idénticas de validación.

### 4.2 ¿Dónde estaba?

- `Clases/Hacienda.cs:253-432` (en la versión previa al refactor).

### 4.3 ¿Por qué afecta SRP?

Porque `Hacienda` respondía a múltiples change drivers:

- Cambios en la lógica de venta (driver 1).
- Cambios en el formato de creación de vacunas (driver 2).
- Cambios en los eventos publicados (driver 3).
- Cambios en la coordinación de potreros/reses (driver 4).

Cada driver obligaba a modificar la misma clase.

### 4.4 ¿Qué cambió?

Se extrajo `FabricadorVacunas` (visible en `02-diseno/diagramas/TO-BE.puml`) con responsabilidad única: crear y añadir vacunas al inventario. `Hacienda` delega las cuatro sobrecargas de `crear_vacuna` a `FabricadorVacunas.Crear / CrearLote`.

`RegistroVenta` mantiene la separación ya lograda: registro de ventas como agregado separado, encapsulado en una clase con responsabilidad única (mantener historial de ventas).

### 4.5 ¿Por qué esa frontera y no otra?

- `FabricadorVacunas` agrupa todas las operaciones de creación de vacunas. La frontera coincide con el cambio driver: cuando cambien las reglas de creación de vacunas, solo se modifica `FabricadorVacunas`.
- `RegistroVenta` agrupa el historial de ventas. La frontera coincide con el cambio driver: persistir nuevas ventas (sin tocar la lógica de venta).
- `Hacienda` se conserva como fachada de coordinación: una sola responsabilidad (cohesiva) consistente con la fachada del agregado.

### 4.6 ¿Qué alternativa más simple o más compleja evaluamos?

- **Más simple:** no extraer `FabricadorVacunas`, dejar las cuatro copias en `Hacienda`. **Rechazada:** violaba SRP y duplicaba validación.
- **Más compleja:** introducir una `IVacunaFactory` con reflection y registro de tipos. **Rechazada:** solo existen dos tipos (`Bacteriana`, `Viva`); añadir la abstracción completa incrementaba la complejidad sin cliente ni variación real. Ver `04-evidencia/bitacora-ia/BITACORA-IA.md`, decisión sobre la fábrica de vacunas.

### 4.7 ¿Qué gana el sistema?

- `Hacienda` pasa de 464 a 301 líneas.
- Las validaciones de creación de vacunas están centralizadas en `FabricadorVacunas.ValidarDatosBasicos` y `ValidarCantidadYLoteBase`.
- Las cuatro sobrecargas se reducen a cinco líneas de delegación en `Hacienda`.

### 4.8 ¿Qué costo/trade-off aceptamos?

- `FabricadorVacunas` sigue acoplada a las clases concretas `Bacteriana` y `Viva`. Si aparece un tercer tipo de vacuna, la fábrica concreta debe modificarse. Es una decisión consciente documentada en `04-evidencia/bitacora-ia/BITACORA-IA.md`.

### 4.9 ¿Dónde está en el código final?

- `03-src/redisenado/HaciendaNEW/Bib_Hacienda/Bib_Hacienda/Clases/FabricadorVacunas.cs` (170 líneas).
- `03-src/redisenado/HaciendaNEW/Bib_Hacienda/Bib_Hacienda/Clases/Hacienda.cs:250-272` (delegación).
- `03-src/redisenado/HaciendaNEW/Bib_Hacienda/Bib_Hacienda/Clases/RegistroVenta.cs` (23 líneas).

### 4.10 ¿Cómo se demuestra?

- Compilación NEW: PASS con `dotnet build` sobre los proyectos de dominio y MVC.
- Verifier: `VerificarVentaGenericaRes/Lacteo/Carne/Piel` siguen pasando, evidencia que la fachada sigue operativa.

## 5. OCP — Open/Closed Principle

### 5.1 Eje principal: SC-1 — productos derivados

La Hacienda vende (o venderá) productos derivados del ganado: lácteos, carne, piel. El eje de variación es "tipo de producto vendible".

### 5.2 ¿Qué cambia en el modelo?

- `Producto` (abstracta) con `Nombre` validado.
- `IInventarioVendible<T>` con `contiene(T)` y `retirar(T)`.
- `IInventario<T> : IInventarioVendible<T>` añade `agregar(T)`.
- `InventarioLacteos`, `InventarioCarnes`, `InventarioPieles` implementan `IInventario<T>`.
- `Hacienda.vender<T>(IInventarioVendible<T>, T, uint)` ejecuta la política genérica: valida entradas, retira del inventario, registra venta.

### 5.3 Diseño real

ANTES (sin `Producto`):

- `vender_res(...)` solo vendía una `Res` dentro de un `Potrero`.
- Cada nuevo tipo de producto exigía un nuevo método en `Hacienda`.

DESPUÉS:

- `vender<T>(IInventarioVendible<T>, T, uint)` vende cualquier `Producto` conocido mediante un inventario que cumpla `IInventarioVendible<T>`.
- `PersistenciaService` usa el formato `V2|<fecha>|<monto>|<tipo>|<nombre>` que registra el `GetType().Name` del producto. Tipos no conocidos al recargar se reconstruyen como `ProductoPersistido` (snapshot estable: nombre original + tipo).

### 5.4 Evidencia empírica (no hipotética)

La métrica real de Fase 4 (`04-evidencia/metricas/SC1-METRICA-OCP.md`) muestra que añadir **Carne** requirió:

- 0 clases existentes modificadas.
- 2 clases nuevas: `Carne.cs` (`Bib_Hacienda/Clases/Carne.cs`) y `InventarioCarnes.cs` (`Bib_Hacienda/Clases/InventarioCarnes.cs`).
- 0 archivos existentes modificados.
- 2 archivos nuevos.

### 5.5 Límite explícito

El segundo eje natural de variación — "agregar un nuevo tipo de vacuna" — sigue dependiendo de extender `ICreacionVacuna`, `FabricadorVacunas` y `Hacienda.crear_vacuna`. No se introdujo una `IVacunaFactory` con reflection porque solo existen dos tipos (`Bacteriana`, `Viva`); añadir la abstracción completa incrementaría la complejidad sin cliente ni variación real. Ver `04-evidencia/bitacora-ia/BITACORA-IA.md`, decisiones sobre la fábrica de vacunas y alimentar_res.

### 5.6 ¿Qué ganamos / qué aceptamos?

- **Ganamos:** una política de venta única; nuevas variantes = nuevas implementaciones.
- **Aceptamos:** el eje "tipo de vacuna" no está cerrado a modificación. Deuda consciente.

## 6. LSP — Liskov Substitution Principle

### 6.1 Criterio

Para cada jerarquía se revisaron:

- Por qué ES-UN.
- Por qué herencia y no composición.
- Precondiciones.
- Postcondiciones.
- Invariantes.
- Excepciones.

### 6.2 Matriz de herencias

| # | Superclase | Subclase | ¿Por qué ES-UN? | ¿Por qué herencia y no composición? | Precondiciones | Postcondiciones | Invariantes | Excepciones | Veredicto |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Producto` | `Res` | Una Res ES UN Producto vendible que además tiene peso, edad y vacunas aplicadas. | La identidad vendible es la superclase; tener una jerarquía de productos vendibles_polimórficos tiene sentido. | No fortalece las precondiciones de `Producto(nombre)`. | Conserva el nombre de `Producto`. | `Nombre` único, inmutable tras construcción. | Mismas que `Producto`. | PASS |
| 2 | `Producto` | `Lacteo` | Un lácteo ES UN producto vendible con nombre. | No aporta estado nuevo; la composición no aportaría valor. | No fortalece precondiciones. | Conserva el nombre. | Mismas que `Producto`. | Mismas. | PASS |
| 3 | `Producto` | `Carne` | La carne ES UN producto vendible con nombre. | Igual que `Lacteo`. | No fortalece precondiciones. | Conserva el nombre. | Mismas. | Mismas. | PASS |
| 4 | `Producto` | `Piel` | La piel ES UN producto vendible con nombre. | Igual que `Lacteo`. | No fortalece precondiciones. | Conserva el nombre. | Mismas. | Mismas. | PASS |
| 5 | `Res` | `Ternero` | Un ternero ES UNA res con edad 0-12 meses. | La categoría etaria tiene semántica específica en el dominio (peso mínimo, vacunas, tipo de potrero). | Acepta edad 0-12 (`Ternero.cs:13-19`). | `MaxVacunasBacterianas = 3`, `MaxVacunasVivas = 1`. | Edad se fija una vez en el constructor. | Rechaza edad > 12 (`Exception`). | PASS |
| 6 | `Res` | `Cebon` | Un cebón ES UNA res con edad 13-48 meses. | Igual que `Ternero`. | Acepta edad 13-48 (`Cebon.cs:13-19`). | `MaxVacunasBacterianas = 1`, `MaxVacunasVivas = 4`. | Edad fija una vez. | Rechaza edad ≤ 12 o > 48. | PASS |
| 7 | `Res` | `Novillo` | Un novillo ES UNA res con edad > 48 meses. | Igual que `Ternero`. | Acepta edad > 48 (`Novillo.cs:13-19`). | `MaxVacunasBacterianas = 2`, `MaxVacunasVivas = 2`. | Edad fija una vez. | Rechaza edad ≤ 48. | PASS |
| 8 | `Vacuna` | `Bacteriana` | Una vacuna bacteriana ES UNA vacuna con período de aplicación. | El tipo biológico tiene semántica distinta (período de aplicación, límites). | `periodo >= 2 && <= 4` (`Bacteriana.cs:24-26`). | `Tipo = Bacteriana`. | Lote único. | Rechaza fuera de rango. | PASS |
| 9 | `Vacuna` | `Viva` | Una vacuna viva ES UNA vacuna con grado de atenuación. | Igual que `Bacteriana`. | `enum_l_atenuaciones` (10, 20, 30). | `Tipo = Viva`. | Lote único. | Sin excepciones nuevas. | PASS |
| 10 | `IInventario<Res>` | `Potrero` | `Potrero` cumple el contrato de inventario de reses (`agregar`, `contiene`, `retirar`). | Es una realización de interfaz, no herencia de clase. La frontera refleja el rol de "inventario de reses" — bajo la rúbrica del enunciado, las herencias de clase son Producto/Res/Vacuna. | LSP contractual: `agregar` con semántica real; sin `NotImplementedException`; sin precondiciones ocultas. | Métodos operacionales observables. | Mismas. | Mismas. | PASS |

### 6.3 Defecto conocido cerrado

El defecto original era que `Res.Edad` tenía un setter público virtual que permitía que un `Ternero` rechazara una edad que la base `Res` aceptaba. Esto fortalecía precondiciones en una operación polimórfica. Solución:

- `Res.Edad` ahora es solo lectura (`Res.cs:34`).
- Cada subtipo valida su rango en su propio constructor antes de delegar al constructor base.
- Ningún método común de `Res` muta la edad.

Ver `ADR-005` y la matriz consolidada de esta sección.

### 6.4 Herencia vs composición — por qué no composición

Para las jerarquías de `Producto`/`Res`/`Vacuna`:

- **ES-UN real:** la operación común de cada subclase responde a `código base` y `código derivado` con la misma semántica. Una `Res` puede leerse como `Producto` sin sorpresas.
- **Polimorfismo justificado:** la venta genérica trata cualquier `Producto` polimórficamente.
- **Composición no aportaría:** no existe un "campo Res" dentro de `Producto` que sea intercambiable. La categoría etaria y biológica es identidad, no estado secundario.

Las realizaciones de interfaz como `Potrero : IInventario<Res>` no entran en la pregunta "herencia de clase vs composición" porque son contratos. La pregunta aplica solo a jerarquías de clase (Producto, Res, Vacuna).

### 6.5 Herencias no problemáticas

`ProductoPersistido` también es `Producto`, pero se usa solo en carga desde persistencia para tipos no conocidos. No afecta el código de negocio.

### 6.6 Realización de interfaz (no herencia de clase)

> Las realizaciones de interfaz en el proyecto son:
> - `Potrero : IInventario<Res>` — capacidad de inventario de reses.
> - `InventarioLacteos`, `InventarioCarnes`, `InventarioPieles : IInventario<T>` — capacidad de inventario de productos derivados.
> - `ValidadorRes : IValidadorRes`, etc. — capacidades de validación.
> - `PersistenciaService : IPersistencia*` (cinco puertos).
> - `Hacienda : IVacunacion`, `IVentaRes`, `ICreacionVacuna` — roles del modelo.
>
> Estas realizaciones son LSP contractual: el cumplimiento de la interfaz es completo o no se compila. La sección §6.2 se concentra en **herencias de clase** (Producto, Res, Vacuna), como requiere la rúbrica.

## 7. ISP — Interface Segregation Principle

### 7.1 Criterio

ISP se demuestra desde los CLIENTES, no por el tamaño de la interfaz. Para cada interfaz se identifican:

- Quiénes la consumen.
- Qué operaciones ofrece.
- Qué operaciones usa cada cliente.
- Si algún cliente depende de algo que no necesita.

### 7.2 Matriz de interfaces

| # | Interfaz | Ofrece | Cliente(s) | Métodos usados | ¿Cliente obligado a algo innecesario? |
|---|---|---|---|---|---|
| 1 | `IInventarioVendible<T>` | `contiene`, `retirar` | `Hacienda.vender<T>` | ambos | NO. Diseñado específicamente para venta. |
| 2 | `IInventario<T>` | `agregar` (extiende) | `Potrero`, `InventarioLacteos`, `InventarioCarnes`, `InventarioPieles` | `agregar`, `contiene`, `retirar` | NO. Extensión deliberada para casos que también necesitan alta. |
| 3 | `IValidadorRes` | `ValidarRes` | `PersistenciaService` (vía `IInterceptor`) | `ValidarRes` | NO. Un solo método. |
| 4 | `IValidadorPotrero` | `ValidarPotrero` | `PersistenciaService` | `ValidarPotrero` | NO. |
| 5 | `IValidadorVacuna` | `ValidarVacuna` | `PersistenciaService` | `ValidarVacuna` | NO. |
| 6 | `IValidadorVenta` | `ValidarVenta` | `PersistenciaService` | `ValidarVenta` | NO. |
| 7 | `IPersistenciaPotreros` | `CargarPotreros`, `GuardarPotreros` | `PotreroService` | ambos | NO. |
| 8 | `IPersistenciaReses` | `CargarReses`, `GuardarReses` | `PotreroService`, `ResService`, `VacunaService` | ambos | NO. |
| 9 | `IPersistenciaVacunas` | `CargarVacunas`, `GuardarVacunas`, `CargarVacunasAplicadas`, `GuardarVacunasAplicadas` | `VacunaService` | todos | NO. |
| 10 | `IPersistenciaVentas` | `CargarVentas`, `GuardarVentas` | `ResService`, `VentaService` | ambos | NO. |
| 11 | `IPersistenciaUsuarios` | `CargarUsuarios`, `GuardarUsuarios` | `UsuarioService` | ambos | NO. |
| 12 | `IVacunacion` | `aplicar_vacuna` | esperada por consumidores externos | `aplicar_vacuna` | NO. |
| 13 | `IVentaRes` | `vender_res` | esperada por consumidores externos | `vender_res` | NO. |
| 14 | `ICreacionVacuna` | cuatro overloads `crear_vacuna` | esperada por consumidores externos | todos | NO. |
| 15 | `IAutenticacion` | `AutorizarOperacion` | histórica, no usada activamente | `AutorizarOperacion` | OBSOLETO. No representa cliente real. |

### 7.3 ANTES (defecto cerrado)

OLD tenía `IValidarInformacion` monolítica con cuatro métodos. Cada validador heredaba y lanzaba `NotImplementedException` en tres de ellos. Esto violaba ISP: cada cliente de validación estaba forzado a depender de métodos que no necesitaba.

### 7.4 DESPUÉS

- Cuatro interfaces segregadas.
- Cada `Validador*` implementa una sola interfaz.
- La composición de proxies Castle se realiza en `Program.cs`, no en el dominio.

## 8. DIP — Dependency Inversion Principle

### 8.1 Mapa de inversiones

| Política (alto nivel) | Abstracción | Detalle (bajo nivel) | Implementación | Composición |
|---|---|---|---|---|
| `PotreroService` | `IPersistenciaPotreros`, `IPersistenciaReses` | archivos TXT en MVC | `PersistenciaService` | `Program.cs:71-77` |
| `ResService` | `IPersistenciaReses`, `IPersistenciaVentas` | archivos TXT en MVC | `PersistenciaService` | `Program.cs:71-77` |
| `VacunaService` | `IPersistenciaVacunas`, `IPersistenciaPotreros`, `IPersistenciaReses` | archivos TXT en MVC | `PersistenciaService` | `Program.cs:71-77` |
| `VentaService` | `Hacienda` (modelo de dominio) | `Hacienda` (concreto) | `Hacienda` | inyección por constructor |
| `UsuarioService` | `IPersistenciaUsuarios` | archivo TXT en MVC | `PersistenciaService` | `Program.cs:123-129` |
| `Hacienda.vender<T>` | `IInventarioVendible<T>` | inventarios concretos | `Potrero`, `InventarioLacteos`, `InventarioCarnes`, `InventarioPieles` | parámetros en llamada |
| `PersistenciaService` | `IValidador*` | validadores concretos decorados | `Validador*` con `InterceptorValidarInformacion` | `Program.cs:29-69` |

### 8.2 Composition root

`p_mvcHacienda/Program.cs` es el **único** lugar donde se conocen las implementaciones concretas y se enlazan con las abstracciones:

1. **Validadores** (`Program.cs:29-33`): registra los cuatro `Validador*` concretos.
2. **Interceptor Castle** (`Program.cs:35-36`): registra el interceptor que decora los validadores.
3. **Proxies de interfaces** (`Program.cs:38-69`): crea un proxy para cada `IValidador*` que decora el concreto con el interceptor.
4. **Persistencia** (`Program.cs:71-77`): una sola instancia de `PersistenciaService` registrada detrás de los cinco puertos.
5. **Composición de Hacienda** (`Program.cs:79-129`): construye `RegistroVenta` y `FabricadorVacunas` explícitamente, los entrega a `new Hacienda(registroVentas, fabricadorVacunas)`, e hidrata desde persistencia. Es DI/externalización de construcción, no DIP.
6. **Servicios** (`Program.cs:118-129`): registra los cinco servicios de aplicación.

### 8.3 ¿Por qué Program.cs conoce concretos?

La raíz de composición es precisamente el lugar donde las abstracciones se conectan con los detalles. Los controladores y servicios no deben asumir esa responsabilidad. Que `Program.cs` instancie `PersistenciaService`, `ValidadorRes` o `RegistroVenta` no viola DIP: es exactamente la responsabilidad de un composition root.

### 8.4 ¿Qué NO es DIP?

DIP NO significa "poner una interfaz delante de cada `new`". Ejemplos del proyecto que NO necesitan interfaz:

- `RegistroVenta`: existe constructor `Hacienda(registroVentas, ...)` que aplica DI, no DIP. DIP requeriría `IRegistroVenta`; por decisión consciente no se introduce (no hay segundo cliente).
- `FabricadorVacunas`: igual. Solo Hacienda la usa; DIP requeriría `IFabricadorVacunas` que no se justifica.
- `Producto`, `Res`, `Lacteo`, `Carne`, `Piel`, `Bacteriana`, `Viva`: entidades del dominio. No hay variación detrás de cada `new`. DIP no se aplica.
- `List<T>`: tipo framework. No hay variación.

### 8.5 Lo que el constructor de Hacienda muestra y lo que NO

El constructor `Hacienda(RegistroVenta, FabricadorVacunas)` demuestra:
- **Inyección de dependencias (DI):** Hacienda no construye sus colaboradores; los recibe.
- **Externalización de la construcción:** el `new` de `RegistroVenta` y `FabricadorVacunas` se ejecuta en `Program.cs`, no en Hacienda.
- **Composition root:** `Program.cs` es el único lugar donde `new RegistroVenta()` y `new FabricadorVacunas(...)` aparecen.

Este constructor NO es DIP porque:
- `RegistroVenta` y `FabricadorVacunas` son clases **concretas** (no interfaces).
- Hacienda depende de tipos concretos; no se invirtió la dirección de la dependencia.
- DIP ocurriría si Hacienda dependiera de `IRegistroVenta` o `IFabricadorVacunas`. Por decisión consciente (ver `04-evidencia/bitacora-ia/BITACORA-IA.md`, decisión sobre IFabricadorVacunas) tales interfaces NO existen: no hay cliente alternativo que justifique la abstracción.

La DIP real del proyecto se demuestra con:
- `PotreroService` → `IPersistenciaPotreros` ← `PersistenciaService` (no conoce archivos TXT). Dirección de dependencia: servicio → puerto ← adaptador.
- `ResService` → `IPersistenciaReses`, `IPersistenciaVentas` ← `PersistenciaService`.
- `VacunaService` → `IPersistenciaVacunas`, `IPersistenciaPotreros`, `IPersistenciaReses` ← `PersistenciaService`.
- `UsuarioService` → `IPersistenciaUsuarios` ← `PersistenciaService`.

## 9. Componente investigativo — Composition Root e inversión de dependencias

### 9.1 Conceptos

- **Inversión de dependencias (DIP):** un módulo de ALTO NIVEL que expresa la política de negocio no debe depender de un módulo de BAJO NIVEL que contiene detalles técnicos. Ambos deben depender de una ABSTRACCIÓN. La dirección de la dependencia se invierte respecto al modelo tradicional.

- **Inyección de dependencias (DI):** patrón de implementación mediante el cual un objeto recibe sus colaboradores en lugar de construirlos. Puede hacerse por constructor, propiedad o método. NO es lo mismo que DIP: DI es una técnica; DIP es un principio.

- **Composition root:** lugar único donde se construye el grafo de objetos y se enlazan las abstracciones con sus implementaciones. Típicamente cerca del punto de entrada de la aplicación.

### 9.2 Aplicación en Hacienda

En `HaciendaNEW`, el composition root es `p_mvcHacienda/Program.cs`. Características:

- Constructor: además de configurar MVC, autenticación y otros servicios de infraestructura, instancia `RegistroVenta`, `FabricadorVacunas`, `PersistenciaService`, `Validador*` y los `IValidador*` decorados.
- Hidratación: tras crear `Hacienda`, carga potreros/reses/ventas/vacunas desde los archivos TXT.
- Servicios: registra los cinco servicios de aplicación como `Singleton`.

**Ejemplo concreto de la diferencia DIP / DI en el proyecto:**

| Mecanismo | Ejemplo | ¿DIP? | ¿DI? | ¿Composition root? |
|---|---|---|---|---|
| `PersistenciaService : IPersistenciaPotreros` | `PotreroService` depende de `IPersistenciaPotreros`, no de `PersistenciaService`. La abstracción vive en dominio y la implementación en MVC. | **SÍ** — el servicio no conoce archivos TXT. | **SÍ** — `PersistenciaService` se inyecta vía `Program.cs`. | **SÍ** — el binding ocurre en `Program.cs`. |
| `Hacienda(RegistroVenta, FabricadorVacunas)` | Hacienda recibe `RegistroVenta` y `FabricadorVacunas` concretos por constructor. | **NO** — no hay abstracción entre `Hacienda` y sus colaboradores. | **SÍ** — Hacienda no hace `new`. | **SÍ** — los `new` viven en `Program.cs`. |
| `Hacienda.vender<T>(IInventarioVendible<T>, T, uint)` | El método recibe una abstracción como parámetro. | **SÍ** — la política depende de la interfaz, no del inventario concreto. | **NO** — es un parámetro de método, no inyección. | **NO** — la elección del concreto ocurre en el controller/view. |

**Regla nemotécnica:**

- **DIP:** ¿hay una interfaz cuyo dueño es el módulo de alto nivel y el bajo nivel la implementa? Si SÍ, hay DIP.
- **DI:** ¿el objeto recibe sus dependencias en lugar de construirlas? Si SÍ, hay DI.
- **DIP sin DI:** validadores concretos + interceptores Castle (no se inyectan en el constructor de `PersistenciaService`).
- **DI sin DIP:** `Hacienda(RegistroVenta, FabricadorVacunas)` — inyección de concretos.
- **DIP + DI + Composition root:** `PotreroService → IPersistenciaPotreros ← PersistenciaService`, orquestado en `Program.cs`.

### 9.3 Beneficio

- Toda la lógica de "ensamblar el sistema" está en un solo archivo.
- Los servicios de aplicación no saben dónde está la configuración ni qué implementaciones se eligieron.
- Una prueba de integración puede construir manualmente `Hacienda` con dependencias in-memory; la raíz de composición no es un singleton oculto.

### 9.4 Fuentes

- Material docente del curso: `Resources/3. Principios_SOLID.pptx`, `Resources/2_Intro_Arquitectura.pptx`.
- Material oficial Microsoft: [Inversion of Control (IoC) principle](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/architectural-principles#dependency-inversion).

## 10. ADR — Architectural Decision Records

Seis ADR en `02-diseno/adr/`:

| ADR | Decisión | Hallazgo |
|---|---|---|
| ADR-001 | Toolchain y build NEW: `net8.0` + `ProjectReference` | Discrepancia entre DLL compilada y fuente. |
| ADR-002 | Modelo SC-1: `Producto` + `IInventarioVendible<T>` + venta genérica | SC-1 con productos derivados. |
| ADR-003 | TO-BE reducido al vertical materializado | Discrepancia UML aspiracional vs código real. |
| ADR-004 | Persistencia con cinco puertos segregados | Servicios acoplados a `PersistenciaService` monolítico. |
| ADR-005 | Jerarquía Res con LSP caracterizado sin composición | Subtipos fortalecían precondiciones. |
| ADR-006 | Extracción de `FabricadorVacunas` desde Hacienda | SRP: `Hacienda` concentraba ~180 líneas de duplicación en cuatro `crear_vacuna`. |

Detalle en `02-diseno/adr/ADR-001..006.md`.

## 11. Decisiones deliberadamente rechazadas

| Decisión rechazada | Aportaba | Costo añadido | Por qué rechazada |
|---|---|---|---|
| `IVacunaFactory` con reflection | Cerrar el eje "tipo de vacuna" a extensión. | Una jerarquía completa con reflection para solo dos tipos. | Sin cliente ni variación real; `FabricadorVacunas` documenta la deuda. Ver `04-evidencia/bitacora-ia/BITACORA-IA.md`, decisión sobre la fábrica de vacunas. |
| `IFabricadorVacunas` por simetría DIP | Abstracción "completa" del factory. | Un puerto sin uso. | No hay cliente distinto de Hacienda. Ver `04-evidencia/bitacora-ia/BITACORA-IA.md`, decisión sobre IFabricadorVacunas. |
| Servicio separado para `alimentar_res` | Una clase más. | Múltiples actores sin reducción de presión de cambio. | La operación coordina Potrero, Res y eventos de forma cohesiva. Ver `04-evidencia/bitacora-ia/BITACORA-IA.md`, decisión sobre alimentar_res. |
| Interfaz por cada servicio MVC | "Pure DIP" completo. | Doce interfaces adicionales. | No hay segundo cliente ni variación. |
| Reescritura completa de `PersistenciaService` | Cinco archivos separados. | Mayor cambio conductor de errores. | Los cinco puertos segregados ya cumplen ISP y DIP. |
| Arquitectura hexagonal completa | Separación de dominio/infraestructura. | Sobreingeniería para el alcance real. | El proyecto solo tiene una capa de infraestructura (MVC). |
| `IClock` para `DateTime.Now` | Testabilidad temporal. | Abstracción para un único uso. | No existe prueba que lo requiera. |

## 12. Relación con solicitudes futuras

| SC | Estado | Punto de extensión TO-BE | Qué se modificaría | Qué permanece estable |
|---|---|---|---|---|
| SC-1 | IMPLEMENTADA | `Producto`/`IInventario<T>`/`Inventario*` | Nuevas clases para nuevos productos. | `Hacienda.vender<T>`, `Venta`, `RegistroVenta`. |
| SC-2 | NO IMPLEMENTADA (analizada) | Añadir `Chip` a `Res` y un puerto `IPersistenciaChips`. | Modelo y persistencia. | Sistema de venta y validación. |
| SC-3 | NO IMPLEMENTADA (analizada) | `HistoriaClinica` agregada a `Res`, `IPersistenciaClinica`. | Modelo y persistencia. | Sistema de venta y validación. |

El análisis de SC-2 y SC-3 está en `01-diagnostico/FASE-2-CAMBIOS.md`.

## 13. Métrica SC-1 contractual (Lacteo + Carne + Piel)

### 13.1 Solicitud de cambio

SC-1: "La Hacienda comenzará a vender productos derivados del ganado, incluyendo lácteos, carne y piel" (literal del enunciado).

### 13.2 Costo de cambio — OLD vs NEW

| Arquitectura | Clases existentes modificadas | Archivos existentes modificados | Clases nuevas | Archivos nuevos |
|---|---:|---:|---:|---:|
| OLD (contrafactual) | 4 | 6 | 0 | 0 |
| NEW (estado final) | 0 | 0 | 6 | 6 |

Detalle OLD (estimación de Fase 2, contrafactual):

- `Hacienda` (modificación de política para soportar productos derivados).
- `Venta` (cambio de tipo).
- `PersistenciaService` (nuevos formatos de archivo).
- `IVentaRes.cs` (firma).
- `Views/Venta/Index.cshtml` (vista).
- Servicio de consulta de ventas (nuevos DTOs).

Detalle NEW (estado final 2026-08-10):

Para incorporar la **terna completa** Lacteo + Carne + Piel:

1. `Clases/Lacteo.cs` — `Lacteo : Producto`.
2. `Clases/InventarioLacteos.cs` — `IInventario<Lacteo>`.
3. `Clases/Carne.cs` — `Carne : Producto`.
4. `Clases/InventarioCarnes.cs` — `IInventario<Carne>`.
5. `Clases/Piel.cs` — `Piel : Producto`.
6. `Clases/InventarioPieles.cs` — `IInventario<Piel>`.

No se modificaron `Hacienda.vender<T>`, `Venta`, `RegistroVenta`, `IInventarioVendible<T>`, `IInventario<T>`, `PersistenciaService` ni la vista `Venta/Index.cshtml` para reconocer la terna. El formato V2 guarda el nombre de tipo y recarga tipos no conocidos como `ProductoPersistido`, conservando tipo original, nombre y monto sin un `if` nuevo por variante.

### 13.3 Prueba focal de OCP (añadir Carne únicamente)

Como prueba focal de extensibilidad, añadir SOLO Carne (sin Lacteo/Piel preexistentes) costó:

| Arquitectura | Clases existentes modificadas | Archivos existentes modificados | Clases nuevas | Archivos nuevos |
|---|---:|---:|---:|---:|
| NEW — solo Carne | 0 | 0 | 2 | 2 |

Esta métrica focal es **complementaria** a la SC-1 contractual; no la sustituye. La SC-1 contractual es la terna Lacteo + Carne + Piel.

### 13.4 Interpretación

El resultado apoya OCP en el eje aprobado "tipo de producto vendible": la política estable (`Hacienda.vender<T>`) permanece cerrada y la capacidad crece agregando implementaciones. No afirma que toda la aplicación sea cerrada a cualquier cambio. El costo de seis clases nuevas es dominio/inventario real, no inflación de métrica.

## 14. Límites y costos aceptados

- **Eje "tipo de vacuna" no cerrado a modificación.** Agregar un tercer tipo de vacuna requiere extender `ICreacionVacuna`, `FabricadorVacunas` y `Hacienda.crear_vacuna`. Aceptado por ausencia de cliente.
- **`PersistenciaService` grande internamente.** Implementa cinco puertos en una clase. La segregación es solo a nivel de interfaz. Aceptado por ausencia de presión de cambio en formato.
- **Categorías etarias como subtipos.** No hay envejecimiento/cambio de categoría. Aceptado por modelado vigente.
- **Producto.Nombre con setter público.** Conservado para no romper consumidores. Pendiente de caracterización.
- **`Hacienda.alimentar_res` con suscripción a eventos inline.** Aceptado por cohesión de la operación.

## 15. Conclusión

La arquitectura TO-BE de HaciendaNEW:

- Materializa SC-1 (lácteos, carne, piel) mediante `Producto` + `IInventarioVendible<T>` + `vender<T>` genérico.
- Comprueba OCP en el eje "tipo de producto vendible" con la métrica real de Fase 4 (0/0/2/2).
- Cumple LSP en todas las jerarquías relevantes (Producto, Res, Vacuna).
- Cumple ISP con interfaces segregadas por capacidad para validadores y persistencia.
- Demuestra DIP con un composition root pedagógico en `Program.cs` y cinco puertos de persistencia.
- Aplica SRP mediante la extracción de `FabricadorVacunas` (2026-08-10) y la conservación de `RegistroVenta`.
- Mantiene límites explícitos y rechaza la sobreingeniería.

La correspondencia UML ↔ código se comprobó contra el código de `03-src/redisenado/HaciendaNEW/` y las verificaciones de `HaciendaNEW.Verification`.
