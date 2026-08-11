# Fase 0 — Lectura en frío

**Simón Bedoya Urrea — 000547028**

Con la primera vista a la biblioteca de clases podemos ver que es un programa de una hacienda que gestiona todas sus reses, tanto las vacunas que tienen, su alimentación y las ventas.

Las entidades principales son `Hacienda`, que es la clase donde está casi toda la lógica del negocio, junto con sus potreros, las ventas y las vacunas; `Res`, que es la clase padre de los novillos, cebones y terneros, y donde se almacena toda la información de estos animales; `Vacuna`, que posee la información de lote, vencimiento y aplicación y es la clase padre de las vacunas vivas y bacterianas; y `Venta`, que guarda la información de las ventas, pero no tiene métodos, pues casi todo está en la clase `Hacienda`.

## Lugares más costosos de cambiar

En primer lugar, el más obvio: `Hacienda`. Todos los métodos están en esta clase y sería muy costoso hacer cambios en ella, pues podrían afectar el funcionamiento de todo el programa.

Luego está `Res`. Teniendo en cuenta cambios futuros, muchos de los métodos puestos en `Hacienda` podrían desplazarse a esta clase. Además, al ser la clase padre de los novillos, cebones y terneros, un cambio en ella podría afectar las subclases.

Finalmente está `Vacuna`. Un cambio en esta clase podría afectar el comportamiento de las vacunas vivas y las vacunas bacterianas.

## Pregunta para el ingeniero

Yo le preguntaría por qué dejó tantas responsabilidades a la clase `Hacienda`. Todo está completamente acoplado a ella y casi no deja responsabilidades a otras clases como `Vacuna`, `Res` o `Potrero`.
