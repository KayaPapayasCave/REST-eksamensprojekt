using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary.Models;
using ClassLibrary.Interfaces.DB;

namespace RESTEksamensprojekt.Controllers.DB
{
    /// <summary>
    /// API controller providing CRUD endpoints for light data using a database repository.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LightDBController : ControllerBase
    {
        private readonly ILightRepositoryDB repo;

        /// <summary>
        /// Initializes a new instance of <see cref="LightDBController"/> with dependency injection.
        /// </summary>
        /// <param name="repository">The light database repository.</param>
        public LightDBController(ILightRepositoryDB repository)
        {
            repo = repository;
        }

        /// <summary>
        /// Retrieves all light records from the database.
        /// </summary>
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

        /// <summary>
        /// Retrieves a light record by its ID.
        /// </summary>
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

        /// <summary>
        /// Creates a new light record in the database.
        /// </summary>
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

        /// <summary>
        /// Deletes a light record by ID.
        /// </summary>
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
