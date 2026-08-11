# Fase 2 — Análisis de los cambios solicitados

## Sistema Hacienda AS-IS

Este documento analiza el sistema en su estado actual. Su propósito no es proponer todavía una arquitectura nueva, sino determinar cuánto costaría incorporar los tres cambios solicitados y qué partes del sistema podrían verse afectadas.

# 1. Objetivo

El objetivo de esta fase es identificar qué partes del sistema Hacienda tendrían que modificarse para implementar:

- **SC-1:** venta de productos derivados del ganado.
- **SC-2:** chips para geolocalizar las reses.
- **SC-3:** historia clínica de cada res.

Para cada solicitud revisamos:

- Las clases y los archivos que necesariamente tendrían que cambiar.
- Los archivos o clases nuevos que probablemente habría que crear.
- Las funciones actuales que podrían dejar de operar correctamente.
- El nivel de riesgo de cada cambio.

En esta fase no definimos todavía cómo debería quedar la arquitectura final.
Únicamente analizamos el impacto que tendrían los cambios sobre el sistema
actual.
Metodología humanizada
# 2. Cómo realizamos el análisis

## 2.1 Material revisado

Tomamos el código fuente como fuente principal. Revisamos especialmente:

- Las clases de dominio de `Bib_Hacienda`.
- Los controladores, servicios y vistas de `p_mvcHacienda`.
- Las interfaces y validaciones.
- La persistencia en archivos de texto.
- Los eventos e interceptores.
- `Program.cs`.
- Los archivos de datos existentes.
- El diagrama UML AS-IS.
- El inventario de hallazgos realizado en la fase anterior.

Los diagramas y documentos nos ayudaron a entender el contexto, pero las
conclusiones se comprobaron directamente en el código.

## 2.2 Clasificación de los elementos

Para evitar contar dos veces un mismo elemento, usamos tres grupos:

| Grupo | Significado |
|---|---|
| Elementos que se deben modificar | Archivos o clases existentes que necesariamente requieren cambios. |
| Elementos nuevos | Archivos o clases que todavía no existen y que serían necesarios para cumplir el alcance mínimo. |
| Elementos que se deben verificar | Partes que podrían no modificarse, pero cuyo funcionamiento puede verse afectado indirectamente. |

Los archivos de datos antiguos no se cuentan como modificaciones cuando sea
posible mantener su compatibilidad desde el código.
Resumen ejecutivo más natural
# 3. Resumen general

Después de seguir las rutas del código correspondientes a las tres solicitudes,
obtuvimos el siguiente resultado:

| Solicitud | Clases existentes por modificar | Archivos existentes por modificar | Áreas afectadas | Riesgo |
|---|---:|---:|---|---|
| SC-1 — Productos derivados | 4 | 6 | Dominio, aplicación, persistencia y presentación | Alto |
| SC-2 — Chips y geolocalización | 4 | 5 | Dominio, aplicación, persistencia y presentación | Alto |
| SC-3 — Historia clínica | 5 | 8 | Dominio, aplicación, persistencia, presentación y arranque | Alto |

La historia clínica es el cambio que afecta más archivos. Sin embargo, los tres
cambios tienen riesgos importantes:

- En SC-1 existe el riesgo de retirar una res cuando solo se está vendiendo un
  producto derivado.
- En SC-2 existe el riesgo de asociar un chip con la res equivocada o dejar de
  cargar los registros actuales.
- En SC-3 existe el riesgo de que la información clínica y las vacunas queden
  inconsistentes.
Ejemplo de SC-1 humanizado
# 4. SC-1 — Venta de productos derivados

## 4.1 Alcance utilizado

La solicitud indica que la hacienda quiere comenzar a vender productos
derivados, como lácteos, carne y piel.

Para poder medir el impacto definimos un alcance mínimo. El sistema deberá
permitir registrar:

- La categoría del producto vendido.
- El valor de la venta.
- La res y el potrero de origen.
- La recuperación de esa información después de reiniciar.
- La visualización de la categoría en el historial.

También debe mantenerse una diferencia fundamental: vender la res completa
debe retirarla del potrero, pero vender un producto derivado no debe hacerlo.

