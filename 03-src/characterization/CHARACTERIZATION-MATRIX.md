# Caracterización OLD ↔ NEW

Los dos ejecutables reciben las mismas entradas y recorren las mismas operaciones. Resultado vigente: **23 escenarios, 22 MATCH, una diferencia estructural deliberada y cero diferencias de comportamiento**.

| ID | Escenario | Observable comparado | Resultado |
|---|---|---|---|
| C01 | Crear potrero | Mensaje y cantidad | MATCH |
| C02 | Potrero duplicado sin distinguir mayúsculas | Excepción y estado | MATCH |
| C03 | Añadir res | Mensaje, evento, cantidad y tipo | MATCH |
| C04 | Edad incompatible con potrero | Excepción y estado | MATCH |
| C05 | Búsqueda parcial | Potrero encontrado | MATCH |
| C06 | Alimentar una unidad | Mensaje, evento y peso | MATCH |
| C07 | Alimentar cero | Operación aceptada y peso sin cambio | MATCH |
| C08 | Crear vacuna bacteriana | Mensaje e inventario | MATCH |
| C09 | Lote duplicado | Excepción e inventario | MATCH |
| C10 | Aplicar vacuna válida | Mensaje, evento y colecciones | MATCH |
| C11 | Venta legacy | Mensaje, retiro, venta y monto | MATCH |
| C12 | Vacuna vencida | Excepción, lote, fecha y estado | MATCH |
| C13 | Vacuna duplicada | Excepción y estado | MATCH |
| C14 | Límite bacteriano | Excepción y cantidades | MATCH |
| C15 | Límite de vacuna viva | Excepción y cantidades | MATCH |
| C16 | Límites bacteriano y vivo independientes | Aplicación exitosa y cantidades | MATCH |
| C17 | Orden entre límite y vencimiento | Excepción de límite antes de vencimiento | MATCH |
| C18 | Lectura de `L_ventas` | Tipo, cantidad y monto | MATCH |
| C19 | Sobrecargas de `alimentar_res` | Firmas públicas | MATCH |
| C20 | Existencia de `IValidarInformacion` | OLD existe; NEW no existe | DIFERENCIA ESTRUCTURAL |
| C21 | Mutación de `L_ventas` | Misma lista viva; la adición queda visible | MATCH |
| C22 | Setter de `Edad` | Setter público, asignación válida y rechazo inválido | MATCH |
| C23 | Setter de `L_vacunas_aplicadas` | Setter público e identidad de la lista | MATCH |

C20 es deliberado: NEW reemplaza la interfaz monolítica por `IValidadorRes`, `IValidadorPotrero`, `IValidadorVacuna` e `IValidadorVenta`. Los demás casos conservan mensajes, reglas, excepciones, orden de validación, estado y API pública observada.

## Ejecutar

```bash
dotnet run --project 03-src/phase4/Characterization/Old/Characterization.Old.csproj
dotnet run --project 03-src/phase4/Characterization/New/Characterization.New.csproj
```

Las salidas completas están en `OLD-OUTPUT.md` y `NEW-OUTPUT.md`.
