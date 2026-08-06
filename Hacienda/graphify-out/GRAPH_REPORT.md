# Graph Report - .  (2026-08-05)

## Corpus Check
- 237 files · ~209,618 words
- Verdict: corpus is large enough that graph structure adds value.

## Summary
- 662 nodes · 880 edges · 100 communities (71 shown, 29 thin omitted)
- Extraction: 88% EXTRACTED · 12% INFERRED · 0% AMBIGUOUS · INFERRED: 103 edges (avg confidence: 0.83)
- Token cost: 0 input · 0 output

## Community Hubs (Navigation)
- Hacienda Domain Contracts
- NuGet Package Documentation
- MVC Controllers and Models
- User Authentication Management
- Vaccine Management Workflow
- ASP.NET Launch Configuration
- SOLID C# Skills
- SOLID Engineering Documentation
- Hacienda Data and Documentation
- Pasture Management Workflow
- SOLID Refactoring Agents
- Ranch Operations and Vaccination
- Interceptors and App Composition
- Sales Controller and Service
- Pasture Livestock Events
- Cattle Controller and Service
- Sales Validation and Dates
- File Persistence and Cattle View
- jQuery Unobtrusive Validation
- Cattle Validation Implementation
- Vaccine Validation Implementation
- Sales Validation Implementation
- SOLID Engineering Rules
- Base Domain Validation
- Pasture Validation Implementation
- Cattle Type Hierarchy
- Minified Unobtrusive Validation
- SOLID Toolkit Manifest
- Architecture Audit Tooling
- MVC Project Dependencies
- DIP Audit Tooling
- ISP Audit Tooling
- LSP Audit Tooling
- OCP Audit Tooling
- SRP Audit Tooling
- System Memory Third-Party Notices
- System Numerics Third-Party Notices
- Unsafe Package Third-Party Notices
- Text Encoding Third-Party Notices
- Tasks Extensions Third-Party Notices
- ValueTuple Third-Party Notices
- Pasture Index View
- Source Agricultural Technology Image
- Vaccine Application View
- Public Agricultural Technology Image
- Public Cattle Pasture Image
- Validation Source Files
- Castle Core Logo
- Cattle Vaccine Detail View
- User Index View
- Vaccine Index View
- Sales Index View
- Source Pasture Image
- Source Cattle Image
- Pasture Details View
- Public Pasture Image
- Toolkit Installation Script
- Codebase Mapping Agent
- Async Interfaces Package Icon
- Options Package Icon
- System Buffers Package Icon
- System Memory License
- Numerics Vectors License
- Unsafe Runtime License
- Text Encodings Web License
- Tasks Extensions License
- ValueTuple Package License
- LSP Foundations Documentation
- MVC View Imports
- Account Login View
- Pasture Creation View
- Bib Hacienda Project
- Castle Apache License
- Castle DictionaryAdapter Documentation
- Async Interfaces Platform Logo
- Dependency Injection Package Logo
- Extensions Primitives Package Logo
- System Memory Package Logo
- Numerics Vectors Package Logo

## God Nodes (most connected - your core abstractions)
1. `Bib_Hacienda.Clases` - 33 edges
2. `Potrero` - 28 edges
3. `PersistenciaService` - 28 edges
4. `List<` - 27 edges
5. `Hacienda` - 26 edges
6. `Res` - 19 edges
7. `Vacuna` - 15 edges
8. `Bib_Hacienda.Reglas` - 14 edges
9. `Venta` - 12 edges
10. `p_mvcHacienda.Servicios` - 12 edges

## Surprising Connections (you probably didn't know these)
- `Separation of Engineering Stages` --semantically_similar_to--> `One Writer Policy`  [INFERRED] [semantically similar]
  README.md → docs/solid/workflow.md
- `Stable Abstraction` --semantically_similar_to--> `Behavior-Preserving SOLID Changes`  [INFERRED] [semantically similar]
  docs/solid/anti-overengineering.md → README.md
- `PersistenciaService` --references--> `InterceptorValidarInformacion`  [EXTRACTED]
  p_mvcHacienda/Servicios/PersistenciaService.cs → Bib_Hacienda/Bib_Hacienda/Aspectos/InterceptorValidarInformacion.cs
- `Autenticacion` --references--> `List<`  [EXTRACTED]
  Bib_Hacienda/Bib_Hacienda/Clases/Autenticacion.cs → p_mvcHacienda/Views/Res/Index.cshtml
- `Hacienda` --references--> `List<`  [EXTRACTED]
  Bib_Hacienda/Bib_Hacienda/Clases/Hacienda.cs → p_mvcHacienda/Views/Res/Index.cshtml

