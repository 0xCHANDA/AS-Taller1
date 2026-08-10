# Matriz de caracterización OLD ↔ NEW

**Fecha:** 2026-08-10  
**Resultado:** 20 escenarios (11 originales + 9 nuevos), 18 MATCH, 2 DELIBERATE_STRUCTURAL, 0 BEHAVIORAL_MISMATCH.

Los ejecutables están en `03-src/phase4/Characterization/Old` y `New`. Esta ruta es infraestructura no productiva. OLD se ejecutó primero. La primera ejecución NEW detectó C07 y C10; ambos se restauraron al contrato OLD y se repitió la suite completa. En una segunda iteración C12, C14, C15 y C17 se restauraron al contrato OLD; la suite completa se repitió nuevamente y todos están MATCH.

## Escenarios C01–C11 (línea base)

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

## Escenarios C12–C17 (extendidos: vacuna vencida, duplicado, límites, combinados)

| ID | Nombre y propósito | Precondiciones / entradas / operaciones ordenadas | Observable OLD | Observable NEW | Resultado | Clasificación |
|---|---|---|---|---|---|---|
| C12 | Vacuna vencida | Potrero P1 ternero + Lola (5m, 100kg); crear bacteriana Vencida con fechas 2020-01-01 / 2019-06-01; aplicar a Lola | `Exception:…[Evento] La vacuna 'Vencida' del lote 'C12-VENC' está vencida desde 1/1/2020` | `Exception:…[Evento] La vacuna 'Vencida' del lote 'C12-VENC' está vencida desde 1/1/2020` | MATCH | Mensaje, tipo de excepción y post-estado idénticos (vacunas=1;aplicadas=0). |
| C13 | Aplicación duplicada | P1+Lola; crear dos bacterianas "Duplicada" con lotes C13-DUP-A y C13-DUP-B; aplicar primera OK; intentar segunda | `Exception:…La vacuna 'Duplicada' ya fue aplicada a la res 'Lola'.` | `Exception:…La vacuna 'Duplicada' ya fue aplicada a la res 'Lola'.` | MATCH | Mensaje, tipo de excepción y post-estado idénticos (vacunas=1;aplicadas=1). |
| C14 | Límite bacteriano ternero (max=3) | P1+Lola; crear y aplicar 3 bacterianas (C14-B1/B2/B3); crear cuarta (C14-B4); intentar aplicar | `Exception:…No se puede aplicar más vacunas bacterianas a la res 'Lola'. Ya tiene las 3 permitidas.` | `Exception:…No se puede aplicar más vacunas bacterianas a la res 'Lola'. Ya tiene las 3 permitidas.` | MATCH | Mensaje, tipo de excepción y post-estado idénticos (vacunas=1;aplicadas=3). |
| C15 | Límite viva ternero (max=1) | P1+Lola; crear y aplicar viva C15-V1 (Atenuacion10); crear segunda viva C15-V2; intentar aplicar | `Exception:…No se puede aplicar más vacunas vivas a la res 'Lola'. Ya tiene las 1 permitidas.` | `Exception:…No se puede aplicar más vacunas vivas a la res 'Lola'. Ya tiene las 1 permitidas.` | MATCH | Mensaje, tipo de excepción y post-estado idénticos (vacunas=1;aplicadas=1). |
| C16 | Límites independientes: bacteriano lleno + viva exitosa | P1+Lola; aplicar 3 bacterianas (C16-B1/B2/B3) hasta agotar límite; crear viva C16-V1 y aplicar | `OK\|…La res 'Lola' ha completado su esquema de vacunación.\|vacunas=0;aplicadas=4` | `OK\|…La res 'Lola' ha completado su esquema de vacunación.\|vacunas=0;aplicadas=4` | MATCH | Ambos permiten viva tras límite bacteriano alcanzado; contadores independientes preservados. |
| C17 | Combinado: límite bacteriano + vacuna vencida | P1+Lola; aplicar 3 bacterianas (C17-B1/B2/B3); crear bacteriana vencida C17-E (2020-01-01); intentar aplicar | `Exception:…No se puede aplicar más vacunas bacterianas a la res 'Lola'. Ya tiene las 3 permitidas.` | `Exception:…No se puede aplicar más vacunas bacterianas a la res 'Lola'. Ya tiene las 3 permitidas.` | MATCH | Orden de validación restaurado al contrato OLD: límite se evalúa antes que vencimiento. Post-estado idéntico (vacunas=1;aplicadas=3). |

