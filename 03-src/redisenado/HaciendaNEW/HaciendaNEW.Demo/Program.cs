using System;
using System.Collections.Generic;
using Bib_Hacienda.Clases;
using static Bib_Hacienda.Clases.Potrero;

namespace HaciendaNEW.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("=== HACIENDA NEW — DEMOSTRACIÓN SOLID ===");
            Console.WriteLine();

            var hacienda = new Hacienda();
            Console.WriteLine("[1] Potrero y res");
            Console.WriteLine("    > " + hacienda.crear_potrero("Terneros Norte", l_tipos_potreros.ternero).Trim());
            Console.WriteLine("    > " + hacienda.anadir_res_potrero("Terneros Norte", "Lola", 8, 180).Trim());
            Console.WriteLine("    > " + hacienda.alimentar_res("Terneros Norte", "Lola", 10).Trim());
            Console.WriteLine();

            Console.WriteLine("[2] Vacuna y vacunación");
            DateTime aplicacion = new DateTime(2026, 8, 10);
            DateTime vencimiento = new DateTime(2030, 8, 10);
            Console.WriteLine("    > " + hacienda.crear_vacuna("Bovina", "L1", vencimiento, aplicacion, 4).Trim());
            Console.WriteLine("    > " + hacienda.aplicar_vacuna(hacienda.L_vacunas[0], "Lola", "Terneros Norte").Trim());
            Console.WriteLine();

            Console.WriteLine("[3] Venta tradicional (res)");
            var otroPotrero = new Potrero("Engorde", l_tipos_potreros.cebon);
            hacienda.L_potreros.Add(otroPotrero);
            hacienda.anadir_res_potrero("Engorde", "Toro", 24, 410);
            Console.WriteLine("    > " + hacienda.vender_res("Engorde", "Toro", 1500).Trim());
            Console.WriteLine();

            Console.WriteLine("[4] SC-1 — Productos derivados (lácteos, carne, piel)");

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

            Console.WriteLine("[5] DI — construcción externalizada (no DIP)");
            var registroExterno = new RegistroVenta();
            var inventarioVacunas = new List<Vacuna>();
            var fabricadorExterno = new FabricadorVacunas(inventarioVacunas);
            var haciendaConDI = new Hacienda(registroExterno, fabricadorExterno);
            haciendaConDI.crear_vacuna("Compartida", "DI-001", vencimiento, aplicacion, 4);
            Console.WriteLine("    Inventario compartido Hacienda/Fabricador: " +
                (ReferenceEquals(haciendaConDI.L_vacunas, fabricadorExterno.L_vacunas) &&
                 haciendaConDI.L_vacunas.Count == 1));
            Console.WriteLine();

            Console.WriteLine("=== FIN DEMO — SC-1 LACTEO/CARNE/PIEL: PASS ===");
        }
    }
}
