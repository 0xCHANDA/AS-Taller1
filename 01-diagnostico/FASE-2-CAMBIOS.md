# Fase 2 - Los cambios que vienen

Análisis del impacto de cambio sobre el sistema Hacienda AS-IS.

**Línea base:** commit `ce5048463a5e3c3b7aed3ac32f17dcd17597c75c`, 8 de agosto de 2026.

**Documento AS-IS: no contiene arquitectura TO-BE ni implementación.**

# 1. Objetivo

Establecer una línea base verificable del costo y del riesgo de implementar hoy SC-1, SC-2 y SC-3 sobre el sistema actual. El análisis determina qué clases y archivos existentes deben editarse, qué artefactos serían aditivos y qué conducta observable queda expuesta a regresión. No diseña la arquitectura futura.

# 2. Metodología

## 2.1 Fuentes y prioridad de evidencia

Se revisaron el enunciado y la rúbrica, `Bib_Hacienda`, `p_mvcHacienda`, controladores, servicios, modelos, interfaces, validaciones, eventos, interceptores, persistencia, vistas, `Program.cs`, datos, UML AS-IS, inventario de hallazgos y puntos de dolor. La fuente principal fue el código actual; los diagramas y documentos se usaron como apoyo.

Las líneas citadas corresponden al estado del commit `ce5048463a5e3c3b7aed3ac32f17dcd17597c75c`.

## 2.2 Verificación del análisis

Se reconstruyeron las rutas principales del sistema y se revisaron por separado SRP, OCP, LSP, ISP y DIP. Los contratos de comportamiento se identificaron directamente en el código y luego se comprobaron con los casos de caracterización. Ninguna conclusión se dejó sustentada únicamente en una herramienta automática.

## 2.3 Regla de conteo

| **Conjunto** | **Regla** | **¿Se suma a existentes modificados?** |
| --- | --- | --- |
| A — Must-modify | Elemento existente que debe editarse para completar el mínimo de forma coherente con las responsabilidades AS-IS. | Sí |
| B — Aditivo | Clase/archivo inexistente necesario o probable. | No |
| C — Regresión | Elemento que puede permanecer sin edición, pero consume contratos o datos modificados. | No |

Una vista, .csproj o archivo de datos cuenta como archivo, no como clase. Los Datos legacy no son must-modify si el lector puede mantener compatibilidad sin edición manual. Para no maquillar el mínimo, no se trasladan reglas de dominio al controller aprovechando listas públicas: Hacienda conserva las mutaciones, ResController la entrada MVC y PersistenciaService el almacenamiento.

## 2.4 Directo, transitivo y regresión

- Directo: expresa el dato, regla, entrada o salida nueva.
- Transitivo: cambia por composición, formato o integración.
- Solo regresión: no se edita, pero su salida actual debe verificarse.
Severidad Alta significa que el cambio puede eliminar/mutar una res incorrectamente, corromper o perder datos, impedir la carga o alterar una regla sanitaria/financiera observable. No se usó “Alta” como sinónimo de clase grande.

## 2.5 Baseline

El baseline se ejecutó antes de redactar el documento. Sus artefactos bin/obj se restauraron antes de cerrar el entregable; la comprobación final del worktree se registra en la validación posterior al DOCX. No se modificó source. El MVC compila contra una DLL mediante HintPath (p_mvcHacienda.csproj:L14-L18), no contra la fuente. Bib_Hacienda apunta a .NET Framework 4.7.2 (Bib_Hacienda.csproj:L4-L14). No hay pruebas: el .csproj solo declara una carpeta Tests vacía (L140-L142).

| **Comando ejecutado** | **Resultado** |
| --- | --- |
| git rev-parse HEAD | ce5048463a5e3c3b7aed3ac32f17dcd17597c75c |
| dotnet --info | SDK 10.0.110; runtimes 10.0.10 |
| dotnet restore p_mvcHacienda.sln | Éxito |
| dotnet build p_mvcHacienda.sln --no-restore | Éxito: 0 errores, 5 advertencias de nulabilidad |
| dotnet build Bib_Hacienda.sln | Fallo preexistente MSB3644: falta targeting pack .NET Framework 4.7.2 |
| Búsqueda de *Tests.csproj y atributos de test | No se encontró suite ejecutable |

## 2.6 Desacuerdos consolidados

- IVentaRes no se modifica en SC-1: mantiene la venta de res.
- Ternero/Cebon/Novillo no se modifican en SC-2: el chip se conecta después y heredan el estado.
- ResService ya lista/busca reses; no necesita cambio en SC-2/SC-3.
- Vacuna/Viva/Bacteriana, IVacunacion, VacunaService y VacunaController son regresión, no must-modify, en SC-3.
- Potrero no participa en registrar un evento clínico sobre una res existente.
## 2.7 Decisiones de alcance

