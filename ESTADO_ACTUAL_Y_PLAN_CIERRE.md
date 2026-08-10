# Estado Actual y Plan de Cierre — Hacienda

> **Estados:** VERIFICADO = evidencia directa contrastada; PROBABLE PERO NO VERIFICADO = inferencia razonable; PARCIAL = evidencia insuficiente; AUSENTE = ubicación inspeccionada y vacía; INCORRECTO/INCONSISTENTE = contradice source/enunciado; BLOQUEADO = falta dato/entorno; NO APLICA = fuera del requisito.

## 0. Metadatos de auditoría

| Campo | Valor |
|---|---|
| Corte | 2026-08-09 20:37:57 -05:00 |
| Git | branch `main`; commit `b20615fd33f9cf3bea3c95c8c830a4a1abe7d72a` |
| Working tree | Ya estaba sucio. Antes de crear este reporte: 243 entradas (`199 D`, `42 M`, `2 ??`): eliminación preexistente de `01-diagnostico/graphify-out/`, generados `bin/obj` y dos no rastreados. Builds permitidos actualizaron generados; no se editó source. |
| SDK | .NET SDK 10.0.110; runtimes 10.0.10; sin runtime 8, `global.json` ni targeting pack .NET Framework 4.7.2. |
| Fuente de verdad | `01-diagnostico/Enunciado y rubrica.docx`, leído completo. |
| Restricción | Auditoría read-only; este reporte es el único artefacto creado. Sin refactor, paquetes, commit, regeneración UML ni limpieza destructiva. |

### Baseline ejecutado

| CWD/sistema | Comando | Resultado |
|---|---|---|
| raíz | `dotnet --info` | Exit 0; SDK 10.0.110. |
| MVC OLD | restore + `dotnet build p_mvcHacienda.sln --no-restore` | Exit 0; 5 warnings de nulabilidad, 0 errores. |
| MVC NEW | mismos comandos | Exit 0; mismos 5 warnings. **Compila contra DLL OLD**, no contra source NEW. |
| Bib OLD/NEW | restore + build de `Bib_Hacienda.sln` | Exit 1, `MSB3644`: faltan reference assemblies net472. |
| MVC OLD/NEW | `dotnet test ... --no-build` | Exit 0 sin salida: **cero tests**, no “tests verdes”. |
| MVC OLD/NEW | `dotnet run --no-build` | Falla: runtime 8 ausente. |
| MVC OLD/NEW | `DOTNET_ROLL_FORWARD=Major timeout 8s dotnet run ...` | Host inicia y escucha; prueba solo arranque con DLL legacy. |
| raíz | SHA-256 de las 4 `Bib_Hacienda.dll` | Todas idénticas: `1ae86a970170b8af099f620a8ec283bcaee61837a9b826f17213f643e611967a`. |

**Limitaciones:** roster/grupo no declarados; video inexistente; net472 no compilable en este host; DOCX se cita por sección, no por línea estable; graphify se usó solo como mapa auxiliar.

## 1. Resumen ejecutivo

El proyecto está **avanzado en diagnóstico AS-IS y Fase 2**, pero **no está en estado de entrega**. La línea base de SC-1/2/3 es el artefacto más sólido: define alcance mínimo, conteo, listas nominales y riesgos. El AS-IS tiene exportaciones detalladas, tres hallazgos propios, una refutación IA válida, mapa y dolores priorizados.

El bloqueo central es que “NEW” ejecutable **es OLD**: ambos MVC referencian por `HintPath` una DLL idéntica. El source NEW cambia `Hacienda`, `Venta`, `Res` y contratos, pero sus clientes siguen usando `vender_res`, `L_ventas`, `Venta.Res/Potrero` y el constructor legacy. `Res : Producto` tampoco invoca el único constructor base. El build verde del MVC no demuestra compilabilidad ni conducta del rediseño.

El TO-BE tiene cuatro PNG con colores/leyenda, pero sin fuente editable y describe otra solución (`ItemVenta`, AppServices, comandos, puertos, serializadores) que no existe. El source implementa un slice diferente (`Producto/IInventario<T>`) e incompleto. Hay **0 ADR, 0 tests, 0 métricas, 0 bitácora y 0 video**. El README raíz documenta OpenCode, no Hacienda.

**Camino crítico:** declarar equipo/grupo/SC; recuperar build real NEW; crear 8 caracterizaciones OLD; restaurar equivalencia NEW; completar una sola ruta SC-1 end-to-end; reducir/alinear TO-BE; cerrar ADR, métrica, bitácora, README y video. **Nota actual estimada: 1.44/5.00.**

## 2. Estado general

