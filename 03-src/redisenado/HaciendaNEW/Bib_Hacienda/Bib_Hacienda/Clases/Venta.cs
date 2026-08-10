using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bib_Hacienda.Clases
{
    public class Venta
    {
        private DateTime fecha;
        private Producto producto;
        private uint monto;
        private Potrero potrero;
        private Res res;

        // Constructor TO-BE (venta de cualquier Producto)
        public Venta(
            DateTime fecha,
            Producto producto,
            uint monto)
        {
            this.Fecha = fecha;
            this.Producto = producto;
            this.Monto = monto;
        }

        // Constructor legacy (venta de una Res dentro de un Potrero)
        public Venta(Potrero potrero, DateTime fecha, Res res, uint monto)
        {
            this.Potrero = potrero;
            this.Fecha = fecha;
            this.Res = res;
            this.Monto = monto;
            this.Producto = res;
        }

        public DateTime Fecha
        {
            get => fecha;
            set => fecha = value;
        }

        public Producto Producto
        {
            get => producto;
            set => producto = value;
        }

        public uint Monto
        {
            get => monto;
            set => monto = value;
        }

        public Potrero Potrero
        {
            get => potrero;
            set => potrero = value;
        }

        public Res Res
        {
            get => res;
            set => res = value;
        }
    }
}
