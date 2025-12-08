using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary.Models;
using ClassLibrary.Interfaces.DB;

namespace RESTEksamensprojekt.Controllers.DB
{
    /// <summary>
    /// API controller providing CRUD endpoints for humidity data using a database repository.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HumidityDBController : ControllerBase
    {
        private readonly IHumidityRepositoryDB repo;

        /// <summary>
        /// Initializes a new instance of <see cref="HumidityDBController"/> with dependency injection.
        /// </summary>
        /// <param name="repository">The humidity database repository.</param>
        public HumidityDBController(IHumidityRepositoryDB repository)
        {
            repo = repository;
        }

        /// <summary>
        /// Retrieves all humidity records from the database.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                List<Humidity> result = await repo.GetAllAsync();
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
        /// Retrieves a humidity record by its ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                Humidity? result = await repo.GetByIdAsync(id);
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

        /// <summary>
        /// Creates a new humidity record in the database.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody] Humidity value)
        {
            try
            {
                Humidity? created = await repo.AddHumidityAsync(value);
                string uri = $"{Request.Path}/{created?.Id}";
                return Created(uri, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a humidity record by ID.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Humidity? deleted = await repo.DeleteHumidityAsync(id);
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
