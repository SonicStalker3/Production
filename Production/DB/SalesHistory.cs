//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Production.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class SalesHistory
    {
        public int SalesHistoryID { get; set; }
        public int AgentID { get; set; }
        public int ProductID { get; set; }
        public Nullable<System.DateTime> SaleDate { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
    
        public virtual Agent Agent { get; set; }
    }
}