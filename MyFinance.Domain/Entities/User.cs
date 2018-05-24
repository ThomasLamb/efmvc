﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MyFinance.Domain
{
    public class User
    {
        [Key]
        [Required]
        [Display(Name = "User Name")]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(64)]
        public byte[] PasswordHash { get; set; }

        [Required]
        [MaxLength(128)]
        public byte[] PasswordSalt { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [MaxLength(200)]
        public string Email { get; set; }

        [MaxLength(200)]
        public string Comment { get; set; }

        [Display(Name = "Approved?")]
        public bool IsApproved { get; set; }

        [Display(Name = "Crate Date")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Last Login Date")]
        public DateTime? DateLastLogin { get; set; }

        [Display(Name = "Last Activity Date")]
        public DateTime? DateLastActivity { get; set; }

        [Display(Name = "Last Password Change Date")]
        public DateTime DateLastPasswordChange { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

    }
}