## Import Cycles
- None detected.

## Hyperedges (group relationships)
- **Independent SOLID Audit Team** — _opencode_agents_codebase_cartographer_codebase_cartographer, _opencode_agents_srp_auditor_srp_auditor, _opencode_agents_ocp_auditor_ocp_auditor, _opencode_agents_lsp_auditor_lsp_auditor, _opencode_agents_isp_auditor_isp_auditor, _opencode_agents_dip_auditor_dip_auditor, _opencode_agents_architecture_auditor_architecture_auditor [EXTRACTED 1.00]
- **Approved Refactor Delivery Chain** — _opencode_agents_refactor_planner_refactor_planner, _opencode_agents_refactor_implementer_refactor_implementer, _opencode_agents_test_guardian_test_guardian, _opencode_agents_adversarial_reviewer_adversarial_reviewer [EXTRACTED 1.00]
- **Familia de auditoría SOLID** — _opencode_skills_solid_analysis_protocol_skill_solid_analysis_protocol, _opencode_skills_solid_dip_csharp_skill_dependency_inversion_principle, _opencode_skills_solid_isp_csharp_skill_interface_segregation_principle, _opencode_skills_solid_lsp_csharp_skill_liskov_substitution_principle, _opencode_skills_solid_ocp_csharp_skill_open_closed_principle, _opencode_skills_solid_srp_csharp_skill_single_responsibility_principle, _opencode_skills_solid_reporting_skill_solid_reporting [EXTRACTED 1.00]
- **Flujo de refactorización que preserva conducta** — _opencode_skills_csharp_codebase_map_skill_csharp_codebase_map, _opencode_skills_solid_analysis_protocol_skill_solid_analysis_protocol, _opencode_skills_dotnet_refactor_verification_skill_dotnet_refactor_verification, _opencode_skills_behavior_preserving_refactor_csharp_skill_behavior_preserving_refactor_csharp, _opencode_skills_solid_reporting_skill_solid_reporting [INFERRED 0.85]
- **SOLID Refactoring Workflow** — agents_repository_discovery, agents_baseline_verification, agents_independent_solid_audits, agents_minimal_reversible_refactoring, agents_characterization_tests, agents_adversarial_review [EXTRACTED 1.00]
- **Options Pattern Capabilities** — bib_hacienda_bib_hacienda_packages_microsoft_extensions_options_8_0_2_package_strongly_typed_configuration, bib_hacienda_bib_hacienda_packages_microsoft_extensions_options_8_0_2_package_options_validation, bib_hacienda_bib_hacienda_packages_microsoft_extensions_options_8_0_2_package_options_validation_source_generator, bib_hacienda_bib_hacienda_packages_microsoft_extensions_options_8_0_2_package_options_monitoring_and_caching [EXTRACTED 1.00]
- **Extensions Primitive Types** — bib_hacienda_bib_hacienda_packages_microsoft_extensions_primitives_8_0_0_package_ichangetoken, bib_hacienda_bib_hacienda_packages_microsoft_extensions_primitives_8_0_0_package_stringvalues, bib_hacienda_bib_hacienda_packages_microsoft_extensions_primitives_8_0_0_package_stringsegment [EXTRACTED 1.00]
- **SOLID Toolkit Components** — manifest_specialized_agents, manifest_solid_commands, manifest_csharp_skills, manifest_solid_documentation, manifest_installation_and_validation_scripts [EXTRACTED 1.00]
- **Five SOLID Principles** — docs_solid_source_alignment_single_responsibility_principle, docs_solid_source_alignment_open_closed_principle, docs_solid_source_alignment_liskov_substitution_principle, docs_solid_source_alignment_interface_segregation_principle, docs_solid_source_alignment_dependency_inversion_principle [EXTRACTED 1.00]
- **Independent SOLID Audit Pipeline** — docs_solid_workflow_multiagent_workflow, docs_solid_workflow_independent_solid_audits, docs_solid_report_template_cross_principle_consolidation, docs_solid_workflow_one_writer_policy, docs_solid_report_template_adversarial_review [EXTRACTED 1.00]
- **Hacienda Domain Modules** — p_mvchacienda_readme_pasture_management, p_mvchacienda_readme_cattle_management, p_mvchacienda_readme_vaccine_management, p_mvchacienda_readme_sales_management, p_mvchacienda_readme_user_access_management [EXTRACTED 1.00]
- **Mechanized Farming Scene** — p_mvchacienda_imagenes_tecnologia_farmer, p_mvchacienda_imagenes_tecnologia_tractor, p_mvchacienda_imagenes_tecnologia_cultivated_field, p_mvchacienda_imagenes_tecnologia_agricultural_mechanization [INFERRED 0.95]
- **Mechanized Crop Cultivation** — p_mvchacienda_wwwroot_imagenes_tecnologia_farmer, p_mvchacienda_wwwroot_imagenes_tecnologia_tractor, p_mvchacienda_wwwroot_imagenes_tecnologia_cultivated_field [EXTRACTED 1.00]

