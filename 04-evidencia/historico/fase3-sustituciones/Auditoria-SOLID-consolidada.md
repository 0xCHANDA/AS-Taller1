# Auditoria SOLID consolidada

## Alcance

Se audito el repositorio completo, diferenciando:

- **OLD:** `03-src/original/HaciendaOLD`
- **NEW:** `03-src/redisenado/HaciendaNEW`

Cada version contiene:

- Biblioteca de dominio `Bib_Hacienda`, .NET Framework 4.7.2.
- Aplicacion ASP.NET Core MVC, .NET 8.
- No se encontraron proyectos de pruebas.
- No se modifico codigo de produccion.

El baseline ejecutable no pudo establecerse. Ademas, los proyectos web referencian un DLL precompilado mediante `HintPath`, por lo que una compilacion web no garantiza que el codigo fuente de dominio inspeccionado sea el utilizado.

## Resultado ejecutivo

| Estado | Cantidad consolidada |
|---|---:|
| Confirmados | 10 |
| Sospechados | 2 |
| Falsos positivos rechazados | 5 |
| Trade-offs aceptados | 0 |

### Conclusion

- **OLD** presenta defectos confirmados de LSP, ISP, DIP, SRP y limites arquitectonicos.
- **NEW** conserva casi todos esos defectos y anade inconsistencias bloqueantes en `Res : Producto` y en el rediseno de ventas.
- NEW introduce una mejora local mediante `IInventario<T>`, pero su integracion no esta terminada.
- No se justifica una reescritura completa ni crear una interfaz por clase.

## Hallazgos confirmados

### SOLID-ARCH-001 - Compilacion contra un DLL potencialmente obsoleto

**Principio:** Arquitectura/DIP  
**Estado:** confirmado  
**Severidad:** alta  
**Confianza:** 99 %

#### Ubicaciones

- `03-src/original/HaciendaOLD/p_mvcHacienda/p_mvcHacienda.csproj:14-18`
- `03-src/redisenado/HaciendaNEW/p_mvcHacienda/p_mvcHacienda.csproj:14-18`

#### Evidencia

Ambos proyectos web contienen:

```xml
<Reference Include="Bib_Hacienda">
  <HintPath>..\Bib_Hacienda\Bib_Hacienda\bin\Debug\Bib_Hacienda.dll</HintPath>
</Reference>
```

No existe un `ProjectReference` hacia el proyecto de dominio.

#### Consecuencia

El proyecto MVC puede compilar contra una version anterior del DLL aunque el codigo fuente actual de `Bib_Hacienda` tenga errores o contratos incompatibles. Esto explica por que algunas incompatibilidades de NEW pueden permanecer ocultas.

#### Refactor minimo

- Incluir ambos proyectos en un unico grafo de compilacion.
- Reemplazar `Reference/HintPath` por `ProjectReference`.
- Verificar desde un checkout limpio, sin DLL preexistente.

### SOLID-ARCH-002 - Rediseno de ventas NEW incompleto

**Principio:** Arquitectura  
**Relacionados:** LSP, ISP  
**Estado:** confirmado  
**Severidad:** bloqueante  
**Confianza:** 100 %

#### Ubicaciones

- `03-src/redisenado/HaciendaNEW/Bib_Hacienda/Bib_Hacienda/Clases/Hacienda.cs:30-34,142-166`
- `03-src/redisenado/HaciendaNEW/Bib_Hacienda/Bib_Hacienda/Clases/Venta.cs:10-46`
- `03-src/redisenado/HaciendaNEW/Bib_Hacienda/Bib_Hacienda/Interfaces/IVentaRes.cs:10-14`
- `03-src/redisenado/HaciendaNEW/Bib_Hacienda/Bib_Hacienda/Clases/Validaciones/ValidarVenta.cs:12-18`
- `03-src/redisenado/HaciendaNEW/p_mvcHacienda/Controllers/ResController.cs:131-169`
- `03-src/redisenado/HaciendaNEW/p_mvcHacienda/Servicios/VentaService.cs:18-57`
- `03-src/redisenado/HaciendaNEW/p_mvcHacienda/Servicios/PersistenciaService.cs:132-170,389-450`
- `03-src/redisenado/HaciendaNEW/p_mvcHacienda/Program.cs:53-57`

