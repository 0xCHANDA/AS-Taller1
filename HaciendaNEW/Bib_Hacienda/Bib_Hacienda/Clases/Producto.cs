using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bib_Hacienda.Clases
{
    public abstract class Producto
    {
        private string nombre;

        protected Producto(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException(
                    "El nombre del producto no puede estar vacío.",
                    nameof(nombre));

            Nombre = nombre;
        }

        public string Nombre
        {
            get => nombre;
            protected set => nombre = value;
        }
    }
}