- Aceptado: publicar listas nominales A/B/C, registrar comandos del baseline, caracterizar el defecto actual del monto cero, mejorar citas y hacer descubrible SC-3 desde Res/Index; por ello SC-3 aumenta de 7 a 8 archivos.
- Aceptado: la clase clínica, su archivo, el archivo de datos y la edición del .csproj son condicionales al escenario mínimo declarado; aquí se cuentan porque un evento repetible con fecha/concepto/observación no cabe en los tipos actuales.
- Rechazado: reemplazar los conteos por múltiples cifras. El enunciado exige definir primero un escenario operacional mínimo; cada cifra final queda explícitamente condicionada por ese alcance.
- Rechazado: contar VentaController.Create/Venta.Create como must-modify. Son scaffolding inerte; la única escritura operativa comprobada parte de Res/Index y el mínimo mantiene esa ruta.
- Rechazado: contar ValidadorRes en SC-2. El chip es opcional para la existencia válida de una Res y su validez se comprueba en la operación de conexión, como Hacienda ya valida parámetros operacionales; cambiar el validador global rompería las reses legacy sin chip.
# 3. Resumen ejecutivo

| **Solicitud** | **Clases existentes** | **Archivos existentes** | **Áreas directas** | **Riesgo** |
| --- | --- | --- | --- | --- |
| SC-1 — Productos derivados | 4 | 6 | Dominio, aplicación, persistencia, presentación | Alta |
| SC-2 — Chips/geolocalización | 4 | 5 | Dominio, aplicación, persistencia, presentación | Alta |
| SC-3 — Historia clínica | 5 | 8 | Dominio, aplicación, persistencia, presentación, composición/build | Alta |

SC-3 tiene el mayor blast radius por archivos y capas. SC-1 amenaza directamente una transacción que elimina una res. SC-2 concentra identidad y compatibilidad de 226 registros. PersistenciaService, Hacienda y ResController aparecen en las tres solicitudes.

# 4. SC-1 — Venta de productos derivados

## 4.1 Alcance asumido

Solicitud literal: “La hacienda en el futuro va a comenzar a vender productos derivados del ganado como: lácteos, carne, piel”.

Implementación mínima: desde una res activa registrar categoría lácteo/carne/piel, monto positivo, res y potrero de origen; persistir/recuperar la categoría; mostrarla; y no retirar la res en ventas de derivados. La venta de la res completa conserva el retiro. Se excluyen inventario, cantidad/unidad, cliente, factura, faenado y producción.

## 4.2 AS-IS y ruta

Escritura real: Views/Res/Index.cshtml:L162-L209 → ResController.Vender():L131-L180 → Hacienda.vender_res():L143-L168 → Venta → PersistenciaService.GuardarVentas()/GuardarReses(). Lectura: VentaController.Index():L17-L25 → VentaService:L17-L58 → Views/Venta/Index.cshtml:L77-L170.

Venta almacena estructuralmente Potrero y Res (Venta.cs:L11-L28), y ValidadorVenta exige que no sean nulos (ValidarVenta.cs:L12-L19); Hacienda siempre retira la res (Hacienda.cs:L155-L160); persistencia usa siete campos ganaderos (PersistenciaService.cs:L154-L163,L389-L452); la vista calcula precio/kg y desreferencia Res/Potrero (Venta/Index:L95-L126).

## 4.3 Matriz de impacto

| **Elemento** | **Tipo** | **Ubicación** | **Modificación** | **Razón** | **Evidencia** | **Impacto** | **Riesgo** | **Conf.** |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| Venta / Venta.cs | Clase+archivo | .../Clases/Venta.cs | Añadir categoría/semántica | Solo Potrero, Fecha, Res, Monto | constructor/propiedades L11-L28 | Directo | Construcción legacy | Alta |
| Hacienda / Hacienda.cs | Clase+archivo | .../Clases/Hacienda.cs | Registrar derivado sin retiro | vender_res siempre elimina | vender_res L143-L168 | Directo | Venta/retiro de res | Alta |
| ResController / ResController.cs | Clase+archivo | .../Controllers/ResController.cs | Recibir categoría | Vender solo recibe monto | L131-L180 | Directo | Validación HTTP | Alta |
| PersistenciaService / .cs | Clase+archivo | .../Servicios/PersistenciaService.cs | Round-trip de categoría | Formato fijo de 7 campos | L132-L171,L389-L452 | Directo | Histórico | Alta |
| Views/Res/Index.cshtml | Archivo | .../Views/Res/Index.cshtml | Capturar variante | Modal solo monto | L162-L209 | Directo | Modales | Alta |
| Views/Venta/Index.cshtml | Archivo | .../Views/Venta/Index.cshtml | Mostrar categoría/columnas válidas | Vista solo ganadera | L77-L170 | Directo | Historial/estadísticas | Alta |

No se editan IVentaRes, ValidadorVenta, VentaService ni VentaController. El mínimo conserva Res/Potrero/monto > 0; el service sigue ordenando y agregando Monto/Fecha; el controller de ventas solo consulta.

VentaController sí contiene acciones Create GET/POST y existe Views/Venta/Create.cshtml, pero ambas son scaffolding sin creación efectiva (VentaController.cs:L33-L64; Venta/Create.cshtml:L1-L5). Se registran en C, no en A.

## 4.4 Dependencias y blast radius

| **Área** | **Clasificación** | **Elementos** |
| --- | --- | --- |
| Dominio | Directo | Venta; Hacienda.vender_res |
| Aplicación | Directo | ResController.Vender |
| Persistencia | Directo | GuardarVentas/CargarVentas |
| Presentación | Directo | Res/Index; Venta/Index |
| Validación | Solo regresión | ValidadorVenta |
| Composición/build | Transitivo | Program; DLL Bib_Hacienda |
| Datos | Solo regresión | Ventas.txt; Reses.txt |

