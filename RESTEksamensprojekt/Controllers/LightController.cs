using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary;

namespace RESTEksamensprojekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LightController : ControllerBase
    {
        private readonly LightRepository repo;

        // Dependency Injection via Constructor
        public LightController(LightRepository repository)
        {
            this.repo = repository;
        }

        // GET: api/<LightController>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public ActionResult<IEnumerable<Light>> Get()
        {
            try
            { 
                List<Light> result = repo.GetAll();
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
        public ActionResult<Light> Get(int id)
        {
            try
            { 
                Light? result = repo.GetById(id);
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
        public ActionResult<Light> Post([FromBody] Light value)
        {
            try
            {
                Light? created = repo.AddLight(value);
                string uri = $"{Request.Path}/{created?.Id}";
                return Created(uri, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<LightController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Light> Put(int id, [FromBody] Light value)
        {
            try
            {
                value.Id = id;
                Light? updated = repo.UpdateLight(value);

                if (updated == null)
                    return NotFound($"Ingen light med id: {id}");
                else
                    return Ok(updated);
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
        public ActionResult<Light> Delete(int id)
        {
            try
            { 
                Light? deleted = repo.DeleteLight(id);

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
