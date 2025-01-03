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
    
    public partial class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public Nullable<int> MinAgentPrice { get; set; }
        public Nullable<int> Size { get; set; }
        public Nullable<int> WeightNet { get; set; }
        public Nullable<int> WeightGross { get; set; }
        public byte[] QualityCertificate { get; set; }
        public Nullable<int> StandardNumber { get; set; }
        public Nullable<int> CostPrice { get; set; }
        public Nullable<int> ProductionTime { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public Nullable<int> RequiredWorkers { get; set; }
        public Nullable<int> MaterialsRequiredID { get; set; }
    }
}