## 4.5 Riesgos

### R-SC1-01 — Venta de res deja de retirar exactamente un animal

**Cambio que lo puede provocar:** Separar venta ganadera/derivado

**Ruta afectada:** Res/Index → ResController → Hacienda

**Comportamiento observable actual:** Agregar Venta y retirar la Res

**Razón técnica:** Una condición errónea conserva ganado vendido o elimina la res al vender un derivado.

**Severidad:** Alta

**Evidencia:** Hacienda.cs:L149-L161

### R-SC1-02 — Ventas históricas dejan de cargar

**Cambio que lo puede provocar:** Añadir categoría al formato

**Ruta afectada:** Program → CargarVentas

**Comportamiento observable actual:** Leer siete campos actuales

**Razón técnica:** El parser posicional puede desplazar monto/tipo o rechazar líneas legacy.

**Severidad:** Alta

**Evidencia:** PersistenciaService.cs:L389-L452; Datos/Ventas.txt:L1-L2

### R-SC1-03 — Historial/estadísticas fallan

**Cambio que lo puede provocar:** Ampliar semántica de Venta

**Ruta afectada:** VentaController → VentaService → vista

**Comportamiento observable actual:** Orden, suma, promedio, precio/kg

**Razón técnica:** La vista supone siempre Res, Potrero y Peso.

**Severidad:** Alta

**Evidencia:** VentaService.cs:L17-L58; Venta/Index:L95-L170

### R-SC1-04 — Venta inválida se guarda o una válida no persiste

**Cambio que lo puede provocar:** Descoordinar modelo y validador

**Ruta afectada:** GuardarVentas → ValidadorVenta

**Comportamiento observable actual:** Exigir Res/Potrero/monto > 0

**Razón técnica:** Relajar globalmente admite ventas ganaderas incompletas; mantenerlo con modelo incompatible bloquea guardado.

**Severidad:** Alta

**Evidencia:** ValidarVenta.cs:L12-L19

### R-SC1-05 — Monto cero retira la res aunque la persistencia lo rechace

**Cambio que lo puede provocar:** Conservar sin caracterizar la validación distribuida

**Ruta afectada:** ResController.Vender → Hacienda.vender_res → GuardarVentas

**Comportamiento observable actual:** El controller solo rechaza monto < 0; Hacienda muta antes de validar; GuardarVentas retorna un error que el controller ignora.

**Razón técnica:** SC-1 puede ocultar o agravar este defecto AS-IS: la res sale del potrero y el mensaje HTTP puede indicar éxito sin venta durable.

**Severidad:** Alta

**Evidencia:** ResController.cs:L145-L172; Hacienda.cs:L155-L160; ValidarVenta.cs:L12-L19

## 4.6 Escenarios de caracterización requeridos

| **Precondición** | **Acción** | **Resultado AS-IS que debe protegerse** |
| --- | --- | --- |
| Una res activa única | POST /Res/Vender con monto válido | Se agrega una Venta y se retira exactamente esa res. |
| Ventas.txt con 7 campos | Reiniciar/cargar | Se reconstruyen las dos ventas actuales. |
| Ventas cargadas | Abrir /Venta/Index | Se muestran res/potrero/peso/precio-kg y se calculan totales. |
| Una res activa | POST /Res/Vender con monto 0 | Caracterizar defecto: dominio retira la res; validación de guardado rechaza la Venta y el retorno es ignorado. |

## 4.7 Presión SOLID

La dimensión categoría + efecto de retiro encuentra modelo, operación, formato y UI cerrados sobre una venta ganadera: presión OCP confirmada. La presión SRP es causal: el actor comercial cambia vender_res y el endpoint de venta, mientras actores de manejo ganadero/veterinario cambian alimentación, vacunación y consulta sanitaria en esas mismas clases (Hacienda.cs:L142-L265,L450-L557; ResController.cs:L37-L61,L105-L180). Para ISP, no se observan clientes tipados por IVentaRes forzados a depender de operaciones ajenas; no hay violación demostrada.

## Métrica final SC-1

| **Métrica** | **Valor** |
| --- | --- |
| Clases existentes a modificar | 4 |
| Archivos existentes a modificar | 6 |
| Clases nuevas estimadas | 0 |
| Archivos nuevos estimados | 0 |
| Elementos expuestos a regresión | 5 |

Lista nominal de clases existentes a modificar:

1. Bib_Hacienda.Clases.Venta
1. Bib_Hacienda.Clases.Hacienda
1. p_mvcHacienda.Controllers.ResController
1. p_mvcHacienda.Servicios.PersistenciaService
Lista nominal de archivos existentes a modificar:

1. Hacienda/Bib_Hacienda/Bib_Hacienda/Clases/Venta.cs
1. Hacienda/Bib_Hacienda/Bib_Hacienda/Clases/Hacienda.cs
1. Hacienda/p_mvcHacienda/Controllers/ResController.cs
1. Hacienda/p_mvcHacienda/Servicios/PersistenciaService.cs
1. Hacienda/p_mvcHacienda/Views/Res/Index.cshtml
1. Hacienda/p_mvcHacienda/Views/Venta/Index.cshtml
Clases nuevas estimadas:

- Ninguna.
Archivos nuevos estimados:

- Ninguno.
C — clases existentes solo expuestas a regresión (no se suman a A):

