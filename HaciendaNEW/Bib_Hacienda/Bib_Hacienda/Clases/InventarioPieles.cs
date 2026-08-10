using Bib_Hacienda.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bib_Hacienda.Clases
{
    public class InventarioPieles : IInventario<Piel>
    {
        private readonly List<Piel> pieles = new List<Piel>();

        public IReadOnlyList<Piel> Pieles => pieles;
        public void agregar(Piel producto)
        {
            try
            {
                if (producto == null)
                    throw new ArgumentNullException(nameof(producto));

                pieles.Add(producto);
            }
            catch (Exception er)
            {
                throw new Exception("Error inesperado en el metodo agregar : " + er.Message);
            }
        }

        public bool contiene(Piel producto)
        {
            try
            {
                return producto != null && pieles.Contains(producto);
            }
            catch (Exception er)
            {
                throw new Exception("Error inesperado en el metodo contiene: " + er.Message);
            }
        }

        public Piel retirar(Piel producto)
        {
            try
            {
                if (producto == null)
                    throw new ArgumentNullException(nameof(producto));

                if (!pieles.Remove(producto))
                    throw new InvalidOperationException(
                        "El lácteo no se encuentra en el inventario.");

                return producto;
            }
            catch (Exception er)
            {
                throw new Exception("Error inesperado en el metodo retirar" + er.Message);
            }
        }
    }
}