#### Evidencia

- `Hacienda` sigue declarando `IVentaRes`, pero ya no implementa `vender_res`.
- `L_ventas` esta comentada.
- MVC sigue usando `L_ventas` y `vender_res`.
- NEW `Venta` contiene `Producto`, pero persistencia y validacion esperan `Potrero` y `Res`.
- La carga de ventas intenta invocar un constructor que NEW `Venta` ya no posee.

#### Consecuencia

El codigo fuente NEW no representa una aplicacion coherente compilable. El DLL precompilado puede esconder la contradiccion.

#### Refactor minimo

Tomar una decision explicita:

1. Completar verticalmente el contrato nuevo `Producto/RegistroVenta`; o
2. Restaurar temporalmente el contrato OLD de venta.

No deben convivir ambos contratos incompletos.

### SOLID-LSP-001 - `Res : Producto` no construye correctamente la superclase

**Principio:** LSP  
**Estado:** confirmado  
**Severidad:** bloqueante  
**Confianza:** 100 %

#### Ubicaciones

- `03-src/redisenado/HaciendaNEW/Bib_Hacienda/Bib_Hacienda/Clases/Producto.cs:9-27`
- `03-src/redisenado/HaciendaNEW/Bib_Hacienda/Bib_Hacienda/Clases/Res.cs:12-40`

#### Evidencia

`Producto` solo dispone de:

```csharp
protected Producto(string nombre)
```

`Res` no invoca `base(nombre)`. Ademas, declara otro campo y otra propiedad `Nombre`, ocultando el estado heredado.

#### Contrato roto

Todo `Res` utilizado como `Producto` debe:

- construir un estado base valido;
- conservar la invariante de nombre;
- exponer una sola identidad de nombre.

Actualmente eso no ocurre.

#### Consecuencia

- Error de compilacion por ausencia de constructor base sin parametros.
- Si solo se anadiera `base(nombre)`, podrian quedar dos valores de nombre divergentes segun se observe el objeto como `Res` o como `Producto`.

#### Refactor minimo

- Invocar `base(nombre)`.
- Eliminar el campo y propiedad duplicados de `Res`.
- Usar exclusivamente `Producto.Nombre`.
- Caracterizar primero si existen consumidores que modifican publicamente `Res.Nombre`.

### SOLID-LSP-002 - Los validadores no son sustituibles por `Validacion`

**Principio:** LSP  
**Relacionado:** ISP  
**Estado:** confirmado  
**Severidad:** alta  
**Confianza:** 100 %

#### Ubicaciones

En OLD y NEW:

- `Clases/Validaciones/Validacion.cs:11-17`
- `Clases/Validaciones/ValidarRes.cs:10-34`
- `Clases/Validaciones/ValidarPotrero.cs:10-34`
- `Clases/Validaciones/ValidarVacuna.cs:10-34`
- `Clases/Validaciones/ValidarVenta.cs:10-34`
- `Interfaces/IValidarInformacion.cs:11-23`

#### Evidencia

`Validacion` promete cuatro operaciones:

- `ValidarRes`
- `ValidarPotrero`
- `ValidarVacuna`
- `ValidarVenta`

Cada subclase solo implementa una significativamente y lanza `NotImplementedException` en las otras tres.

#### Contrato roto

Una instancia tratada como `Validacion` deberia admitir todas las operaciones declaradas. La sustitucion introduce tres excepciones de operacion no soportada.

#### Contraevidencia

`PersistenciaService` utiliza tipos concretos y llama unicamente al metodo correspondiente. Eso evita actualmente algunas fallas, pero no hace valido el contrato polimorfico.

#### Refactor minimo

Reemplazar la jerarquia ancha por un contrato tipado:

```csharp
IValidador<in T>
{
    bool Validar(T value);
}
```

Cada validador expondria unicamente su capacidad real. No se necesita una interfaz diferente para cada validador.

### SOLID-LSP-003 - Los subtipos de `Res` fortalecen la precondicion de `Edad`

**Principio:** LSP  
**Estado:** confirmado  
**Severidad:** media  
**Confianza:** 98 %

#### Ubicaciones

OLD:

