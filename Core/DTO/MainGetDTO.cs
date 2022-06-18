﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class MainGetDTO
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string OrderOne { get; set; }
        public string OrderTwo { get; set; }
        public string OrderThree { get; set; }
        public string[] Images { get; set; }
        public string PathImage { get; set; }
    }
}
