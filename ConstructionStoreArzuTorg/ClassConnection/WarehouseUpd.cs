using Microsoft.SqlServer.ReportingServices2005;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionStoreArzuTorg.ClassConnection
{
    public class WarehouseUpd
    {
        public int ID { get; set; }
        public string НазваниеТовара { get; set; }
        public string НазваниеКатегории { get; set; }
        public int Количество { get; set; } 
    }
}
