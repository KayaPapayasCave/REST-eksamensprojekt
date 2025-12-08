using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary.Models;
using ClassLibrary.Interfaces.Local;
using ClassLibrary.Services.Local;

namespace RESTEksamensprojekt.Controllers.Local
{
    /// <summary>
    /// API controller providing CRUD endpoints for light data.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LightController : ControllerBase
    {
        private readonly ILightRepository repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightController"/> class
        /// using dependency injection.
        /// </summary>
        /// <param name="repository">The light repository used for data access.</param>
        public LightController(LightRepository repository)
        {
            repo = repository;
        }

        /// <summary>
        /// Retrieves all stored light measurements.
        /// </summary>
        /// <returns>
        /// 200 OK with a list of light measurements if data exists;
        /// 204 No Content if the repository is empty.
        /// </returns>
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

        /// <summary>
        /// Retrieves a specific light measurement by ID.
        /// </summary>
        /// <param name="id">The ID of the light record.</param>
        /// <returns>
        /// 200 OK with the record if found; 404 Not Found otherwise.
        /// </returns>
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

        /// <summary>
        /// Creates a new light record.
        /// </summary>
        /// <param name="value">The light data to store.</param>
        /// <returns>
        /// 201 Created with the created record; 400 Bad Request if input is invalid.
        /// </returns>
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

        /// <summary>
        /// Updates an existing light record.
        /// </summary>
        /// <param name="id">The ID of the record to update.</param>
        /// <param name="value">The updated light values.</param>
        /// <returns>
        /// 200 OK with updated data; 404 Not Found if the record does not exist;
        /// 400 Bad Request on error.
        /// </returns>
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

        /// <summary>
        /// Deletes a light record by ID.
        /// </summary>
        /// <param name="id">The ID of the record.</param>
        /// <returns>
        /// 200 OK with the deleted record; 404 Not Found if nothing was deleted.
        /// </returns>
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