- `03-src/original/HaciendaOLD/Bib_Hacienda/Bib_Hacienda/Clases/Res.cs:31-35`
- `03-src/original/HaciendaOLD/Bib_Hacienda/Bib_Hacienda/Clases/Ternero.cs:19-24`
- `03-src/original/HaciendaOLD/Bib_Hacienda/Bib_Hacienda/Clases/Cebon.cs:19-24`
- `03-src/original/HaciendaOLD/Bib_Hacienda/Bib_Hacienda/Clases/Novillo.cs:19-24`

NEW:

- `03-src/redisenado/HaciendaNEW/Bib_Hacienda/Bib_Hacienda/Clases/Res.cs:33-37`
- `03-src/redisenado/HaciendaNEW/Bib_Hacienda/Bib_Hacienda/Clases/Ternero.cs:19-24`
- `03-src/redisenado/HaciendaNEW/Bib_Hacienda/Bib_Hacienda/Clases/Cebon.cs:19-24`
- `03-src/redisenado/HaciendaNEW/Bib_Hacienda/Bib_Hacienda/Clases/Novillo.cs:19-24`

#### Evidencia

`Res.Edad` admite cualquier `ushort`. Los overrides restringen el conjunto:

- `Ternero`: `0..12`
- `Cebon`: `13..48`
- `Novillo`: mayor de `48`

Una asignacion valida segun `Res` puede lanzar `Exception` dependiendo del subtipo real.

#### Contraevidencia

`Potrero` comprueba el rango antes de construir cada subtipo y no se encontraron asignaciones posteriores de `Edad`.

Esto reduce el impacto actual, pero no elimina la incompatibilidad del setter publico.

#### Refactor minimo

- Hacer `Edad` inmutable despues de la construccion, o
- mantener un unico contrato de validacion en `Res`;
- eliminar los setters sobrescritos.

La categoria etaria podria calcularse desde la edad si el dominio permite que una res cambie de categoria.

### SOLID-LSP-004 - `Potrero.agregar` incumple `IInventario<Res>`

**Principio:** LSP/ISP  
**Estado:** confirmado  
**Severidad:** media  
**Confianza:** 95 %

#### Ubicaciones

- `03-src/redisenado/HaciendaNEW/Bib_Hacienda/Bib_Hacienda/Interfaces/IInventario.cs:10-15`
- `03-src/redisenado/HaciendaNEW/Bib_Hacienda/Bib_Hacienda/Clases/Potrero.cs:202-205`

#### Evidencia

`IInventario<T>` anuncia `agregar`, pero `Potrero.agregar(Res)` tiene cuerpo vacio. No agrega, no rechaza y no informa un resultado.

#### Consecuencia

Un cliente que utiliza `IInventario<Res>` observa una aparente ejecucion correcta sin cambio de estado.

#### Refactor minimo

- Delegar en una unica operacion real de admision de reses; o
- retirar `IInventario<Res>` de `Potrero` mientras no pueda cumplir el contrato.

### SOLID-DIP-001 - Politica de aplicacion depende de persistencia concreta

**Principio:** DIP  
**Estado:** confirmado  
**Severidad:** alta  
**Confianza:** 96 %

#### Ubicaciones

OLD y NEW:

- `Servicios/PotreroService.cs:9-16,20-38,71-97`
- `Servicios/VacunaService.cs:9-15,19-109`
- `Servicios/UsuarioService.cs:9-50`
- `Controllers/PotreroController.cs:11-20,63-80`
- `Controllers/ResController.cs:11-22,105-169`
- `Servicios/PersistenciaService.cs:12-638`

#### Evidencia

Servicios y controladores dependen directamente de `PersistenciaService`, que fija:

- archivos;
- rutas;
- delimitadores;
- formatos;
- `System.IO`;
- hidratacion;
- validacion;
- Castle;
- estado HTTP.

La inyeccion por constructor es DI, pero no DIP: el consumidor sigue dependiendo del detalle concreto.

#### Refactor minimo

Crear un unico puerto de persistencia propiedad de la aplicacion, limitado a las operaciones realmente usadas. `PersistenciaService` implementaria ese puerto y `Program` seleccionaria el adaptador.

No se recomienda un repositorio por entidad.