| Fase | Estado | % evidencia | Evidencia fuerte | Problema principal | Siguiente paso |
|---|---|---:|---|---|---|
| 0 Lectura fría | PARCIAL/BLOQUEADO | 65% | 2 hojas completas | Roster y contraste ausentes | Declarar integrantes |
| 1 AS-IS | PARCIAL avanzado | 78% | PDF, 5 PNG, inventario, mapa, dolores | Sin editable; `Program` omitido; 2 hallazgos sin línea | Cerrar trazabilidad |
| 2 SC | VERIFICADO con supuestos | 90% | DOCX forense y listas | Alcances condicionados | Congelar baseline |
| 3 TO-BE | INCONSISTENTE | 28% | 4 PNG con leyenda | Sin editable/ADR; no corresponde a código | Reducir al vertical real |
| 4 Implementación/evidencia | INCORRECTO/AUSENTE | 5% | Slice SC-1 parcial | NEW ejecuta OLD; carpetas vacías | Build + tests + SC-1 |
| 5 Video | AUSENTE | 5% | Material diagnóstico parcial | Sin enlace/roles/demo válida | Cerrar evidencia y grabar |
| Organización | PARCIAL | 20% | Carpetas existen | README equivocado | README final |

Los porcentajes miden cumplimiento, no cantidad de archivos.

## 3. Matriz completa Enunciado → Evidencia