1. Bib_Hacienda.Interfaces.IVentaRes
1. Bib_Hacienda.Clases.Validaciones.ValidadorVenta
1. p_mvcHacienda.Servicios.VentaService
1. p_mvcHacienda.Controllers.VentaController
1. p_mvcHacienda.Program
1. Bib_Hacienda.Clases.Potrero
1. Bib_Hacienda.Clases.Res
C — archivos existentes solo expuestos a regresión/transición (no se suman a A):

1. Hacienda/Bib_Hacienda/Bib_Hacienda/Interfaces/IVentaRes.cs
1. Hacienda/Bib_Hacienda/Bib_Hacienda/Clases/Validaciones/ValidarVenta.cs
1. Hacienda/p_mvcHacienda/Servicios/VentaService.cs
1. Hacienda/p_mvcHacienda/Controllers/VentaController.cs
1. Hacienda/p_mvcHacienda/Program.cs
1. Hacienda/Bib_Hacienda/Bib_Hacienda/Clases/Potrero.cs
1. Hacienda/Bib_Hacienda/Bib_Hacienda/Clases/Res.cs
1. Hacienda/p_mvcHacienda/Views/Venta/Create.cshtml
1. Hacienda/p_mvcHacienda/Datos/Ventas.txt
1. Hacienda/p_mvcHacienda/Datos/Reses.txt
1. Hacienda/p_mvcHacienda/p_mvcHacienda.csproj
Elementos conductuales expuestos a regresión:

1. Venta ganadera y retiro de la res
1. Validación de venta completa y monto positivo
1. Round-trip de ventas legacy de siete columnas
1. Listado, filtros y estadísticas de ventas
1. Persistencia de reses activas tras la venta
Control automático: 4=longitud de clases; 6=longitud de archivos; 0=longitud de clases nuevas; 0=longitud de archivos nuevos; 5=longitud de regresiones. Resultado: OK.

# 5. SC-2 — Chips y geolocalización

## 5.1 Alcance asumido

Solicitud literal: “La hacienda tiene la necesidad de conectar a las reses chips para la geolocalización”.

Conectar después del alta un ChipId no vacío/no asociado a otra res; registrar latitud/longitud válidas como última ubicación; persistir/recuperar y mostrar. El alta sin chip sigue válida. Se excluyen IoT en tiempo real, mapas, MQTT, nube, historial, geofencing y chip como clave primaria.

## 5.2 AS-IS y ruta

Ciclo actual: Create view → ResController.Create → PotreroService → Hacienda.anadir_res_potrero → Potrero.anadir_res → subtipos → Res. Res solo tiene nombre, peso, edad y vacunas (Res.cs:L13-L38). Guardar/CargarReses manejan cinco columnas (PersistenciaService.cs:L94-L130,L345-L387). Potrero.buscar_res usa coincidencia parcial de nombre (L163-L198).

Ruta mínima: Res/Index → ResController → Hacienda localiza/evita ChipId duplicado → Res conserva chip/posición → Guardar/CargarReses mantiene compatibilidad → Res/Index muestra ubicación. La creación y los constructores quedan intactos.

## 5.3 Matriz de impacto

| **Elemento** | **Tipo** | **Ubicación** | **Modificación** | **Razón** | **Evidencia** | **Impacto** | **Riesgo** | **Conf.** |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| Res / Res.cs | Clase+archivo | .../Clases/Res.cs | ChipId y última posición opcionales | No existe identidad técnica/ubicación | L13-L38 | Directo | Estado heredado | Alta |
| Hacienda / Hacienda.cs | Clase+archivo | .../Clases/Hacienda.cs | Conectar y evitar duplicado | Coordina mutaciones/búsquedas | buscar_potrero L91-L125 | Directo | Buscar/alimentar/vender/vacunar | Alta |
| ResController / .cs | Clase+archivo | .../Controllers/ResController.cs | Acción HTTP/validar entrada | No hay endpoint | L25-L35,L64-L103 | Directo | Listado/acciones | Alta |
| PersistenciaService / .cs | Clase+archivo | .../Servicios/PersistenciaService.cs | Round-trip compatible | Formato de 5 campos | L94-L130,L345-L387 | Directo | 226 registros | Alta |
| Views/Res/Index.cshtml | Archivo | .../Views/Res/Index.cshtml | Capturar/mostrar | Tabla sin chip/posición | L45-L88 | Directo | Tabla/modales | Alta |

Ternero/Cebon/Novillo solo heredan el estado. Potrero, ResService, ValidarRes, Create.cshtml y Program no cambian bajo conexión posterior y firma estable de CargarReses.

ValidadorRes permanece sin cambio porque el chip es opcional para que una res legacy siga siendo válida; la no vaciedad, unicidad y rango de coordenadas pertenecen a la operación de conexión. Una comprobación global no condicional podría invalidar reses actuales sin chip (ValidarRes.cs:L12-L19; PersistenciaService.GuardarReses():L103-L118).

## 5.4 Blast radius

| **Área** | **Clasificación** | **Elementos** |
| --- | --- | --- |
| Dominio | Directo | Res; Hacienda |
| Aplicación | Directo | ResController |
| Persistencia | Directo | Guardar/CargarReses |
| Presentación | Directo | Res/Index |
| Validación | Solo regresión | ValidadorRes |
| Jerarquía | Solo regresión | Ternero/Cebon/Novillo |
| Datos | Solo regresión | Reses.txt legacy |
| Build | Transitivo | DLL Bib_Hacienda |

