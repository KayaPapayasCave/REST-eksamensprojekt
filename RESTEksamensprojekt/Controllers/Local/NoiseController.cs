using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary.Models;
using ClassLibrary.Interfaces.Local;
using ClassLibrary.Services.Local;

namespace RESTEksamensprojekt.Controllers.Local
{
    /// <summary>
    /// API controller providing CRUD endpoints for noise data.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NoiseController : ControllerBase
    {
        private readonly INoiseRepository repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoiseController"/> class
        /// using dependency injection.
        /// </summary>
        /// <param name="repository">The noise repository used for data access.</param>
        public NoiseController(INoiseRepository repository)
        {
            repo = repository;
        }

        /// <summary>
        /// Retrieves all stored noise measurements.
        /// </summary>
        /// <returns>
        /// 200 OK with a list of noise measurements if data exists;
        /// 204 No Content if the repository is empty.
        /// </returns>
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

        /// <summary>
        /// Retrieves a specific noise measurement by ID.
        /// </summary>
        /// <param name="id">The ID of the noise record.</param>
        /// <returns>
        /// 200 OK with the record if found; 404 Not Found otherwise.
        /// </returns>
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

        /// <summary>
        /// Creates a new noise record.
        /// </summary>
        /// <param name="value">The noise data to store.</param>
        /// <returns>
        /// 201 Created with the created record; 400 Bad Request if input is invalid.
        /// </returns>
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

        /// <summary>
        /// Updates an existing noise record.
        /// </summary>
        /// <param name="id">The ID of the record to update.</param>
        /// <param name="value">The updated noise values.</param>
        /// <returns>
        /// 200 OK with updated data; 404 Not Found if the record does not exist;
        /// 400 Bad Request on error.
        /// </returns>
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

        /// <summary>
        /// Deletes a noise record by ID.
        /// </summary>
        /// <param name="id">The ID of the record.</param>
        /// <returns>
        /// 200 OK with the deleted record; 404 Not Found if nothing was deleted.
        /// </returns>
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