| ID | Requisito | Evidencia encontrada | Estado | Calidad/riesgo | Acción |
|---|---|---|---|---|---|
| F0-01 | Roster real | Solo nombres Santiago y Sebastián en hojas; README no declara equipo | BLOQUEADO | Dos nombres no prueban tamaño | Declarar roster |
| F0-02 | Una hoja por integrante | 2 hojas | PROBABLE | Cumple solo si equipo=2 | Confirmar |
| F0-03 | Qué hace/entidades | Presente en ambas | VERIFICADO | Específico | — |
| F0-04 | Tres lugares + razón | Exactamente 3 por hoja | VERIFICADO | Concreto | — |
| F0-05 | Pregunta al autor | Presente | VERIFICADO | Defendible | — |
| F0-06 | Sin modificar | Sin hash/tag; DOCX Sebastián no rastreado | NO VERIFICADO | Riesgo temporal | Registrar hashes sin editar |
| F0-07 | Contraste en video | No existe | AUSENTE | Debilita C6/C7 | Crear tabla/guion |
| F1-01 | AS-IS editable | Solo PDF/PNG | AUSENTE | Incumple entregable | Recuperar fuente |
| F1-02 | Exportación | PDF + `diagramas/1..4.png` | VERIFICADO | Detallada | Conservar |
| F1-03 | Notación extendida | Visibilidad/tipos/firmas/relaciones | PARCIAL | Usa `N`; alta densidad | Normalizar |
| F1-04 | Todas las clases | Dominio+MVC+persistencia; no `Program` | PARCIAL | Root omitido | Añadir/justificar |
| F1-05 | Fidelidad | Alta en muestras auditadas | VERIFICADO | No prueba validez SOLID | Checklist final |
| F1-06 | Hallazgos con ID/síntoma/principio | H-01..H-03 | VERIFICADO | Solo 3, pero mínimo propio cumple | Conservar |
| F1-07 | Archivo/clase/línea | H-01 con línea; H-02/H-03 sin línea | PARCIAL | Viola literal rúbrica | Añadir rangos |
| F1-08 | Impacto/severidad/origen | Presente | PARCIAL | Impacto genérico | Conectar a SC |
| F1-09 | 3 propios | H-01..H-03 “Propio” | VERIFICADO | Técnicamente válidos | Preparar defensa |
| F1-10 | Refutación IA | Rechazo de interfaz por entidad | VERIFICADO | Argumento DIP correcto | Llevar a bitácora |
| F1-11 | Mapa dependencias | 5 PNG + `dependencias.png` | VERIFICADO | Alto/bajo/dirección | Añadir DLL/root |
| F1-12 | 3 dolores priorizados | Persistencia, Hacienda, validación | VERIFICADO | Criterio y orden explícitos | Añadir líneas |
| F2-01 | SC-1 clases/archivos/riesgo | 4 clases/6 archivos + listas | VERIFICADO CON SUPUESTOS | Alcance mínimo explícito | Congelar |
| F2-02 | SC-2 | 4/5 + listas | VERIFICADO CON SUPUESTOS | Chip opcional, sin IoT | Congelar |
| F2-03 | SC-3 | 5/8 + 1 clase/2 archivos nuevos | VERIFICADO CON SUPUESTOS | Clínica mínima | Congelar |
| F2-04 | Aritmética/trazabilidad | Listas igualan cifras; líneas de hubs vigentes | VERIFICADO | Commit/rutas históricas | No cambiar OLD |
| F3-01 | TO-BE editable | 4 PNG únicamente | AUSENTE | No reproducible | Crear fuente |
| F3-02 | Export/colores/leyenda | Presente en 4 hojas | VERIFICADO | Claro pero denso | Mantener |
| F3-03 | Conecta dolores | Hacienda/persistencia/controllers intervenidos | PARCIAL | Sin IDs/ADR | Trazar |
| F3-04 | Cubre 3 SC | Solo SC-1 clara | PARCIAL | SC-2/3 ausentes | Explicar |
| F3-05 | SRP | AppServices/notas | PARCIAL | No implementados | Reducir a frontera real |
| F3-06 | OCP | Venta extensible dibujada | PARCIAL | UML y C# usan modelos distintos | Elegir uno |
| F3-07 | LSP formal | Solo “se verificará” | AUSENTE | Código contradice | Matriz+tests/deuda |
| F3-08 | ISP por clientes | Interfaces pequeñas dibujadas | PARCIAL | Ausentes; `Potrero.agregar` no-op | Basar en clientes |
| F3-09 | DIP+root | Conceptualmente dibujado | PARCIAL | Nombres inconsistentes y sin código | Unificar/materializar mínimo |
| F3-10 | 5 ADR | Directorio vacío | AUSENTE | C3 crítico | Crear 5 válidos |
| F4-01 | NEW compila desde source | MVC compila DLL OLD; Bib bloqueada/rota | INCORRECTO | C4 puede ser 0 | Build real |
| F4-02 | NEW ejecuta | Host arranca OLD DLL | INCORRECTO | Evidencia engañosa | Ejecutar NEW real |
| F4-03 | UML↔código 1:1 | Divergencia masiva | INCORRECTO | Tope C2/C4<=3 | Alinear |
| F4-04 | Programa principal/demo | `Program` solo host/carga | PARCIAL | Sin recorrido reproducible | Runner/guion |
| F4-05 | 8 caracterizaciones | Carpeta vacía; 0 tests | AUSENTE | Sin preservación | Runners aislados |
| F4-06 | SC elegida/justificada | SC-1 inferida; plan antiguo recomienda SC-3 | INCONSISTENTE | Decisión no oficial | Declarar SC-1 |
| F4-07 | SC funcional | Slice dominio incompleto | AUSENTE | Sin MVC/TXT/demo | Completar vertical |
| F4-08 | Métrica old/new | Carpeta vacía | AUSENTE | C5 casi cero | Diff exclusivo |
| F4-09 | Otras 2 SC en TO-BE | Solo baseline AS-IS | AUSENTE | Impide excelente | Añadir análisis |
| F4-10 | Bitácora IA | Carpeta vacía | AUSENTE | Penalización C6=0 | Crear |
| F5-01 | Enlace/duración | Sin video | AUSENTE | C7=0 | Grabar <=20 |
| F5-02 | Todos/roles | No roster/roles | BLOQUEADO | -1.5 individual posible | Asignar |
| F5-03 | Ejecución en vivo | Demo actual sería OLD | INCORRECTO | Desacredita defensa | Corregir antes |
| ORG-01 | Carpetas | Existen 00..04 | VERIFICADO | Contenido vacío parcial | — |
| ORG-02 | README equipo/roles | README del workbench | AUSENTE | No sirve | README final |
| ORG-03 | Ejecución reproducible | README interno dice JSON/`Archivos`, pero código usa TXT/`Datos` | INCORRECTO | Instrucción engañosa | Comandos verificados |
| ORG-04 | SC/video en README | Ausentes | AUSENTE | C5/C7 | Añadir |

## 4. Auditoría Fase 0

Se identifican Santiago Hernández (`Analisis-SantiagoHM.md:2`) y Sebastián Quijano (DOCX y metadata). Ambas hojas cubren todos los puntos. **No se concluye que el equipo tenga dos miembros**: si son dos, cumple; si hay más, faltan hojas. No existe contraste inicial-final ni prueba de inmutabilidad. Hacer ese contraste sin editar las hojas originales.

## 5. Auditoría AS-IS

Las hojas 1–4 cubren dominio, validaciones/eventos/aspectos, MVC y persistencia. Son mayormente fieles: `Hacienda.vender_res` (`Hacienda.cs:143-168`), validación (`Validacion.cs:11-17`) y mezcla File/HTTP/Castle (`PersistenciaService.cs:12-56`) aparecen. Falta `Program.cs:29-87`, composition root real, y fuente editable.

