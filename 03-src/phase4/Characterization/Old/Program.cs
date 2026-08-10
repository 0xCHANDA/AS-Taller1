using System;
using Bib_Hacienda.Clases;
using static Bib_Hacienda.Clases.Potrero;

namespace Characterization.Old
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            EjecutarEscenarios();
        }

        private static void EjecutarEscenarios()
        {
            var hacienda = new Hacienda();

            Resultado("C01", () => hacienda.crear_potrero("P1", l_tipos_potreros.ternero),
                () => $"potreros={hacienda.L_potreros.Count}");

            Resultado("C02", () => hacienda.crear_potrero("p1", l_tipos_potreros.ternero),
                () => $"potreros={hacienda.L_potreros.Count}");

            Resultado("C03", () => hacienda.anadir_res_potrero("P1", "Lola", 5, 100),
                () => $"reses={hacienda.L_potreros[0].L_reses.Count};tipo={hacienda.L_potreros[0].L_reses[0].GetType().Name}");

            Resultado("C04", () => hacienda.anadir_res_potrero("P1", "Mayor", 13, 100),
                () => $"reses={hacienda.L_potreros[0].L_reses.Count}");

            Resultado("C05", () => hacienda.buscar_potrero("p").Identificacion,
                () => $"potreros={hacienda.L_potreros.Count}");

            Resultado("C06", () => hacienda.alimentar_res("P1", "Lola"),
                () => $"peso={hacienda.L_potreros[0].L_reses[0].Peso}");

            Resultado("C07", () => hacienda.alimentar_res("P1", "Lola", 0),
                () => $"peso={hacienda.L_potreros[0].L_reses[0].Peso}");

            DateTime aplicacion = new DateTime(2026, 8, 10);
            DateTime vencimiento = new DateTime(2030, 8, 10);
            Resultado("C08", () => hacienda.crear_vacuna("Bovina", "L1", vencimiento, aplicacion, 4),
                () => $"vacunas={hacienda.L_vacunas.Count}");

            Resultado("C09", () => hacienda.crear_vacuna("Repetida", "l1", vencimiento, aplicacion, 4),
                () => $"vacunas={hacienda.L_vacunas.Count}");

            Resultado("C10", () => hacienda.aplicar_vacuna(hacienda.L_vacunas[0], "Lola", "P1"),
                () => $"inventario={hacienda.L_vacunas.Count};aplicadas={hacienda.L_potreros[0].L_reses[0].L_vacunas_aplicadas.Count}");

            Resultado("C11", () => hacienda.vender_res("P1", "Lola", 1200),
                () => $"reses={hacienda.L_potreros[0].L_reses.Count};ventas={hacienda.L_ventas.Count};monto={hacienda.L_ventas[0].Monto}");
        }

        private static void Resultado(string id, Func<string> operacion, Func<string> estado)
        {
            try
            {
                string salida = operacion();
                Console.WriteLine($"{id}|OK|{Limpiar(salida)}|{estado()}");
            }
            catch (Exception er)
            {
                Console.WriteLine($"{id}|EXCEPTION|{er.GetType().Name}:{Limpiar(er.Message)}|{estado()}");
            }
        }

        private static string Limpiar(string texto)
        {
            return texto.Replace("\r", "").Replace("\n", "\\n");
        }
    }
}
