using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Bib_Hacienda.Clases;
using Bib_Hacienda.Clases.Validaciones;
using Bib_Hacienda.Interfaces;
using Bib_Hacienda.Reglas;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using p_mvcHacienda.Servicios;
using static Bib_Hacienda.Clases.Potrero;

namespace HaciendaNEW.Verification
{
    internal class Program
    {
        private static int _fallos = 0;

        private static void Main(string[] args)
        {
            Console.WriteLine("=== HaciendaNEW.Verification ===");

            VerificarAssemblySource();
            VerificarProductoRes();
            VerificarConstructoresVenta();
            VerificarVentaLegacy();
            VerificarValidadores();
            VerificarPuertosPersistencia();
            VerificarBibHaciendaSinDependenciasTecnicas();
            VerificarControladoresNoDependenDeDominioPersistencia();
            VerificarContratoVendibleEstrecho();
            VerificarPotreroAgregarReal();
            VerificarInventariosSinDuplicadoNiNoOp();
            VerificarVentaGenericaRes();
            VerificarVentaGenericaLacteo();
            VerificarVentaGenericaPiel();
            VerificarVentaProductoDefinidoEnVerifier();
            VerificarVentaProductoAusenteONull();
            VerificarContratoRes();
            VerificarPersistenciaVentas();
            VerificarVentaServicePorPotrero();
            VerificarIndexVentaSinNulos();
            VerificarPersistenciaVentasLegacyAbortaNumeroMalformado();
            VerificarRegistroVentaCargada();
            VerificarComposicionValidadores();

            if (_fallos == 0)
            {
                Console.WriteLine("TODAS LAS VERIFICACIONES PASARON.");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine($"FALLARON {_fallos} VERIFICACIONES.");
                Environment.Exit(1);
            }
        }

        private static void VerificarAssemblySource()
        {
            var location = typeof(Venta).Assembly.Location;
            Assert(location.Contains("redisenado/HaciendaNEW") || location.Contains("redisenado\\HaciendaNEW"),
                $"El assembly debe provenir del source NEW: {location}");
            Assert(!location.Contains("original/HaciendaOLD") && !location.Contains("original\\HaciendaOLD"),
                $"El assembly NO debe provenir del source OLD: {location}");
            Console.WriteLine("[OK] Assembly source apunta a NEW.");
        }

        private static void VerificarProductoRes()
        {
            var res = new Ternero("Lola", 100, 5);
            Assert(res.Nombre == "Lola", "Nombre de la res debe ser 'Lola'.");

            res.Nombre = "LolaActualizada";
            Assert(res.Nombre == "LolaActualizada", "Setter público de Nombre debe funcionar.");

            try
            {
                res.Nombre = "";
                Fail("Setter de Nombre debe rechazar valores vacíos.");
            }
            catch (ArgumentException)
            {
                // comportamiento esperado
            }

            try
            {
                var _ = new Ternero("", 100, 5);
                Fail("Constructor de Res debe rechazar nombres vacíos vía base.");
            }
            catch (ArgumentException)
            {
                // comportamiento esperado
            }

            Console.WriteLine("[OK] Producto/Res integridad y validación.");
        }

        private static void VerificarConstructoresVenta()
        {
            var ahora = new DateTime(2026, 8, 9, 20, 0, 0, DateTimeKind.Local);
            var lacteo = new Lacteo("Leche entera");

            var ventaToBe = new Venta(ahora, lacteo, 500);
            Assert(ventaToBe.Fecha == ahora, "Venta TO-BE conserva Fecha.");
            Assert(ventaToBe.Producto == lacteo, "Venta TO-BE conserva Producto.");
            Assert(ventaToBe.Monto == 500, "Venta TO-BE conserva Monto.");
            Assert(ventaToBe.Potrero == null && ventaToBe.Res == null,
                "Venta TO-BE no tiene Potrero/Res.");

            var potrero = new Potrero("P1", l_tipos_potreros.ternero);
            var res = new Ternero("Lola", 100, 5);
            var ventaLegacy = new Venta(potrero, ahora, res, 750);
            Assert(ventaLegacy.Potrero == potrero, "Venta legacy conserva Potrero.");
            Assert(ventaLegacy.Res == res, "Venta legacy conserva Res.");
            Assert(ventaLegacy.Producto == res, "Venta legacy expone Res como Producto.");
            Assert(ventaLegacy.Fecha == ahora, "Venta legacy conserva Fecha.");
            Assert(ventaLegacy.Monto == 750, "Venta legacy conserva Monto.");

            Console.WriteLine("[OK] Constructores Venta TO-BE y legacy.");
        }

        private static void VerificarVentaLegacy()
        {
            var hacienda = new Hacienda();
            hacienda.crear_potrero("P1", l_tipos_potreros.ternero);
            hacienda.anadir_res_potrero("P1", "Lola", 5, 100);

            var potrero = hacienda.L_potreros[0];
            Assert(potrero.L_reses.Count == 1, "Potrero debe contener la res antes de vender.");

            string mensaje = hacienda.vender_res("P1", "Lola", 1200);
            Assert(mensaje == "Venta de la res Lola realizada con exito",
                $"Mensaje legacy exacto. Obtenido: '{mensaje}'");

            Assert(potrero.L_reses.Count == 0, "La res debe haber sido retirada del potrero.");

            var ventas = hacienda.L_ventas;
            Assert(ventas.Count == 1, "Debe existir una venta registrada.");
            var venta = ventas[0];
            Assert(venta.Potrero.Identificacion == "P1", "Venta registrada con potrero P1.");
            Assert(venta.Res.Nombre == "Lola", "Venta registrada con res Lola.");
            Assert(venta.Monto == 1200, "Venta registrada con monto 1200.");

            Console.WriteLine("[OK] Venta legacy: retiro y registro exactos.");
        }

