using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary.Models;
using ClassLibrary.Interfaces.DB;

namespace RESTEksamensprojekt.Controllers.DB
{
    [Route("api/[controller]")]
    [ApiController]
    public class LightDBController : ControllerBase
    {
        private readonly ILightRepositoryDB repo;

        // Dependency Injection via Constructor
        public LightDBController(ILightRepositoryDB repository)
        {
            repo = repository;
        }

        // GET: api/<LightController>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> GetAll()
        {
            try
            { 
                List<Light> result = await repo.GetAllAsync();
                if (result.Count == 0)
                    return NoContent();
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<LightController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get(int id)
        {
            try
            { 
                Light? result = await repo.GetByIdAsync(id);
                if (result == null)
                    return NotFound($"Ingen light med id: {id}");
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<LightController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody] Light value)
        {
            try
            {
                Light? created = await repo.AddLightAsync(value);
                string uri = $"{Request.Path}/{created?.Id}";
                return Created(uri, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<LightController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            { 
                Light? deleted = await repo.DeleteLightAsync(id);

                if (deleted == null)
                    return NotFound($"Ingen light med id {id}");
                else
                    return Ok(deleted);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
