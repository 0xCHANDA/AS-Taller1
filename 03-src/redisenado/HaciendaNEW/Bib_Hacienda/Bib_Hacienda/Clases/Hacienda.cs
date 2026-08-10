using Bib_Hacienda.Eventos;
using Bib_Hacienda.Interfaces;
using Bib_Hacienda.Reglas;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Bib_Hacienda.Clases.Potrero;

namespace Bib_Hacienda.Clases
{
    public class Hacienda : IVacunacion, IVentaRes, ICreacionVacuna
    {
        //Atributos
        private List<Potrero> l_potreros;
        private RegistroVenta registroVentas;
        private List<Vacuna> l_vacunas;

        //Accesores públicos para los servicios (get público, set privado)
        public List<Potrero> L_potreros 
        { 
            get => l_potreros; 
            private set => l_potreros = value; 
        }

        public List<Venta> L_ventas => registroVentas.Ventas.ToList();

        public List<Vacuna> L_vacunas 
        { 
            get => l_vacunas; 
            private set => l_vacunas = value; 
        }

        //Eventos
        private PublisherVacunacionCompletada publisher_vacunacion_completa = new PublisherVacunacionCompletada();
        private PublisherVacunaVencida publisher_vacuna_vencida = new PublisherVacunaVencida();
        private PublisherPesoMin publisher_peso_min = new PublisherPesoMin();
        private PublisherPesoVenta publisher_peso_ideal = new PublisherPesoVenta();


        //EventHandler
        internal void EventHandler() { }

        //Constructor vacío
        public Hacienda()
        {
            l_potreros = new List<Potrero>();
            registroVentas = new RegistroVenta();
            l_vacunas = new List<Vacuna>();
        }

