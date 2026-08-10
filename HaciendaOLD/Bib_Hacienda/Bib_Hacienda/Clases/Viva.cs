using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bib_Hacienda.Clases
{
    public class Viva : Vacuna //Hereda de Vacuna
    {

        //Enum para las atenuaciones
        public enum enum_l_atenuaciones
        {
            Atenuacion10 = 10,
            Atenuacion20 = 20,
            Atenuacion30 = 30
        }

        //Atributos
        private enum_l_atenuaciones periodo_atenuacion;

        //Constructor
        public Viva(string nombre, string lote, DateTime fecha_vencimiento, DateTime fecha_aplicacion, enum_l_atenuaciones periodo_atenuacion) : base(nombre, lote, fecha_vencimiento, fecha_aplicacion)
        {
            this.periodo_atenuacion = periodo_atenuacion;
        }
    }
}