## Communities (100 total, 29 thin omitted)

### Community 0 - "Hacienda Domain Contracts"
Cohesion: 0.08
Nodes (18): Bacteriana, uint, l_tipos_potreros, ICreacionVacuna, DateTime, enum_l_atenuaciones, IVentaRes, ReglaPotrero (+10 more)

### Community 1 - "NuGet Package Documentation"
Cohesion: 0.06
Nodes (35): Async Interfaces Runtime Dependency, Bib Hacienda Build, Castle Core Runtime Dependency, Dependency Injection Abstractions Runtime Dependency, Object Pool Runtime Dependency, Options Runtime Dependency, Extensions Primitives Runtime Dependency, System Buffers Runtime Dependency (+27 more)

### Community 2 - "MVC Controllers and Models"
Cohesion: 0.08
Nodes (19): Claim, Controller, p_mvcHacienda.Controllers, p_mvcHacienda.Models, p_mvcHacienda.Servicios, IEnumerable, ILogger, HttpGet (+11 more)

### Community 3 - "User Authentication Management"
Cohesion: 0.09
Nodes (11): Autenticacion, Usuario, string, IAutenticacion, ActionResult, HttpGet, HttpPost, ValidateAntiForgeryToken (+3 more)

### Community 4 - "Vaccine Management Workflow"
Cohesion: 0.11
Nodes (14): DateTime, enum_l_atenuaciones, enum_l_atenuaciones, Viva, ActionResult, enum_l_atenuaciones, HttpGet, HttpPost (+6 more)

### Community 5 - "ASP.NET Launch Configuration"
Cohesion: 0.08
Nodes (25): ASPNETCORE_ENVIRONMENT, applicationUrl, commandName, dotnetRunMessages, environmentVariables, launchBrowser, applicationUrl, commandName (+17 more)

### Community 6 - "SOLID C# Skills"
Cohesion: 0.08
Nodes (25): Interfaz del agente Arquitectura C#, Arquitectura C#, Interfaz del agente Refactorización segura C#, Refactorización C# que preserva conducta, Interfaz del agente Mapa de código C#, Cartografía de código C#, Interfaz del agente Verificación .NET, Verificación .NET (+17 more)

### Community 7 - "SOLID Engineering Documentation"
Cohesion: 0.09
Nodes (23): Anti-Overengineering Gates, Policy Boundary, Stable Abstraction, Confirmed Finding, Regression Protection, SOLID Finding Schema, Adversarial Review, Codebase and Architecture Map (+15 more)

### Community 8 - "Hacienda Data and Documentation"
Cohesion: 0.12
Nodes (23): Pasture Records, Cattle Records, User Records, Vaccine Inventory Records, Applied Vaccine Records, Sales Records, Build Output Manifest, Static Web Assets Manifest (+15 more)

### Community 9 - "Pasture Management Workflow"
Cohesion: 0.13
Nodes (10): l_tipos_potreros, ActionResult, HttpGet, HttpPost, l_tipos_potreros, PotreroController, HttpPost, Dictionary (+2 more)

### Community 10 - "SOLID Refactoring Agents"
Cohesion: 0.10
Nodes (20): Adversarial Reviewer, Behavior-Preserving Refactor C#, .NET Refactor Verification, SOLID Analysis Protocol, Behavior-Preserving Refactor C#, .NET Refactor Verification, Refactor Implementer, Behavior-Preserving Refactor C# (+12 more)

### Community 11 - "Ranch Operations and Vaccination"
Cohesion: 0.16
Nodes (7): Hacienda, Vacuna, DateTime, string, PublisherVacunacionCompletada, PublisherVacunaVencida, IVacunacion

### Community 12 - "Interceptors and App Composition"
Cohesion: 0.13
Nodes (10): InterceptorAutenticacion, IHttpContextAccessor, IInvocation, InterceptorValidarInformacion, IHttpContextAccessor, IInvocation, Bib_Hacienda.Aspectos, p_mvcHacienda (+2 more)