        private static void VerificarValidadores()
        {
            VerificarValidadorRes();
            VerificarValidadorPotrero();
            VerificarValidadorVacuna();
            VerificarValidadorVenta();
            VerificarAusenciaMetodosAjenosYNotImplemented();
        }

        private static void VerificarValidadorRes()
        {
            var validador = new ValidadorRes();
            var valida = new Ternero("Lola", 100, 5);

            Assert(validador.ValidarRes(valida), "ValidadorRes acepta res válida.");
            Assert(!validador.ValidarRes(null), "ValidadorRes rechaza null.");
            Assert(!validador.ValidarRes(new Ternero("Lola", 0, 5)), "ValidadorRes rechaza peso 0.");
            Assert(!validador.ValidarRes(new Ternero("Lola", 100, 0)), "ValidadorRes rechaza edad 0.");

            Console.WriteLine("[OK] ValidadorRes.");
        }

        private static void VerificarValidadorPotrero()
        {
            var validador = new ValidadorPotrero();
            var valida = new Potrero("P1", l_tipos_potreros.ternero);

            Assert(validador.ValidarPotrero(valida), "ValidadorPotrero acepta potrero válido.");
            Assert(!validador.ValidarPotrero(null), "ValidadorPotrero rechaza null.");
            Assert(!validador.ValidarPotrero(new Potrero("", l_tipos_potreros.ternero)), "ValidadorPotrero rechaza identificación vacía.");

            Console.WriteLine("[OK] ValidadorPotrero.");
        }

        private static void VerificarValidadorVacuna()
        {
            var validador = new ValidadorVacuna();
            var valida = new Bacteriana("VacunaA", "Lote1", DateTime.Now.AddMonths(1), DateTime.Now, 4);

            Assert(validador.ValidarVacuna(valida), "ValidadorVacuna acepta vacuna válida.");
            Assert(!validador.ValidarVacuna(null), "ValidadorVacuna rechaza null.");
            Assert(!validador.ValidarVacuna(new Bacteriana("", "Lote1", DateTime.Now.AddMonths(1), DateTime.Now, 4)), "ValidadorVacuna rechaza nombre vacío.");
            Assert(!validador.ValidarVacuna(new Bacteriana("VacunaA", "", DateTime.Now.AddMonths(1), DateTime.Now, 4)), "ValidadorVacuna rechaza lote vacío.");

            Console.WriteLine("[OK] ValidadorVacuna.");
        }

        private static void VerificarValidadorVenta()
        {
            var validador = new ValidadorVenta();
            var potrero = new Potrero("P1", l_tipos_potreros.ternero);
            var res = new Ternero("Lola", 100, 5);

            var legacy = new Venta(potrero, DateTime.Now, res, 100);
            Assert(validador.ValidarVenta(legacy), "ValidadorVenta acepta venta legacy.");

            var toBe = new Venta(DateTime.Now, new Lacteo("Queso"), 200);
            Assert(validador.ValidarVenta(toBe), "ValidadorVenta acepta venta TO-BE.");

            Assert(!validador.ValidarVenta(null), "ValidadorVenta rechaza null.");
            Assert(!validador.ValidarVenta(new Venta(DateTime.Now, null, 0)), "ValidadorVenta rechaza monto 0 y sin producto.");
            Assert(!validador.ValidarVenta(new Venta(DateTime.Now, (Producto)null, 100)), "ValidadorVenta rechaza venta sin producto ni Potrero/Res.");

            Console.WriteLine("[OK] ValidadorVenta.");
        }

        private static void VerificarAusenciaMetodosAjenosYNotImplemented()
        {
            var validadores = new (Type Tipo, string MetodoPropio, string[] MetodosAjenos)[]
            {
                (typeof(ValidadorRes), "ValidarRes", new[] { "ValidarPotrero", "ValidarVacuna", "ValidarVenta" }),
                (typeof(ValidadorPotrero), "ValidarPotrero", new[] { "ValidarRes", "ValidarVacuna", "ValidarVenta" }),
                (typeof(ValidadorVacuna), "ValidarVacuna", new[] { "ValidarRes", "ValidarPotrero", "ValidarVenta" }),
                (typeof(ValidadorVenta), "ValidarVenta", new[] { "ValidarRes", "ValidarPotrero", "ValidarVacuna" }),
            };

            foreach (var (tipo, metodoPropio, metodosAjenos) in validadores)
            {
                var metodosPublicos = tipo.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(m => !m.IsSpecialName)
                    .Select(m => m.Name)
                    .ToList();

                Assert(metodosPublicos.Contains(metodoPropio),
                    $"{tipo.Name} debe exponer {metodoPropio}.");

                foreach (var ajeno in metodosAjenos)
                {
                    Assert(!metodosPublicos.Contains(ajeno),
                        $"{tipo.Name} NO debe exponer método ajeno {ajeno}.");
                }

                // Ningún método declarado debe lanzar NotImplementedException
                foreach (var metodo in tipo.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(m => !m.IsSpecialName))
                {
                    try
                    {
                        object instancia = Activator.CreateInstance(tipo);
                        object[] parametros = metodo.GetParameters()
                            .Select(p => p.ParameterType.IsValueType ? Activator.CreateInstance(p.ParameterType) : null)
                            .ToArray();
                        metodo.Invoke(instancia, parametros);
                    }
                    catch (TargetInvocationException tie) when (tie.InnerException is NotImplementedException)
                    {
                        Fail($"{tipo.Name}.{metodo.Name} lanza NotImplementedException.");
                    }
                    catch
                    {
                        // Otros errores (null, etc.) son esperados y aceptables para esta comprobación
                    }
                }
            }

            Console.WriteLine("[OK] Validadores sin métodos ajenos ni NotImplementedException.");
        }