| Hallazgo inventario | Veredicto | Evidencia | Debilidad |
|---|---|---|---|
| H-01 ISP validadores | Confirmado | `Validacion.cs:11-17`; `ValidarRes.cs:12-33` | Bien trazado |
| H-02 SRP Hacienda | Confirmado | `Hacienda.cs:61-557` | Sin línea en DOCX |
| H-03 OCP Potrero | Confirmado condicionado | `Potrero.cs:62-102` | Sin línea; eje no es SC aprobada |

La refutación IA es válida: entidades del dominio concretas no son automáticamente detalles de bajo nivel. Los dolores #1 Persistencia, #2 Hacienda y #3 validación están correctamente ordenados; falta enlazarlos a líneas/SC. Graphify apoya navegación, no causalidad.

## 6. Auditoría SC-1 / SC-2 / SC-3

| SC | Must-modify AS-IS | Nuevos | Riesgo dominante | Dictamen |
|---|---:|---:|---|---|
| SC-1 derivados | 4 clases/6 archivos | 0/0 | Venta/retiro/formato 7 campos | Verificado bajo alcance declarado |
| SC-2 chip/geo | 4/5 | 0/0 | 226 registros, identidad/compatibilidad | Verificado bajo alcance declarado |
| SC-3 clínica | 5/8 | 1 clase/2 archivos | Reglas vacunales/arranque/formato | Verificado bajo alcance declarado |

El auditor OCP disputa 4/6 para SC-1 por clientes expuestos. Esta consolidación conserva 4/6 porque el DOCX separa explícitamente **modificación** de **regresión** y mantiene firmas de `VentaService/Controller/Program`; no es una cifra universal. En TO-BE, SC-1 está dividida entre dos modelos incompatibles; SC-2/3 no tienen respuesta.

## 7. Auditoría TO-BE + SOLID

### Hallazgos consolidados

| ID | Estado; severidad/conf. | Evidencia exacta | Presión/cliente/consecuencia | Mínima acción/test/trade-off |
|---|---|---|---|---|
| ARCH-001 | Confirmado; Blocker/100 | MVC csproj `:15-17`; DLL SHA igual; NEW `Hacienda.cs:16,30-34,143` | Artefacto ejecutado debe derivar de NEW; evaluador ejecuta OLD | Build reproducible + prueba assembly/API; no mezclar con refactor |
| ARCH-002 | Confirmado; High/98 | 4 PNG vs ausencia de tipos | UML 1:1; tipos fantasma | Reducir al vertical; checklist bidireccional |
| OCP-001 | Confirmado; Blocker/99 | `Producto`, inventarios y `vender<T>` sin clientes | SC-1 no es extensión ejecutable | Res+Lácteo/Piel end-to-end; retiro/round-trip |
| SRP-001 | Confirmado; High/96 | OLD `Hacienda.cs:61-557`; NEW `:61-419` | Dominio/casos/eventos/mensajes cambian juntos | Sacar solo coordinación; evitar modelo anémico |
| SRP/DIP-001 | Confirmado; High/99 | `PersistenciaService.cs:12-642` | TXT, File, validación, Castle, HTTP | Puerto mínimo+codec; golden round-trip; no repo/entidad |
| ISP/LSP-001 | Confirmado; High/100 | Interfaz ancha + 3 `NotImplementedException` por validador | Implementaciones no cumplen contrato | Contrato estrecho; pruebas sin métodos falsos; riesgo API |
| LSP-001 | Confirmado preexistente; High/99 | `Res.Edad:31-35`; setters subtipos | Base acepta valores que subtipo rechaza | Caracterizar; deuda explícita o slice separado |
| LSP-002 | Confirmado; Blocker/100 | `Producto.cs:13`; NEW `Res.cs:12,24-40` | Constructor/invariante `Nombre` incompatibles | Restaurar legacy o unificar; test identidad |
| ISP/LSP-002 | Confirmado; Medium/98 | `IInventario.cs:10-15`; `Potrero.agregar:202-205` vacío | Venta no usa `agregar`; cliente genérico pierde datos | Contrato extracción o implementación real |
| ARCH/DIP-001 | Confirmado; High/98 | `ResController.cs:11-22,105-169` | Controller coordina dominio+persistencia | Mover esos casos; tests ActionResult/mensajes |
| DIP-001 | Confirmado; High/99 | Bib csproj `:34-65`; Aspectos HTTP/Castle | Núcleo depende de frameworks externos | Mover adaptador tras caracterizar |

**Falsos positivos rechazados:** interfaz por entidad; todo switch=OCP; concretos en `Program`=DIP; herencia Vacuna=LSP; tamaño de `VacunaController`=SRP. **Trade-offs:** TXT y dos soluciones son válidos; el problema es dirección y reproducibilidad.