No incluimos todavía cantidades, unidades de medida, clientes, facturación,
inventarios de productos ni procesos de sacrificio.
## 4.2 Cómo funciona actualmente

La venta activa del sistema comienza en la vista de reses:

`Views/Res/Index.cshtml` → `ResController.Vender()` →
`Hacienda.vender_res()` → `PersistenciaService`.

Actualmente, la clase `Venta` solo representa la venta completa de una res.
Guarda el potrero, la res, la fecha y el monto. Además,
`Hacienda.vender_res()` siempre retira la res del potrero.

Esto significa que el sistema no puede representar una venta derivada sin
modificar varias partes. Si simplemente se reutilizara la operación actual,
vender leche o piel podría eliminar incorrectamente la res del inventario.
## 4.3 Archivos que tendrían que cambiar

| Elemento | Cambio necesario | Motivo | Riesgo |
|---|---|---|---|
| `Venta.cs` | Guardar la categoría de la venta | Actualmente solo representa una venta ganadera | Alto |
| `Hacienda.cs` | Diferenciar entre vender una res y vender un derivado | Hoy siempre retira la res | Alto |
| `ResController.cs` | Recibir y validar la categoría | Actualmente solo recibe el monto | Alto |
| `PersistenciaService.cs` | Guardar y recuperar la categoría | El formato actual tiene siete campos fijos | Alto |
| `Views/Res/Index.cshtml` | Permitir seleccionar el tipo de venta | El formulario solo pide el monto | Alto |
| `Views/Venta/Index.cshtml` | Mostrar correctamente las nuevas ventas | La vista supone que toda venta corresponde a una res completa | Alto |
## 4.4 Principales riesgos

### Venta o retiro incorrecto de una res

La operación actual siempre elimina la res del potrero. Al agregar productos
derivados, una condición incorrecta podría producir dos errores:

- Retirar la res al vender únicamente un derivado.
- Conservar la res después de venderla completamente.

Ambos errores afectan directamente el inventario ganadero.

### Incompatibilidad con las ventas existentes

`PersistenciaService` lee las ventas usando posiciones fijas. Si se agrega una
columna sin conservar el formato anterior, las ventas existentes podrían dejar
de cargar.

### Fallos en el historial

La vista de ventas supone que todas las ventas tienen una res, un potrero y un
peso. Esa suposición debe revisarse para evitar errores al mostrar productos
derivados.
Ejemplo de SC-2 humanizado
# 5. SC-2 — Chips y geolocalización

## 5.1 Alcance utilizado

Para medir este cambio asumimos que una res podrá recibir un chip después de
haber sido registrada.

El chip tendrá:

- Un identificador único.
- Una última latitud conocida.
- Una última longitud conocida.

La res podrá existir sin chip, ya que los 226 registros actuales no tienen esa
información. No incluimos comunicación en tiempo real, mapas, MQTT, geocercas,
historial de recorridos ni integración con proveedores externos.
## 5.2 Impacto sobre el sistema actual

La clase `Res` solo almacena nombre, peso, edad y vacunas. Por esta razón,
tendría que ampliarse para conservar el chip y la última posición.

La asociación debería realizarse desde `Hacienda`, porque esta clase ya
coordina la búsqueda y modificación de las reses. También sería necesario
agregar una acción al controlador y modificar la persistencia.

El principal cuidado está en no hacer obligatorio el chip durante la creación.
Si se cambiara el constructor de `Res`, el cambio se propagaría a `Ternero`,
`Cebon`, `Novillo`, `Potrero` y a todos los registros existentes.
## 5.3 Archivos que tendrían que cambiar

| Elemento | Cambio necesario | Motivo | Riesgo |
|---|---|---|---|
| `Res.cs` | Almacenar chip y última posición | La información no existe en el modelo actual | Alto |
| `Hacienda.cs` | Asociar el chip y evitar duplicados | Se necesita controlar la unicidad global | Alto |
| `ResController.cs` | Recibir los datos del chip | Actualmente no existe una acción para esta operación | Alto |
| `PersistenciaService.cs` | Guardar y cargar los nuevos campos | `Reses.txt` utiliza cinco columnas | Alto |
| `Views/Res/Index.cshtml` | Conectar y mostrar el chip | La interfaz no muestra ubicación | Alto |
Ejemplo de SC-3 humanizado
# 6. SC-3 — Historia clínica

