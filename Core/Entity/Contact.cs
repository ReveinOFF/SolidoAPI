using Core.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity
{
    public class Contact : IBaseEntity
    {
        [Range(1,1)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string DateOne { get; set; }
        [Required]
        public string DateTwo { get; set; }
        [Required]
        public string AddressOne { get; set; }
        [Required]
        public string AddressTwo { get; set; }
    }
}
