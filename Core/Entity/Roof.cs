using Core.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity
{
    public class Roof : IBaseEntity
    {
        [Range(1, 1)]
        public int Id { get; set; }
        [Required]
        public string Image { get; set; }
        public string PathImage { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Price { get; set; }
    }
}
