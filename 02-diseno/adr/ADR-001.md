# ADR-001 — Toolchain y build del source NEW

**Estado:** ACEPTADO
**Fecha:** 2026-08-10
**Evidencia:** `p_mvcHacienda.csproj:14-16`, `Bib_Hacienda.csproj:1-12` y los ejecutables de caracterización OLD/NEW.

## Contexto

OLD `Bib_Hacienda` compila con `net472` y requiere el targeting pack de .NET Framework, no disponible en este host Linux. NEW usa `net8.0` y el MVC referencia el proyecto de dominio directamente. La caracterización de OLD necesita conservar un artefacto legacy separado y claramente identificado.

## Alternativas

| ID | Descripción | Evaluación |
|---|---|---|
| A | Instalar targeting pack net472 en el host | Descartada: sin acceso al sistema ni garantía en entorno del evaluador. |
| B | Migrar OLD a net8 sin cambios funcionales | Descartada: riesgo de deriva conductual sin cobertura de tests. |
| C | NEW net8 con `ProjectReference`; OLD caracterizado mediante DLL legacy | **Elegida.** Build reproducible; OLD ejecutado con artefacto compilado real. |

## Decisión

Alternativa C. Bib_Hacienda NEW compila `net8.0`. MVC NEW referencia por `ProjectReference`, eliminando el `HintPath`. OLD se caracteriza con `Characterization/Old/lib/Bib_Hacienda.dll`, dentro de una carpeta identificada explícitamente como OLD.

## Consecuencias

- **Positivo:** Build NEW reproducible en cualquier host SDK net8.0.
- **Positivo:** Cambios en Bib_Hacienda se propagan automáticamente al MVC.
- **Negativo:** OLD no compila desde source en este host; la caracterización depende del artefacto precompilado.
- **Riesgo mitigado:** OLD y NEW se cargan desde rutas distintas y el verificador comprueba que NEW proviene del source rediseñado.

## Principios involucrados

Esta es una decisión de reproducibilidad, no una aplicación de DIP. Un `ProjectReference` mejora el build, pero no invierte una dependencia de negocio.

## Verificación

```bash
dotnet build 03-src/redisenado/HaciendaNEW/Bib_Hacienda/Bib_Hacienda/Bib_Hacienda.csproj
dotnet build 03-src/redisenado/HaciendaNEW/p_mvcHacienda/p_mvcHacienda.csproj
```