        private static void VerificarPuertosPersistencia()
        {
            var interfacesEsperadas = new[]
            {
                typeof(IPersistenciaPotreros),
                typeof(IPersistenciaReses),
                typeof(IPersistenciaVacunas),
                typeof(IPersistenciaVentas),
                typeof(IPersistenciaUsuarios)
            };

            var implementadas = typeof(PersistenciaService).GetInterfaces();

            foreach (var esperada in interfacesEsperadas)
            {
                Assert(implementadas.Contains(esperada),
                    $"PersistenciaService debe implementar {esperada.Name}.");
            }

            Console.WriteLine("[OK] PersistenciaService implementa los puertos de persistencia.");
        }

        private static void VerificarBibHaciendaSinDependenciasTecnicas()
        {
            var bibAssembly = typeof(Venta).Assembly;
            var referencias = bibAssembly.GetReferencedAssemblies();

            Assert(!referencias.Any(r => r.Name.Contains("Castle")),
                "Bib_Hacienda no debe referenciar Castle.");
            Assert(!referencias.Any(r => r.Name.Contains("AspNet")),
                "Bib_Hacienda no debe referenciar AspNet.");

            Console.WriteLine("[OK] Bib_Hacienda sin dependencias técnicas de infraestructura.");
        }

        private static void VerificarControladoresNoDependenDeDominioPersistencia()
        {
            var assemblyMvc = typeof(PersistenciaService).Assembly;
            var controladores = assemblyMvc.GetTypes()
                .Where(t => t.IsPublic && t.Name.EndsWith("Controller"))
                .ToList();

            var tiposProhibidos = new[] { typeof(Hacienda), typeof(PersistenciaService) };

            foreach (var controlador in controladores)
            {
                var constructores = controlador.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
                foreach (var ctor in constructores)
                {
                    foreach (var param in ctor.GetParameters())
                    {
                        Assert(!tiposProhibidos.Contains(param.ParameterType),
                            $"{controlador.Name} no debe depender de {param.ParameterType.Name}.");
                    }
                }
            }

            Console.WriteLine("[OK] Controladores no dependen directamente de Hacienda ni PersistenciaService.");
        }

        private static void VerificarContratoVendibleEstrecho()
        {
            var metodo = typeof(Hacienda).GetMethods()
                .Single(m => m.Name == "vender"
                    && m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IInventarioVendible<>));
            Assert(metodo != null, "Hacienda debe exponer el método genérico vender<T>.");

            var parametros = metodo.GetParameters();
            Assert(parametros.Length == 3, "vender<T> debe recibir inventario, producto y monto.");
            Assert(parametros[0].ParameterType.GetGenericTypeDefinition() == typeof(IInventarioVendible<>),
                "vender<T> debe depender de IInventarioVendible<T>, no del contrato ancho.");

            Assert(typeof(IInventario<>).GetInterfaces().Any(i => i.GetGenericTypeDefinition() == typeof(IInventarioVendible<>)),
                "IInventario<T> debe heredar de IInventarioVendible<T>.");

            Console.WriteLine("[OK] Contrato vendible estrecho separado del contrato completo.");
        }

        private static void VerificarPotreroAgregarReal()
        {
            var potrero = new Potrero("P1", l_tipos_potreros.ternero);
            var resValida = new Ternero("Lola", 100, 5);

            potrero.agregar(resValida);
            Assert(potrero.L_reses.Count == 1, "agregar debe añadir la res al potrero.");
            Assert(potrero.contiene(resValida), "contiene debe encontrar la res recién agregada.");

            try
            {
                potrero.agregar(null);
                Fail("agregar debe rechazar null.");
            }
            catch (ArgumentNullException)
            {
                // esperado
            }

            try
            {
                potrero.agregar(new Ternero("Lola", 120, 6));
                Fail("agregar debe rechazar res duplicada por nombre.");
            }
            catch (InvalidOperationException)
            {
                // esperado
            }

            try
            {
                potrero.agregar(new Novillo("Toro", 400, 60));
                Fail("agregar debe rechazar res cuya edad no corresponde al tipo de potrero.");
            }
            catch (InvalidOperationException)
            {
                // esperado
            }

            // Llenar el potrero al máximo para probar capacidad
            var potreroLleno = new Potrero("P2", l_tipos_potreros.ternero);
            for (int i = 0; i < 150; i++)
            {
                potreroLleno.agregar(new Ternero($"Res{i}", 100, (ushort)(i % 12 + 1)));
            }
            try
            {
                potreroLleno.agregar(new Ternero("Extra", 100, 5));
                Fail("agregar debe rechazar res cuando el potrero está lleno.");
            }
            catch (InvalidOperationException)
            {
                // esperado
            }

            Console.WriteLine("[OK] Potrero.agregar con semántica real de capacidad/tipo/duplicado.");
        }

