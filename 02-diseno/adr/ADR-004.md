# ADR 4 — Aplicación de vacunas

## ADR-004: Mover la regla de vacunación a `Res`

### Contexto

El código original tenía muchos `if/else`. Especialmente de los que vimos en clase que condicionan por tipo.

En `Hacienda.aplicar_vacuna()`:

```csharp
if (res is Ternero)
{
    ...
}
else if (res is Novillo)
{
    ...
}
else if (res is Cebon)
{
    ...
}
```

Además:

```csharp
if (vacuna is Bacteriana)
{
    ...
}
else if (vacuna is Viva)
{
    ...
}
```

### Alternativa 1

Mantener todos los `if/else` en `Hacienda`.

**Se descarta** porque cada nuevo tipo de res obliga a modificar `Hacienda`.

### Alternativa 2

Delegar la operación:

```csharp
res.AplicarVacuna(vacuna);
```

y hacer que `Res` determine las reglas correspondientes.

### Decisión

La responsabilidad de determinar si una res puede recibir una vacuna pertenece a `Res`.

Entonces:

```csharp
public void AplicarVacuna(Vacuna vacuna)
{
    // validar vacuna
    // verificar vencimiento
    // verificar duplicados
    // verificar límite
    // agregar vacuna
}
```

Mientras `Hacienda` coordina:

```csharp
public string AplicarVacuna(...)
{
    Potrero potrero = buscar_potrero(idPotrero);
    Res res = potrero.buscar_res(nombre);

    res.AplicarVacuna(vacuna);
    L_vacunas.Remove(vacuna);

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
