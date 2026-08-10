# ADR 1 — Abstracción de inventarios

## ADR-001: Crear `IInventario`

### Contexto

El código original está acoplado a `Potrero`.

Por ejemplo en el método de vender tenemos esto , dejandonos una gran dificultad para vender los nuevos productos:

```csharp
Potrero potrero = buscar_potrero(idPotrero);

potrero.eliminar_res(nombre);
```

Pero el requisito de la creación de productos para vender nos obliga a crea diferentes inventarios:

```csharp
Potrero
InventarioLacteos
InventarioPieles
InventarioCarne
```

### Alternativa 1

Mantener `Potrero` como único inventario.

Esto generaria que cada nuevo producto tenga lógica especial en `Hacienda` . Esto nos obligaria a crear un método de vender por cada producto, lo cual violaria el principio open close.   

### Alternativa 2

Crear:

```csharp
public interface IInventario
{ 
void Agregar(Productoproducto);
Producto Retirar(Productoproducto);
boolC ontiene(Productoproducto);
}
```

Y así podemos tener implementaciones: Potrero , InventarioCarnes, InventarioLacteos, InventarioPieles. 

### Decisión

adoptamos  `IInventario`.

Así `Hacienda` depende de:

```
IInventario
```

en lugar de:

```
Potrero
```

### ¿Qué ganamos?

se puede cambiar el método de vender_res a uno de vender, haciendo que no tengamos que tener un nuevo método para vender por cada producto.     

### Consecuencia negativa

La interfaz agrega una capa de abstracción y obliga a que cada inventario implemente operaciones comunes.

Además, no todos los inventarios necesariamente tendrán exactamente la misma lógica interna.

### Principios

- **DIP**
- **OCP**
- **ISP**

---

# ADR 2 — Generalización de productos

## ADR-002: Crear la abstracción `Producto`

### Contexto

Originalmente la venta estaba especializada en reses:

```
vender_res(...)
```

Pero decidimos implementar la SC-1

| SC-1 | La hacienda en el futuro va a comenzar a vender productos derivados del ganado como: lácteos, carne, piel |
| --- | --- |

```
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

**Se descarta** porque  viola OCP.

### Alternativa 2

Creamos una abstracción:

```csharp
public abstract class Producto
{
public string Nombre {get;protected set; };
}
```

Con:

```
Producto
 ├── Res
 ├── Lacteo
 ├── Piel
 └── Carne
```

### Decisión

crear `Producto` como abstracción común para los elementos que se van a vender .

Entonces la operación general trabaja con:

```
Producto producto
```

en lugar de:

```
Res res
```

### Consecuencia negativa

La abstracción `Producto` debe contener únicamente comportamiento verdaderamente común.

Si se agregan propiedades específicas de reses:

```
Peso Edad Vacunas
```

a `Producto`, se estaría contaminando la abstracción.

### Principios

- **OCP**
- **LSP**
- **DIP**

---

# ADR 3 — Separar registro de ventas

## ADR-003: Crear `RegistroVenta`

### Contexto

Actualmente `Hacienda` mantiene la lista de ventas y además realiza la venta.

Esto significa que `Hacienda` tiene dos responsabilidades:

```
1. Coordinar una venta
2. Administrar el historial de ventas
```

### Alternativa 1

Mantener dentro de `Hacienda` el registro de las ventas

```
List<Venta>
```

No afecta el comportamiento del programa, pero si deja muchas responsabilidades a la clase de Hacienda.  

### Alternativa 2

Crear un RegistroVenta, responsable de Registrar ventas ( o sea de guardarlas) , asi toda la resposibilidad de las ventas no queda en Hacienda 

### Decisión

elegimos la opción 2:

### Consecuencia negativa

Ahora existe una clase adicional y la venta requiere delegar el registro:

```
registroVentas.Registrar(venta);
```

Pero esto se acepta porque la responsabilidad queda correctamente encapsulada.

### Principio

**SRP**

---

# ADR 4 — Aplicación de vacunas

## ADR-004: Mover la regla de vacunación a `Res`

### Contexto

El codigo original tenía muchos  `if/else`. Especialmente de los que vimos en clase que condicionan por tipo . 

En `Hacienda.aplicar_vacuna()` 

```
if (resisTernero)
{
    ...
}elseif (resisNovillo)
{
    ...
}elseif (resisCebon)
{
    ...
}
```

Además:

```
if (vacunaisBacteriana)
{
    ...
}elseif (vacunaisViva)
{
    ...
}
```

### Alternativa 1

Mantener todos los `if/else` en `Hacienda`.

**Se descarta** porque cada nuevo tipo de res obliga a modificar `Hacienda`.

### Alternativa 2

Delegar la operación:

```
res.AplicarVacuna(vacuna);
```

y hacer que `Res` determine las reglas correspondientes.

### Decisión

La responsabilidad de determinar si una res puede recibir una vacuna pertenece a `Res`.

Entonces:

```
publicvoidAplicarVacuna(Vacunavacuna)
{// validar vacuna// verificar vencimiento// verificar duplicados// verificar límite// agregar vacuna
}
```

Mientras `Hacienda` coordina:

```
publicstringAplicarVacuna(...)
{Potreropotrero=buscar_potrero(idPotrero);Resres=potrero.buscar_res(nombre);res.AplicarVacuna(vacuna);L_vacunas.Remove(vacuna);

    ...
}
```

### Consecuencia negativa

`Res` conoce conceptos relacionados con vacunación.

Pero esto es aceptable porque **una vacuna aplicada forma parte del estado y comportamiento propio de una res**.

### Principios

- **SRP**
- **OCP**
- **LSP**