        private static void VerificarInventariosSinDuplicadoNiNoOp()
        {
            var lacteos = new InventarioLacteos();
            var leche = new Lacteo("Leche entera");

            lacteos.agregar(leche);
            Assert(lacteos.contiene(leche), "InventarioLacteos contiene el lácteo agregado.");
            Assert(!lacteos.contiene(null), "contiene(null) devuelve false.");

            try
            {
                lacteos.agregar(new Lacteo("Leche entera"));
                Fail("InventarioLacteos debe rechazar duplicado por nombre.");
            }
            catch (Exception ex) when (ex.InnerException is InvalidOperationException)
            {
                // esperado
            }

            try
            {
                lacteos.agregar(null);
                Fail("InventarioLacteos debe rechazar agregar null.");
            }
            catch (Exception ex) when (ex.InnerException is ArgumentNullException)
            {
                // esperado
            }

            try
            {
                lacteos.retirar(new Lacteo("Leche descremada"));
                Fail("retirar lácteo inexistente debe lanzar excepción (no no-op).");
            }
            catch (Exception ex) when (ex.InnerException is InvalidOperationException)
            {
                // esperado
            }

            var pieles = new InventarioPieles();
            var piel = new Piel("Cuero negro");
            pieles.agregar(piel);
            Assert(pieles.contiene(piel), "InventarioPieles contiene la piel agregada.");
            Assert(!pieles.contiene(null), "contiene(null) devuelve false en pieles.");

            try
            {
                pieles.agregar(new Piel("Cuero negro"));
                Fail("InventarioPieles debe rechazar duplicado por nombre.");
            }
            catch (Exception ex) when (ex.InnerException is InvalidOperationException)
            {
                // esperado
            }

            try
            {
                pieles.retirar(new Piel("Cuero marrón"));
                Fail("retirar piel inexistente debe lanzar excepción (no no-op).");
            }
            catch (Exception ex) when (ex.InnerException is InvalidOperationException)
            {
                // esperado
            }

            Console.WriteLine("[OK] Inventarios sin duplicados ni no-ops.");
        }

        private static void VerificarVentaGenericaRes()
        {
            var hacienda = new Hacienda();
            hacienda.crear_potrero("P1", l_tipos_potreros.ternero);
            var potrero = hacienda.L_potreros[0];
            var res = new Ternero("Lola", 100, 5);
            potrero.agregar(res);

            string mensaje = hacienda.vender(potrero, res, 1200u);
            Assert(mensaje.Contains("Lola"), "Mensaje de venta genérica menciona la res.");
            Assert(!potrero.contiene(res), "La res fue retirada del potrero.");

            var ventas = hacienda.L_ventas;
            Assert(ventas.Count == 1, "Debe registrarse una venta TO-BE.");
            Assert(ventas[0].Producto == res, "Venta TO-BE referencia la res como producto.");
            Assert(ventas[0].Monto == 1200, "Monto de venta genérica registrado.");
            Assert(ventas[0].Potrero == null && ventas[0].Res == null,
                "Venta genérica de Res no conserva Potrero/Res (formato TO-BE).");

            Console.WriteLine("[OK] Venta genérica de Res.");
        }

        private static void VerificarVentaGenericaLacteo()
        {
            var hacienda = new Hacienda();
            var inventario = new InventarioLacteos();
            var lacteo = new Lacteo("Queso");
            inventario.agregar(lacteo);

            string mensaje = hacienda.vender(inventario, lacteo, 500u);
            Assert(mensaje.Contains("Queso"), "Mensaje de venta genérica menciona el lácteo.");
            Assert(!inventario.contiene(lacteo), "El lácteo fue retirado del inventario.");
            Assert(hacienda.L_ventas.Count == 1, "Debe registrarse una venta de lácteo.");
            Assert(hacienda.L_ventas[0].Producto == lacteo, "Venta TO-BE referencia el lácteo.");

            Console.WriteLine("[OK] Venta genérica de Lacteo.");
        }

        private static void VerificarVentaGenericaPiel()
        {
            var hacienda = new Hacienda();
            var inventario = new InventarioPieles();
            var piel = new Piel("Cuero");
            inventario.agregar(piel);

            string mensaje = hacienda.vender(inventario, piel, 800u);
            Assert(mensaje.Contains("Cuero"), "Mensaje de venta genérica menciona la piel.");
            Assert(!inventario.contiene(piel), "La piel fue retirada del inventario.");
            Assert(hacienda.L_ventas.Count == 1, "Debe registrarse una venta de piel.");
            Assert(hacienda.L_ventas[0].Producto == piel, "Venta TO-BE referencia la piel.");

            Console.WriteLine("[OK] Venta genérica de Piel.");
        }

        private static void VerificarVentaProductoDefinidoEnVerifier()
        {
            var hacienda = new Hacienda();
            var inventario = new InventarioVerificador();
            var producto = new ProductoVerificador("ProductoExterno");
            inventario.agregar(producto);

            string mensaje = hacienda.vender(inventario, producto, 999u);
            Assert(mensaje.Contains("ProductoExterno"), "Venta de producto externo menciona su nombre.");
            Assert(!inventario.contiene(producto), "Producto externo retirado de su inventario.");
            Assert(hacienda.L_ventas.Count == 1, "Debe registrarse una venta del producto externo.");
            Assert(hacienda.L_ventas[0].Producto == producto, "Venta TO-BE referencia el producto externo.");

            Console.WriteLine("[OK] Venta genérica de Producto definido solo en el verifier.");
        }

