# ADR 1 — Abstracción de inventarios

## ADR-001: Crear `IInventario`

### Contexto

El código original está acoplado a `Potrero`.

Por ejemplo en el método de vender tenemos esto, dejándonos una gran dificultad para vender los nuevos productos:

```csharp
Potrero potrero = buscar_potrero(idPotrero);

potrero.eliminar_res(nombre);
```

Pero el requisito de la creación de productos para vender nos obliga a crear diferentes inventarios:

```csharp
Potrero
InventarioLacteos
InventarioPieles
InventarioCarne
```

### Alternativa 1

Mantener `Potrero` como único inventario.

Esto generaría que cada nuevo producto tenga lógica especial en `Hacienda`. Esto nos obligaría a crear un método de vender por cada producto, lo cual violaría el principio open close.

### Alternativa 2

Crear:

```csharp
public interface IInventario
{
    void Agregar(Producto producto);
    Producto Retirar(Producto producto);
    bool Contiene(Producto producto);
}
```

Y así podemos tener implementaciones: `Potrero`, `InventarioCarnes`, `InventarioLacteos`, `InventarioPieles`.

### Decisión

Adoptamos `IInventario`.

Así `Hacienda` depende de:

`IInventario`

en lugar de:

`Potrero`

### ¿Qué ganamos?

Se puede cambiar el método de `vender_res` a uno de `vender`, haciendo que no tengamos que tener un nuevo método para vender por cada producto.

### Consecuencia negativa

La interfaz agrega una capa de abstracción y obliga a que cada inventario implemente operaciones comunes.

Además, no todos los inventarios necesariamente tendrán exactamente la misma lógica interna.

### Principios

- **DIP**
- **OCP**
- **ISP**
