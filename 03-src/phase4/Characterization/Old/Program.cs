using System;
using System.Linq;
using System.Reflection;
using Bib_Hacienda.Clases;
using static Bib_Hacienda.Clases.Potrero;

namespace Characterization.Old
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            EjecutarEscenarios();
            EjecutarEscenariosExtendidos();
            EjecutarReflexionApi();
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

        // ================================================================
        // Nuevos escenarios extendidos C12–C17 (cada uno con Hacienda limpia)
        // ================================================================
        private static void EjecutarEscenariosExtendidos()
        {
            DateTime vencFuturo = new DateTime(2030, 8, 10);
            DateTime appFuturo  = new DateTime(2026, 8, 10);
            DateTime vencPasado = new DateTime(2020, 1, 1);
            DateTime appPasado  = new DateTime(2019, 6, 1);

            // C12: Vacuna vencida — crear vacuna con fecha pasada, intentar aplicar
            {
                var h = new Hacienda();
                h.crear_potrero("P1", l_tipos_potreros.ternero);
                h.anadir_res_potrero("P1", "Lola", 5, 100);
                h.crear_vacuna("Vencida", "C12-VENC", vencPasado, appPasado, 4);
                Resultado("C12",
                    () => h.aplicar_vacuna(h.L_vacunas[0], "Lola", "P1"),
                    () => $"vacunas={h.L_vacunas.Count};aplicadas={h.L_potreros[0].L_reses[0].L_vacunas_aplicadas.Count}");
            }

            // C13: Aplicación duplicada — mismo nombre, diferente lote; segunda aplicación rechazada
            {
                var h = new Hacienda();
                h.crear_potrero("P1", l_tipos_potreros.ternero);
                h.anadir_res_potrero("P1", "Lola", 5, 100);
                h.crear_vacuna("Duplicada", "C13-DUP-A", vencFuturo, appFuturo, 4);
                h.crear_vacuna("Duplicada", "C13-DUP-B", vencFuturo, appFuturo, 4);
                var dupA = h.L_vacunas[0];
                var dupB = h.L_vacunas[1];
                h.aplicar_vacuna(dupA, "Lola", "P1");
                Resultado("C13",
                    () => h.aplicar_vacuna(dupB, "Lola", "P1"),
                    () => $"vacunas={h.L_vacunas.Count};aplicadas={h.L_potreros[0].L_reses[0].L_vacunas_aplicadas.Count}");
            }

            // C14: Límite bacteriano ternero (max 3) — cuarta bacteriana rechazada
            {
                var h = new Hacienda();
                h.crear_potrero("P1", l_tipos_potreros.ternero);
                h.anadir_res_potrero("P1", "Lola", 5, 100);
                h.crear_vacuna("Bac1", "C14-B1", vencFuturo, appFuturo, 4);
                h.crear_vacuna("Bac2", "C14-B2", vencFuturo, appFuturo, 4);
                h.crear_vacuna("Bac3", "C14-B3", vencFuturo, appFuturo, 4);
                h.aplicar_vacuna(h.L_vacunas[0], "Lola", "P1");
                h.aplicar_vacuna(h.L_vacunas[0], "Lola", "P1");
                h.aplicar_vacuna(h.L_vacunas[0], "Lola", "P1");
                h.crear_vacuna("Bac4", "C14-B4", vencFuturo, appFuturo, 4);
                Resultado("C14",
                    () => h.aplicar_vacuna(h.L_vacunas[0], "Lola", "P1"),
                    () => $"vacunas={h.L_vacunas.Count};aplicadas={h.L_potreros[0].L_reses[0].L_vacunas_aplicadas.Count}");
            }

            // C15: Límite viva ternero (max 1) — segunda viva rechazada
            {
                var h = new Hacienda();
                h.crear_potrero("P1", l_tipos_potreros.ternero);
                h.anadir_res_potrero("P1", "Lola", 5, 100);
                h.crear_vacuna("Viva1", "C15-V1", vencFuturo, appFuturo, Viva.enum_l_atenuaciones.Atenuacion10);
                h.crear_vacuna("Viva2", "C15-V2", vencFuturo, appFuturo, Viva.enum_l_atenuaciones.Atenuacion10);
                h.aplicar_vacuna(h.L_vacunas[0], "Lola", "P1");
                Resultado("C15",
                    () => h.aplicar_vacuna(h.L_vacunas[0], "Lola", "P1"),
                    () => $"vacunas={h.L_vacunas.Count};aplicadas={h.L_potreros[0].L_reses[0].L_vacunas_aplicadas.Count}");
            }

            // C16: Límite bacteriano alcanzado pero viva aún permitida (límites independientes)
            {
                var h = new Hacienda();
                h.crear_potrero("P1", l_tipos_potreros.ternero);
                h.anadir_res_potrero("P1", "Lola", 5, 100);
                h.crear_vacuna("BacX1", "C16-B1", vencFuturo, appFuturo, 4);
                h.crear_vacuna("BacX2", "C16-B2", vencFuturo, appFuturo, 4);
                h.crear_vacuna("BacX3", "C16-B3", vencFuturo, appFuturo, 4);
                h.aplicar_vacuna(h.L_vacunas[0], "Lola", "P1");
                h.aplicar_vacuna(h.L_vacunas[0], "Lola", "P1");
                h.aplicar_vacuna(h.L_vacunas[0], "Lola", "P1");
                h.crear_vacuna("VivaX", "C16-V1", vencFuturo, appFuturo, Viva.enum_l_atenuaciones.Atenuacion10);
                Resultado("C16",
                    () => h.aplicar_vacuna(h.L_vacunas[0], "Lola", "P1"),
                    () => $"vacunas={h.L_vacunas.Count};aplicadas={h.L_potreros[0].L_reses[0].L_vacunas_aplicadas.Count}");
            }

            // C17: Combinado — vacuna vencida con límite bacteriano ya alcanzado (expiración evalúa primero)
            {
                var h = new Hacienda();
                h.crear_potrero("P1", l_tipos_potreros.ternero);
                h.anadir_res_potrero("P1", "Lola", 5, 100);
                h.crear_vacuna("BacY1", "C17-B1", vencFuturo, appFuturo, 4);
                h.crear_vacuna("BacY2", "C17-B2", vencFuturo, appFuturo, 4);
                h.crear_vacuna("BacY3", "C17-B3", vencFuturo, appFuturo, 4);
                h.aplicar_vacuna(h.L_vacunas[0], "Lola", "P1");
                h.aplicar_vacuna(h.L_vacunas[0], "Lola", "P1");
                h.aplicar_vacuna(h.L_vacunas[0], "Lola", "P1");
                h.crear_vacuna("ExpBac", "C17-E", vencPasado, appPasado, 4);
                Resultado("C17",
                    () => h.aplicar_vacuna(h.L_vacunas[0], "Lola", "P1"),
                    () => $"vacunas={h.L_vacunas.Count};aplicadas={h.L_potreros[0].L_reses[0].L_vacunas_aplicadas.Count}");
            }
        }

        // ================================================================
        // Reflexión de API pública C18–C20
        // ================================================================
        private static void EjecutarReflexionApi()
        {
            // C18: Semántica de L_ventas (acceso observable después de una venta)
            {
                var h = new Hacienda();
                h.crear_potrero("P1", l_tipos_potreros.ternero);
                h.anadir_res_potrero("P1", "Lola", 5, 100);
                h.vender_res("P1", "Lola", 1200);
                // Verificar que L_ventas es accesible y refleja la venta
                int ventasCount = h.L_ventas.Count;
                uint monto = h.L_ventas[0].Monto;
                string tipo = h.L_ventas.GetType().Name;
                Console.WriteLine($"C18|API|L_ventas tipo={tipo};Count={ventasCount};Monto[0]={monto}|-");
            }

            // C19: Superficie de sobrecargas alimentar_res
            {
                var metodos = typeof(Hacienda).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(m => m.Name == "alimentar_res")
                    .ToArray();
                int total = metodos.Length;
                bool tieneDefault = metodos.Any(m => m.GetParameters().Any(p => p.HasDefaultValue));
                bool tieneDosParams = metodos.Any(m => m.GetParameters().Length == 2);
                bool tieneTresParams = metodos.Any(m => m.GetParameters().Length == 3);
                Console.WriteLine($"C19|API|alimentar_res overloads={total};defaultParam={tieneDefault};dosParams={tieneDosParams};tresParams={tieneTresParams}|-");
            }

            // C20: Existencia de IValidarInformacion
            {
                var t = Type.GetType("Bib_Hacienda.Interfaces.IValidarInformacion, Bib_Hacienda");
                string existe = t != null ? "EXISTS" : "ABSENT";
                Console.WriteLine($"C20|API|IValidarInformacion={existe}|-");
            }
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
