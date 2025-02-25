﻿namespace P01_2022RR656_2022ZL650.Models
{
    public class Reservas
    {
        public int ReservaID { get; set; }
        public int? UsuarioID { get; set; }
        public int? EspacioID { get; set; }
        public DateTime FechaReserva { get; set; }
        public int? CantidadHoras { get; set; }
        public bool Estado { get; set; } = true;
    }
}