## 5.5 Riesgos

### R-SC2-01 — Reses actuales dejan de cargar

**Cambio que lo puede provocar:** Extender Reses.txt

**Ruta afectada:** Program → CargarReses → Potrero

**Comportamiento observable actual:** Reconstruir 226 líneas de 5 campos

**Razón técnica:** Campos obligatorios o índices desplazados rompen parseo/arranque.

**Severidad:** Alta

**Evidencia:** PersistenciaService.cs:L345-L387; Datos/Reses.txt:L1-L226

### R-SC2-02 — Chip conectado a res equivocada

**Cambio que lo puede provocar:** Usar búsqueda nominal actual

**Ruta afectada:** Hacienda.buscar_potrero → Potrero.buscar_res

**Comportamiento observable actual:** Coincidencia parcial; ambigüedad genera error

**Razón técnica:** La asociación debe manejar ambigüedad y unicidad global.

**Severidad:** Alta

**Evidencia:** Potrero.cs:L163-L198

### R-SC2-03 — Constructores/clasificación se rompen

**Cambio que lo puede provocar:** Hacer chip obligatorio en constructor

**Ruta afectada:** Create → Potrero → subtipos

**Comportamiento observable actual:** Tres parámetros y rangos de edad

**Razón técnica:** Cambiar base propaga a tres subclases/callers; el mínimo lo evita.

**Severidad:** Alta

**Evidencia:** Res.cs:L22-L28; Ternero.cs:L14-L24; Cebon.cs:L14-L24; Novillo.cs:L14-L24; Potrero.cs:L62-L102

### R-SC2-04 — Alimentar/vender/vacunar no encuentra la res

**Cambio que lo puede provocar:** Reemplazar nombre por ChipId

**Ruta afectada:** Controllers/Hacienda/VacunaService

**Comportamiento observable actual:** Identidad actual potrero+nombre

**Razón técnica:** Cambiar clave altera animal mutado y mensajes.

**Severidad:** Alta

**Evidencia:** Hacienda.cs:L149-L160,L457-L470

## 5.6 Escenarios de caracterización requeridos

| **Precondición** | **Acción** | **Resultado AS-IS que debe protegerse** |
| --- | --- | --- |
| Reses.txt con 226 líneas de 5 campos | Cargar aplicación | Se reconstruyen todas las reses sin exigir chip. |
| Edades 12, 13, 48 y 49 | Crear por potrero | Se mantienen rangos y excepciones actuales de los tres subtipos. |
| Res identificada por potrero+nombre | Alimentar, vender, vacunar | Cada operación sigue localizando el mismo animal. |
| Venta histórica contiene snapshot de Res | CargarVentas | Se conserva el histórico; el chip de una res ya vendida queda fuera del tracking activo por supuesto de alcance. |

## 5.7 Presión SOLID

Una ampliación cohesiva de estado no confirma por sí sola OCP: la presión se clasifica sospechada. LSP sí presenta riesgo preexistente: Res.Edad acepta cualquier ushort, mientras los overrides refuerzan precondiciones y lanzan (Res.cs:L31-L35; Ternero.cs:L19-L24; Cebon.cs:L19-L24; Novillo.cs:L19-L24). El chip vuelve costoso reemplazar la instancia al cruzar una etapa.

## Métrica final SC-2

| **Métrica** | **Valor** |
| --- | --- |
| Clases existentes a modificar | 4 |
| Archivos existentes a modificar | 5 |
| Clases nuevas estimadas | 0 |
| Archivos nuevos estimados | 0 |
| Elementos expuestos a regresión | 6 |

Lista nominal de clases existentes a modificar:

1. Bib_Hacienda.Clases.Res
1. Bib_Hacienda.Clases.Hacienda
1. p_mvcHacienda.Controllers.ResController
1. p_mvcHacienda.Servicios.PersistenciaService
Lista nominal de archivos existentes a modificar:

1. Hacienda/Bib_Hacienda/Bib_Hacienda/Clases/Res.cs
1. Hacienda/Bib_Hacienda/Bib_Hacienda/Clases/Hacienda.cs
1. Hacienda/p_mvcHacienda/Controllers/ResController.cs
1. Hacienda/p_mvcHacienda/Servicios/PersistenciaService.cs
1. Hacienda/p_mvcHacienda/Views/Res/Index.cshtml
Clases nuevas estimadas:

- Ninguna.
Archivos nuevos estimados:

- Ninguno.
C — clases existentes solo expuestas a regresión (no se suman a A):

1. Bib_Hacienda.Clases.Ternero
1. Bib_Hacienda.Clases.Cebon
1. Bib_Hacienda.Clases.Novillo
1. Bib_Hacienda.Clases.Potrero
1. p_mvcHacienda.Servicios.ResService
1. Bib_Hacienda.Clases.Validaciones.ValidadorRes
1. p_mvcHacienda.Program
1. p_mvcHacienda.Servicios.VacunaService
C — archivos existentes solo expuestos a regresión/transición (no se suman a A):

