using Bib_Hacienda.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bib_Hacienda.Interfaces
{
    public interface IInventario<T> where T : Producto
    {
        void agregar(T producto);
        T retirar(T producto);
        bool contiene(T producto);
    }
}


