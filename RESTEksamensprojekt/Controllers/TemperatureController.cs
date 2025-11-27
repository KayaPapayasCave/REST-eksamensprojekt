using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary;

namespace RESTEksamensprojekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        private readonly TemperatureRepository repo;

        // Dependency Injection via Constructor
        public TemperatureController(TemperatureRepository repository)
        {
            this.repo = repository;
        }

        // GET: api/<TemperatureController>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public ActionResult<IEnumerable<Temperature>> Get()
        {
            try
            {
                List<Temperature> result = repo.GetAll();
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

        // GET api/<TemperatureController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Temperature> Get(int id)
        {
            try
            { 
                Temperature? result = repo.GetById(id);
                if (result == null)
                    return NotFound($"Ingen temperature med id: {id}");
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<TemperatureController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Temperature> Post([FromBody] Temperature value)
        {
            try
            {
                Temperature? created = repo.AddTemperature(value);
                string uri = $"{Request.Path}/{created?.Id}";
                return Created(uri, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<TemperatureController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Temperature> Put(int id, [FromBody] Temperature value)
        {
            try
            {
                value.Id = id;
                Temperature? updated = repo.UpdateTemperature(value);

                if (updated == null)
                    return NotFound($"Ingen temperature med id: {id}");
                else
                    return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<TemperatureController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Temperature> Delete(int id)
        {
            try
            { 
                Temperature? deleted = repo.DeleteTemperature(id);

                if (deleted == null)
                    return NotFound($"Ingen temperature med id {id}");
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
