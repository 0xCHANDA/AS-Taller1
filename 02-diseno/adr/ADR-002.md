# ADR 2 — Generalización de productos

## ADR-002: Crear la abstracción `Producto`

### Contexto

Originalmente la venta estaba especializada en reses:

`vender_res(...)`

Pero decidimos implementar la SC-1:

| SC-1 | La hacienda en el futuro va a comenzar a vender productos derivados del ganado como: lácteos, carne, piel |
| ---- | --------------------------------------------------------------------------------------------------------- |

```text
Res
Lacteos
Pieles
Carne
```

Si se crean métodos:

```csharp
vender_res()
vender_lacteo()
vender_piel()
vender_carne()
```

cada nuevo producto obliga a modificar `Hacienda`.

### Alternativa 1

Crear un método independiente por producto.

```csharp
VenderRes()
VenderLacteo()
VenderPiel()
```

**Se descarta** porque viola OCP.

### Alternativa 2

Creamos una abstracción:

```csharp
public abstract class Producto
{
    public string Nombre { get; protected set; }
}
```

Con:

```text
Producto
├── Res
├── Lacteo
├── Piel
└── Carne
```

### Decisión

Crear `Producto` como abstracción común para los elementos que se van a vender.

Entonces la operación general trabaja con:

`Producto producto`

en lugar de:

`Res res`

### Consecuencia negativa

La abstracción `Producto` debe contener únicamente comportamiento verdaderamente común.

Si se agregan propiedades específicas de reses:

`Peso` `Edad` `Vacunas`

a `Producto`, se estaría contaminando la abstracción.

### Principios

- **OCP**
- **LSP**
- **DIP**
