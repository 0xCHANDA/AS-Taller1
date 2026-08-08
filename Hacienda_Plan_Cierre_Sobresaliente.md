# 🐄 Hacienda — Estado del Proyecto y Plan de Cierre

> **Objetivo:** llevar la entrega desde el estado actual hasta una versión **sobresaliente, coherente, compilable y defendible**, alineada con la rúbrica.
>
> **Diagnóstico general:** la documentación de **Fase 1 y Fase 2** está bastante avanzada, pero el mayor riesgo está en **Fase 3 y Fase 4**: diseño TO-BE, implementación, pruebas y evidencia.

---

## 📊 Estado General por Fases

| Fase | Estado | Evidencia encontrada |
|---|---|---|
| **Fase 0 — Lectura en frío** | ❌ Falta | No se encontró `/00-lectura-en-frio` ni hojas individuales. |
| **Fase 1 — Diagnóstico AS-IS** | 🟡 Parcial avanzado | `docs/as-is/DIAGNOSTICO_AS_IS.md`, `uml-as-is.puml`, `inventario de hallazgos.docx`, `puntos de dolor priorizados.docx`. |
| **Fase 2 — Cambios futuros** | 🟡 Parcial avanzado | `Fase 2 — Los cambios que vienen.docx`, con medición de `SC-1`, `SC-2` y `SC-3`. |
| **Fase 3 — Diseño TO-BE** | ❌ Falta | No se encontró diagrama TO-BE ni ADR. |
| **Fase 4 — Implementación y evidencia** | 🔴 Falta crítica | Existe el sistema MVC original y el MVC compila, pero no hay rediseño implementado ni evidencia suficiente. |
| **Fase 5 — Video** | ❌ Falta | No se encontró enlace ni guion final. |
| **Organización final** | ❌ Falta | Los documentos están dispersos o dentro de `Hacienda/docs`. |

---

# 🚨 Problemas Importantes Detectados

## 1. Contradicción en la autoría de hallazgos

El diagnóstico AS-IS indica que **todos los hallazgos son asistidos por IA**, pero la rúbrica exige como mínimo:

> **3 hallazgos propios del equipo.**

Sin embargo, sí existe un archivo:

```text
inventario de hallazgos.docx
```

donde aparecen `H-01`, `H-02` y `H-03` marcados como **Propio**.

### Riesgo

La entrega puede quedar internamente contradictoria si:

- el diagnóstico principal dice que todo fue asistido;
- el inventario dice que algunos hallazgos son propios;
- ambos documentos se entregan sin consolidación.

### Acción requerida

Unificar el diagnóstico final y conservar como propios, idealmente:

- `IValidarInformacion / Validacion`
- `Hacienda` como clase central
- `Potrero.anadir_res()` dependiendo de tipos concretos

---

## 2. Falta el diseño TO-BE

Actualmente no se encontró:

- diagrama TO-BE;
- arquitectura final;
- ADR;
- correspondencia entre arquitectura propuesta y código.

### Impacto

Este es uno de los faltantes más graves porque el TO-BE soporta:

- argumentación SOLID;
- implementación;
- extensibilidad;
- decisiones arquitectónicas;
- defensa en video.

---

## 3. Falta implementación rediseñada

Existe el sistema MVC original y el proyecto MVC compila, pero no hay todavía una versión rediseñada completa dentro de una estructura de entrega final.

### Riesgo crítico

El criterio de implementación puede terminar en **0** si:

- el código final no compila;
- no ejecuta;
- no corresponde al TO-BE;
- no existe una demostración funcional.

---

## 4. No hay pruebas de caracterización

La rúbrica exige mínimo:

> **8 casos de caracterización.**

Actualmente no se encontró evidencia suficiente.

Las pruebas deben permitir comparar:

```text
Sistema original
      ↓
Misma entrada
      ↓
Sistema rediseñado
      ↓
Comparación de comportamiento/salida
```

---

## 5. Falta bitácora de uso de IA

Sin bitácora de IA, el criterio asociado puede quedar directamente en:

> **0 puntos.**

Debe registrarse:

| Qué propuso la IA | Qué decidió el equipo | Argumento técnico |
|---|---|---|
| Propuesta concreta | Aceptada / modificada / rechazada | Justificación |

---

## 6. Falta evidencia visual de UML

Existe:

```text
docs/as-is/uml-as-is.puml
```

pero falta exportarlo a una imagen:

