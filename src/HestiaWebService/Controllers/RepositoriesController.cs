using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hestia.DAL;
using Hestia.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hestia.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RepositoriesController : ControllerBase
    {
        private readonly HestiaContext _context;
        private readonly ILogger<RepositoriesController> _logger;

        public RepositoriesController([NotNull] HestiaContext context, ILogger<RepositoriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        ///     Gets all available repository ids and names.
        /// </summary>
        /// <returns>Full list of repository ids and names.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<RepositoryIdentifier>> GetAllRepositories()
        {
            _logger.LogDebug($"Invoking GET on {typeof(RepositoriesController).Name}");

            var identifiers = _context.Repositories
                                      .ToArray()
                                      .Select(repo => repo.AsRepositoryIdentifier());

            return new ActionResult<IEnumerable<RepositoryIdentifier>>(identifiers);
        }

        /// <summary>
        ///     Looks up a repository by id.
        /// </summary>
        /// <param name="id">id of the repository to lookup.</param>
        /// <returns>Full details of a repository with the id provided.</returns>
        /// <response code="404">Returns 404 when a repository for id was not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Repository), 200)]
        public async Task<ActionResult<Repository>> GetRepositoryById(long id)
        {
            _logger.LogDebug($"Invoking GET by id with id=${id} on {typeof(RepositoriesController).Name}");
            var repository = await _context.Repositories.FindAsync(id);
            if (repository == null)
            {
                _logger.LogDebug("Repository with {id} not found.");
                return NotFound();
            }

            return repository;
        }
    }
}