        //Metodo para crear potreros
        public string crear_potrero(string indentificacion, l_tipos_potreros tipo_potrero)
        {
            try
            {
                //Validar que el nombre no este vacio o nulo
                if (string.IsNullOrWhiteSpace(indentificacion))
                {
                    throw new ArgumentException("El nombre de la res no puede estar vacío", nameof(indentificacion));
                }
                if (l_potreros.Any(p => p.Identificacion.Equals(indentificacion, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException($"Ya existe un potrero con el nombre '{indentificacion}'.");
                }

                //Crear nuevo potrero

                Potrero nuevo_potrero = new Potrero(indentificacion, tipo_potrero);

                l_potreros.Add(nuevo_potrero);

                return ($"El potrero {indentificacion} se a añadido a la hacienda. ");

            }
            catch (Exception er)
            {
                throw new Exception("Error inesperado en el metodo crear_potrero: " + er.Message);
            }
        }

        //Metodo para buscar potreros por el nombre
        public Potrero buscar_potrero(string nombre)
        {
            try
            {
                // Validar nombre
                if (string.IsNullOrWhiteSpace(nombre))
                {
                    throw new ArgumentException("El nombre de búsqueda no puede estar vacío.");
                }

                // Buscar potreros que contengan el texto (ignorando mayúsculas/minúsculas)
                var potreros_encontrados = l_potreros
                    .Where(p => p.Identificacion.IndexOf(nombre, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                // Si no hay resultados
                if (potreros_encontrados.Count == 0)
                {
                    throw new Exception($"No se encontró ningún potrero con el nombre o coincidencia '{nombre}'.");
                }

                // Si hay más de un resultado, mostrar opciones
                if (potreros_encontrados.Count > 1)
                {
                    throw new Exception($" se encontró mas de un potrero con el nombre o coincidencia '{nombre}'.");
                }

                //  devolver potrero
                return potreros_encontrados.First();
            }
            catch (Exception er)
            {
                throw new Exception("Error inesperado en el método buscar_potrero: " + er.Message);
            }
        }
        
        //Metodo para  anadir res a un potrero 
        public string anadir_res_potrero (string id_potrero, string nombre, ushort edad, uint peso)
        {
            try
            {
                Potrero potrero = buscar_potrero(id_potrero);
                string resultado = potrero.anadir_res(nombre, edad, peso);  // ✅ Capturar el mensaje
                return resultado;  // ✅ Retornar el mensaje del potrero (incluye eventos)
            }
            catch (Exception er)
            {
                throw new Exception("Error inesperado en el método anadir_res_potrero: " + er.Message);
            }
        }

        //Sobrecarga conservada para compatibilidad con consumidores existentes.
        public string vender<T>(IInventario<T> inventario, T producto, uint monto) where T : Producto
        {
            return vender((IInventarioVendible<T>)inventario, producto, monto);
        }

        //metodo para vender
        public string vender<T>(IInventarioVendible<T> inventario, T producto, uint monto) where T : Producto
        {
            if (inventario == null)
                throw new ArgumentNullException(nameof(inventario));

            if (producto == null)
                throw new ArgumentNullException(nameof(producto));

            if (!inventario.contiene(producto))
                throw new InvalidOperationException(
                    $"El producto '{producto.Nombre}' no se encuentra en el inventario.");

            Venta venta = new Venta(
                DateTime.Now,
                producto,
                monto
            ){

            };
            registroVentas.registrar(venta);

            inventario.retirar(producto);

            return $"Venta de '{producto.Nombre}' realizada con éxito.";
        }

        //Metodo para registrar una venta ya creada (por ejemplo, restaurada desde persistencia)
        //sin exponer la coleccion interna de registroVentas.
        public void registrar_venta_cargada(Venta venta)
        {
            if (venta == null)
                throw new ArgumentNullException(nameof(venta));

            registroVentas.registrar(venta);
        }

        //Metodo para vender res (legacy)
        public string vender_res(string id_potrero, string nombre, uint monto)
        {
            try
            {
                Potrero potrero = buscar_potrero(id_potrero);
                Res res = potrero.buscar_res(nombre);

                if (potrero == null) throw new ArgumentNullException(nameof(potrero));
                if (res == null) throw new ArgumentNullException(nameof(res));

                Venta venta = new Venta(potrero, DateTime.Now, res, monto);
                registroVentas.registrar(venta);
                potrero.L_reses.Remove(res);

                return $"Venta de la res {res.Nombre} realizada con exito";
            }
            catch (Exception er)
            {
                throw new Exception("Error inesperado en el metodo vender_res: " + er.Message);
            }
        }

        //Metodo para alimentar una res
        public string alimentar_res(string id_potrero, string nombre, uint cantidad = 1)
        {
            try
            {
                Potrero potrero = buscar_potrero(id_potrero);
                Res res = potrero.buscar_res(nombre);

                //Validar parámetros
                if (potrero == null) throw new ArgumentNullException(nameof(potrero));
                if (res == null) throw new ArgumentNullException(nameof(res));

                res.Alimentar(cantidad);

                string mensaje_eventos = "";

                //Suscribirse a los eventos con lambdas para acumular mensajes
                publisher_peso_min.evt_peso_min += (mensaje) =>
                {
                    if (!string.IsNullOrEmpty(mensaje))
                        mensaje_eventos += mensaje + "\n";
                };

                publisher_peso_ideal.evt_peso_venta += (mensaje) =>
                {
                    if (!string.IsNullOrEmpty(mensaje))
                        mensaje_eventos += mensaje + "\n";
                };

                //Disparar los eventos con la res actualizada
                publisher_peso_min.Informar_Peso_Min(res);
                publisher_peso_ideal.Informar_Peso_Venta(res);

                //Construir mensaje de retorno
                string mensaje_final = $"La res '{res.Nombre}' ha sido alimentada, ahora pesa {res.Peso} kg.";
                if (!string.IsNullOrEmpty(mensaje_eventos))
                {
                    mensaje_final += "\n" + mensaje_eventos.TrimEnd();
                }

                return mensaje_final;
            }
            catch (Exception er)
            {
                throw new Exception("Error inesperado en el metodo alimentar_res: " + er.Message);
            }
        }

        //Metodo para crear y añadir vacuna al inventario
        public string crear_vacuna(string nombre, string lote, DateTime fecha_vencimiento, DateTime fecha_aplicacion, uint periodo_aplicacion)
        {
            try
            {
                //Validar parámetros
                if (string.IsNullOrWhiteSpace(nombre))
                    throw new ArgumentException("El nombre de la vacuna no puede estar vacío", nameof(nombre));

                if (string.IsNullOrWhiteSpace(lote))
                    throw new ArgumentException("El lote de la vacuna no puede estar vacío", nameof(lote));

                //Validar que la fecha de vencimiento sea posterior a la fecha de aplicación
                if (fecha_vencimiento <= fecha_aplicacion)
                    throw new Exception("La fecha de vencimiento debe ser posterior a la fecha de aplicación");

                //Verificar si ya existe una vacuna con el mismo lote
                if (l_vacunas.Any(v => v.Lote.Equals(lote, StringComparison.OrdinalIgnoreCase)))
                    throw new Exception($"Ya existe una vacuna con el lote '{lote}' en el inventario");

                //Crear vacuna bacteriana
                Bacteriana nueva_vacuna = new Bacteriana(nombre, lote, fecha_vencimiento, fecha_aplicacion, periodo_aplicacion);

                //Agregar al inventario
                l_vacunas.Add(nueva_vacuna);

                return $"Vacuna bacteriana '{nombre}' del lote '{lote}' agregada al inventario con éxito. Período de aplicación: {periodo_aplicacion} semanas.";
            }
            catch (Exception er)
            {
                throw new Exception("Error inesperado en el método crear_vacuna (bacteriana): " + er.Message);
            }
        }

        //Metodo para crear vacuna viva individual
        public string crear_vacuna(string nombre, string lote, DateTime fecha_vencimiento, DateTime fecha_aplicacion, Viva.enum_l_atenuaciones grado_atenuacion)
        {
            try
            {
                //Validar parámetros
                if (string.IsNullOrWhiteSpace(nombre))
                    throw new ArgumentException("El nombre de la vacuna no puede estar vacío", nameof(nombre));

                if (string.IsNullOrWhiteSpace(lote))
                    throw new ArgumentException("El lote de la vacuna no puede estar vacío", nameof(lote));

                //Validar que la fecha de vencimiento sea posterior a la fecha de aplicación
                if (fecha_vencimiento <= fecha_aplicacion)
                    throw new Exception("La fecha de vencimiento debe ser posterior a la fecha de aplicación");

                //Verificar si ya existe una vacuna con el mismo lote
                if (l_vacunas.Any(v => v.Lote.Equals(lote, StringComparison.OrdinalIgnoreCase)))
                    throw new Exception($"Ya existe una vacuna con el lote '{lote}' en el inventario");

                //Crear vacuna viva
                Viva nueva_vacuna = new Viva(nombre, lote, fecha_vencimiento, fecha_aplicacion, grado_atenuacion);

                //Agregar al inventario
                l_vacunas.Add(nueva_vacuna);

                return $"Vacuna viva '{nombre}' del lote '{lote}' agregada al inventario con éxito. Grado de atenuación: {(int)grado_atenuacion}.";
            }
            catch (Exception er)
            {
                throw new Exception("Error inesperado en el método crear_vacuna (viva): " + er.Message);
            }
        }

        //Metodo para crear lote de vacunas bacterianas
        public string crear_vacuna(string nombre, string lote_base, DateTime fecha_vencimiento, DateTime fecha_aplicacion, uint periodo_aplicacion, uint cantidad)
        {
            try
            {
                //Validar cantidad
                if (cantidad <= 0)
                    throw new ArgumentException("La cantidad debe ser mayor a 0", nameof(cantidad));

                if (cantidad > 100)
                    throw new ArgumentException("No se pueden crear más de 100 vacunas en un solo lote", nameof(cantidad));

                //Validar parámetros
                if (string.IsNullOrWhiteSpace(nombre))
                    throw new ArgumentException("El nombre de la vacuna no puede estar vacío", nameof(nombre));

                if (string.IsNullOrWhiteSpace(lote_base))
                    throw new ArgumentException("El lote base no puede estar vacío", nameof(lote_base));

                //Validar fechas
                if (fecha_vencimiento <= fecha_aplicacion)
                    throw new Exception("La fecha de vencimiento debe ser posterior a la fecha de aplicación");

                int vacunas_creadas = 0;

                //Crear múltiples vacunas con lotes numerados
                for (int i = 1; i <= cantidad; i++)
                {
                    string lote_numerado = $"{lote_base}-{i:D3}";

                    //Verificar si ya existe
                    if (l_vacunas.Any(v => v.Lote.Equals(lote_numerado, StringComparison.OrdinalIgnoreCase)))
                    {
                        continue;
                    }

                    //Crear vacuna bacteriana
                    Bacteriana nueva_vacuna = new Bacteriana(nombre, lote_numerado, fecha_vencimiento, fecha_aplicacion, periodo_aplicacion);
                    l_vacunas.Add(nueva_vacuna);
                    vacunas_creadas++;
                }

                if (vacunas_creadas == 0)
                    throw new Exception($"No se pudo crear ninguna vacuna. Todos los lotes ya existen en el inventario");

                return $"Lote de vacunas bacterianas creado con éxito:\n" +
                "- Nombre: {nombre}\n" +
                $"- Cantidad creada: {vacunas_creadas} de {cantidad}\n" +
                $"- Lotes: {lote_base}-001 a {lote_base}-{vacunas_creadas:D3}\n" +
                $"- Período de aplicación: {periodo_aplicacion} semanas";
            }
            catch (Exception er)
            {
                throw new Exception("Error inesperado en el método crear_vacuna (lote bacteriano): " + er.Message);
            }
        }

        //Metodo para crear lote de vacunas vivas
        public string crear_vacuna(string nombre, string lote_base, DateTime fecha_vencimiento, DateTime fecha_aplicacion, Viva.enum_l_atenuaciones grado_atenuacion, uint cantidad)
        {
            try
            {
                //Validar cantidad
                if (cantidad <= 0)
                    throw new ArgumentException("La cantidad debe ser mayor a 0", nameof(cantidad));

                if (cantidad > 100)
                    throw new ArgumentException("No se pueden crear más de 100 vacunas en un solo lote", nameof(cantidad));

                //Validar parámetros
                if (string.IsNullOrWhiteSpace(nombre))
                    throw new ArgumentException("El nombre de la vacuna no puede estar vacío", nameof(nombre));

                if (string.IsNullOrWhiteSpace(lote_base))
                    throw new ArgumentException("El lote base no puede estar vacío", nameof(lote_base));

                //Validar fechas
                if (fecha_vencimiento <= fecha_aplicacion)
                    throw new Exception("La fecha de vencimiento debe ser posterior a la fecha de aplicación");

                int vacunas_creadas = 0;

                //Crear múltiples vacunas con lotes numerados
                for (int i = 1; i <= cantidad; i++)
                {
                    string lote_numerado = $"{lote_base}-{i:D3}";

                    //Verificar si ya existe
                    if (l_vacunas.Any(v => v.Lote.Equals(lote_numerado, StringComparison.OrdinalIgnoreCase)))
                    {
                        continue;
                    }

                    //Crear vacuna viva
                    Viva nueva_vacuna = new Viva(nombre, lote_numerado, fecha_vencimiento, fecha_aplicacion, grado_atenuacion);
                    l_vacunas.Add(nueva_vacuna);
                    vacunas_creadas++;
                }

                if (vacunas_creadas == 0)
                    throw new Exception($"No se pudo crear ninguna vacuna. Todos los lotes ya existen en el inventario");

                return $"Lote de vacunas vivas creado con éxito:\n" +
                $"- Nombre: {nombre}\n" +
                $"- Cantidad creada: {vacunas_creadas} de {cantidad}\n" +
                $"- Lotes: {lote_base}-001 a {lote_base}-{vacunas_creadas:D3}\n" +
                $"- Grado de atenuación: {(int)grado_atenuacion}";
            }
            catch (Exception er)
            {
                throw new Exception("Error inesperado en el método crear_vacuna (lote vivo): " + er.Message);
            }
        }

              
        //Metodo nuevo para aplicar vacuna - hacienda

        public string aplicar_vacuna(Vacuna vacuna,string nombre,string idPotrero)
        {
            Potrero potrero = buscar_potrero(idPotrero);

            Res res = potrero.buscar_res(nombre);

            if (vacuna == null)
                throw new ArgumentNullException(nameof(vacuna));

            res.aplicar_vacuna(vacuna);

            L_vacunas.Remove(vacuna);

            publisher_vacunacion_completa
                .Informar_Vacunacion_Completada(res, res.CantidadVacunasBacterianas, res.CantidadVacunasVivas);

            return $"Vacuna aplicada correctamente a la res {res.Nombre}.";
        }
    }
}