```text
uml-as-is.png
```

o:

```text
uml-as-is.svg
```

La entrega debe conservar:

- fuente editable;
- imagen exportada.

Esto aplica tanto para **AS-IS** como para **TO-BE**.

---

## 7. Limitación del entorno de compilación

El proyecto actual compila el MVC utilizando una DLL ya generada.

La biblioteca fuente `net472` no compila directamente en este entorno Linux porque falta el targeting pack correspondiente.

### Implicación

No conviene confundir:

```text
"El MVC compila"
```

con:

```text
"Todo el código fuente original compila de forma reproducible en Linux"
```

La documentación debe explicarlo claramente.

---

# ✅ TODO para Terminar en Nivel Sobresaliente

---

## Fase 0 — Lectura en frío

### [ ] 1. Crear estructura final

Crear:

```text
/
├── 00-lectura-en-frio/
├── 01-diagnostico/
├── 02-diseno/
├── 03-src/
├── 04-evidencia/
└── README.md
```

---

### [ ] 2. Crear una hoja por integrante

Cada integrante debe responder, antes de revisar profundamente la arquitectura:

1. ¿Qué hace el sistema?
2. ¿Cuáles parecen ser sus entidades principales?
3. ¿Cuáles son tres lugares costosos de cambiar?
4. ¿Qué pregunta le haría al programador original?

Ubicación sugerida:

```text
00-lectura-en-frio/
├── integrante-1.md
├── integrante-2.md
├── integrante-3.md
└── integrante-4.md
```

---

# Fase 1 — Diagnóstico AS-IS

## [ ] 3. Consolidar diagnóstico

Unificar:

```text
docs/as-is/DIAGNOSTICO_AS_IS.md
inventario de hallazgos.docx
puntos de dolor priorizados.docx
```

en un documento final coherente.

Ejemplo:

```text
01-diagnostico/
└── DIAGNOSTICO_AS_IS_FINAL.md
```

---

## [ ] 4. Corregir la autoría de hallazgos

Dejar mínimo **3 hallazgos propios**.

Candidatos recomendados:

### Hallazgo propio 1

```text
IValidarInformacion / Validacion
```

Analizar:

- segregación de interfaces;
- responsabilidades;
- acoplamiento.

### Hallazgo propio 2

```text
Hacienda como clase central
```

Analizar:

- exceso de responsabilidades;
- coordinación;
- posible violación de SRP;
- dependencia excesiva del núcleo.

### Hallazgo propio 3

```text
Potrero.anadir_res()
```

Analizar la dependencia con tipos concretos y su impacto sobre:

- OCP;
- DIP;
- extensibilidad.

---

## [ ] 5. Mantener una refutación explícita a IA

Conservar una refutación técnicamente defendible.

La ya existente es adecuada:

> Rechazar la propuesta de “crear interfaces para todas las entidades concretas”, porque DIP no exige abstraer todas las entidades del dominio.

La refutación debe demostrar que el equipo:

- no acepta automáticamente las sugerencias;
- entiende el principio;
- puede justificar una decisión contraria.

---

## [ ] 6. Exportar UML AS-IS

Fuente existente:

```text
docs/as-is/uml-as-is.puml
```

Generar:

```text
01-diagnostico/
├── uml-as-is.puml
└── uml-as-is.svg
```

o:

```text
uml-as-is.png
```

---

## [ ] 7. Eliminar contradicciones sobre asistencia IA

El diagnóstico debe diferenciar claramente:

```text
Propio
Asistido por IA
Validado por el equipo
Refutado por el equipo
```

Evitar afirmar que todo fue asistido si existen hallazgos propios.

---

# Fase 2 — Cambios futuros

## [ ] 8. Integrar documento de solicitudes de cambio

Mover o convertir:

```text
Fase 2 — Los cambios que vienen.docx
```

a:

```text
01-diagnostico/
└── CAMBIOS_FUTUROS.md
```

Conservar el análisis de:

- `SC-1`
- `SC-2`
- `SC-3`

---

## [ ] 9. Elegir una solicitud de cambio para implementar

### Recomendación

Implementar:

> **SC-3 — Historia clínica de cada res**

### Razones

Tiene buena relación con:

- SRP;
- OCP;
- separación de responsabilidades;
- crecimiento futuro del dominio.

Además, según el análisis existente, su impacto medido sobre la arquitectura original parece menor que `SC-2`, lo que facilita construir una comparación antes/después defendible.

---

