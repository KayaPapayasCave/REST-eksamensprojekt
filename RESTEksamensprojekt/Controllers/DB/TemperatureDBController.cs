using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary.Models;
using ClassLibrary.Interfaces.DB;

namespace RESTEksamensprojekt.Controllers.DB
{
    /// <summary>
    /// API controller providing CRUD endpoints for temperature data using a database repository.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureDBController : ControllerBase
    {
        private readonly ITemperatureRepositoryDB repo;

        /// <summary>
        /// Initializes a new instance of <see cref="TemperatureDBController"/> with dependency injection.
        /// </summary>
        /// <param name="repository">The temperature database repository.</param>
        public TemperatureDBController(ITemperatureRepositoryDB repository)
        {
            repo = repository;
        }

        /// <summary>
        /// Retrieves all temperature records from the database.
        /// </summary>
        /// <returns>
        /// 200 OK with list of temperature records; 204 No Content if empty.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                List<Temperature> result = await repo.GetAllAsync();
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
        /// Retrieves a temperature record by its ID.
        /// </summary>
        /// <param name="id">The ID of the temperature record.</param>
        /// <returns>
        /// 200 OK with the record if found; 404 Not Found otherwise.
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                Temperature? result = await repo.GetByIdAsync(id);
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

        /// <summary>
        /// Creates a new temperature record in the database.
        /// </summary>
        /// <param name="value">The temperature data to store.</param>
        /// <returns>
        /// 201 Created with the new record; 400 Bad Request if input is invalid.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody] Temperature value)
        {
            try
            {
                Temperature? created = await repo.AddTemperatureAsync(value);
                string uri = $"{Request.Path}/{created?.Id}";
                return Created(uri, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a temperature record by ID.
        /// </summary>
        /// <param name="id">The ID of the temperature record to delete.</param>
        /// <returns>
        /// 200 OK with deleted record; 404 Not Found if record does not exist.
        /// </returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Temperature? deleted = await repo.DeleteTemperatureAsync(id);
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
