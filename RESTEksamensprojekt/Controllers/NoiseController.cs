using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary;

namespace RESTEksamensprojekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoiseController : ControllerBase
    {
        private readonly NoiseRepository repo;

        // Dependency Injection via Constructor
        public NoiseController(NoiseRepository repository)
        {
            this.repo = repository;
        }

        // GET: api/<NoiseController>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public ActionResult<IEnumerable<Noise>> Get()
        {
            List<Noise> result = repo.GetAll();
            if (result.Count == 0)
                return NoContent();
            else
                return Ok(result);
        }

        // GET api/<NoiseController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Noise> Get(int id)
        {
            Noise? result = repo.GetById(id);
            if (result == null)
                return NotFound($"Ingen noise med id: {id}");
            else
                return Ok(result);
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
            catch (ArgumentException ex)
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
            catch (ArgumentException ex)
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
            Noise? deleted = repo.DeleteNoise(id);

            if (deleted == null)
                return NotFound($"Ingen noise med id {id}");
            else
                return Ok(deleted);
        }
    }
}
