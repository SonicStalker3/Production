using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production.DB
{
    public static class DBContext
    {
        public static ProductionEntities _context = new ProductionEntities();
        public static ProductionEntities GetContext() 
        {
            return _context;
        }

/*        public static string AllSuppliers(this Material material) 
        {
            return string.Join(" ", material.Suppliers.ToList());
        }*/

    }
}
