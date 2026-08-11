using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bib_Hacienda.Clases
{
    public class RegistroVenta
    {
        private readonly List<Venta> ventas = new List<Venta>();

        public IReadOnlyList<Venta> Ventas => ventas;

        // Hacienda conserva su contrato legacy de exponer una List<Venta> viva.
        // El acceso mutable queda interno para no ampliar la API nueva de RegistroVenta.
        internal List<Venta> VentasMutables => ventas;

        public void registrar(Venta venta)
        {
            if (venta == null)
                throw new ArgumentNullException(nameof(venta));

            ventas.Add(venta);
        }
    }
}
