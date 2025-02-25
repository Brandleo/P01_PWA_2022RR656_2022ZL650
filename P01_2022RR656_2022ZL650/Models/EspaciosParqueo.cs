﻿namespace P01_2022RR656_2022ZL650.Models
{
    public class EspaciosParqueo
    {
        public int EspacioID { get; set; }
        public int? SucursalID { get; set; }
        public int? Numero { get; set; }
        public string Ubicacion { get; set; }
        public decimal? CostoPorHora { get; set; }
        public string Estado { get; set; }
    }
}
