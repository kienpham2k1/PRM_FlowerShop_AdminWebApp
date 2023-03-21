using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DataAccess.Models
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        [Required] public int UserId { get; set; }
        [Required] public string Username { get; set; }
        [Required] public string UserPassword { get; set; }
        [Required] public string UserAddress { get; set; }
        [Required] public int? RoleId { get; set; }
        [Required] public DateTime? Birthdate { get; set; }
        [Required] public string PhoneNumber { get; set; }

        [Required] public virtual Role Role { get; set; }
        [Required] public virtual ICollection<Order> Orders { get; set; }
    }
}
