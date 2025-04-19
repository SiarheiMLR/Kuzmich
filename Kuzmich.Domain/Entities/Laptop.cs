using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kuzmich.Domain.Entities
{
    public class Laptop
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Price { get; set; }
        public string? Image { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