        private static void VerificarVentaProductoAusenteONull()
        {
            var hacienda = new Hacienda();
            var inventario = new InventarioLacteos();
            var presente = new Lacteo("Leche");
            var ausente = new Lacteo("Yogur");
            inventario.agregar(presente);

            int ventasAntes = hacienda.L_ventas.Count;

            try
            {
                hacienda.vender(inventario, ausente, 100u);
                Fail("Vender producto ausente debe fallar.");
            }
            catch (InvalidOperationException)
            {
                // esperado
            }
            Assert(hacienda.L_ventas.Count == ventasAntes,
                "Producto ausente: no se debe registrar venta parcial.");
            Assert(inventario.contiene(presente), "Producto presente no debe verse afectado por venta fallida.");

            try
            {
                hacienda.vender(inventario, null, 100u);
                Fail("Vender producto null debe fallar.");
            }
            catch (ArgumentNullException)
            {
                // esperado
            }
            Assert(hacienda.L_ventas.Count == ventasAntes,
                "Producto null: no se debe registrar venta parcial.");

            Console.WriteLine("[OK] Venta rechaza producto ausente/null sin venta parcial.");
        }

        private static void VerificarContratoRes()
        {
            var subtipos = new[] { typeof(Ternero), typeof(Cebon), typeof(Novillo) };

            foreach (var tipo in subtipos)
            {
                var propiedadEdad = tipo.GetProperty("Edad");
                Assert(propiedadEdad != null, $"{tipo.Name} expone propiedad Edad.");
                Assert(propiedadEdad.GetSetMethod() == null, $"{tipo.Name}.Edad no tiene setter público.");
                Assert(!propiedadEdad.CanWrite, $"{tipo.Name}.Edad no es escribible.");

                var propiedadPeso = tipo.GetProperty("Peso");
                Assert(propiedadPeso != null && propiedadPeso.GetSetMethod() != null,
                    $"{tipo.Name}.Peso conserva setter público (Alimentar depende de él).");
            }

            // Rangos válidos por subtipo
            var ternero = new Ternero("T", 100, 12);
            Assert(ternero.Edad == 12, "Ternero acepta edad límite superior 12.");
            Assert(ternero.MaxVacunasBacterianas == ReglaVacuna.max_bac_ternero, "Ternero max bacterianas.");
            Assert(ternero.MaxVacunasVivas == ReglaVacuna.max_viv_ternero, "Ternero max vivas.");

            var cebon = new Cebon("C", 300, 13);
            Assert(cebon.Edad == 13, "Cebon acepta edad límite inferior 13.");
            var cebonMax = new Cebon("CMax", 400, 48);
            Assert(cebonMax.Edad == 48, "Cebon acepta edad límite superior 48.");

            var novillo = new Novillo("N", 500, 49);
            Assert(novillo.Edad == 49, "Novillo acepta edad límite inferior 49.");

            // Rangos inválidos
            try { var _ = new Ternero("T", 100, 13); Fail("Ternero debe rechazar edad 13."); }
            catch (Exception) { /* esperado */ }

            try { var _ = new Cebon("C", 300, 12); Fail("Cebon debe rechazar edad 12."); }
            catch (Exception) { /* esperado */ }

            try { var _ = new Cebon("C", 300, 49); Fail("Cebon debe rechazar edad 49."); }
            catch (Exception) { /* esperado */ }

            try { var _ = new Novillo("N", 500, 48); Fail("Novillo debe rechazar edad 48."); }
            catch (Exception) { /* esperado */ }

            // Alimentar modifica peso, no edad
            var res = new Ternero("T", 100, 5);
            res.Alimentar(10);
            Assert(res.Peso == 110, "Alimentar incrementa el peso.");
            Assert(res.Edad == 5, "Alimentar no modifica la edad.");

            try { res.Alimentar(0); Fail("Alimentar con 0 debe fallar."); }
            catch (Exception) { /* esperado */ }

            // Vacunación aplicable respetando máximos por subtipo
            var fecha = DateTime.Now.AddMonths(1);
            var vacunaBacteriana = new Bacteriana("BacA", "L1", fecha, DateTime.Now, 2);
            var vacunaViva = new Viva("VivA", "L2", fecha, DateTime.Now, Viva.enum_l_atenuaciones.Atenuacion10);

            ternero.aplicar_vacuna(vacunaBacteriana);
            Assert(ternero.CantidadVacunasBacterianas == 1, "Ternero acepta vacuna bacteriana.");
            Assert(vacunaBacteriana.PuedeAplicarseA(ternero),
                "Ternero aún admite más bacterianas (máximo 3).");

            var terneroViva = new Ternero("TV", 100, 5);
            terneroViva.aplicar_vacuna(vacunaViva);
            Assert(terneroViva.CantidadVacunasVivas == 1, "Ternero acepta vacuna viva.");
            Assert(!vacunaViva.PuedeAplicarseA(terneroViva),
                "Ternero alcanzó el máximo de vacunas vivas (1).");

            var cebonBac = new Cebon("CB", 300, 24);
            var vacunaBacterianaCebon = new Bacteriana("BacC", "L3", fecha, DateTime.Now, 3);
            cebonBac.aplicar_vacuna(vacunaBacterianaCebon);
            Assert(cebonBac.CantidadVacunasBacterianas == 1, "Cebon acepta vacuna bacteriana.");
            Assert(!vacunaBacterianaCebon.PuedeAplicarseA(cebonBac),
                "Cebon alcanzó el máximo de vacunas bacterianas (1).");

            Console.WriteLine("[OK] Contrato común Res/Ternero/Cebon/Novillo: inmutabilidad de Edad, rangos, Alimentar y vacunación.");
        }

