using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace MyFinance.Domain
{
    public class Role
    {
        [Key]
        [Display(Name = "Role Name")]
        [Required]
        [MaxLength(100)]
        public string RoleName { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
