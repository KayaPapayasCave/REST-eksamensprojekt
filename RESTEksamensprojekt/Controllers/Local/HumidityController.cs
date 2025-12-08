using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary.Models;
using ClassLibrary.Interfaces.Local;
using ClassLibrary.Services.Local;

namespace RESTEksamensprojekt.Controllers.Local
{
    /// <summary>
    /// API controller providing CRUD endpoints for humidity data.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HumidityController : ControllerBase
    {
        private readonly IHumidityRepository repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="HumidityController"/> class
        /// using dependency injection.
        /// </summary>
        /// <param name="repository">The humidity repository used for data access.</param>
        public HumidityController(HumidityRepository repository)
        {
            repo = repository;
        }

        /// <summary>
        /// Retrieves all stored humidity measurements.
        /// </summary>
        /// <returns>
        /// 200 OK with a list of humidity measurements if data exists;
        /// 204 No Content if the repository is empty.
        /// </returns>
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

        /// <summary>
        /// Retrieves a specific humidity measurement by ID.
        /// </summary>
        /// <param name="id">The ID of the humidity record.</param>
        /// <returns>
        /// 200 OK with the record if found; 404 Not Found otherwise.
        /// </returns>
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

        /// <summary>
        /// Creates a new humidity record.
        /// </summary>
        /// <param name="value">The humidity data to store.</param>
        /// <returns>
        /// 201 Created with the created record; 400 Bad Request if input is invalid.
        /// </returns>
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

        /// <summary>
        /// Updates an existing humidity record.
        /// </summary>
        /// <param name="id">The ID of the record to update.</param>
        /// <param name="value">The updated humidity values.</param>
        /// <returns>
        /// 200 OK with updated data; 404 Not Found if the record does not exist;
        /// 400 Bad Request on error.
        /// </returns>
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

        /// <summary>
        /// Deletes a humidity record by ID.
        /// </summary>
        /// <param name="id">The ID of the record.</param>
        /// <returns>
        /// 200 OK with the deleted record; 404 Not Found if nothing was deleted.
        /// </returns>
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