1. Hacienda/Bib_Hacienda/Bib_Hacienda/Clases/Ternero.cs
1. Hacienda/Bib_Hacienda/Bib_Hacienda/Clases/Cebon.cs
1. Hacienda/Bib_Hacienda/Bib_Hacienda/Clases/Novillo.cs
1. Hacienda/Bib_Hacienda/Bib_Hacienda/Clases/Potrero.cs
1. Hacienda/p_mvcHacienda/Servicios/ResService.cs
1. Hacienda/Bib_Hacienda/Bib_Hacienda/Clases/Validaciones/ValidarRes.cs
1. Hacienda/p_mvcHacienda/Program.cs
1. Hacienda/p_mvcHacienda/Servicios/VacunaService.cs
1. Hacienda/p_mvcHacienda/Views/Res/Create.cshtml
1. Hacienda/p_mvcHacienda/Views/Vacuna/Aplicar.cshtml
1. Hacienda/p_mvcHacienda/Datos/Reses.txt
Elementos conductuales expuestos a regresión:

1. Creación/clasificación y constructores de subtipos
1. Carga de Reses.txt de cinco columnas
1. Alimentación y eventos de peso
1. Vacunación y selectores potrero–res
1. Venta y búsqueda nominal
1. Listado y estadísticas de reses
Control automático: 4=longitud de clases; 5=longitud de archivos; 0=longitud de clases nuevas; 0=longitud de archivos nuevos; 6=longitud de regresiones. Resultado: OK.

# 6. SC-3 — Historia clínica

## 6.1 Alcance asumido

Solicitud literal: “Además de las vacunas, se va a requerir tener la historia clínica de cada res en un futuro”.

Registrar por res eventos no vacunales repetibles con fecha, concepto y observación; persistir/recuperar; consultar junto con vacunas. L_vacunas_aplicadas sigue siendo la fuente vacunal y no se duplica. Se excluyen veterinario, recetas, diagnóstico codificado, adjuntos, agenda y facturación.

## 6.2 AS-IS y ruta

Res solo contiene List<Vacuna> (Res.cs:L17-L36). Hacienda.aplicar_vacuna valida duplicados/límites/vencimiento, agrega y retira inventario (L469-L549). VacunaService realiza cuatro guardados (L72-L92). Persistencia usa ocho campos vacunales (L217-L270,L529-L599). ResController.DetalleVacunas entrega List<Vacuna> a una vista exclusiva (DetalleVacunas.cshtml:L1-L64).

Ruta mínima: DetalleVacunas → ResController → Hacienda localiza/agrega evento → Res conserva colección → Persistencia guarda/carga archivo repetible → Program carga explícitamente → vista combina fuentes. El nuevo .cs debe agregarse al Compile explícito del proyecto clásico (Bib_Hacienda.csproj:L100-L135).

## 6.3 Matriz de impacto

| **Elemento** | **Tipo** | **Ubicación** | **Modificación** | **Razón** | **Evidencia** | **Impacto** | **Riesgo** | **Conf.** |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| Res / Res.cs | Clase+archivo | .../Clases/Res.cs | Colección de eventos | Solo vacunas | L13-L38 | Directo | Estado vacunal | Alta |
| Hacienda / Hacienda.cs | Clase+archivo | .../Clases/Hacienda.cs | Registrar evento sobre res | No hay operación clínica general | buscar_potrero L91-L125; aplicar_vacuna L450-L557 | Directo | Vacunación/búsqueda | Alta |
| Bib_Hacienda.csproj | Archivo proyecto | .../Bib_Hacienda.csproj | Incluir nuevo .cs | Compile enumera fuentes | L100-L135 | Transitivo | Build/DLL | Alta |
| ResController / .cs | Clase+archivo | .../Controllers/ResController.cs | Alta/consulta combinada | Retorna solo List<Vacuna> | L37-L61 | Directo | Detalle/errores | Alta |
| PersistenciaService / .cs | Clase+archivo | .../Servicios/PersistenciaService.cs | Guardar/cargar eventos | Solo formato vacunal | L217-L270,L529-L599 | Directo | Integridad sanitaria | Alta |
| Program / Program.cs | Clase+archivo | .../Program.cs | Cargar eventos al inicio | Carga explícita por archivo | L38-L63 | Transitivo | Carga parcial | Alta |
| DetalleVacunas.cshtml | Archivo | .../Views/Res/DetalleVacunas.cshtml | Formulario/lista combinada | Modelo/tabla solo vacunas | L1-L64 | Directo | Detalle vacunal | Alta |
| Views/Res/Index.cshtml | Archivo | .../Views/Res/Index.cshtml | Hacer descubrible la historia y distinguir su acceso del conteo vacunal | La columna se llama Vacunas y el enlace solo muestra Count | L58,L75-L79 | Directo | Listado y acceso sanitario | Alta |

Aditivo dentro del escenario medido: una clase estructurada, su archivo fuente y un archivo de datos. El conteo no presupone nuevos controller, service, view o ViewModel. Vacuna/Viva/Bacteriana, IVacunacion, VacunaService y VacunaController son regresión. Res/Index sí se modifica para que la capacidad sea descubrible y no quede escondida bajo un contador denominado “Vacunas”.

## 6.4 Blast radius