### Community 13 - "Sales Controller and Service"
Cohesion: 0.19
Nodes (7): IFormCollection, ActionResult, HttpPost, ValidateAntiForgeryToken, VentaController, Dictionary, VentaService

### Community 14 - "Pasture Livestock Events"
Cohesion: 0.24
Nodes (6): Potrero, string, PublisherPesoMin, PublisherPesoVenta, PublisherPotreroLleno, PublisherPotreroMitad

### Community 15 - "Cattle Controller and Service"
Cohesion: 0.26
Nodes (5): ActionResult, HttpGet, ResController, Dictionary, ResService

### Community 16 - "Sales Validation and Dates"
Cohesion: 0.17
Nodes (5): Venta, DateTime, uint, IValidarInformacion, DateTime

### Community 17 - "File Persistence and Cattle View"
Cohesion: 0.29
Nodes (4): IHttpContextAccessor, string, PersistenciaService, List<

### Community 18 - "jQuery Unobtrusive Validation"
Cohesion: 0.22
Nodes (4): escapeAttributeValue(), onError(), onReset(), validationInfo()

### Community 19 - "Cattle Validation Implementation"
Cohesion: 0.20
Nodes (5): ValidadorRes, Potrero, Res, Vacuna, Venta

### Community 20 - "Vaccine Validation Implementation"
Cohesion: 0.20
Nodes (5): ValidadorVacuna, Potrero, Res, Vacuna, Venta

### Community 21 - "Sales Validation Implementation"
Cohesion: 0.20
Nodes (5): ValidadorVenta, Potrero, Res, Vacuna, Venta

### Community 22 - "SOLID Engineering Rules"
Cohesion: 0.22
Nodes (9): Adversarial Review, Baseline Verification, Behavioral Substitutability, Characterization Tests, Independent SOLID Audits, Minimal Reversible Refactoring, Repository Discovery, SOLID C# Engineering (+1 more)

### Community 23 - "Base Domain Validation"
Cohesion: 0.22
Nodes (5): Validacion, Potrero, Res, Vacuna, Venta

### Community 24 - "Pasture Validation Implementation"
Cohesion: 0.22
Nodes (5): ValidadorPotrero, Potrero, Res, Vacuna, Venta

### Community 25 - "Cattle Type Hierarchy"
Cohesion: 0.25
Nodes (7): Cebon, Novillo, Res, string, uint, ushort, Ternero

### Community 26 - "Minified Unobtrusive Validation"
Cohesion: 0.38
Nodes (3): f(), p(), u()

### Community 27 - "SOLID Toolkit Manifest"
Cohesion: 0.33
Nodes (6): C# Architecture and SOLID Skills, Installation and Validation Scripts, SOLID Audit and Refactoring Commands, SOLID Engineering Documentation, SOLID C# Engineering Toolkit, Specialized Audit and Refactoring Agents

### Community 28 - "Architecture Audit Tooling"
Cohesion: 0.40
Nodes (5): Architecture Auditor, Architecture C#, C# Codebase Map, SOLID Analysis Protocol, Architecture Audit Command

### Community 29 - "MVC Project Dependencies"
Cohesion: 0.40
Nodes (5): net8.0, Castle.Core (5.2.1), Microsoft.AspNetCore.Http (2.3.0), p_mvcHacienda, Microsoft.NET.Sdk.Web

### Community 30 - "DIP Audit Tooling"
Cohesion: 0.50
Nodes (4): DIP Auditor, SOLID Analysis Protocol, SOLID DIP C#, SOLID DIP Command

### Community 31 - "ISP Audit Tooling"
Cohesion: 0.50
Nodes (4): ISP Auditor, SOLID Analysis Protocol, SOLID ISP C#, SOLID ISP Command

### Community 32 - "LSP Audit Tooling"
Cohesion: 0.50
Nodes (4): LSP Auditor, SOLID Analysis Protocol, SOLID LSP C#, SOLID LSP Command

### Community 33 - "OCP Audit Tooling"
Cohesion: 0.50
Nodes (4): OCP Auditor, SOLID Analysis Protocol, SOLID OCP C#, SOLID OCP Command

### Community 34 - "SRP Audit Tooling"
Cohesion: 0.50
Nodes (4): SOLID Analysis Protocol, SOLID SRP C#, SRP Auditor, SOLID SRP Command

### Community 35 - "System Memory Third-Party Notices"
Cohesion: 0.50
Nodes (4): .NET Core Third-Party Resources, Slicing-by-8, Unicode Data, zlib

### Community 36 - "System Numerics Third-Party Notices"
Cohesion: 0.50
Nodes (4): .NET Core Third-Party Resources, Slicing-by-8, Unicode Data, zlib

