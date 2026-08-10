using Bib_Hacienda.Clases;
using Bib_Hacienda.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bib_Hacienda.Clases
{
    public class InventarioLacteos : IInventario<Lacteo>

    {
        private readonly List<Lacteo> lacteos = new List<Lacteo>();

        public IReadOnlyList<Lacteo> Lacteos => lacteos;
        public void agregar(Lacteo producto)
        {
            try {
                if (producto == null)
                    throw new ArgumentNullException(nameof(producto));

                lacteos.Add(producto);
            }
            catch (Exception er)
            {
                throw new Exception("Error inesperado en el metodo agregar: " + er.Message);
            }
        }

        public bool contiene(Lacteo lacteo)
        {
            try
            {
                return lacteo != null && lacteos.Contains(lacteo);
            }
            catch (Exception er)
            {
                throw new Exception("Error inesperado en el metodo contiene: " + er.Message);
            }
        }

        public Lacteo retirar(Lacteo lacteo)
        {
            try {
                if (lacteo == null)
                    throw new ArgumentNullException(nameof(lacteo));

                if (!lacteos.Remove(lacteo))
                    throw new InvalidOperationException(
                        "El lácteo no se encuentra en el inventario.");

                return lacteo;
            }
            catch (Exception er)
            {
                throw new Exception("Error inesperado en el metodo retirar" + er.Message);
            }

        }
    }
}

