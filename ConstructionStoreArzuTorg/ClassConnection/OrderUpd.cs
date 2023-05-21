using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionStoreArzuTorg.ClassConnection
{
    public class OrderUpd
    {
        public int ID_Заказа { get; set; }
        public string ФамилияКлиента { get; set; }
        public string ФамилияСотрудника { get; set; }
        public decimal Сумма { get; set; }
        public System.DateTime Дата_заказа { get; set; }
    }
}
