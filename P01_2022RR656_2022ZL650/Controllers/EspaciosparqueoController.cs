using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P01_2022RR656_2022ZL650.Models;

namespace P01_2022RR656_2022ZL650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspaciosparqueoController : ControllerBase
    {
        private readonly BaseParqueoContext _BaseParqueoContexto;
        public EspaciosparqueoController(BaseParqueoContext BaseParqueoContext)
        {
            _BaseParqueoContexto = BaseParqueoContext;
        }
        [HttpPost]
        [Route("Add/espacio")]
        public IActionResult GuardarEspacio([FromBody] EspaciosParqueo espacio)
        {
            try
            {
                _BaseParqueoContexto.EspaciosParqueo.Add(espacio);
                _BaseParqueoContexto.SaveChanges();
                return Ok(espacio);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetDisponibles")]
        public IActionResult GetEspaciosDisponibles()
        {
            var espacios = (from e in _BaseParqueoContexto.EspaciosParqueo
                                              join ee in _BaseParqueoContexto.Sucursales
                                              on e.SucursalID equals ee.SucursalID
                                              where e.Estado == "Disponible"
                                              select e).ToList();

            if (espacios.Count == 0)
            {
                return NotFound("no hay");
            }

            return Ok(espacios);
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEspacio(int id, [FromBody] EspaciosParqueo espacioModificado)
        {
            EspaciosParqueo? espacioActual = (from e in _BaseParqueoContexto.EspaciosParqueo
                                              where e.EspacioID == id
                                              select e).FirstOrDefault();

            if (espacioActual == null)
            {
                return NotFound();
            }

            espacioActual.Numero = espacioModificado.Numero;
            espacioActual.Ubicacion = espacioModificado.Ubicacion;
            espacioActual.CostoPorHora = espacioModificado.CostoPorHora;
            espacioActual.Estado = espacioModificado.Estado;

            _BaseParqueoContexto.Entry(espacioActual).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _BaseParqueoContexto.SaveChanges();

            return Ok(espacioActual);
        }
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarEspacio(int id)
        {
            EspaciosParqueo? espacio = (from e in _BaseParqueoContexto.EspaciosParqueo
                                        where e.EspacioID == id
                                        select e).FirstOrDefault();
            if (espacio == null)
            {
                return NotFound();
            }

            _BaseParqueoContexto.EspaciosParqueo.Attach(espacio);
            _BaseParqueoContexto.EspaciosParqueo.Remove(espacio);
            _BaseParqueoContexto.SaveChanges();

            return Ok(espacio);
        }
    }
}