| **Área** | **Clasificación** | **Elementos** |
| --- | --- | --- |
| Dominio | Directo | Res; Hacienda |
| Aplicación | Directo | ResController |
| Persistencia | Directo | PersistenciaService + nuevo archivo |
| Presentación | Directo | DetalleVacunas; Res/Index |
| Composición | Transitivo | Program |
| Build | Transitivo | Bib_Hacienda.csproj/DLL |
| Vacunación | Solo regresión | Hacienda.aplicar_vacuna; VacunaService/Controller; tipos |
| Datos legacy | Solo regresión | VacunasAplicadas.txt |

## 6.5 Riesgos

### R-SC3-01 — Eventos alteran reglas vacunales

**Cambio que lo puede provocar:** Generalizar L_vacunas_aplicadas

**Ruta afectada:** VacunaController → Service → Hacienda

**Comportamiento observable actual:** Duplicados, máximos, vencimiento, inventario

**Razón técnica:** Loops/is asumen Bacteriana/Viva.

**Severidad:** Alta

**Evidencia:** Hacienda.cs:L469-L549

### R-SC3-02 — Historia e inventario quedan inconsistentes

**Cambio que lo puede provocar:** Agregar guardado a secuencia no transaccional

**Ruta afectada:** VacunaService → Guardar*

**Comportamiento observable actual:** Mutar memoria y sobrescribir archivos

**Razón técnica:** Una excepción conserva solo parte del estado.

**Severidad:** Alta

**Evidencia:** VacunaService.cs:L72-L92

### R-SC3-03 — Vacunas aplicadas dejan de cargar

**Cambio que lo puede provocar:** Reutilizar formato para otros eventos

**Ruta afectada:** Program → CargarVacunasAplicadas

**Comportamiento observable actual:** Ocho campos Bacteriana/Viva

**Razón técnica:** Tipo desconocido sería Viva o rompería parser.

**Severidad:** Alta

**Evidencia:** PersistenciaService.cs:L529-L599

### R-SC3-04 — Detalle vacunal desaparece/duplica

**Cambio que lo puede provocar:** Cambiar modelo de vista

**Ruta afectada:** ResController.DetalleVacunas → vista

**Comportamiento observable actual:** Tabla List<Vacuna>

**Razón técnica:** Combinar sin distinguir omite/duplica/etiqueta mal.

**Severidad:** Alta

**Evidencia:** ResController.cs:L37-L61; vista:L1-L64

### R-SC3-05 — Una línea clínica impide cargar ventas/inventario

**Cambio que lo puede provocar:** Añadir carga al único try

**Ruta afectada:** Program singleton

**Comportamiento observable actual:** Orden de cinco cargas

**Razón técnica:** Una excepción detiene las posteriores.

**Severidad:** Alta

**Evidencia:** Program.cs:L38-L73

## 6.6 Escenarios de caracterización requeridos

| **Precondición** | **Acción** | **Resultado AS-IS que debe protegerse** |
| --- | --- | --- |
| Una res con vacunas aplicadas | Aplicar vacuna repetida o superar máximo | Se mantienen rechazo por nombre/lote y máximos por subtipo. |
| Vacuna vencida/disponible | Intentar aplicar | Se rechaza vencida; una válida pasa a la res y sale del inventario. |
| VacunasAplicadas.txt de 8 campos | Reiniciar | Se reconstruyen Bacteriana/Viva en L_vacunas_aplicadas. |
| Res con y sin vacunas | Abrir DetalleVacunas | Se conserva tabla o mensaje de vacío. |
| Una línea clínica inválida | Ejecutar secuencia de arranque | Caracterizar que el único try detiene cargas posteriores. |
| Res vacunada | Vender y reiniciar | Caracterizar la conservación o pérdida sanitaria AS-IS sin presentarla como conducta deseada. |

## 6.7 Presión SOLID

La presión OCP está confirmada en la proyección y persistencia sanitaria especializadas en Vacuna: una segunda clase de registro obliga a editar Res, controller, vistas, persistencia y arranque. No se atribuye la presión a la jerarquía Vacuna ni se afirma que un evento clínico sea subtipo de Vacuna. Hacienda y PersistenciaService reciben un change driver veterinario adicional, lo que amplifica SRP. Para ISP, no se observan clientes tipados por IVacunacion forzados a depender de operaciones ajenas; no hay violación demostrada. Vacuna/Viva/Bacteriana se protegen, pero no se editan ni prueban una violación LSP para SC-3.

## Métrica final SC-3

| **Métrica** | **Valor** |
| --- | --- |
| Clases existentes a modificar | 5 |
| Archivos existentes a modificar | 8 |
| Clases nuevas estimadas | 1 |
| Archivos nuevos estimados | 2 |
| Elementos expuestos a regresión | 5 |

Lista nominal de clases existentes a modificar:

1. Bib_Hacienda.Clases.Res
1. Bib_Hacienda.Clases.Hacienda
1. p_mvcHacienda.Controllers.ResController
1. p_mvcHacienda.Servicios.PersistenciaService
1. p_mvcHacienda.Program
Lista nominal de archivos existentes a modificar:

1. Hacienda/Bib_Hacienda/Bib_Hacienda/Clases/Res.cs
1. Hacienda/Bib_Hacienda/Bib_Hacienda/Clases/Hacienda.cs
1. Hacienda/Bib_Hacienda/Bib_Hacienda/Bib_Hacienda.csproj
1. Hacienda/p_mvcHacienda/Controllers/ResController.cs
1. Hacienda/p_mvcHacienda/Servicios/PersistenciaService.cs
1. Hacienda/p_mvcHacienda/Program.cs
1. Hacienda/p_mvcHacienda/Views/Res/DetalleVacunas.cshtml
1. Hacienda/p_mvcHacienda/Views/Res/Index.cshtml
Clases nuevas estimadas:

