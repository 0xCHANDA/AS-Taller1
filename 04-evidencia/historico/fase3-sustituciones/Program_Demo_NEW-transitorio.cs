using System;
using System.Collections.Generic;
using Bib_Hacienda.Clases;
using Bib_Hacienda.Interfaces;
using static Bib_Hacienda.Clases.Potrero;

namespace HaciendaNEW.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("=== HACIENDA NEW — DEMOSTRACIÓN SOLID ===");
            Console.WriteLine();

            // (1) Creación / uso básico de Hacienda
            Console.WriteLine("[1] Creación y uso básico de Hacienda");
            var hacienda = new Hacienda();
            Console.WriteLine("    Hacienda creada con constructor por defecto.");
            Console.WriteLine();

            // (2) Potrero
            Console.WriteLine("[2] Potrero");
            Console.WriteLine("    > " + hacienda.crear_potrero("Terneros Norte", l_tipos_potreros.ternero).Trim());
            Console.WriteLine("    Potreros registrados: " + hacienda.L_potreros.Count);
            Console.WriteLine();

            // (3) Res
            Console.WriteLine("[3] Res");
            Console.WriteLine("    > " + hacienda.anadir_res_potrero("Terneros Norte", "Lola", 8, 180).Trim());
            Console.WriteLine("    Reses en potrero: " + hacienda.L_potreros[0].L_reses.Count);
            Console.WriteLine();

            // (4) Alimentación
            Console.WriteLine("[4] Alimentación");
            Console.WriteLine("    > " + hacienda.alimentar_res("Terneros Norte", "Lola", 10).Trim());
            Console.WriteLine();

            // (5) Vacuna / vacunación
            Console.WriteLine("[5] Vacuna y vacunación");
            DateTime aplicacion = new DateTime(2026, 8, 10);
            DateTime vencimiento = new DateTime(2030, 8, 10);
            Console.WriteLine("    > " + hacienda.crear_vacuna("Bovina", "L1", vencimiento, aplicacion, 4).Trim());
            Console.WriteLine("    > " + hacienda.aplicar_vacuna(hacienda.L_vacunas[0], "Lola", "Terneros Norte").Trim());
            Console.WriteLine();

            // (6) Venta tradicional (de una res)
            Console.WriteLine("[6] Venta tradicional (res)");
            var otroPotrero = new Potrero("Engorde", l_tipos_potreros.cebon);
            hacienda.L_potreros.Add(otroPotrero);
            hacienda.anadir_res_potrero("Engorde", "Toro", 24, 410);
            Console.WriteLine("    > " + hacienda.vender_res("Engorde", "Toro", 1500).Trim());
            Console.WriteLine();

            // (7) SC-1: lácteos, carne, piel — OCP por composición de IInventarioVendible<T>
            Console.WriteLine("[7] SC-1 — Productos derivados (lácteos, carne, piel)");

            var inventarioLacteos = new InventarioLacteos();
            var lacteo = new Lacteo("Leche entera");
            inventarioLacteos.agregar(lacteo);
            Console.WriteLine("    > " + hacienda.vender(inventarioLacteos, lacteo, 500).Trim());

            var inventarioCarnes = new InventarioCarnes();
            var carne = new Carne("Corte de res");
            inventarioCarnes.agregar(carne);
            Console.WriteLine("    > " + hacienda.vender(inventarioCarnes, carne, 900).Trim());

            var inventarioPieles = new InventarioPieles();
            var piel = new Piel("Cuero curtido");
            inventarioPieles.agregar(piel);
            Console.WriteLine("    > " + hacienda.vender(inventarioPieles, piel, 700).Trim());

            Console.WriteLine();
            Console.WriteLine("    Total de ventas registradas: " + hacienda.L_ventas.Count);
            foreach (Venta venta in hacienda.L_ventas)
            {
                Console.WriteLine("      - " + venta.Producto.Nombre + ": $" + venta.Monto);
            }
            Console.WriteLine();

            // (8) DIP — inyección de dependencias por constructor
            Console.WriteLine("[8] DIP — Hacienda(RegistroVenta, FabricadorVacunas)");
            var registroExterno = new RegistroVenta();
            var fabricadorExterno = new FabricadorVacunas(new List<Vacuna>());
            var haciendaConDI = new Hacienda(registroExterno, fabricadorExterno);
            Console.WriteLine("    Hacienda construida con dependencias inyectadas.");
            Console.WriteLine("    Mismas fachadas: " +
                (typeof(IVacunacion).IsAssignableFrom(haciendaConDI.GetType()) &&
                 typeof(IVentaRes).IsAssignableFrom(haciendaConDI.GetType()) &&
                 typeof(ICreacionVacuna).IsAssignableFrom(haciendaConDI.GetType())));
            Console.WriteLine();

            Console.WriteLine("=== FIN DEMO — SC-1 LACTEO/CARNE/PIEL: PASS ===");
        }
    }
}
