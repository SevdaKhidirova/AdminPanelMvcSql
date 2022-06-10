using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bizLandTask.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public List<Portfolio> Portfolios { get; set; }
    }
}
