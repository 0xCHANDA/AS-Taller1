# Salida de caracterización - OLD

```text
C01|OK|El potrero P1 se a añadido a la hacienda. |potreros=1
C02|EXCEPTION|Exception:Error inesperado en el metodo crear_potrero: Ya existe un potrero con el nombre 'p1'.|potreros=1
C03|OK|La res Lola ha sido añadida al potrero P1 con exito.\n[Evento] La res 'Lola' tiene un peso 100, está en desnutrición.|reses=1;tipo=Ternero
C04|EXCEPTION|Exception:Error inesperado en el método anadir_res_potrero: Error inesperado en el metodo anadir_res: La res no puede ser añadida al potrero P1 porque su edad no corresponde al tipo de potrero|reses=1
C05|OK|P1|potreros=1
C06|OK|La res 'Lola' ha sido alimentada, ahora pesa 101 kg.\n[Evento] La res 'Lola' tiene un peso 101, está en desnutrición.|peso=101
C07|OK|La res 'Lola' ha sido alimentada, ahora pesa 101 kg.\n[Evento] La res 'Lola' tiene un peso 101, está en desnutrición.|peso=101
C08|OK|Vacuna bacteriana 'Bovina' del lote 'L1' agregada al inventario con éxito. Período de aplicación: 4 semanas.|vacunas=1
C09|EXCEPTION|Exception:Error inesperado en el método crear_vacuna (bacteriana): Ya existe una vacuna con el lote 'l1' en el inventario|vacunas=1
C10|OK|Vacuna aplicada correctamente a la res Lola. [Evento] La res 'Lola' aún no ha completado su esquema de vacunación. Bacterianas: 1, Vivas: 0|inventario=0;aplicadas=1
C11|OK|Venta de la res Lola realizada con exito|reses=0;ventas=1;monto=1200
C12|EXCEPTION|Exception:Error inesperado en el metodo aplicar_vacuna: [Evento] La vacuna 'Vencida' del lote 'C12-VENC' está vencida desde 01/01/2020|vacunas=1;aplicadas=0
C13|EXCEPTION|Exception:Error inesperado en el metodo aplicar_vacuna: La vacuna 'Duplicada' ya fue aplicada a la res 'Lola'.|vacunas=1;aplicadas=1
C14|EXCEPTION|Exception:Error inesperado en el metodo aplicar_vacuna: No se puede aplicar más vacunas bacterianas a la res 'Lola'. Ya tiene las 3 permitidas.|vacunas=1;aplicadas=3
C15|EXCEPTION|Exception:Error inesperado en el metodo aplicar_vacuna: No se puede aplicar más vacunas vivas a la res 'Lola'. Ya tiene las 1 permitidas.|vacunas=1;aplicadas=1
C16|OK|Vacuna aplicada correctamente a la res Lola. [Evento] La res 'Lola' ha completado su esquema de vacunación.|vacunas=0;aplicadas=4
C17|EXCEPTION|Exception:Error inesperado en el metodo aplicar_vacuna: No se puede aplicar más vacunas bacterianas a la res 'Lola'. Ya tiene las 3 permitidas.|vacunas=1;aplicadas=3
C18|API|L_ventas tipo=List`1;Count=1;Monto[0]=1200|-
C19|API|alimentar_res overloads=2;defaultParam=False;dosParams=True;tresParams=True|-
C20|API|IValidarInformacion=EXISTS|-
```
