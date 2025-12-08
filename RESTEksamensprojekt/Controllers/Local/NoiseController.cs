using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary.Models;
using ClassLibrary.Interfaces.Local;

namespace RESTEksamensprojekt.Controllers.Local
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoiseController : ControllerBase
    {
        private readonly INoiseRepository repo;

        // Dependency Injection via Constructor
        public NoiseController(INoiseRepository repository)
        {
            repo = repository;
        }

        // GET: api/<NoiseController>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public ActionResult<IEnumerable<Noise>> Get()
        {
            try
            { 
                List<Noise> result = repo.GetAll();
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

        // GET api/<NoiseController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Noise> Get(int id)
        {
            try
            { 
                Noise? result = repo.GetById(id);
                if (result == null)
                    return NotFound($"Ingen noise med id: {id}");
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<NoiseController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Noise> Post([FromBody] Noise value)
        {
            try
            {
                Noise? created = repo.AddNoise(value);
                string uri = $"{Request.Path}/{created?.Id}";
                return Created(uri, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<NoiseController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Noise> Put(int id, [FromBody] Noise value)
        {
            try
            {
                value.Id = id;
                Noise? updated = repo.UpdateNoise(value);

                if (updated == null)
                    return NotFound($"Ingen noise med id: {id}");
                else
                    return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<NoiseController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Noise> Delete(int id)
        {
            try
            { 
                Noise? deleted = repo.DeleteNoise(id);

                if (deleted == null)
                    return NotFound($"Ingen noise med id {id}");
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
