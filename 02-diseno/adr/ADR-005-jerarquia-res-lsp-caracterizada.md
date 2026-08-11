# ADR 5 — Generalización del método de venta

## ADR-005: Crear un método `Vender` general para cualquier producto

### Contexto

El método original de `Hacienda` estaba diseñado únicamente para vender reses:

```csharp
public string vender_res(string id_potrero, string nombre, uint monto)
```

Esto hacía que `Hacienda` conociera directamente a `Potrero` y a `Res`.

Con la creación de nuevos productos e inventarios:

```text
Potrero
InventarioLacteos
InventarioPieles
InventarioCarne
```

mantener métodos como:

```csharp
vender_res()
vender_lacteo()
vender_piel()
vender_carne()
```

haría que cada nuevo producto obligara a modificar `Hacienda`.

### Alternativa 1

Crear un método de venta para cada tipo de producto:

```csharp
VenderRes(...)
VenderLacteo(...)
VenderPiel(...)
VenderCarne(...)
```

Se descarta porque `Hacienda` tendría que modificarse cada vez que aparezca un nuevo producto, violando OCP.

Además, aumentaría la cantidad de lógica que tiene `Hacienda`.

### Alternativa 2

Crear un único método general:

```csharp
Vender<T>(IInventario<T> inventario, T producto, uint monto)
    where T : Producto
```

El método recibe el inventario y el producto que se quieren vender.

Internamente:

```csharp
inventario.Retirar(producto);

Venta venta = new Venta(producto, DateTime.Now, monto);

registroVentas.Registrar(venta);
```

De esta forma, el método no necesita saber si está vendiendo una:

```text
Res
Lacteo
Piel
Carne
```

### Decisión

Se decidió reemplazar `vender_res()` por un método general `Vender()` que trabaje con la abstracción `IInventario<T>` y `Producto`.

Así `Hacienda` solamente coordina la operación:

```text
Hacienda
   ↓
IInventario<T>
   ↓
Retirar(producto)

Producto
   ↓
Venta
   ↓
RegistroVentas
```

Por ejemplo, para vender una res:

```csharp
Vender(potrero, res, monto);
```

Y para vender un lácteo:

```csharp
Vender(inventarioLacteos, lacteo, monto);
```

El método es el mismo.

### ¿Qué ganamos?

- No necesitamos crear un método de venta por cada producto.
- `Hacienda` deja de depender directamente de `Potrero`.
- Se pueden agregar nuevos productos sin modificar el método `Vender`.
- Los diferentes inventarios pueden manejar sus propios productos.
- `Venta` deja de estar acoplada exclusivamente a `Res`.
- Se aplica OCP, porque agregar un nuevo tipo de producto no requiere modificar la lógica existente.

### Consecuencia negativa

El método utiliza genéricos e interfaces, por lo que la solución es un poco más compleja que el método original `vender_res()`.

Además, cada nuevo producto debe heredar de `Producto` y contar con un inventario que implemente `IInventario<T>`.

Aceptamos esta complejidad porque permite que el sistema pueda crecer sin tener que modificar `Hacienda` cada vez que aparezca un nuevo producto.

### Principios involucrados

- **OCP:** se pueden agregar nuevos productos sin modificar `Vender()`.
- **DIP:** `Hacienda` trabaja con `IInventario<T>` en lugar de depender de un inventario concreto.
- **SRP:** `Hacienda` coordina la venta, mientras `RegistroVentas` se encarga de almacenarlas y el inventario de retirar el producto.
- **LSP:** cualquier producto que cumpla el contrato de `Producto` puede utilizarse donde se espera un `Producto`.
- **ISP:** `Hacienda` utiliza únicamente las operaciones del inventario que necesita (retirar).
