using Bib_Hacienda.Clases;
using Bib_Hacienda.enums;
using Bib_Hacienda.Eventos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bib_Hacienda.Clases
{
    public abstract class Res:Producto
    {

        //Atributos
        private uint peso;
        private ushort edad;
        private List<Vacuna> l_vacunas_aplicadas;

        internal void EventHandler() { }

        //Constructor
        public Res(string nombre, uint peso, ushort edad) : base(nombre)
        {
            this.Peso = peso;
            this.edad = edad;
            this.l_vacunas_aplicadas = new List<Vacuna>();
        }

        //Accesores
        // Edad es inmutable tras la construcción: cada subtipo valida su rango
        // en su propio constructor. Esto elimina el setter virtual fortalecible
        // y evita llamar a miembros virtuales desde el constructor base.
        public ushort Edad => edad;
        public List<Vacuna> L_vacunas_aplicadas { get => l_vacunas_aplicadas;  }
        public uint Peso { get => peso; set => peso = value; }

        //metodo desde Res
        public void aplicar_vacuna(Vacuna vacuna)
        {
            try
            {
                {
                    if (vacuna == null)
                        throw new ArgumentNullException(nameof(vacuna));

                    if (vacuna.EstaVencida())
                        throw new InvalidOperationException(
                            $"La vacuna '{vacuna.Nombre}' está vencida.");

                    if (l_vacunas_aplicadas.Any(v =>
                        v.Nombre.Equals(vacuna.Nombre) ||
                        v.Lote.Equals(vacuna.Lote)))
                    {
                        throw new InvalidOperationException(
                            $"La vacuna '{vacuna.Nombre}' ya fue aplicada a la res '{Nombre}'.");
                    }

                    if (!vacuna.PuedeAplicarseA(this))
                    {
                        throw new InvalidOperationException(
                            $"La res '{Nombre}' no puede recibir más vacunas de este tipo.");
                    }


                    l_vacunas_aplicadas.Add(vacuna);
                }
            }
            catch (Exception err)
            {
                throw new Exception("Error inesperado en el metodo aplicar_vacuna: " + err.Message);
            }
        }

        public abstract byte MaxVacunasBacterianas { get; }
        public abstract byte MaxVacunasVivas { get; }

        public ushort CantidadVacunasBacterianas
        {
            get
            {
                return (ushort)L_vacunas_aplicadas
                    .Count(v => v.Tipo == TipoVacuna.Bacteriana);
            }
        }

        public ushort CantidadVacunasVivas
        {
            get
            {
                return (ushort)L_vacunas_aplicadas
                    .Count(v => v.Tipo == TipoVacuna.Viva);
            }
        }

        public void Alimentar(uint cantidad)
        {
            try {
                if (cantidad == 0)
                    throw new ArgumentException(
                        "La cantidad debe ser mayor que 0.");

                Peso += cantidad;
            }
            catch (Exception er)
            {
                throw new Exception("Error inesperado en el metodo alimentar: " + er.Message);
            }
           
        }
    }
}



