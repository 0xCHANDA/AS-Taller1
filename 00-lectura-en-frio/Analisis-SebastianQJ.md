# Lectura en frío

**Sebastián Quijano Jaramillo**

El sistema corresponde a una aplicación para administrar una hacienda ganadera. Su propósito es gestionar los potreros y las reses de la hacienda, registrar la alimentación y las ventas de los animales, controlar las vacunas y sus aplicaciones, y administrar el acceso de los usuarios al sistema.

Las entidades principales del sistema son `Hacienda`, que actúa como la entidad central encargada de administrar los potreros, las vacunas y las ventas; `Potrero`, que representa el espacio donde se ubican las reses; `Res`, que representa al ganado y es la clase padre de `Ternero`, `Cebon` y `Novillo`; `Vacuna`, que representa las vacunas aplicadas al ganado y es la clase padre de `Bacteriana` y `Viva`; `Venta`, que registra la información relacionada con la venta de una res, como el potrero, la fecha y el monto; y `Usuario`, que representa a las personas autorizadas para acceder y utilizar el sistema.

## Lugares más costosos de cambiar

### `Hacienda.cs`

La clase `Hacienda` sería uno de los lugares más costosos para realizar cambios, ya que en ella se concentra gran parte de la lógica de negocio del sistema y se administran las principales entidades, como los potreros, las vacunas y las ventas. Por esto, una modificación sería un proceso demorado y costoso.

### `PersistenciaService.cs`

Creo que sería uno de los lugares más difíciles de cambiar porque ahí es donde se guarda y se carga casi toda la información del sistema. Si se le agrega algo nuevo a una entidad o se cambia un dato, también habría que cambiar la forma en que se guarda y se lee esa información. Si algo queda mal, los datos pueden no cargarse correctamente.

### `Res`

También sería un lugar costoso de cambiar porque es la clase principal de los animales. Si se cambia algo en ella, como una propiedad o la forma en que funciona, es muy probable que también haya que modificar las clases `Ternero`, `Cebon` y `Novillo`, además de otras partes del sistema que trabajan con las reses.

## Pregunta para el ingeniero

¿Por qué la clase `Hacienda` concentra tantas responsabilidades, como crear potreros, administrar reses, aplicar vacunas y registrar ventas, en lugar de separar esas operaciones en clases especializadas?
