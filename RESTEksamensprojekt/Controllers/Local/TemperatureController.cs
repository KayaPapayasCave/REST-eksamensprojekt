using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary.Models;
using ClassLibrary.Interfaces.Local;

namespace RESTEksamensprojekt.Controllers.Local
{
    /// <summary>
    /// API controller providing CRUD endpoints for temperature data.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        private readonly ITemperatureRepository repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemperatureController"/> class
        /// using dependency injection.
        /// </summary>
        /// <param name="repository">The temperature repository used for data access.</param>
        public TemperatureController(TemperatureRepository repository)
        {
            repo = repository;
        }

        /// <summary>
        /// Retrieves all stored temperature measurements.
        /// </summary>
        /// <returns>
        /// 200 OK with a list of temperatures if data exists;
        /// 204 No Content if the repository is empty.
        /// </returns>
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

        /// <summary>
        /// Retrieves a specific temperature measurement by id.
        /// </summary>
        /// <param name="id">The id of the temperature record.</param>
        /// <returns>
        /// 200 OK if found; 404 Not Found otherwise.
        /// </returns>
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

        /// <summary>
        /// Creates a new temperature record.
        /// </summary>
        /// <param name="value">The temperature data to store.</param>
        /// <returns>
        /// 201 Created with the created record; 400 Bad Request if the input is invalid.
        /// </returns>
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

        /// <summary>
        /// Updates an existing temperature record.
        /// </summary>
        /// <param name="id">The id of the record to update.</param>
        /// <param name="value">The updated temperature values.</param>
        /// <returns>
        /// 200 OK with updated data, 404 Not Found if no record exists,
        /// or 400 Bad Request on error.
        /// </returns>
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

        /// <summary>
        /// Deletes a temperature record by id.
        /// </summary>
        /// <param name="id">The id of the record.</param>
        /// <returns>
        /// 200 OK with the deleted item; 404 Not Found if nothing was deleted.
        /// </returns>
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