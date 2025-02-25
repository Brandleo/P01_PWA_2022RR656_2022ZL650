using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_2022RR656_2022ZL650.Models;

namespace P01_2022RR656_2022ZL650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {


        public readonly BaseParqueoContext _parqueoContexto;



        public UsuariosController(BaseParqueoContext parqueoContext)
        {
            _parqueoContexto = parqueoContext;


        }


        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get()
        {
            List<Usuarios> listadoUsuarios = (from u in _parqueoContexto.Usuarios select u).ToList();


            if (listadoUsuarios.Count == 0)
            {
                return NotFound("No hay registros de usuarios en las base de datos");
            }

            return Ok(listadoUsuarios);

        }

        [HttpPost]
        [Route("Add")]


        public IActionResult Guardarusuario([FromBody] Usuarios usuario)
        {
            try
            {

                _parqueoContexto.Usuarios.Add(usuario);
                _parqueoContexto.SaveChanges();
                return Ok(usuario);


            }

            catch (Exception ex)
            {


                return BadRequest(ex.Message);

            }

        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarUsuario(int id, [FromBody] Usuarios usuarioModificar)
        {
            Usuarios? usuarioActual = (from u in _parqueoContexto.Usuarios
                                       where u.UsuarioID == id
                                       select u).FirstOrDefault();


            if (usuarioActual == null) { return NotFound(); }

            usuarioActual.Nombre = usuarioModificar.Nombre;
            usuarioActual.Correo = usuarioModificar.Correo;
            usuarioActual.Contrasena = usuarioModificar.Contrasena;
            usuarioActual.Telefono = usuarioModificar.Telefono;
            usuarioActual.Rol = usuarioModificar.Rol;





            _parqueoContexto.Entry(usuarioActual).State = EntityState.Modified;
            _parqueoContexto.SaveChanges();

            return Ok(usuarioModificar);


        }

        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult eliminarUsuario(int id)
        {

            Usuarios? usuario = (from u in _parqueoContexto.Usuarios
                                 where u.UsuarioID == id
                                 select u).FirstOrDefault();


            if (usuario == null)
            {
                return NotFound();
            }


            _parqueoContexto.Usuarios.Attach(usuario);
            _parqueoContexto.Usuarios.Remove(usuario);
            _parqueoContexto.SaveChanges();

            return Ok(usuario);


        }




        [HttpPost]
        [Route("Logueo/{correo}/{contrasena}")]
        public IActionResult IniciarSesion(string correo, string contrasena)
        {
            Usuarios? usuario = (from u in _parqueoContexto.Usuarios
                                 where u.Correo == correo && u.Contrasena == contrasena
                                 select u).FirstOrDefault();

            if (usuario == null)
            {
                return Unauthorized("Credenciales inválidas");
            }

            return Ok("Credenciales válidas");
        }



    }

}