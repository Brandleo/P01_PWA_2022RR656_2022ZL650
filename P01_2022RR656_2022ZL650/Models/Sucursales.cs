﻿using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace P01_2022RR656_2022ZL650.Models
{
    public class Sucursales
    {
        [Key]
        public int SucursalID { get; set; }
        public string? Nombre { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Administrador { get; set; }
        public int? NumeroEspacios { get; set; }
    }
}
