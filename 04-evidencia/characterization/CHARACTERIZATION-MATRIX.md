# Matriz de caracterización OLD ↔ NEW

**Fecha:** 2026-08-10  
**Resultado:** 11 escenarios OLD, 11 equivalentes NEW, 11 MATCH, 0 MISMATCH finales.

Los ejecutables están en `03-src/phase4/Characterization/Old` y `New`. Esta ruta es infraestructura no productiva. OLD se ejecutó primero. La primera ejecución NEW detectó C07 y C10; ambos se restauraron al contrato OLD y se repitió la suite completa.

| ID | Nombre y propósito | Precondiciones / entradas / operaciones ordenadas | Observable OLD y NEW | Resultado | Evidencia |
|---|---|---|---|---|---|
| C01 | Alta de potrero | Hacienda vacía; crear `P1`, tipo ternero | Mensaje exacto; `potreros=1` | MATCH | `old-output.txt`, `new-output.txt` |
| C02 | Duplicado case-insensitive | C01; crear `p1` | Misma excepción envuelta; lista permanece en 1 | MATCH | mismas rutas |
| C03 | Inserción de res | C01; añadir Lola, 5 meses, 100 kg | Mensaje y evento de bajo peso; una `Ternero` | MATCH | mismas rutas |
| C04 | Edad incompatible | C03; añadir 13 meses a potrero ternero | Misma excepción; continúa una res | MATCH | mismas rutas |
| C05 | Búsqueda parcial | C01; buscar `p` | Retorna `P1`; estado sin cambio | MATCH | mismas rutas |
| C06 | Alimentación normal | C03; alimentar una unidad | Peso 101, mensaje y evento exactos | MATCH | mismas rutas |
| C07 | Alimentación de cero | C06; alimentar 0 | Operación válida, peso 101 y mismo mensaje/evento | MATCH | mismas rutas |
| C08 | Alta vacuna bacteriana | Fechas 2026-08-10/2030-08-10; lote L1 | Mensaje exacto; inventario=1 | MATCH | mismas rutas |
| C09 | Lote de vacuna duplicado | C08; lote `l1` | Misma excepción; inventario=1 | MATCH | mismas rutas |
| C10 | Aplicación de vacuna | C03+C08; aplicar L1 a Lola | Mensaje incluyendo evento de esquema; inventario=0, aplicadas=1 | MATCH | mismas rutas |
| C11 | Venta legacy | C03; vender Lola por 1200 | Mensaje exacto; reses=0, ventas=1, monto=1200 | MATCH | mismas rutas |

## Mismatches detectados y resueltos

| Caso | Primera salida NEW | Autoridad OLD | Resolución estrecha |
|---|---|---|---|
| C07 | Excepción para cantidad 0 | OLD acepta 0 sin cambiar peso | `Res.Alimentar` volvió a permitir cero. |
| C10 | Omitía el mensaje del evento | OLD devuelve el mensaje del esquema | `Hacienda.aplicar_vacuna` captura y concatena el evento. |

## Reproducción

```bash
scripts/phase4-safe-dotnet.sh run 03-src/phase4/Characterization/Old/Characterization.Old.csproj
scripts/phase4-safe-dotnet.sh run 03-src/phase4/Characterization/New/Characterization.New.csproj
```

La comparación es línea por línea por ID. Los archivos retenidos contienen la salida canónica del runner, sin el ruido de restore/build del wrapper.