# Fase 3 — Diseño TO-BE

## [ ] 10. Crear UML TO-BE

El nuevo diagrama debe extender el modelo actual y mostrar claramente:

- qué se conserva;
- qué se divide;
- qué se abstrae;
- qué se agrega;
- qué dependencias se invierten.

### Convención visual sugerida

Usar colores distintos para identificar cambios relacionados con:

- **Negro:** clases conservadas
- **SRP:** responsabilidades separadas
- **OCP:** puntos de extensión
- **ISP:** interfaces segregadas
- **DIP:** dependencias invertidas
- **LSP:** jerarquías validadas o rediseñadas

> El color debe complementar la explicación, no sustituirla.

---

## [ ] 11. Crear mínimo 5 ADR

Ubicación:

```text
02-diseno/adr/
```

Ejemplo:

```text
ADR-001-separar-persistencia.md
ADR-002-segregar-validadores.md
ADR-003-historia-clinica.md
ADR-004-abstraer-repositorios.md
ADR-005-composition-root.md
```

### Decisiones sugeridas

1. Separar persistencia.
2. Dividir validadores por interfaz.
3. Separar ventas ganaderas/productos o historia clínica.
4. Abstraer repositorios.
5. Definir un Composition Root.

---

## [ ] 12. Argumentar SOLID con evidencia

No basta escribir:

> “Aplicamos SRP.”

Cada aplicación debe explicar:

```text
Hallazgo observado
        ↓
Clase afectada
        ↓
Principio SOLID relacionado
        ↓
Cambio realizado
        ↓
Riesgo reducido
        ↓
Impacto sobre futuras modificaciones
```

Ejemplo:

```text
H-02 muestra que Hacienda coordina demasiadas responsabilidades.
Esto aumenta el costo de cambio y el acoplamiento.
Se separa la gestión de historia clínica en HistoriaClinicaService.
La clase Hacienda deja de conocer detalles médicos.
Esto reduce razones de cambio y mejora SRP.
```

---

## [ ] 13. Verificar herencias

Revisar especialmente:

```text
Res
├── Ternero
├── Cebon
└── Novillo
```

Preguntas técnicas:

- ¿Una subclase puede sustituir realmente a `Res`?
- ¿Rompe precondiciones?
- ¿Cambia comportamiento esperado?
- ¿La diferencia pertenece a identidad/tipo o a estado/etapa?

Si la jerarquía no pasa LSP, considerar:

```text
Composición
```

o:

```text
Strategy
```

en lugar de herencia.

---

## [ ] 14. Definir inversiones de dependencia

Para cada DIP aplicado, documentar explícitamente:

```text
Módulo de alto nivel
Módulo de bajo nivel
Abstracción
Implementación
Composition Root
```

Aplicarlo, como mínimo, en:

- persistencia;
- validación;
- posiblemente historia clínica.

---

# Fase 4 — Implementación y Evidencia

## [ ] 15. Implementar rediseño en `/03-src`

El código final debe:

- compilar;
- ejecutar;
- corresponder uno a uno con el TO-BE;
- demostrar las decisiones documentadas.

Estructura sugerida:

```text
03-src/
├── Hacienda.Domain/
├── Hacienda.Application/
├── Hacienda.Infrastructure/
├── Hacienda.ConsoleDemo/
└── Hacienda.Tests/
```

> Ajustar la estructura al alcance real del taller. No crear capas artificiales solo por “parecer arquitectura limpia”.

---

## [ ] 16. Crear programa principal de demostración

Debe recorrer escenarios clave.

Como mínimo:

```text
1. Crear potrero
2. Crear res
3. Alimentar
4. Vacunar
5. Vender
6. Consultar ventas
7. Usuarios / login, si aplica
8. Ejecutar la solicitud de cambio elegida
```

Para `SC-3`, incluir:

```text
Registrar evento clínico
Consultar historia clínica
Agregar nueva intervención
```

---

## [ ] 17. Crear mínimo 8 casos de caracterización

Los casos deben ejecutarse contra:

```text
Sistema original
```

y:

```text
Sistema rediseñado
```

para comparar salidas y comportamiento.

Ejemplo de tabla:

