using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionStoreArzuTorg.ClassConnection
{
    internal class RezervUpd
    {
        public int ID { get; set; }
        public System.DateTime Дата { get; set; }
        public string ФамилияКлиента { get; set; }
        public decimal Цена { get; set; }
        public string НазваниеСтатуса { get; set; }
        public string DateString { get; set; }
    }
}
