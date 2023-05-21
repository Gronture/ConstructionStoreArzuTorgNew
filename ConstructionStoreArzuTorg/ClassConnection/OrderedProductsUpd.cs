using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionStoreArzuTorg.ClassConnection
{
    public class OrderedProductsUpd
    {
        public int ID { get; set; }
        public string НазваниеТовара { get; set; }
        public int Количество { get; set; }
        public int Заказ { get; set; }
        public string НазваниеСтатуса { get; set; }
        public decimal Сумма { get; set; }
    }
}