## Escenarios C18–C20 (reflexión de API pública)

| ID | Aspecto | Observable OLD | Observable NEW | Resultado | Clasificación |
|---|---|---|---|---|---|
| C18 | Semántica de `L_ventas` | `tipo=List`1;Count=1;Monto[0]=1200` | `tipo=List`1;Count=1;Monto[0]=1200` | MATCH | Comportamiento observable idéntico. Internamente OLD usa `List<Venta>` directo; NEW computa `registroVentas.Ventas.ToList()`. La copia es transparente para consumidores que solo leen Count/[]. |
| C19 | Superficie de sobrecargas `alimentar_res` | `overloads=2;defaultParam=False;dosParams=True;tresParams=True` | `overloads=1;defaultParam=True;dosParams=False;tresParams=True` | DELIBERATE_STRUCTURAL | OLD define 2 métodos separados `(id,nombre)` y `(id,nombre,cantidad)`. NEW consolida en 1 método `(id,nombre,cantidad=1)` con parámetro por defecto. La semántica de invocación es equivalente: llamar con 2 args sigue funcionando. |
| C20 | Existencia de `IValidarInformacion` | `EXISTS` | `ABSENT` | DELIBERATE_STRUCTURAL | OLD definía `IValidarInformacion` monolítico con 4 métodos. NEW lo reemplazó por 4 interfaces granulares `IValidadorRes`, `IValidadorPotrero`, `IValidadorVacuna`, `IValidadorVenta` (ISP). La interfaz monolítica no existe en NEW. |

## Clasificación de divergencias

| Tipo | Casos | Significado |
|---|---|---|
| MATCH | C01–C18 | Comportamiento observable idéntico |
| DELIBERATE_STRUCTURAL | C19, C20 | Diferencias en superficie de API pública (consolidación de sobrecargas, granularidad de interfaces ISP) documentadas como decisiones arquitectónicas |

## Mismatches detectados y resueltos (fases anteriores)

| Caso | Primera salida NEW | Autoridad OLD | Resolución estrecha |
|---|---|---|---|
| C07 | Excepción para cantidad 0 | OLD acepta 0 sin cambiar peso | `Res.Alimentar` volvió a permitir cero. |
| C10 | Omitía el mensaje del evento | OLD devuelve el mensaje del esquema | `Hacienda.aplicar_vacuna` captura y concatena el evento. |
| C12 | Mensaje simplificado sin lote ni fecha | OLD incluye lote y fecha vía publisher de evento | `Hacienda.aplicar_vacuna` restaurado al contrato OLD: el publisher de vencimiento emite el mensaje completo. |
| C14 | Mensaje genérico "de este tipo" | OLD menciona tipo concreto (bacterianas) y cantidad | `PuedeAplicarseA` restaurado al mensaje OLD con tipo concreto y límite numérico. |
| C15 | Mensaje genérico "de este tipo" | OLD menciona tipo concreto (vivas) y cantidad | `PuedeAplicarseA` restaurado al mensaje OLD con tipo concreto y límite numérico. |
| C17 | Orden invertido (vencimiento antes que límite) | OLD evalúa límite antes que vencimiento | Orden de validación en `Hacienda.aplicar_vacuna` restaurado al contrato OLD. |

## Resolución de C17 (cerrado)

En la iteración anterior, NEW evaluaba `EstaVencida()` antes de `PuedeAplicarseA()` en `Res.aplicar_vacuna`, produciendo un BEHAVIORAL_MISMATCH. Se restauró la validación de límites antes del vencimiento (contrato OLD de `Hacienda.aplicar_vacuna`). La suite completa se repitió y C17 ahora es MATCH. No hay BEHAVIORAL_MISMATCH pendientes.

## Reproducción

```bash
scripts/phase4-safe-dotnet.sh run 03-src/phase4/Characterization/Old/Characterization.Old.csproj
scripts/phase4-safe-dotnet.sh run 03-src/phase4/Characterization/New/Characterization.New.csproj
```

La comparación es línea por línea por ID. Los archivos retenidos contienen la salida canónica del runner, sin el ruido de restore/build del wrapper.
