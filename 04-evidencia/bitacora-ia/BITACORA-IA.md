# Bitácora de uso de IA

Usamos la IA para revisar ideas y comparar opciones, pero no aplicamos todo lo que sugirió. Cada propuesta se evaluó pensando en el problema real del proyecto, en conservar el comportamiento anterior y en no agregar complejidad innecesaria.

| Qué propuso la herramienta | Decisión del equipo | Argumento técnico |
|---|---|---|
| Crear `RegistroVenta` para encargarse de guardar las ventas. | **ACEPTADA** | Antes `Hacienda` coordinaba potreros, vacunas y también almacenaba las ventas. Con `RegistroVenta`, esa parte queda separada y `Hacienda` solo coordina la operación. Esto mejora SRP. |
| Crear una clase base `Producto` para representar lo que se puede vender. | **ACEPTADA** | Nos servía para manejar de la misma forma productos como `Res`, `Lacteo`, `Piel` y `Carne`, sin amarrar la venta a un solo tipo. |
| Crear un método distinto para cada producto, por ejemplo `vender_res`, `vender_piel` o `vender_lacteo`. | **RECHAZADA** | Cada producto nuevo obligaría a modificar `Hacienda` y agregar otro método. Preferimos una sola operación de venta que trabaje con la abstracción `Producto`. |
| Usar una interfaz de inventario con las operaciones necesarias para agregar, retirar y consultar productos. | **CORREGIDA** | La idea era correcta, pero usamos `IInventarioVendible<T>` para mantener el tipo concreto de cada inventario. Así `Potrero`, `InventarioLacteos` e `InventarioPieles` comparten el mismo contrato sin perder seguridad de tipos. |
| Volver inmutable `Producto.Nombre` para asegurar que se venda el mismo objeto que se retira. | **CORREGIDA** | Quitar el setter cambiaba la API que ya existía. La solución fue registrar en la venta exactamente el objeto devuelto por `retirar`. |
| Crear una clase abstracta `Animal` antes de `Res`. | **RECHAZADA** | El sistema solo maneja reses. Agregar otra capa de herencia no resolvía una necesidad actual. |
| Crear interfaces para todos los servicios y llevar al código todas las clases del primer UML. | **RECHAZADA** | Varias abstracciones no tenían otra implementación ni un caso real de cambio. Dejamos solamente las interfaces que el sistema sí usa. |
| Tratar todos los `switch` como violaciones de OCP. | **RECHAZADA** | Un `switch` no es un problema por sí solo. Para demostrar OCP nos concentramos en el cambio importante de la SC-1: poder vender nuevos tipos de producto. |
| Aprovechar el rediseño para cambiar mensajes, excepciones y reglas del sistema anterior. | **RECHAZADA** | El objetivo era conservar el comportamiento observable. Cuando NEW produjo resultados diferentes, se corrigió para que volviera a comportarse como OLD. |
| Elegir la SC-3, historia clínica, como cambio principal. | **RECHAZADA** | La SC-1 permitía mostrar OCP de forma más clara y con menos cambios mezclados. Las otras propuestas se revisaron, pero no se implementaron. |
| Separar la creación de vacunas de `Hacienda` en `FabricadorVacunas`. | **ACEPTADA** | Había lógica repetida en cuatro métodos de creación de vacunas. La nueva clase concentra esa tarea y `Hacienda` solo delega. |
| Crear una fábrica genérica con reflexión, registro de tipos e interfaz propia. | **RECHAZADA** | Solo existen dos tipos de vacuna. Para este alcance, una fábrica genérica agregaba más complejidad de la que resolvía. |
| Recibir `RegistroVenta` y `FabricadorVacunas` por el constructor de `Hacienda`. | **ACEPTADA** | Permite crear esos colaboradores desde `Program.cs` y facilita las pruebas. Es inyección de dependencias, aunque no demuestra DIP porque las dos dependencias siguen siendo clases concretas. |
| Mover `alimentar_res` a un servicio nuevo. | **RECHAZADA** | Esa operación coordina el potrero, la res y sus eventos. Separarla habría agregado otra clase sin dejar una responsabilidad más clara. |
| Devolver una copia desde `Hacienda.L_ventas` para proteger el registro interno. | **RECHAZADA** | OLD expone una lista viva y algunos consumidores podrían modificarla. Para no cambiar comportamiento público, NEW conserva esa semántica y la comprueba en C21. |
| Eliminar los setters públicos de `Edad` y `L_vacunas_aplicadas`. | **CORREGIDA** | Mejoraba encapsulamiento, pero rompía la API de OLD. Se conservaron los setters y se añadieron C22/C23 para verificar reglas e identidad de las listas. |

## Conclusión

La IA nos ayudó a ver alternativas y detectar problemas, pero las decisiones finales salieron de revisar el código y las pruebas. Aceptamos las ideas que resolvían una necesidad real, corregimos las que rompían compatibilidad y descartamos las que hacían el diseño más complicado sin aportar valor.
