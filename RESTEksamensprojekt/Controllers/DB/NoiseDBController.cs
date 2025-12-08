using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary.Models;
using ClassLibrary.Interfaces.DB;

namespace RESTEksamensprojekt.Controllers.DB
{
    /// <summary>
    /// API controller providing CRUD endpoints for noise data using a database repository.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NoiseDBController : ControllerBase
    {
        private readonly INoiseRepositoryDB repo;

        /// <summary>
        /// Initializes a new instance of <see cref="NoiseDBController"/> with dependency injection.
        /// </summary>
        /// <param name="repository">The noise database repository.</param>
        public NoiseDBController(INoiseRepositoryDB repository)
        {
            repo = repository;
        }

        /// <summary>
        /// Retrieves all noise records from the database.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                List<Noise> result = await repo.GetAllAsync();
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
        /// Retrieves a noise record by its ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                Noise? result = await repo.GetByIdAsync(id);
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
        /// Creates a new noise record in the database.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody] Noise value)
        {
            try
            {
                Noise? created = await repo.AddNoiseAsync(value);
                string uri = $"{Request.Path}/{created?.Id}";
                return Created(uri, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a noise record by ID.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Noise? deleted = await repo.DeleteNoiseAsync(id);
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
