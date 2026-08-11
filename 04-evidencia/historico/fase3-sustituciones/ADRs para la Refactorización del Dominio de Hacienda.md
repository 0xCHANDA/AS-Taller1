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

---

## 1. SRP

### Problema 1: `Hacienda` hacía demasiadas cosas

`Hacienda` inicialmente se encargaba de:

- Crear y buscar potreros.
- Agregar y eliminar reses.
- Alimentar reses.
- Crear vacunas.
- Aplicar vacunas.
- Administrar el inventario de vacunas.
- Registrar ventas.

**Cambio:** se trasladaron responsabilidades a las clases que corresponden al dominio:

- `Potrero` → administra sus reses.
- `Res` → administra su estado y vacunas aplicadas.
- `Venta` → representa una venta.
- `RegistroVentas` → administra el historial de ventas.
- `InventarioLacteos` → administra lácteos.
- `InventarioPieles` → administra pieles.

**Ganancia:** `Hacienda` deja de ser una clase que conoce y controla toda la lógica interna del sistema.

---

### Problema 2: `Hacienda` tenía la lógica de aplicación de vacunas

Antes `Hacienda.aplicar_vacuna()` hacía prácticamente todo:

- Buscar la res.
- Validar la vacuna.
- Comprobar si ya estaba aplicada.
- Contar vacunas.
- Determinar límites según el tipo de res.
- Validar vencimiento.
- Agregar la vacuna.

**Cambio:** la responsabilidad propia de la res pasó a `Res.aplicar_vacuna()`.

Así:

```
Hacienda
   ↓ busca
Potrero
   ↓ busca
Res
   ↓ aplica
Vacuna
```

**Ganancia:** la lógica relacionada con el estado de una res queda dentro de `Res` y no dentro de `Hacienda`.

---

### Problema 3: `Venta` estaba acoplada a `Res`

Antes:

```
privateResres;
```

Eso hacía que `Venta` solo pudiera representar ventas de reses.

**Cambio:** se generalizó el concepto de producto vendido.

**Ganancia:** `Venta` puede representar ventas de diferentes productos y no tiene que cambiar cuando aparezcan nuevos productos.

---

## 2. OCP — Open/Closed Principle

### Problema 1: `vender_res()`

Antes existía:

```
vender_res(...)
```

Si aparecían:

```
vender_lacteo()
vender_piel()
vender_carne()
```

habría que modificar `Hacienda` cada vez.

**Cambio:** se creó una abstracción común para los productos y los inventarios.

**Ganancia:** agregar un nuevo producto no obliga a modificar la lógica general de venta.

---

### Problema 2: `aplicar_vacuna()` tenía muchos `if/else`

Antes:

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

Cada nuevo tipo de res obligaría a modificar este método.

**Cambio:** las reglas relacionadas con cada tipo de res se trasladaron a la propia jerarquía de `Res`.

**Ganancia:** si aparece otro tipo de res, se puede extender la jerarquía sin modificar constantemente la lógica existente.

---

### Problema 3: identificación del tipo de vacuna

Antes se hacía:

```
if (vacunaisBacteriana)
{
    ...
}elseif (vacunaisViva)
{
    ...
}
```

**Cambio:** se introdujo `TipoVacuna` y la lógica de límites se relacionó con esa abstracción.

**Ganancia:** la lógica de `Res` deja de depender constantemente de comprobaciones concretas de tipos.

---

## 3. LSP — Liskov Substitution Principle

Aquí no basta con decir "`Ternero` hereda de `Res`". Hay que demostrar que **puede sustituirla**.

### Problema 1: jerarquía `Res → Ternero/Cebon/Novillo`

Se verificó que:

```
Resres=newTernero(...);
```

sea válido y que las operaciones definidas para `Res` sigan funcionando.

Un `Ternero` continúa siendo una `Res`.

**Ganancia:** `Potrero` puede trabajar con:

```
List<Res>
```

sin tener que conocer cada subtipo.

---

### Problema 2: restricciones de edad

Las subclases tienen reglas diferentes de edad.

Por ejemplo, `Novillo` puede restringir determinados valores.

Pero esa restricción **no debe romper el contrato de `Res`**.

La subclase puede agregar reglas propias del dominio, siempre que los valores aceptados por el contrato de `Res` sigan siendo válidos.

**Ganancia:** las especializaciones representan diferencias reales del dominio sin hacer que el polimorfismo deje de funcionar.

---

### Problema 3: evitar herencias artificiales

Se evaluó crear:

```
Animal
  ↓
Res
  ↓
Ternero
Cebon
Novillo
```

pero se rechazó.

**Razón:** actualmente el sistema solo maneja reses. Crear `Animal` no aportaría un comportamiento común necesario y produciría una abstracción innecesaria.

Esto también evita crear una jerarquía que después no pueda justificarse mediante sustitución real.

---

## 4. ISP — Interface Segregation Principle

### Problema 1: necesidad de un contrato común de inventario

`Potrero`, `InventarioLacteos` e `InventarioPieles` necesitan operaciones de inventario.

Se creó:

```
publicinterfaceIInventario<T>whereT :Producto
{voidagregar(Tproducto);Tretirar(Tproducto);boolcontiene(Tproducto);
}
```

**Ganancia:** cada inventario tiene únicamente las operaciones que realmente necesita para cumplir su responsabilidad.

---

### Problema 2: evitar una interfaz gigante

Una alternativa habría sido:

```
interfaceIHacienda
{agregarRes();retirarRes();venderRes();agregarLacteo();retirarLacteo();venderLacteo();agregarPiel();retirarPiel();venderPiel();aplicarVacuna();alimentarRes();
    ...
}
```

Esto obligaría a las clases a depender de operaciones que no les corresponden.

**Cambio:** se utilizan interfaces más específicas, como `IInventario<T>`.

**Ganancia:** las clases dependen de contratos pequeños y relacionados con su responsabilidad.

---

### Problema 3: `Venta` no debe conocer las operaciones del inventario

`Venta` solamente representa información de una venta.

No necesita saber:

```
cómo agregar productos
cómo retirar productos
cómo buscar productos
```

Eso corresponde al inventario.

**Ganancia:** evitamos colocar operaciones innecesarias en `Venta` y mantenemos cada contrato enfocado.

---

## 5. DIP — Dependency Inversion Principle

### Problema 1: `Hacienda` dependía directamente de `Potrero`

Antes:

```
Hacienda → Potrero
```

Esto hacía que la lógica de alto nivel estuviera directamente ligada a una implementación concreta.

**Cambio:**

```
Hacienda → IInventario
             ↑
       ┌─────┼──────┐
       │     │      │
   Potrero  Lácteos  Pieles
```

**Ganancia:** Hacienda puede trabajar con diferentes inventarios sin conocer sus implementaciones internas.

---

### Problema 2: la venta dependía directamente de `Res`

Antes:

```
Venta → Res
```

Eso impedía representar fácilmente otros productos.

**Cambio:** se introdujo `Producto` como abstracción.

```
Producto
   ↑
 ┌─┴───────┐
Res      Lacteo
           │
          Piel
```

**Ganancia:** la lógica de venta deja de depender exclusivamente de `Res`.

---

### Problema 3: los eventos y lógica específica no deben estar concentrados en `Hacienda`

Antes `Hacienda` disparaba y procesaba directamente varios eventos relacionados con:

- peso mínimo;
- peso de venta;
- vacunación;
- vencimiento.

**Cambio:** parte de la lógica se trasladó a las entidades responsables, mientras `Hacienda` coordina las operaciones.

**Ganancia:** se reduce el acoplamiento entre `Hacienda` y los detalles internos de cada operación.