- Evento clínico estructurado: fecha, concepto y observación
Archivos nuevos estimados:

- Archivo fuente del evento clínico estructurado
- Archivo de datos de eventos clínicos repetibles
C — clases existentes solo expuestas a regresión (no se suman a A):

1. Bib_Hacienda.Clases.Vacuna
1. Bib_Hacienda.Clases.Viva
1. Bib_Hacienda.Clases.Bacteriana
1. Bib_Hacienda.Interfaces.IVacunacion
1. p_mvcHacienda.Servicios.VacunaService
1. p_mvcHacienda.Controllers.VacunaController
1. Bib_Hacienda.Clases.Potrero
C — archivos existentes solo expuestos a regresión/transición (no se suman a A):

1. Hacienda/Bib_Hacienda/Bib_Hacienda/Clases/Vacuna.cs
1. Hacienda/Bib_Hacienda/Bib_Hacienda/Clases/Viva.cs
1. Hacienda/Bib_Hacienda/Bib_Hacienda/Clases/Bacteriana.cs
1. Hacienda/Bib_Hacienda/Bib_Hacienda/Interfaces/IVacunacion.cs
1. Hacienda/p_mvcHacienda/Servicios/VacunaService.cs
1. Hacienda/p_mvcHacienda/Controllers/VacunaController.cs
1. Hacienda/Bib_Hacienda/Bib_Hacienda/Clases/Potrero.cs
1. Hacienda/p_mvcHacienda/Views/Vacuna/Aplicar.cshtml
1. Hacienda/p_mvcHacienda/Datos/VacunasAplicadas.txt
Elementos conductuales expuestos a regresión:

1. Duplicados/límites/vencimiento/inventario de vacunas
1. Persistencia y recarga de vacunas aplicadas
1. Detalle vacunal observable
1. Secuencia de carga del singleton
1. Venta de una res vacunada y conservación sanitaria
Control automático: 5=longitud de clases; 8=longitud de archivos; 1=longitud de clases nuevas; 2=longitud de archivos nuevos; 5=longitud de regresiones. Resultado: OK.

# 7. Comparación

| **Criterio** | **SC-1** | **SC-2** | **SC-3** |
| --- | --- | --- | --- |
| Clases existentes | 4 | 4 | 5 |
| Archivos existentes | 6 | 5 | 8 |
| Archivos nuevos | 0 | 0 | 2 |
| Riesgo dominante | Venta/retiro | Identidad/datos | Coherencia clínica/carga |
| OCP | Confirmada | Sospechada | Confirmada en proyección/persistencia sanitaria |

SC-3 tiene mayor blast radius; SC-1 amenaza más directamente una transacción operativa; SC-2 tiene mayor exposición masiva de registros. La extensión estructural se explica causalmente, no por contar if/switch o líneas.

## 7.1 Cinco archivos con mayor incidencia

| **Archivo** | **SC** | **Incidencia** |
| --- | --- | --- |
| PersistenciaService.cs | SC-1, SC-2, SC-3 | 3 |
| Hacienda.cs | SC-1, SC-2, SC-3 | 3 |
| ResController.cs | SC-1, SC-2, SC-3 | 3 |
| Views/Res/Index.cshtml | SC-1, SC-2, SC-3 | 3 |
| Res.cs | SC-2, SC-3 | 2 |

## 7.2 Contraste con el código

- `PersistenciaService`, `Hacienda`, `ResController` y `Views/Res/Index.cshtml` aparecen en las tres solicitudes de cambio y son los puntos de mayor incidencia.
- Las acciones `Create` de `VentaController` son scaffolding sin escritura efectiva; la venta operativa comienza en `ResController.Vender`.
- La incidencia se comprobó siguiendo llamadas y datos en el código. No se interpretó el número de referencias como prueba automática de acoplamiento de negocio.
# 8. Conclusión

El costo AS-IS proviene de interpretaciones rígidas distribuidas: Venta equivale a una res retirada; Res tiene un esquema fijo de cinco campos; y lo sanitario equivale a List<Vacuna> más un archivo especializado. PersistenciaService, Hacienda y ResController concentran esas decisiones y aparecen en los tres cambios.

Línea base condicionada por los alcances mínimos declarados: SC-1 = 4 clases/6 archivos; SC-2 = 4/5; SC-3 = 5/8, más 1 clase y 2 archivos nuevos. Los elementos de regresión y los datos legacy no se sumaron como modificaciones. La Fase 3 deberá compararse contra estos conjuntos, pero este documento no diseña esa arquitectura.

# Anexo. Incertidumbre residual

- Cantidad/unidad, inventario y sacrificio ampliarían SC-1.
- Proveedor/protocolo, tiempo real, mapas o historial ampliarían SC-2.
- Veterinario, diagnóstico, tratamiento o adjuntos ampliarían SC-3.
- No hay suite de tests y la DLL consumida puede divergir de la fuente net472.
- Los datos versionados prueban formatos, no todos los estados ni concurrencia.