| Caso | Entrada | Original | Rediseñado | Resultado |
|---|---|---|---|---|
| C-01 | Crear res válida | OK | OK | ✅ Equivalente |
| C-02 | Res con datos inválidos | Error | Error | ✅ Equivalente |
| C-03 | Crear potrero | OK | OK | ✅ Equivalente |
| C-04 | Alimentar res | Cambio estado | Cambio estado | ✅ Equivalente |
| C-05 | Vacunar | Registro | Registro | ✅ Equivalente |
| C-06 | Venta válida | Venta creada | Venta creada | ✅ Equivalente |
| C-07 | Consulta de ventas | Lista | Lista | ✅ Equivalente |
| C-08 | Login inválido | Rechazo | Rechazo | ✅ Equivalente |

---

## [ ] 18. Implementar una solicitud de cambio

La modificación debe demostrar que la nueva arquitectura permite:

> **Agregar más clases de las que se modifican.**

Idealmente:

```text
Arquitectura vieja
→ muchas clases modificadas

Arquitectura nueva
→ nuevas clases + pocas clases existentes modificadas
```

---

## [ ] 19. Preparar métricas antes/después

Ejemplo:

| Métrica | Arquitectura original | Arquitectura TO-BE |
|---|---:|---:|
| Archivos modificados para SC-3 | 7 | Y |
| Clases nuevas | 0 | X |
| Clases existentes modificadas | 7 | Y |
| Interfaces nuevas | 0 | Z |
| Puntos de extensión reutilizables | Bajo | Alto |

### Regla

No maquillar los números.

Si el resultado no mejora tanto como se esperaba, explicarlo técnicamente.

---

## [ ] 20. Crear bitácora IA

Ubicación:

```text
04-evidencia/
└── BITACORA_IA.md
```

Formato recomendado:

| Propuesta de IA | Decisión del equipo | Argumento técnico |
|---|---|---|
| Crear interfaz para cada entidad | Rechazada | DIP no obliga a abstraer entidades de dominio sin una necesidad real |
| Separar persistencia | Aceptada | Reduce acoplamiento entre lógica y almacenamiento |
| Usar Strategy para etapa del ganado | Evaluada | Depende de si la jerarquía actual viola LSP |

---

# Fase 5 — README y Video

## [ ] 21. Crear README final

Debe incluir como mínimo:

### Equipo

- integrantes;
- roles;
- responsabilidades.

### Arquitectura

- breve explicación AS-IS;
- breve explicación TO-BE;
- principios SOLID aplicados.

### Ejecución

```bash
dotnet build
dotnet test
dotnet run --project ...
```

### Evidencia

- ubicación de diagramas;
- ADR;
- pruebas;
- métricas;
- bitácora IA.

### Video

```text
Enlace: <URL>
```

---

## [ ] 22. Grabar video final

### Restricciones

- máximo **20 minutos**;
- todos los integrantes participan;
- debe existir ejecución en vivo.

### Contenido mínimo recomendado

1. Contexto del sistema.
2. Hallazgos propios.
3. Hallazgos asistidos.
4. Refutación a IA.
5. UML AS-IS.
6. Problemas priorizados.
7. UML TO-BE.
8. ADR principales.
9. Aplicación de SOLID.
10. Ejecución del programa.
11. Casos de caracterización.
12. Solicitud de cambio implementada.
13. Métrica antes/después.
14. Conclusiones.

---

# 🔥 Prioridad Real de Ejecución

Si el tiempo es corto, **este debe ser el orden**:

## P0 — Bloqueantes

### [ ] 1. Organizar carpetas y consolidar documentos

Sin estructura final, la entrega se vuelve difícil de revisar y defender.

### [ ] 2. Resolver contradicción de hallazgos propios

Es un problema directo contra la rúbrica.

### [ ] 3. Crear TO-BE + ADR

Sin esto no existe una base sólida para la implementación.

### [ ] 4. Implementar rediseño mínimo pero compilable

Código que no compila = riesgo de perder completamente el criterio.

---

## P1 — Evidencia técnica obligatoria

### [ ] 5. Crear 8 casos de caracterización

Demuestran preservación del comportamiento.

### [ ] 6. Implementar una SC con métrica

Idealmente `SC-3`.

### [ ] 7. Crear bitácora IA

Evita perder el criterio asociado por ausencia de evidencia.

---

## P2 — Cierre de entrega

### [ ] 8. README final

### [ ] 9. Exportar UML

### [ ] 10. Preparar y grabar video

---

# 📁 Estructura Final Recomendada