**Desacuerdos:** (1) SC-1 4/6 se conserva condicionado; (2) planner propone corregir LSP, adversarial recomienda caracterizar/deuda por riesgo semántico; (3) net8 vs net472 queda como decisión, no retarget automático; (4) existe consenso en reducir UML.

## 8. Auditoría ADR

**0/5 ADR válidos.** Deben existir, como mínimo:

1. Toolchain/build NEW: net472+targeting pack vs migración aislada.
2. Modelo SC-1: compatibilidad aditiva vs migrar `Venta`.
3. TO-BE completo vs vertical reducido (recomendado: reducido).
4. Persistencia concreta vs puerto mínimo TXT.
5. Jerarquía Res: deuda caracterizada vs composición/estado.

Cada uno: ID/estado, evidencia/hallazgo, 2 alternativas reales, decisión, razones, costo negativo y principios. Si es retrospectivo, decirlo; no fingir cronología.

## 9. Auditoría código y fidelidad UML ↔ código

**TO-BE→código ausente:** `ItemVenta`, `IVendible`, `ProductoGanadero`, categoría, AppServices/interfaces, commands, `ResultadoAplicacion`, puertos/adapters/serializadores e `InicializadorHacienda`.

**Código→TO-BE ausente/divergente:** `Producto`, `IInventario<T>`, inventarios, `RegistroVenta`, servicios/Persistencia reales, interceptores, validadores, publishers y carga singleton.

Rupturas:
- `Hacienda` declara `IVentaRes` pero no `vender_res` (`NEW Hacienda.cs:16,143-167`).
- MVC usa `vender_res/L_ventas` (`ResController.cs:165-169`; `Program.cs:56`; `VentaService.cs:21-48`).
- Persistencia usa `Venta.Res/Potrero` y constructor de 4 argumentos (`:159-160,442`), inexistentes en NEW `Venta.cs:17-46`.
- `Res : Producto` no invoca `Producto(string)` y oculta `Nombre`.

Esto activa el tope de rúbrica C2/C4 y amenaza C4=0.

## 10. Baseline de compilación y ejecución

**OLD:** MVC build/host verificables; Bib fuente bloqueada por entorno; cero tests. **NEW:** MVC build engañoso contra DLL OLD; Bib fuente bloqueada y estáticamente rota; host ejecuta OLD. Los 5 warnings son CS8625 (`AccountController`), CS8618 (`LoginViewModel`) y CS8619 (`UsuarioService`), no el HintPath.

No publicar un comando final hasta disponer de runtime 8 y build fuente NEW. El roll-forward solo sirve como diagnóstico provisional.

## 11. Preservación de comportamiento

**AUSENTE.** Diferencias no autorizadas visibles:
- Mensaje de venta OLD `Hacienda.cs:161` vs NEW `:166`.
- Alimentar cantidad 0: OLD acepta (`:220-231`), NEW rechaza (`Res.cs:100-111`).
- OLD añade mensaje de vacunación completa (`:549`), NEW lo omite (`:418`).
- Cambian excepciones/API y setter de vacunas aplicadas.

Ocho casos mínimos en runners/procesos aislados (assemblies homónimos):
1. potrero válido/duplicado;
2. res válida;
3. edades 12/13/48/49 e inválida;
4. alimentar 0/1/N;
5. crear vacunas;
6. aplicar válida;
7. vencida/duplicada/límite;
8. venta: retiro, registro, mensaje y TXT.

Comparar retorno exacto, tipo/mensaje de excepción, estado/orden y archivo. No cuentan tablas manuales, `NotNull` o `Contains("éxito")`.

## 12. Solicitud de cambio y prueba de OCP

SC elegida **probable SC-1**, no formal. No hay controller, persistencia, demo ni métrica; `Potrero.agregar` es no-op y clientes no compilan. El diff mezcla SC-1 con vacunación, por lo que medirlo completo maquillaría OCP.

DONE exige dos métricas: baseline AS-IS 4/6 bajo supuesto; y diff exclusivo del slice SC-1 NEW, con nuevas/existentes, archivos y puntos de decisión. Tests extra: vender lácteo, piel, ausente/null, round-trip y tercera variante sin editar política.

## 13. Bitácora IA y criterio propio

Carpeta vacía. La refutación del inventario y decisiones como no contar regresiones, no usar centralidad como causalidad y no implementar IoT son material válido, pero no sustituyen la bitácora. Sin ella, C6=0. Registrar propuesta, `ACEPTADO/CORREGIDO/RECHAZADO`, argumento, evidencia y efecto; incluir límite contra implementar UML completo.

## 14. Preparación del video

