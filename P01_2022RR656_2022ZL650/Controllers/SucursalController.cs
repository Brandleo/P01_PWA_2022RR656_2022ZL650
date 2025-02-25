using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P01_2022RR656_2022ZL650.Models;

namespace P01_2022RR656_2022ZL650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SucursalController : ControllerBase
    {
        private readonly BaseParqueoContext _BaseParqueoContexto;
        public SucursalController(BaseParqueoContext BaseParqueoContext)
        {
            _BaseParqueoContexto = BaseParqueoContext;
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<Sucursales> Sucursales = (from e in _BaseParqueoContexto.Sucursales select e).ToList();
            if (Sucursales.Count() == 0)
            {
                return NotFound();
            }
            return Ok(Sucursales);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var sucursalConEspacios = (from s in _BaseParqueoContexto.Sucursales
                                       join e in _BaseParqueoContexto.EspaciosParqueo on s.SucursalID equals e.SucursalID into espaciosGrupo
                                       where s.SucursalID == id
                                       select new
                                       {
                                           Sucursal = s,
                                           Espacios = espaciosGrupo.ToList()
                                       }).FirstOrDefault();

            if (sucursalConEspacios == null)
            {
                return NotFound("No hay sucursal con es id");
            }

            return Ok(sucursalConEspacios);
        }
        [HttpPost]
        [Route("Add/sucursal")]
        public IActionResult GuardarSucursal([FromBody] Sucursales sucursal)
        {
            try
            {
                _BaseParqueoContexto.Sucursales.Add(sucursal);
                _BaseParqueoContexto.SaveChanges();
                return Ok(sucursal);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarSucursal(int id, [FromBody] Sucursales sucursalModificar)
        {
            Sucursales? SActual = (from s in _BaseParqueoContexto.Sucursales
                                 where s.SucursalID == id
                                 select s).FirstOrDefault();
            if (SActual == null)
            {
                return NotFound();
            }

            SActual.Nombre = sucursalModificar.Nombre;
            SActual.Direccion = sucursalModificar.Direccion;
            SActual.Telefono = sucursalModificar.Telefono;
            SActual.Administrador = sucursalModificar.Administrador;
            SActual.NumeroEspacios = sucursalModificar.NumeroEspacios;

            _BaseParqueoContexto.Entry(SActual).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _BaseParqueoContexto.SaveChanges();
            return Ok(SActual);
        }
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarSucursal(int id)
        {
            Sucursales? sucursal = (from s in _BaseParqueoContexto.Sucursales
                                  where s.SucursalID == id
                                  select s).FirstOrDefault();
            if (sucursal == null)
            {
                return NotFound();
            }

            _BaseParqueoContexto.Sucursales.Attach(sucursal);
            _BaseParqueoContexto.Sucursales.Remove(sucursal);
            _BaseParqueoContexto.SaveChanges();

            return Ok(sucursal);
        }








    }
}
