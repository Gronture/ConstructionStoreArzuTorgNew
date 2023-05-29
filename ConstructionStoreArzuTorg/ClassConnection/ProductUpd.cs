using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionStoreArzuTorg.ClassConnection
{
    public class ProductUpd
    {
        public int Post { get; set; }
        public int Rezerv { get; set; }
        public int Ord { get; set; }
        public int ID_Товара { get; set; }
        public string Название { get; set; }
        public string Производитель { get; set; }
        public string Страна_происхождения { get; set; }
        public string НазваниеКатегории { get; set; }
        public string Размеры { get; set; }
        public string ЕдиницаИзмерения { get; set; }
        public decimal Стоимость { get; set; }
        public string Сезонность { get; set; }
        public int Count { get; set; }
        public Nullable<int> СерийныйНомер { get; set; }
        public string Гарантия { get; set; }
        public Nullable<decimal> Стоимость_со_скидкой { get; set; }
        public int  Discount { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public List<string> StatusList { get; set; }
        public int ID { get; set; }
        public decimal SumToReceipt { get; set; }

        public ProductUpd()
        {
            using (var content = new ConstructionStoreEntities())
            {
                StatusList = content.Статус.Select(x => x.Название).ToList();
            }
        }
    }
}
