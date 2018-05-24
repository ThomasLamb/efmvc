using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyFinance.Domain
{
    public class Category 
    {

        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Name Required")]
        [StringLength(25, ErrorMessage = "Must be less than 25 characters")]
        [MaxLength(50)]
        public string Name { get; set;}
        public string Description { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
    }
}