### SOLID-DIP-002 - El dominio depende de ASP.NET y Castle

**Principio:** DIP/Arquitectura  
**Estado:** confirmado  
**Severidad:** media  
**Confianza:** 94 %

#### Ubicaciones

OLD y NEW:

- `Bib_Hacienda.csproj:33-65,101-102`
- `Aspectos/InterceptorValidarInformacion.cs:1-80`
- `Aspectos/InterceptorAutenticacion.cs:1-65`
- `Servicios/PersistenciaService.cs:41-56`

#### Evidencia

La biblioteca de dominio:

- referencia Castle;
- referencia ASP.NET;
- implementa `IInterceptor`;
- consume `IHttpContextAccessor`;
- escribe resultados de validacion en `HttpContext.Items`.

#### Consecuencia

El dominio no puede compilarse o ejecutarse independientemente de infraestructura web y proxy.

#### Refactor minimo

Mover interceptores/decoradores y traduccion HTTP al proyecto MVC. El dominio debe devolver resultados de validacion sin tipos Castle o ASP.NET.

### SOLID-SRP-001 - `PersistenciaService` tiene actores independientes

**Principio:** SRP  
**Relacionado:** DIP/Arquitectura  
**Estado:** confirmado  
**Severidad:** media  
**Confianza:** 94 %

#### Ubicacion

- OLD `03-src/original/HaciendaOLD/p_mvcHacienda/Servicios/PersistenciaService.cs:12-638`
- NEW `03-src/redisenado/HaciendaNEW/p_mvcHacienda/Servicios/PersistenciaService.cs:12-638`

#### Evidencia

La clase cambia por motivos independientes:

- cambios del formato de archivos;
- cambios en las entidades;
- cambios de validacion;
- cambios de Castle;
- cambios de HTTP;
- incorporacion de nuevos tipos;
- reglas de reconstruccion.

La longitud no es la evidencia principal; lo son estos actores independientes.

#### Refactor minimo

Primero sacar de persistencia:

1. composicion de proxies;
2. comunicacion por `HttpContext`.

Despues extraer mapeadores de formato unicamente donde exista presion real.

### SOLID-ARCH-003 - Controladores y servicios comparten la misma orquestacion

**Principio:** Arquitectura/SRP  
**Estado:** confirmado  
**Severidad:** media  
**Confianza:** 97 %

#### Ubicaciones

OLD y NEW:

- `Controllers/PotreroController.cs:63-84`
- `Servicios/PotreroService.cs:20-38`
- `Controllers/ResController.cs:105-180`
- `Servicios/VacunaService.cs:53-92`

#### Evidencia

- `PotreroService.CrearPotrero` persiste el estado.
- `PotreroController.Create` vuelve a persistirlo.
- `ResController` modifica `Hacienda` y coordina persistencia directamente.
- La vacunacion efectua varias escrituras desde el servicio sin una frontera transaccional clara.

#### Consecuencia

Duplicacion de escrituras, manejo inconsistente de errores y posibilidad de estado parcialmente persistido.

#### Refactor minimo

Un servicio de aplicacion debe ser propietario de cada operacion completa. El controlador solo enlaza entrada, invoca la operacion y traduce el resultado a MVC.

### SOLID-ARCH-004 - Startup puede publicar estado parcialmente cargado

**Principio:** Arquitectura  
**Estado:** confirmado  
**Severidad:** alta  
**Confianza:** 90 %

#### Ubicaciones

- `03-src/original/HaciendaOLD/p_mvcHacienda/Program.cs:33-74`
- `03-src/redisenado/HaciendaNEW/p_mvcHacienda/Program.cs:33-74`

#### Evidencia

El singleton `Hacienda` se modifica progresivamente durante la carga. Un `catch` general registra el error, pero devuelve la instancia aunque solo parte de los archivos se haya cargado.

#### Consecuencia

La aplicacion puede iniciar con estado incompleto y posteriormente sobrescribir datos persistidos validos.

#### Refactor minimo

Construir una instancia temporal y publicarla solo cuando la hidratacion completa termine. Ante fallo, adoptar explicitamente una politica:

- abortar startup; o
- iniciar vacio/degradado bloqueando escrituras.

## Hallazgos sospechados

