//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConstructionStoreArzuTorg
{
    using System;
    using System.Collections.Generic;
    
    public partial class Склад
    {
        public int Товар { get; set; }
        public int Количество { get; set; }
        public int ID { get; set; }
    
        public virtual Товар Товар1 { get; set; }
    }
}
