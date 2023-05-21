using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionStoreArzuTorg.ClassConnection
{
    public class DeliveriesUpd
    {
        public int ID { get; set; }
        public System.DateTime Дата { get; set; }
        public string ФамилияСотрудника { get; set; }
        public string НаименованиеПоставщика { get; set; }
        public decimal Сумма { get; set; }
        public string DateString { get; set; }
    }
}
