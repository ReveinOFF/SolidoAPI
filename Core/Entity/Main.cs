using Core.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity
{
    public class Main : IBaseEntity
    {
        [Range(1,1)]
        public int Id { get; set; } 
        [Required]
        public string Company { get; set; }
        [Required]
        public string OrderOne { get; set; }
        [Required]
        public string OrderTwo { get; set; }
        [Required]
        public string OrderThree { get; set; }
        public string[] Images { get; set; }
        public string PathImage { get; set; }
    }
}