        private static void VerificarPersistenciaVentas()
        {
            string tempRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempRoot);

            try
            {
                var env = new FakeWebHostEnvironment { ContentRootPath = tempRoot };
                var httpAccessor = new FakeHttpContextAccessor();

                var persistencia = new PersistenciaService(
                    env,
                    httpAccessor,
                    new ValidadorPotrero(),
                    new ValidadorRes(),
                    new ValidadorVacuna(),
                    new ValidadorVenta());

                var fecha = new DateTime(2026, 8, 10);
                var potrero = new Potrero("P1", l_tipos_potreros.ternero);
                var resLegacy = new Ternero("Lola", 120, 5);

                var ventas = new List<Venta>
                {
                    new Venta(potrero, fecha, resLegacy, 1000),
                    new Venta(fecha, new Lacteo("Queso"), 500),
                    new Venta(fecha, new Piel("Cuero"), 800),
                    new Venta(fecha, new Ternero("Lolita", 200, 6), 1200),
                    new Venta(fecha, new ProductoVerificador("ProductoExterno"), 999)
                };

                string resultado = persistencia.GuardarVentas(ventas);
                Assert(resultado == "Guardado exitosamente",
                    $"GuardarVentas debe reportar éxito. Obtenido: '{resultado}'");

                string rutaVentas = Path.Combine(tempRoot, "Datos", "Ventas.txt");
                Assert(File.Exists(rutaVentas), "Debe crearse Ventas.txt.");

                var lineas = File.ReadAllLines(rutaVentas);
                Assert(lineas.Length == 5, $"Deben persistirse 5 ventas, no {lineas.Length}.");

                var legacyLine = lineas.FirstOrDefault(l => !l.StartsWith("V2", StringComparison.OrdinalIgnoreCase));
                Assert(legacyLine != null, "Debe conservarse al menos una línea legacy.");
                Assert(legacyLine.Split('|').Length == 7,
                    "El registro legacy debe mantener la forma de 7 campos.");
                Assert(lineas.Count(l => l.StartsWith("V2", StringComparison.OrdinalIgnoreCase)) == 4,
                    "Las 4 ventas genéricas deben persistirse con prefijo V2.");

                var cargadas = persistencia.CargarVentas(new List<Potrero>());
                Assert(cargadas.Count == 5, $"Deben recargarse 5 ventas, no {cargadas.Count}.");

                var legacy = cargadas.FirstOrDefault(v => v.Potrero != null && v.Res != null);
                Assert(legacy != null, "Debe recargarse la venta legacy con Potrero/Res.");
                Assert(legacy.Res.Nombre == "Lola" && legacy.Monto == 1000,
                    "Venta legacy conserva nombre y monto.");

                var lacteo = cargadas.FirstOrDefault(v => v.Producto is Lacteo);
                Assert(lacteo != null && lacteo.Producto.Nombre == "Queso" && lacteo.Monto == 500,
                    "Venta genérica de Lacteo se recarga correctamente.");

                var piel = cargadas.FirstOrDefault(v => v.Producto is Piel);
                Assert(piel != null && piel.Producto.Nombre == "Cuero" && piel.Monto == 800,
                    "Venta genérica de Piel se recarga correctamente.");

                var resGenerica = cargadas.FirstOrDefault(v => v.Producto is Ternero t && t.Nombre == "Lolita");
                Assert(resGenerica != null && resGenerica.Monto == 1200,
                    "Venta genérica de Res se recarga correctamente.");

                var externa = cargadas.FirstOrDefault(v => v.Producto != null && v.Producto.Nombre == "ProductoExterno");
                Assert(externa != null && externa.Monto == 999,
                    "Venta de producto externo no se pierde y conserva su nombre.");
                Assert(externa.Producto.GetType().Name == "ProductoPersistido",
                    "Producto externo se recarga como snapshot estable sin modificar el servicio.");

                // Ninguna venta aceptada por el validador debe haberse omitido silenciosamente.
                var nombresAceptados = ventas.Select(v => v.Producto?.Nombre ?? v.Res?.Nombre).ToList();
                var nombresRecargados = cargadas.Select(v => v.Producto?.Nombre ?? v.Res?.Nombre).ToList();
                foreach (var nombre in nombresAceptados)
                {
                    Assert(nombresRecargados.Contains(nombre),
                        $"La venta aceptada de '{nombre}' no debe perderse en la persistencia.");
                }

                Console.WriteLine("[OK] Persistencia de ventas legacy y genéricas (incluido producto externo).");
            }
            finally
            {
                try { Directory.Delete(tempRoot, recursive: true); } catch { }
            }
        }

        private static void VerificarComposicionValidadores()
        {
            var ctor = typeof(PersistenciaService).GetConstructors().Single();
            var paramTypes = ctor.GetParameters().Select(p => p.ParameterType).ToList();

            Assert(!paramTypes.Any(t => t.Name == "IInterceptor"),
                "PersistenciaService no debe recibir IInterceptor; los interceptores se componen en Program.cs.");
            Assert(!paramTypes.Any(t => t.Name == "ProxyGenerator"),
                "PersistenciaService no debe recibir ProxyGenerator.");

            var fields = typeof(PersistenciaService).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            Assert(!fields.Any(f => f.FieldType.Name == "ProxyGenerator"),
                "PersistenciaService no debe contener un campo ProxyGenerator.");

            var methods = typeof(PersistenciaService).GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            Assert(!methods.Any(m => m.Name == "CreateInterfaceProxyWithTarget"),
                "PersistenciaService no debe invocar CreateInterfaceProxyWithTarget.");

            Console.WriteLine("[OK] Composición de validadores/interceptores centralizada en Program.cs.");
        }

        private static void VerificarVentaServicePorPotrero()
        {
            var hacienda = new Hacienda();
            hacienda.crear_potrero("P1", l_tipos_potreros.ternero);
            hacienda.crear_potrero("P2", l_tipos_potreros.ternero);
            hacienda.anadir_res_potrero("P1", "Lola", 5, 100);
            hacienda.anadir_res_potrero("P2", "Toro", 5, 100);

            hacienda.vender_res("P1", "Lola", 1200);
            hacienda.vender_res("P2", "Toro", 1300);

            var lacteos = new InventarioLacteos();
            var queso = new Lacteo("Queso");
            lacteos.agregar(queso);
            hacienda.vender(lacteos, queso, 500);

            var service = new VentaService(hacienda);

            var p1 = service.ObtenerVentasPorPotrero("P1");
            Assert(p1.Count == 1, "ObtenerVentasPorPotrero debe retornar exactamente la venta legacy de P1.");
            Assert(p1[0].Potrero != null && p1[0].Potrero.Identificacion == "P1",
                "La venta legacy conserva su potrero P1.");
            Assert(p1[0].Res != null && p1[0].Res.Nombre == "Lola",
                "La venta legacy conserva su res Lola.");

            var sinPotrero = service.ObtenerVentasPorPotrero("Generico");
            Assert(sinPotrero.Count == 0,
                "Las ventas genéricas sin potrero no deben coincidir con ningún identificador de potrero.");

            var todas = service.ObtenerTodasLasVentas();
            Assert(todas.Count == 3, "VentaService debe ver las tres ventas mixtas.");

            Console.WriteLine("[OK] VentaService filtra ventas sin potrero y no falla con lista mixta.");
        }

        private static void VerificarIndexVentaSinNulos()
        {
            string rutaVista = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "..", "..", "..", "..", "p_mvcHacienda", "Views", "Venta", "Index.cshtml");

            if (!File.Exists(rutaVista))
            {
                Assert(false, $"Debe existir la vista Venta/Index: {rutaVista}");
                return;
            }

            string contenido = File.ReadAllText(rutaVista);
            Assert(contenido.Contains("venta.Res != null"),
                "La vista Venta/Index debe ramificar por venta.Res != null antes de usar propiedades de Res/Potrero.");
            Assert(contenido.Contains("N/A"),
                "La vista Venta/Index debe mostrar N/A para potrero/peso no disponibles.");

            Console.WriteLine("[OK] Vista Venta/Index tiene ramas null-safe.");
        }

        private static void VerificarPersistenciaVentasLegacyAbortaNumeroMalformado()
        {
            string tempRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempRoot);

            try
            {
                var env = new FakeWebHostEnvironment { ContentRootPath = tempRoot };
                var httpAccessor = new FakeHttpContextAccessor();
                var persistencia = new PersistenciaService(
                    env,
                    httpAccessor,
                    new ValidadorPotrero(),
                    new ValidadorRes(),
                    new ValidadorVacuna(),
                    new ValidadorVenta());

                string datosDir = Path.Combine(tempRoot, "Datos");
                Directory.CreateDirectory(datosDir);
                File.WriteAllLines(Path.Combine(datosDir, "Ventas.txt"), new[]
                {
                    "P1|2026-08-10|Lola|XYZ|5|Ternero|1000",
                    "V2|2026-08-10|500|Lacteo|Queso"
                });

                bool aborto = false;
                try
                {
                    persistencia.CargarVentas(new List<Potrero>());
                }
                catch (Exception ex) when (ex.Message.Contains("Error al cargar ventas"))
                {
                    aborto = true;
                }

                Assert(aborto, "CargarVentas debe abortar ante número malformado en registro legacy.");

                Console.WriteLine("[OK] Persistencia legacy aborta en número malformado.");
            }
            finally
            {
                try { Directory.Delete(tempRoot, recursive: true); } catch { }
            }
        }

        private static void VerificarRegistroVentaCargada()
        {
            string tempRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempRoot);

            try
            {
                var env = new FakeWebHostEnvironment { ContentRootPath = tempRoot };
                var httpAccessor = new FakeHttpContextAccessor();
                var persistencia = new PersistenciaService(
                    env,
                    httpAccessor,
                    new ValidadorPotrero(),
                    new ValidadorRes(),
                    new ValidadorVacuna(),
                    new ValidadorVenta());

                var fecha = new DateTime(2026, 8, 10);
                var potrero = new Potrero("P1", l_tipos_potreros.ternero);
                var resLegacy = new Ternero("Lola", 120, 5);

                var ventas = new List<Venta>
                {
                    new Venta(potrero, fecha, resLegacy, 1000),
                    new Venta(fecha, new Lacteo("Queso"), 500),
                    new Venta(fecha, new Ternero("Lolita", 200, 6), 1200)
                };

                string resultado = persistencia.GuardarVentas(ventas);
                Assert(resultado == "Guardado exitosamente",
                    $"GuardarVentas debe reportar éxito. Obtenido: '{resultado}'");

                // Simulación de reinicio: nueva Hacienda con potrero vacío para resolver legacy.
                var haciendaReiniciada = new Hacienda();
                haciendaReiniciada.L_potreros.Add(potrero);

                var ventasCargadas = persistencia.CargarVentas(haciendaReiniciada.L_potreros);
                Assert(ventasCargadas.Count == 3, $"Deben cargarse 3 ventas, no {ventasCargadas.Count}.");

                foreach (var venta in ventasCargadas)
                {
                    haciendaReiniciada.registrar_venta_cargada(venta);
                }

                // Visibilidad a través de Hacienda.L_ventas
                var registradas = haciendaReiniciada.L_ventas;
                Assert(registradas.Count == 3,
                    $"Las ventas cargadas deben ser visibles en Hacienda.L_ventas. Obtenidas: {registradas.Count}.");

                var legacy = registradas.FirstOrDefault(v => v.Potrero != null && v.Res != null);
                Assert(legacy != null && legacy.Res.Nombre == "Lola" && legacy.Monto == 1000,
                    "Venta legacy cargada debe ser visible con su res y monto.");

                var lacteo = registradas.FirstOrDefault(v => v.Producto is Lacteo);
                Assert(lacteo != null && lacteo.Producto.Nombre == "Queso" && lacteo.Monto == 500,
                    "Venta genérica de Lacteo cargada debe ser visible.");

                var resGenerica = registradas.FirstOrDefault(v => v.Producto is Ternero t && t.Nombre == "Lolita");
                Assert(resGenerica != null && resGenerica.Monto == 1200,
                    "Venta genérica de Res cargada debe ser visible.");

                // Visibilidad a través de VentaService
                var service = new VentaService(haciendaReiniciada);
                var todasService = service.ObtenerTodasLasVentas();
                Assert(todasService.Count == 3,
                    $"VentaService debe ver las 3 ventas cargadas. Obtenidas: {todasService.Count}.");

                var p1 = service.ObtenerVentasPorPotrero("P1");
                Assert(p1.Count == 1 && p1[0].Res.Nombre == "Lola",
                    "VentaService debe filtrar la venta legacy por potrero P1.");

                // La lista devuelta por L_ventas es una copia: mutarla no debe alterar el registro.
                int countAntes = haciendaReiniciada.L_ventas.Count;
                var copia = haciendaReiniciada.L_ventas;
                copia.Add(new Venta(fecha, new Lacteo("NoPersistir"), 1));
                Assert(haciendaReiniciada.L_ventas.Count == countAntes,
                    "Mutar la copia devuelta por L_ventas no debe agregar ventas al registro real.");

                // El método nuevo rechaza null.
                try
                {
                    haciendaReiniciada.registrar_venta_cargada(null);
                    Fail("registrar_venta_cargada debe rechazar null.");
                }
                catch (ArgumentNullException)
                {
                    // esperado
                }

                Console.WriteLine("[OK] Ventas cargadas desde persistencia se registran en Hacienda y son visibles por VentaService.");
            }
            finally
            {
                try { Directory.Delete(tempRoot, recursive: true); } catch { }
            }
        }

        private static void Assert(bool condicion, string mensaje)
        {
            if (!condicion)
            {
                Console.WriteLine($"[FALLA] {mensaje}");
                _fallos++;
            }
        }

        private static void Fail(string mensaje)
        {
            Assert(false, mensaje);
        }
    }

    // Producto e inventario definidos únicamente en el verifier para demostrar
    // que Hacienda.vender<T> extiende a nuevos productos sin modificar Hacienda.
    internal class ProductoVerificador : Producto
    {
        public ProductoVerificador(string nombre) : base(nombre)
        {
        }
    }

    internal class InventarioVerificador : IInventario<ProductoVerificador>
    {
        private readonly List<ProductoVerificador> items = new List<ProductoVerificador>();

        public void agregar(ProductoVerificador producto)
        {
            if (producto == null)
                throw new ArgumentNullException(nameof(producto));

            if (items.Any(i => i.Nombre.Equals(producto.Nombre, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"El producto '{producto.Nombre}' ya existe.");

            items.Add(producto);
        }

        public bool contiene(ProductoVerificador producto)
        {
            return producto != null && items.Any(i => i.Nombre.Equals(producto.Nombre, StringComparison.OrdinalIgnoreCase));
        }

        public ProductoVerificador retirar(ProductoVerificador producto)
        {
            if (producto == null)
                throw new ArgumentNullException(nameof(producto));

            var existente = items.FirstOrDefault(i => i.Nombre.Equals(producto.Nombre, StringComparison.OrdinalIgnoreCase));
            if (existente == null)
                throw new InvalidOperationException("El producto no se encuentra en el inventario.");

            items.Remove(existente);
            return existente;
        }
    }

    // Stubs para construir PersistenciaService en el verifier sin arrancar ASP.NET.
    internal class FakeWebHostEnvironment : IWebHostEnvironment
    {
        public string ApplicationName { get; set; } = "HaciendaNEW.Verification";
        public string EnvironmentName { get; set; } = "Development";
        public string ContentRootPath { get; set; } = string.Empty;
        public IFileProvider ContentRootFileProvider { get; set; } = null!;
        public string WebRootPath { get; set; } = string.Empty;
        public IFileProvider WebRootFileProvider { get; set; } = null!;
    }

    internal class FakeHttpContextAccessor : IHttpContextAccessor
    {
        public HttpContext HttpContext { get; set; }
    }
}
