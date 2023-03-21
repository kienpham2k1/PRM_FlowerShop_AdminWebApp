using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DataAccess.Models
{
    public partial class Flower
    {
        public Flower()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int FlowerId { get; set; }

        [Required] public string FowerName { get; set; }
        public string FlowerImage { get; set; }
        [Required] public double? Price { get; set; }
        [Required] public int? Quantity { get; set; }
        [Required][MaxLength(250)] public string Description { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
