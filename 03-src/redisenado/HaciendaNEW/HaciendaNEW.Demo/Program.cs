using System;
using Bib_Hacienda.Clases;
using static Bib_Hacienda.Clases.Potrero;

namespace HaciendaNEW.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("=== DEMOSTRACION HACIENDA NEW ===");

            var hacienda = new Hacienda();
            Console.WriteLine(hacienda.crear_potrero("Terneros Norte", l_tipos_potreros.ternero));
            Console.WriteLine(hacienda.anadir_res_potrero("Terneros Norte", "Lola", 8, 180));
            Console.WriteLine(hacienda.alimentar_res("Terneros Norte", "Lola", 10));
            Console.WriteLine(hacienda.vender_res("Terneros Norte", "Lola", 1500));

            var inventarioLacteos = new InventarioLacteos();
            var lacteo = new Lacteo("Leche entera");
            inventarioLacteos.agregar(lacteo);
            Console.WriteLine(hacienda.vender(inventarioLacteos, lacteo, 500));

            var inventarioCarnes = new InventarioCarnes();
            var carne = new Carne("Corte de res");
            inventarioCarnes.agregar(carne);
            Console.WriteLine(hacienda.vender(inventarioCarnes, carne, 900));

            var inventarioPieles = new InventarioPieles();
            var piel = new Piel("Cuero curtido");
            inventarioPieles.agregar(piel);
            Console.WriteLine(hacienda.vender(inventarioPieles, piel, 700));

            Console.WriteLine($"Ventas registradas: {hacienda.L_ventas.Count}");
            foreach (Venta venta in hacienda.L_ventas)
            {
                Console.WriteLine($"- {venta.Producto.Nombre}: ${venta.Monto}");
            }

            Console.WriteLine("SC-1 LACTEO/CARNE/PIEL: PASS");
        }
    }
}