| Minutos | Disponible | Falta | Estado |
|---|---|---|---|
| 0–3 | AS-IS y dolores | Editable/recorrido legible | PARCIAL |
| 3–6 | 3 propios, refutación, 2 hojas | Roster/contraste | PARCIAL |
| 6–12 | 4 PNG | 5 ADR, LSP formal, código alineado | INSUFICIENTE |
| 12–17 | Host inicia | NEW real, 8 comparaciones, SC y métrica | AUSENTE/ENGAÑOSO |
| 17–20 | Refutación aislada | Bitácora/límite/deuda | PARCIAL |

Sin enlace, roles o duración. No grabar una demo que cargue DLL OLD.

## 15. RIESGOS DE PENALIZACIÓN

| ID | Prob./impacto | Evidencia |
|---|---|---|
| P-01 no compila/ejecuta | Alta/Crítico | NEW fuente no demostrado; C4 puede ser 0 |
| P-02 TO-BE≠código | Cierta/Alto | C2/C4 limitados a 3 |
| P-03 lectura fría | Indeterminada/Alto | 2 hojas; roster ausente |
| P-04 bitácora | Cierta/Crítico | C6=0 |
| P-05 conducta | Alta si integra NEW/Crítico | Venta, alimentar 0, vacuna; -0.5 por caso |
| P-06 video | Cierta hoy/Crítico | C7=0; -1.5 individual posible |
| P-07 >20 min | No verificable/Alto | No hay video |
| P-08 tardía | Alta/Alto | Corte 9 agosto; vence 9 o 10 según grupo no declarado |

## 16. Nota simulada según rúbrica

| Criterio | Peso | Nota | Rango | Bloqueo principal | Acción a 4.5–5 | Conf. |
|---|---:|---:|---:|---|---|---:|
| C1 AS-IS | 15% | 3.80 | 3.4–4.0 | Editable/Program/líneas | Cerrar trazabilidad | 85% |
| C2 TO-BE | 20% | 2.60 | 2.3–3.0 | Inconsistencia/tope | UML editable 1:1 | 95% |
| C3 Argumentación | 20% | 1.40 | 0.8–1.8 | 0 ADR, LSP/DIP nominal | 5 ADR+matrices+evidencia | 95% |
| C4 Implementación | 15% | 0.00 | 0–0.5 | NEW real no compila; 0 tests | Build+8 casos+demo | 98% |
| C5 Extensibilidad | 10% | 0.70 | 0.5–1.2 | SC/métrica ausentes | Vertical+diff+otras SC | 95% |
| C6 IA | 10% | 0.00 | 0 | Bitácora ausente | Bitácora real | 100% |
| C7 Video | 10% | 0.00 | 0 | Video ausente | <=20, todos, demo | 100% |

`3.8×.15 + 2.6×.20 + 1.4×.20 + 0×.15 + .7×.10 = 1.44`.

# **NOTA ACTUAL ESTIMADA: 1.44 / 5.00**

Rango global razonable: 1.2–1.7. No se puntúa intención ni OLD ejecutado como NEW.

## 17. Matriz de trazabilidad

| Hallazgo | Dolor | ADR | TO-BE | Código | Test | SC/beneficio | Ruptura |
|---|---|---|---|---|---|---|---|
| H-01 validación | #3 | Ausente | ISP dibujado parcialmente | Copiado | Ausente | Añadir entidad sin métodos falsos | ADR/código/test |
| H-02 Hacienda | #2 | Ausente | AppServices | No materializado | Ausente | Aislar casos | ADR/código/test |
| H-03 Potrero | Sin dolor asociado | Ausente | No coherente | Switch permanece | Ausente | Nuevo tipo res | Desde dolor |
| Persistencia | #1 | Ausente | Puertos/serializadores | Un concreto | Ausente | SC1/2/3 | Todo tras dolor |
| Source/DLL | No diagnosticado | Ausente | Root ideal | DLL OLD | Build superficial | Precondición SC | Cadena nueva rota |

| SC | AS-IS | Presión | Respuesta TO-BE | Costo esperado | Evidencia |
|---|---|---|---|---|---|
| SC-1 | 4/6 | OCP+SRP | Dos modelos incompatibles | No calculado | Ninguna funcional |
| SC-2 | 4/5 | OCP sospechado; DIP/SRP persistencia | Ausente | Desconocido | Ninguna |
| SC-3 | 5/8+1/2 | OCP sanitario+SRP | Ausente | Desconocido | Ninguna |

Ninguna cadena llega hoy a ADR+implementación+test+beneficio.

## 18. Revisión adversarial

