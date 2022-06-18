using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class ContactDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string DateOne { get; set; }
        public string DateTwo { get; set; }
        public string AddressOne { get; set; }
        public string AddressTwo { get; set; }
    }
}