## 6.1 Alcance utilizado

Para este análisis consideramos una historia clínica básica. Cada res podrá
tener varios eventos con:

- Fecha.
- Concepto.
- Observación.

Las vacunas continuarán almacenándose en la colección actual. No se duplicarán
como eventos clínicos. Tampoco incluimos veterinarios, recetas, archivos
adjuntos, diagnósticos codificados ni facturación.
## 6.2 Impacto sobre el sistema actual

Actualmente, la información sanitaria de una res se limita a una lista de
vacunas. La vista `DetalleVacunas.cshtml`, el controlador y la persistencia
también están diseñados exclusivamente alrededor de ese concepto.

Por lo tanto, agregar una historia clínica no consiste únicamente en añadir una
lista a `Res`. También es necesario:

- Registrar eventos clínicos.
- Guardarlos en un archivo.
- Recuperarlos durante el inicio de la aplicación.
- Presentarlos junto con las vacunas sin mezclarlos.
- Mantener intactas las reglas actuales de vacunación.

Este es el cambio con mayor alcance porque afecta el dominio, la persistencia,
el arranque y la presentación.
Conclusión humanizada
# 8. Conclusión

Los tres cambios son posibles, pero el sistema actual obliga a modificar varias
partes para incorporar cada uno.

Esto ocurre porque algunas ideas del negocio están representadas de una forma
muy específica:

- Una venta siempre significa vender y retirar una res.
- Una res siempre se almacena con cinco campos.
- La información sanitaria siempre se interpreta como una lista de vacunas.

Las clases que más se repiten en el análisis son `Hacienda`,
`PersistenciaService` y `ResController`. Las tres participan en las solicitudes
SC-1, SC-2 y SC-3, por lo que concentran buena parte del costo y del riesgo.

Con el alcance mínimo definido, los resultados son:

- **SC-1:** 4 clases y 6 archivos existentes.
- **SC-2:** 4 clases y 5 archivos existentes.
- **SC-3:** 5 clases y 8 archivos existentes, además de una clase y dos archivos nuevos.

SC-3 es el cambio más amplio. SC-1 tiene el riesgo operativo más inmediato,
porque puede retirar una res incorrectamente. SC-2 afecta una cantidad mayor de
datos existentes, debido a los 226 registros que deben seguir cargando sin chip.

Estos resultados servirán como punto de comparación para la siguiente fase,
en la que sí podrá evaluarse una arquitectura futura.
Cambios de estilo recomendados para todo el documento
Reemplaza sistemáticamente:
Expresión actual	Alternativa natural
Blast radius	Alcance del cambio / partes afectadas
Must-modify	Elementos que deben modificarse
Aditivo	Elementos nuevos
Solo regresión	Elementos que deben verificarse
Conducta observable	Comportamiento actual
Presión OCP confirmada	El diseño dificulta agregar esta variante
Presión sospechada	Podría existir una dificultad, pero falta evidencia
Change driver	Motivo de cambio
Round-trip	Guardado y recuperación
Scaffolding inerte	Código generado que actualmente no se usa
Parser posicional	Lectura por posiciones fijas
Legacy	Existente / anterior
Consumir contratos	Utilizar métodos o datos modificados
Transitivo	Afectado indirectamente
Además:
1. Evita repetir “Solicitud literal”, “Implementación mínima” y “Presión SOLID” de manera idéntica en las tres secciones.
2. Conserva las cifras, rutas y líneas: son la parte más convincente.
3. Reduce las listas nominales duplicadas si ya aparecen en una tabla.
4. Usa primera persona plural en puntos concretos: “revisamos”, “seguimos”, “comprobamos”.
5. No elimines los fallos y limitaciones del baseline; aportan credibilidad.
6. Sustituye “Alta” por “Alto” cuando la columna se llame “Riesgo”.
7. Explica primero el problema con lenguaje del negocio y después menciona SOLID.
8. Mantén los escenarios de caracterización, pero llámalos “Casos que debemos comprobar antes del cambio”.
