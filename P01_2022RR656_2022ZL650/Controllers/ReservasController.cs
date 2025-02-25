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
                return Unauthorized("El usuario no está autorizado para hacer reservas.");
            }

            
            reserva.UsuarioID = usuarioId;

            
            _parqueoContexto.Reservas.Add(reserva);
            _parqueoContexto.SaveChanges();

            return Ok(reserva);
        }







    }
}
