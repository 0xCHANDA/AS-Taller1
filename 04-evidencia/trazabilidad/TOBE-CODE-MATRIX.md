# Correspondencia TO-BE ↔ código

## Fuente normativa resuelta

`02-diseno/diagramas/TO-BE.puml` es la fuente editable y normativa de la arquitectura final. Los cuatro PNG `fase3 uml 1..4.png` se conservan como evidencia histórica de Fase 3, pero quedan **superseded** porque describían tipos aspiracionales no materializados (`ItemVenta`, commands, serializadores y AppServices). No se agregaron cajas fantasma al código para satisfacer imágenes obsoletas.

Render opcional cuando PlantUML esté disponible:

```bash
plantuml 02-diseno/diagramas/TO-BE.puml
```

No se instaló una herramienta nueva para producir PNG.

## TO-BE → código

| Elemento(s) TO-BE | Código |
|---|---|
| Producto, Res, Ternero, Cebon, Novillo | `Bib_Hacienda/Bib_Hacienda/Clases/{Producto,Res,Ternero,Cebon,Novillo}.cs` |
| Lacteo, Carne, Piel | `Clases/{Lacteo,Carne,Piel}.cs` |
| InventarioLacteos, InventarioCarnes, InventarioPieles | `Clases/Inventario{Lacteos,Carnes,Pieles}.cs` |
| Potrero, Hacienda, Venta, RegistroVenta | `Clases/{Potrero,Hacienda,Venta,RegistroVenta}.cs` |
| Vacuna, Bacteriana, Viva | `Clases/{Vacuna,Bacteriana,Viva}.cs` |
| Usuario, Autenticacion | `Clases/{Usuario,Autenticacion}.cs` |
| IInventarioVendible, IInventario | `Interfaces/{IInventarioVendible,IInventario}.cs` |
| IVentaRes, IVacunacion, ICreacionVacuna, IAutenticacion | archivos homónimos en `Interfaces/` |
| IValidadorRes, IValidadorPotrero, IValidadorVacuna, IValidadorVenta | archivos homónimos en `Interfaces/` |
| IPersistenciaPotreros, IPersistenciaReses, IPersistenciaVacunas, IPersistenciaVentas, IPersistenciaUsuarios | `Interfaces/IPersistencia.cs` |
| ValidadorRes, ValidadorPotrero, ValidadorVacuna, ValidadorVenta | `Clases/Validaciones/Validar*.cs` |
| seis Publisher* | `Eventos/Publisher{PesoMin,PesoVenta,PotreroLleno,PotreroMitad,VacunaVencida,VacunacionCompletada}.cs` |
| ReglaRes, ReglaPotrero, ReglaVacuna, TipoVacuna | `Reglas/Regla*.cs`, `enum/TipoVacuna.cs` |
| PotreroService, ResService, VacunaService, VentaService, UsuarioService | `p_mvcHacienda/Servicios/*Service.cs` |
| PersistenciaService, ProductoPersistido | `p_mvcHacienda/Servicios/{PersistenciaService,ProductoPersistido}.cs` |
| InterceptorAutenticacion, InterceptorValidarInformacion | `p_mvcHacienda/Infrastructure/*.cs` |
| AccountController, HomeController, PotreroController, ResController, UsuarioController, VacunaController, VentaController | `p_mvcHacienda/Controllers/*Controller.cs` |
| ErrorViewModel, LoginViewModel | `p_mvcHacienda/Models/*.cs` |
| Program (composition root) | `p_mvcHacienda/Program.cs` |

## Código → TO-BE

| Área de código productivo | Elementos TO-BE | Estado |
|---|---|---|
| `Bib_Hacienda/Clases` (20 clases finales, incluida Carne) | paquete Dominio | MAPEADO |
| `Bib_Hacienda/Interfaces` (15 interfaces) | paquete Contratos | MAPEADO |
| `Clases/Validaciones`, `Eventos`, `Reglas`, `enum` | Validación, eventos y reglas | MAPEADO |
| `p_mvcHacienda/Servicios` | servicios, PersistenciaService y ProductoPersistido | MAPEADO |
| `p_mvcHacienda/Infrastructure` | dos interceptores | MAPEADO |
| `p_mvcHacienda/Controllers` | siete controladores | MAPEADO |
| `p_mvcHacienda/Models` | dos view models | MAPEADO |
| `p_mvcHacienda/Program.cs` | raíz de composición | MAPEADO |
| `HaciendaNEW.Demo`, `HaciendaNEW.Verification`, `03-src/phase4/Characterization` | ejecutables de demo/evidencia, no producción del sistema | NO APLICA |
| `Properties/AssemblyInfo.cs`, `bin/`, `obj/` | metadatos/generados | NO APLICA |

No quedan clases o interfaces productivas relevantes sin representación ni elementos normativos sin código.