### SOLID-SRP-002 - Amplitud de `Hacienda`

**Estado:** sospechado  
**Confianza:** 78 %

`Hacienda` coordina potreros, reses, ventas, vacunas y mensajes, pero puede ser intencionalmente el agregado/fachada central del dominio. Sin historial de cambios o requisitos independientes, no se confirma una violacion unicamente por su tamano.

**Decision:** no dividir todavia.

### SOLID-DIP-003 - Uso directo de `DateTime.Now`

**Estado:** sospechado  
**Confianza:** 72 %

Aparece en ventas, vencimiento y estadisticas. Dificulta pruebas de fronteras temporales, pero no se encontro un requisito de zonas horarias o reloj sustituible.

**Decision:** no introducir `IClock` sin necesidad demostrada.

## Falsos positivos rechazados

1. **Controladores inyectando servicios concretos:** no obliga a crear `IPotreroService`, `IResService`, etc., sin una variacion real.
2. **Persistencia externa dependiendo de entidades del dominio:** la direccion adaptador externo hacia dominio es valida.
3. **Uso de modelos del dominio en vistas:** no existe evidencia que exija DTO para cada pantalla.
4. **Condicionales para clasificar reses:** un `if` o `switch` no constituye por si solo una violacion OCP.
5. **`IInventario<T>` de NEW:** la abstraccion esta expresada en terminos de politica y su direccion es valida; el defecto concreto esta en la implementacion vacia de `Potrero.agregar`.

## Comparacion OLD vs NEW

| Area | OLD | NEW | Resultado |
|---|---|---|---|
| Build graph | DLL por `HintPath` | Igual | Defecto conservado |
| `Res.Edad` | Contratos incompatibles | Igual | Violacion conservada |
| Validadores | Tres operaciones no soportadas | Igual | Violacion conservada |
| Castle/ASP.NET en dominio | Presente | Presente | Violacion conservada |
| Persistencia concreta | Acoplada | Igual | Violacion conservada |
| Ventas | Contrato internamente alineado | Contratos OLD y NEW mezclados | Regresion bloqueante |
| `Producto` | No existe | `Res`, `Lacteo`, `Piel` | Idea valida, `Res` defectuosa |
| Vacunas | Subtipos principalmente de datos | Polimorfismo `Tipo/PuedeAplicarseA` | Mejora local |
| Inventarios | Especificos | `IInventario<T>` | Mejora parcial |
| Pruebas | No encontradas | No encontradas | Riesgo conservado |

## Plan minimo y reversible

| Orden | Accion |
|---:|---|
| 1 | Establecer baseline real y un unico grafo de compilacion |
| 2 | Alinear completamente el contrato de ventas NEW |
| 3 | Reparar `Res : Producto`, usando un unico `Nombre` |
| 4 | Anadir pruebas de caracterizacion de edad, venta, persistencia y validacion |
| 5 | Eliminar los overrides incompatibles de `Edad` |
| 6 | Sustituir `Validacion` por `IValidador<T>` |
| 7 | Mover Castle y HTTP fuera del dominio |
| 8 | Introducir un unico puerto de persistencia |
| 9 | Hacer que los servicios sean duenos de la orquestacion |
| 10 | Hacer atomica la hidratacion de startup |

### Pruebas minimas necesarias

- Edades limite: 12/13 y 48/49.
- Construccion y nombre de una `Res` observada como `Producto`.
- Venta: retiro del inventario, registro y persistencia.
- Validacion de cada entidad sin `NotImplementedException`.
- Round-trip de cada archivo de texto.
- Fallos durante cada fase de carga.
- Una sola escritura al crear un potrero.
- Resultados MVC y mensajes existentes.

## Limitaciones y desacuerdos

- Los agentes especializados de SRP, OCP e ISP no pudieron iniciarse por una configuracion invalida de sus modelos. Sus areas se contrastaron mediante la auditoria arquitectonica, LSP y DIP, pero no se fabrico consenso independiente.
- No se ejecuto build/test, por lo que no se atribuyen fallos de entorno o compilacion a cambios.
- Los errores de NEW senalados como bloqueantes se desprenden directamente de firmas y constructores incompatibles.
- No se modifico codigo de produccion.