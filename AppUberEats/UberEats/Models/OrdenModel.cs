﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UberEats.Models
{
    public class OrdenModel
    {
        public string Fecha { get; set; }

        public string Cliente { get; set; }

        public double Total { get; set; }

        public int IdOrden { get; set; }

        public int IdRestaurante { get; set; }
    }
}