### Community 37 - "Unsafe Package Third-Party Notices"
Cohesion: 0.50
Nodes (4): ASP.NET, Berkeley SoftFloat Release 3e, .NET Runtime Third-Party Resources, Json.NET

### Community 38 - "Text Encoding Third-Party Notices"
Cohesion: 0.50
Nodes (4): .NET Runtime Third-Party Resources, Euclidean Affine Functions and Applications to Calendar Algorithms, FastFloat Algorithm, RFC 3492 Punycode

### Community 39 - "Tasks Extensions Third-Party Notices"
Cohesion: 0.50
Nodes (4): .NET Core Third-Party Resources, Slicing-by-8, Unicode Data, zlib

### Community 40 - "ValueTuple Third-Party Notices"
Cohesion: 0.50
Nodes (4): .NET Core Third-Party Resources, Slicing-by-8, Unicode Data, zlib

### Community 41 - "Pasture Index View"
Cohesion: 0.50
Nodes (3): List<Potrero>, Potrero, static

### Community 42 - "Source Agricultural Technology Image"
Cohesion: 0.67
Nodes (4): Agricultural Mechanization, Cultivated Field, Farmer, Tractor

### Community 43 - "Vaccine Application View"
Cohesion: 0.50
Nodes (3): Bib_Hacienda.Clases, Potrero, Vacuna

### Community 44 - "Public Agricultural Technology Image"
Cohesion: 0.67
Nodes (4): Agricultural Technology, Cultivated Field, Farmer, Tractor

### Community 45 - "Public Cattle Pasture Image"
Cohesion: 0.67
Nodes (4): Adult Cow, Calf, Cow and Calf Photograph, Pasture

### Community 47 - "Castle Core Logo"
Cohesion: 1.00
Nodes (3): Castle.Core Brand Identity, Castle Logo, Stylized Castle Silhouette

### Community 52 - "Source Pasture Image"
Cohesion: 0.67
Nodes (3): Cultivated Pasture, Forage Grass, Tree Border

### Community 53 - "Source Cattle Image"
Cohesion: 1.00
Nodes (3): Adult Cow, Calf, Zebu-Type Cattle

### Community 55 - "Public Pasture Image"
Cohesion: 0.67
Nodes (3): Dense Forage Grass, Pasture Landscape, Tree Border

## Knowledge Gaps
- **174 isolated node(s):** `Bib_Hacienda`, `l_tipos_potreros`, `enum_l_atenuaciones`, `ErrorViewModel`, `p_mvcHacienda` (+169 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **29 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `PersistenciaService` connect `File Persistence and Cattle View` to `User Authentication Management`, `Vaccine Management Workflow`, `Pasture Management Workflow`, `Interceptors and App Composition`, `Sales Controller and Service`, `Cattle Controller and Service`, `Sales Validation and Dates`, `Cattle Validation Implementation`, `Vaccine Validation Implementation`, `Sales Validation Implementation`, `Pasture Validation Implementation`?**
  _High betweenness centrality (0.075) - this node is a cross-community bridge._
- **Why does `Hacienda` connect `Ranch Operations and Vaccination` to `Hacienda Domain Contracts`, `Vaccine Management Workflow`, `Pasture Management Workflow`, `Sales Controller and Service`, `Pasture Livestock Events`, `Cattle Controller and Service`, `Sales Validation and Dates`, `File Persistence and Cattle View`?**
  _High betweenness centrality (0.039) - this node is a cross-community bridge._
- **Why does `Bib_Hacienda.Clases` connect `Hacienda Domain Contracts` to `MVC Controllers and Models`, `User Authentication Management`, `Vaccine Management Workflow`, `Pasture Management Workflow`, `Ranch Operations and Vaccination`, `Interceptors and App Composition`, `Sales Controller and Service`, `Cattle Controller and Service`, `Sales Validation and Dates`?**
  _High betweenness centrality (0.037) - this node is a cross-community bridge._
- **What connects `Bib_Hacienda`, `l_tipos_potreros`, `enum_l_atenuaciones` to the rest of the system?**
  _174 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Hacienda Domain Contracts` be split into smaller, more focused modules?**
  _Cohesion score 0.07682926829268293 - nodes in this community are weakly interconnected._
- **Should `NuGet Package Documentation` be split into smaller, more focused modules?**
  _Cohesion score 0.058823529411764705 - nodes in this community are weakly interconnected._
- **Should `MVC Controllers and Models` be split into smaller, more focused modules?**
  _Cohesion score 0.0773109243697479 - nodes in this community are weakly interconnected._