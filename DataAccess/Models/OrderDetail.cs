using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DataAccess.Models
{
    public partial class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int? OrderId { get; set; }
        public int? FlowerId { get; set; }
        public int? Quantity { get; set; }

        public virtual Flower Flower { get; set; }
        public virtual Order Order { get; set; }
    }
}
