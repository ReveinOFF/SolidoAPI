using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity
{
    public class Admin : IdentityUser
    {
        [Required, MinLength(5)]
        public string Login { get; set; }
    }
}
