# ADR-001 — Toolchain y build del source NEW

**Estado:** ACEPTADO (retrospectivo — documentado post-implementación)
**Fecha:** 2026-08-10
**Evidencia:** `04-evidencia/historico/ESTADO_ACTUAL_Y_PLAN_CIERRE.md:12-27`, `p_mvcHacienda.csproj:14-16`, `Bib_Hacienda.csproj:1-12`

## Contexto

OLD `Bib_Hacienda` compila con `net472` y requiere targeting pack de .NET Framework, no disponible en este host Linux. MVC OLD usa `HintPath` a una DLL precompilada. El source NEW de `Bib_Hacienda` migró a `net8.0` SDK-style sin dependencias de paquetes; el MVC NEW usa `ProjectReference`. Persiste el riesgo de que el MVC OLD arranque como si fuera NEW (las DLL tienen SHA-256 idéntico).

## Alternativas

| ID | Descripción | Evaluación |
|---|---|---|
| A | Instalar targeting pack net472 en el host | Descartada: sin acceso al sistema ni garantía en entorno del evaluador. |
| B | Migrar OLD a net8 sin cambios funcionales | Descartada: riesgo de deriva conductual sin cobertura de tests. |
| C | NEW net8 con `ProjectReference`; OLD caracterizado mediante DLL legacy | **Elegida.** Build reproducible; OLD ejecutado con artefacto compilado real. |

## Decisión

Alternativa C. Bib_Hacienda NEW compila `net8.0` sin paquetes (`Bib_Hacienda.csproj:1-12`). MVC NEW referencia por `ProjectReference` (`p_mvcHacienda.csproj:14-16`), eliminando `HintPath`. OLD se caracteriza con la DLL que acompaña al MVC OLD.

## Consecuencias

- **Positivo:** Build NEW reproducible en cualquier host SDK net8.0.
- **Positivo:** Cambios en Bib_Hacienda se propagan automáticamente al MVC.
- **Negativo:** OLD no compila desde source en este host; la caracterización depende del artefacto precompilado.
- **Riesgo mitigado:** SHA-256 confirma que la DLL OLD usada es exactamente la referenciada.

## Principios SOLID

- **DIP:** `ProjectReference` > `HintPath` en dirección de dependencia.
- Esta decisión es infraestructural, no de diseño de dominio.

## Verificación

```bash
scripts/phase4-safe-dotnet.sh build 03-src/redisenado/HaciendaNEW/Bib_Hacienda/Bib_Hacienda/Bib_Hacienda.sln
scripts/phase4-safe-dotnet.sh build 03-src/redisenado/HaciendaNEW/p_mvcHacienda/p_mvcHacienda.csproj
```
