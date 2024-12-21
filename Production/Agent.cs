//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Production
{
    using System;
    using System.Collections.Generic;
    
    public partial class Agent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Agent()
        {
            this.Orders = new HashSet<Order>();
            this.SalesHistories = new HashSet<SalesHistory>();
        }
    
        public int AgentID { get; set; }
        public string CompanyName { get; set; }
        public int AgentTypeID { get; set; }
        public string LegalAddress { get; set; }
        public string INN { get; set; }
        public string KPP { get; set; }
        public byte[] DirectorName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public byte[] Logo { get; set; }
        public Nullable<int> Priority { get; set; }
        public int SalesHistoryID { get; set; }
        public int UserID { get; set; }
    
        public virtual BusinessType BusinessType { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesHistory> SalesHistories { get; set; }
    }
}