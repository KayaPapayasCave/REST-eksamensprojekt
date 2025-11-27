using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary;

namespace RESTEksamensprojekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HumidityController : ControllerBase
    {
        private readonly HumidityRepository repo;

        // Dependency Injection via Constructor
        public HumidityController(HumidityRepository repository)
        {
            this.repo = repository;
        }

        // GET: api/<HumidityController>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public ActionResult<IEnumerable<Humidity>> Get()
        {
            try 
            { 
                List<Humidity> result = repo.GetAll();
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

        // GET api/<HumidityController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Humidity> Get(int id)
        {
            try
            { 
                Humidity? result = repo.GetById(id);
                if (result == null)
                    return NotFound($"Ingen humidity med id: {id}");
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<HumidityController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Humidity> Post([FromBody] Humidity value)
        {
            try
            {
                Humidity? created = repo.AddHumidity(value);
                string uri = $"{Request.Path}/{created?.Id}";
                return Created(uri, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<HumidityController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Humidity> Put(int id, [FromBody] Humidity value)
        {
            try
            {
                value.Id = id;
                Humidity? updated = repo.UpdateHumidity(value);

                if (updated == null)
                    return NotFound($"Ingen humidity med id: {id}");
                else
                    return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<HumidityController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Humidity> Delete(int id)
        {
            try
            {
                Humidity? deleted = repo.DeleteHumidity(id);

                if (deleted == null)
                    return NotFound($"Ingen humidity med id {id}");
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
