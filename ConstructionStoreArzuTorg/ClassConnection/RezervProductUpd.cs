using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionStoreArzuTorg.ClassConnection
{
    internal class RezervProductUpd
    {
        public int ID { get; set; }
        public string НазваниеТовара { get; set; }
        public int Количество { get; set; }
        public int Резервирование { get; set; }
        public decimal Summ { get; set; }
    }
}