```text
Hacienda/
│
├── 00-lectura-en-frio/
│   ├── integrante-1.md
│   ├── integrante-2.md
│   ├── integrante-3.md
│   └── integrante-4.md
│
├── 01-diagnostico/
│   ├── DIAGNOSTICO_AS_IS_FINAL.md
│   ├── CAMBIOS_FUTUROS.md
│   ├── uml-as-is.puml
│   ├── uml-as-is.svg
│   └── evidencia-hallazgos/
│
├── 02-diseno/
│   ├── TO_BE.md
│   ├── uml-to-be.puml
│   ├── uml-to-be.svg
│   └── adr/
│       ├── ADR-001-separar-persistencia.md
│       ├── ADR-002-segregar-validadores.md
│       ├── ADR-003-historia-clinica.md
│       ├── ADR-004-abstraer-repositorios.md
│       └── ADR-005-composition-root.md
│
├── 03-src/
│   ├── Hacienda.Domain/
│   ├── Hacienda.Application/
│   ├── Hacienda.Infrastructure/
│   ├── Hacienda.ConsoleDemo/
│   └── Hacienda.Tests/
│
├── 04-evidencia/
│   ├── caracterizacion/
│   ├── metricas/
│   ├── BITACORA_IA.md
│   ├── capturas/
│   └── video.md
│
└── README.md
```

---

# ⚠️ Tres Escenarios Probables de Fracaso

## 1. Diseñar mucho y no llegar a código ejecutable

El equipo puede gastar demasiado tiempo perfeccionando UML y documentos.

### Resultado

```text
Buen diseño
+
código incompleto
=
criterio de implementación en riesgo
```

### Mitigación

Crear primero un **TO-BE mínimo implementable**, hacerlo compilar y después mejorar detalles.

---

## 2. Refactorizar demasiado el sistema original

Intentar “arreglar todo” puede introducir:

- regresiones;
- cambios de comportamiento;
- exceso de clases;
- arquitectura artificial.

### Mitigación

Aplicar únicamente cambios justificables por:

- hallazgos;
- SOLID;
- solicitud de cambio elegida;
- evidencia medible.

---

## 3. Tener evidencia correcta pero inconsistente

Ejemplos:

```text
UML muestra una arquitectura
Código implementa otra
README describe una tercera
Diagnóstico contradice inventario
Video menciona decisiones no documentadas
```

### Mitigación

Usar una sola fuente de verdad:

```text
Hallazgo
→ ADR
→ TO-BE
→ Código
→ Prueba
→ Métrica
→ Video
```

Cada elemento debe poder rastrearse al anterior.

---

# 🧭 Cadena de Trazabilidad Recomendada

Para cada decisión importante:

```text
HALLAZGO
   ↓
PROBLEMA
   ↓
PRINCIPIO SOLID
   ↓
ADR
   ↓
CAMBIO EN TO-BE
   ↓
IMPLEMENTACIÓN
   ↓
PRUEBA
   ↓
MÉTRICA
   ↓
EVIDENCIA EN VIDEO
```

Si una clase o abstracción nueva no puede justificarse mediante esta cadena, probablemente sobra.

---

# 📌 Estado Actual Resumido

## Lo más completo

- Diagnóstico AS-IS.
- Inventario de hallazgos.
- Priorización de puntos de dolor.
- Análisis de solicitudes de cambio.
- UML AS-IS editable.
- Sistema MVC original compilable en el contexto actual.

## Lo más atrasado

- TO-BE.
- ADR.
- Implementación rediseñada.
- Pruebas de caracterización.
- Métricas antes/después.
- Bitácora IA.
- Evidencia visual final.
- README.
- Video.

---

# 🎯 Conclusión

Actualmente, el proyecto está fuerte en:

> **Fase 1 + Fase 2 documental**

pero débil precisamente en los elementos de mayor peso técnico:

> **Fase 3 + Fase 4**

Estas fases concentran aproximadamente **65 % de la nota** al considerar:

- diseño;
- argumentación;
- implementación;
- extensibilidad;
- evidencia.

Por lo tanto, el foco ya no debe seguir siendo ampliar el diagnóstico.

La prioridad correcta es:

```text
Diagnóstico consolidado
        ↓
TO-BE
        ↓
ADR
        ↓
Código compilable
        ↓
8 pruebas de caracterización
        ↓
SC implementada
        ↓
Métricas
        ↓
Bitácora IA
        ↓
README
        ↓
Video
```

> **Objetivo final:** que cada afirmación arquitectónica pueda demostrarse con código, pruebas y evidencia verificable.