| Objeción | Sev. | Real/falsa | Corrección mínima |
|---|---|---|---|
| NEW es OLD | Blocker | Real | Build/hash/API NEW |
| SOLID nominal | High | Real | Abstracción con cliente/variación |
| Capas ceremoniales | High | Real | Reducir UML |
| DI sin DIP | High | Real | Puerto mínimo |
| LSP prometido | High | Real | Matriz/test/deuda |
| Potrero no-op | High | Real | Contrato estrecho/semántica |
| Métrica mezclada | High | Real | Diff SC exclusivo |
| Caracterización débil | Blocker | Real | Runners aislados exactos |
| ADR retrospectivo oculto | High | Real | Marcar honestamente |
| TXT “malo” | Low | Falsa | Mantener TXT; aislar frontera |
| Concretos en Program | Low | Falsa | Es composition root |

El plan original de 12 slices se reduce a 4 gates: build real; caracterización; compatibilidad+SC-1 vertical; evidencia. Retarget net8 no es primer paso automático. Una `VentaProducto` paralela solo con ADR y mapper único.

## 19. Tres escenarios más probables de fracaso

1. **Demo NEW ejecuta OLD.** Mecanismo: HintPath+SHA idéntico. Afecta C2/C4/C5; probabilidad muy alta, impacto crítico. Detectar con build limpio/hash/API SC-1. No grabar antes.
2. **Implementar UML completo y quedar sin tests/SC.** Decenas de tipos consumen el plazo. Afecta C3–C7; alta/crítico. Detectar si crecen clases más rápido que casos. Reducir al vertical.
3. **Preservación manual/superficial.** Oculta excepción/estado/TXT y puede cargar misma DLL dos veces. Afecta C4/P-05; alta/crítico. Dos runners, output canónico y comparación exacta.

## 20. Backlog priorizado P0/P1/P2/P3/NO HACER

### P0 — 9

| ID | Tarea | DONE exacto | Dep. | Archivos/rol | Paralelo | Riesgo/tam. |
|---|---|---|---|---|---|---|
| P0-01 | Roster/grupo/roles/SC/deadline | README declara todo y valida hojas | — | README; integrador | Sí | Bajo/XS |
| P0-02 | Congelar OLD/elegir toolchain | ADR net472 vs migración aislada | 01 | ADR; build | No | Alto/S |
| P0-03 | Build source NEW | Bib compila; MVC consume artefacto NEW; hash/API probado | 02 | csproj+contratos; writer | No | Crítico/L |
| P0-04 | 8 caracterizaciones | OLD/NEW aislados comparan salida/excepción/estado/TXT | 02 | tests/evidencia; test guardian | No | Alto/M |
| P0-05 | Compatibilidad + SC-1 vertical | 8 verdes + lácteo/piel persistidos/demo | 03/04 | dominio/app/MVC/TXT; writer | No | Crítico/L |
| P0-06 | TO-BE editable 1:1 | Sin tipos fantasma; checklist bidireccional | 05 | diagramas; arquitecto | No | Alto/M |
| P0-07 | Bitácora IA | Decisiones/argumentos/evidencia/límite | 01 | evidencia; todos | Sí | Medio/S |
| P0-08 | README reproducible | OLD/NEW/tests/SC/evidencia/video | 03/04 | README; integrador | Parcial | Medio/S |
| P0-09 | Video <=20 | Todos, NEW real, link probado | todos | guion/README; todos | No | Crítico/L |

### P1 — 8

1. Cinco ADR válidos (M).
2. Matriz/tests LSP y decisión deuda vs cambio (M).
3. Resolver validadores ISP/LSP sin métodos falsos (M).
4. Puerto mínimo de persistencia, TXT compatible (M/L).
5. Res/Potrero controllers solo adaptan HTTP (M).
6. Métrica SC-1 con diff exclusivo y reglas (S).
7. Explicar costo TO-BE de SC-2/3 (S).
8. AS-IS editable + Program + líneas H-02/H-03 (M).

### P2

- Matriz dolor→hallazgo→ADR→test; contraste lectura fría; mapear DLL/root; ensayo de contradicción; separar generados de evidencia.

### P3

- Warnings de nulabilidad; ortografía documental sin cambiar outputs; limpieza de generados en actividad aprobada separada.

### NO HACER

- No implementar todas las cajas UML, interfaz por clase, repositorio por entidad, mediator/bus/microservicios.
- No presentar DLL OLD como NEW ni retarget a net10.
- No mezclar migración, LSP, puertos y SC-1 en una slice.
- No mejorar mensajes/excepciones/reglas legacy sin autorización.
- No contar `bin/obj/.vs/packages` como source/evidencia.

## 21. Camino crítico hasta entrega

1. Declarar roster/grupo/deadline/SC-1 y congelar OLD.
2. Elegir toolchain y recuperar build del source NEW real.
3. Ejecutar 8 caracterizaciones OLD; restaurar equivalencia NEW.
4. Completar vertical SC-1 y acceptance tests.
5. Medir diff; explicar SC-2/3.
6. Regenerar UML editable reducido y 5 ADR.
7. Bitácora, README, trazabilidad y outputs.
8. Ensayar/grabar/validar video <=20.

