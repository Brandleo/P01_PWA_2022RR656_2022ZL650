using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_2022RR656_2022ZL650.Models;

namespace P01_2022RR656_2022ZL650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {


        public readonly BaseParqueoContext _parqueoContexto;



        public ReservasController(BaseParqueoContext parqueoContext)
        {
            _parqueoContexto = parqueoContext;


        }






        [HttpPost]
        [Route("Reservar/{usuarioId}")]
        public IActionResult Reservar(int usuarioId, [FromBody] Reservas reserva)
        {

            var usuario = (from u in _parqueoContexto.Usuarios
                           join r in _parqueoContexto.Reservas on u.UsuarioID equals usuarioId
                           where u.UsuarioID == usuarioId && u.Rol == "Cliente"
                           select u).FirstOrDefault();

            if (usuario == null)
            {
                return Unauthorized("El usuario no está autorizado para reservar.");
            }


            reserva.UsuarioID = usuarioId;


            _parqueoContexto.Reservas.Add(reserva);
            _parqueoContexto.SaveChanges();

            return Ok(reserva);
        }


        [HttpGet]
        [Route("ReservasActivas/{usuarioId}")]
        public IActionResult ObtenerReservasActivas(int usuarioId)
        {
            var reservasActivas = from r in _parqueoContexto.Reservas
                                  where r.UsuarioID == usuarioId && r.Estado == true
                                  select r;

            if (!reservasActivas.Any())
            {
                return NotFound("No hay reservas activas de este usuario");
            }

            return Ok(reservasActivas.ToList());
        }




        [HttpPut]
        [Route("Cancelar/{reservaId}")]
        public IActionResult CancelarReserva(int reservaId)
        {

            Reservas? reserva = (from r in _parqueoContexto.Reservas
                                 where r.ReservaID == reservaId
                                 select r).FirstOrDefault();

            if (reserva == null)
            {
                return NotFound("No se encontró la reserva");
            }


            if (DateTime.Now >= reserva.FechaReserva)
            {
                return BadRequest("No se puede cancelar la reserva porque ya paso la hora");
            }


            reserva.Estado = false;


            _parqueoContexto.Entry(reserva).State = EntityState.Modified;
            _parqueoContexto.SaveChanges();

            return Ok("Reserva Cancelada");

        }



        [HttpGet]
        [Route("espacios/por-dia/{fecha}")]
        public IActionResult GetEspaciosReservadosPorDia(DateTime fecha)
        {
            var espaciosReservados = (from r in _parqueoContexto.Reservas
                                      join e in _parqueoContexto.EspaciosParqueo
                                      on r.EspacioID equals e.EspacioID
                                      join s in _parqueoContexto.Sucursales
                                      on e.SucursalID equals s.SucursalID
                                      where r.FechaReserva.Date == fecha.Date && r.Estado == true
                                      select new
                                      {
                                          r.ReservaID,
                                          Espacio = e.Numero,
                                          Sucursal = s.Nombre,
                                          FechaReserva = r.FechaReserva
                                      }).ToList();

            if (espaciosReservados.Count == 0)
            {
                return NotFound("No hay espacios reservados en esta fecha.");
            }

            return Ok(espaciosReservados);
        }



        [HttpGet]
        [Route("reservas/entre-fechas/{sucursalId}/{fechaInicio}/{fechaFin}")]
        public IActionResult GetEspaciosReservadosEntreFechas(int sucursalId, DateTime fechaInicio, DateTime fechaFin)
        {
            var espaciosReservados = (from r in _parqueoContexto.Reservas
                                      join e in _parqueoContexto.EspaciosParqueo
                                      on r.EspacioID equals e.EspacioID
                                      join s in _parqueoContexto.Sucursales
                                      on e.SucursalID equals s.SucursalID
                                      where s.SucursalID == sucursalId && r.FechaReserva >= fechaInicio && r.FechaReserva <= fechaFin && r.Estado == true
                                      select new
                                      {
                                          r.ReservaID,
                                          Espacio = e.Numero,
                                          Sucursal = s.Nombre,
                                          FechaReserva = r.FechaReserva
                                      }).ToList();

            if (espaciosReservados.Count == 0)
            {
                return NotFound("No hay espacios reservados en estas fechas");
            }

            return Ok(espaciosReservados);
        }





    }
}
