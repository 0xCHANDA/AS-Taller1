# ADR 3 — Separar registro de ventas

## ADR-003: Crear `RegistroVenta`

### Contexto

Actualmente `Hacienda` mantiene la lista de ventas y además realiza la venta.

Esto significa que `Hacienda` tiene dos responsabilidades:

1. Coordinar una venta
2. Administrar el historial de ventas

### Alternativa 1

Mantener dentro de `Hacienda` el registro de las ventas.

`List`

No afecta el comportamiento del programa, pero sí deja muchas responsabilidades a la clase de `Hacienda`.

### Alternativa 2

Crear un `RegistroVenta`, responsable de registrar ventas (o sea de guardarlas), así toda la responsabilidad de las ventas no queda en `Hacienda`.

### Decisión

Elegimos la opción 2.

### Consecuencia negativa

Ahora existe una clase adicional y la venta requiere delegar el registro:

```csharp
registroVentas.Registrar(venta);
```

Pero esto se acepta porque la responsabilidad queda correctamente encapsulada.

### Principio

- **SRP**