No adelantar arquitectura P1 si build/tests P0 no están verdes.

## 22. Trabajo paralelizable por agentes

| Stream | Responsable | Ahora | Restricción |
|---|---|---|---|
| Roster/README/bitácora | Integrador humano | Sí | No inventar |
| Catálogo/runners OLD | Test Guardian | Sí | No producción |
| Alternativas ADR/UML reducido | Arquitecto | Borrador sí | Publicar tras código verde |
| Líneas hallazgos/AS-IS editable | Integrador | Sí | No tocar hojas frías |
| Build/compatibilidad NEW | Writer único | Tras toolchain | Sin segundo writer |
| Auditorías SOLID | Especialistas | Tras cada slice | Read-only |
| Falsificación final | Adversarial | Tras build/tests/SC | Read-only |

## 23. Definition of Done FINAL

- [ ] Roster/grupo/roles/deadline y una hoja intacta por integrante.
- [ ] Contraste de lectura fría.
- [ ] AS-IS editable/exportado/fiel, con root.
- [ ] Hallazgos con archivo/símbolo/línea/impacto/origen.
- [ ] Tres dolores enlazados a SC.
- [ ] Baselines SC-1/2/3 congelados.
- [ ] SC elegida/justificada.
- [ ] Cinco ADR con alternativas/costo.
- [ ] TO-BE editable, colores, sin tipos fantasma.
- [ ] Correspondencia UML↔código bidireccional.
- [ ] SRP con actores; OCP con eje; LSP formal; ISP con clientes; DIP con root.
- [ ] Bib NEW compila desde source y MVC consume NEW.
- [ ] NEW ejecuta en entorno documentado.
- [ ] 8 casos OLD y NEW comparados; cero deriva no autorizada.
- [ ] SC-1 vende/persiste lácteo y piel; ausente/null atómico; tercera variante extensible.
- [ ] Métrica exclusiva y honesta; SC-2/3 explicadas.
- [ ] Bitácora con aceptado/corregido/rechazado y límite.
- [ ] README con prerequisitos y comandos probados.
- [ ] Demo representativa.
- [ ] Video accesible <=20; todos participan y responden contradicción.
- [ ] Revisión adversarial sin blockers.
- [ ] Estado Git revisado; sin secretos/generados accidentales de entrega.

## 24. Contexto compacto para el próximo agente

**Objetivo:** modernizar Hacienda preservando toda conducta observable; demostrar una SC barata con evidencia. **No tocar OLD; una writer; 8 caracterizaciones antes de producción; no implementar UML aspiracional ni inventar ADR/IA.**

Source of truth: enunciado DOCX. Corte 2026-08-09, commit `b20615f`, main, worktree sucio. Equipo/grupo desconocidos; solo Santiago y Sebastián identificados.

OLD: `03-src/original/HaciendaOLD`; Bib net472, MVC net8 vía HintPath; MVC build OK/5 warnings, Bib bloqueada MSB3644, host arranca solo con roll-forward, 0 tests. Arquitectura: MVC→services+dominio+Persistencia; TXT; Program singleton/root.

NEW: MVC source igual a OLD; las cuatro DLL Bib tienen SHA `1ae86a...`; ejecutar NEW ejecuta OLD. Source añade Producto/Lacteo/Piel/inventarios/RegistroVenta/IInventario/vender genérico y vacunación, pero rompe `IVentaRes`, `L_ventas`, `Venta`, `Res:Producto`; `Potrero.agregar` vacío. SC probable SC-1, no declarada/funcional.

AS-IS: PDF+5 PNG bastante fieles, sin editable/Program. H-01 validación, H-02 Hacienda, H-03 Potrero; 3 propios y refutación IA. Dolores: Persistencia/Hacienda/validación. Fase 2 útil y condicionada: SC1 4/6, SC2 4/5, SC3 5/8+1/2.

TO-BE: 4 PNG con leyenda, sin editable; dibuja ItemVenta/AppServices/commands/puertos/serializadores inexistentes. ADR vacío. Evidencia/metricas/bitácora vacías; README equivocado; sin video. Nota actual 1.44.

Prioridad: roster/toolchain → build NEW real → 8 runners pareados → equivalencia+SC1 vertical → UML reducido → ADR/métrica/bitácora/README/video. No rehacer Fase 2, hojas frías ni graphify; no cambiar outputs legacy sin prueba/autorización. Incertidumbres: roster/deadline, toolchain, deuda LSP, modelo SC-1, consumidores externos y fuente editable UML.
