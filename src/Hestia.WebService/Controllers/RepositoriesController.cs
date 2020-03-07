using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hestia.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hestia.WebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RepositoriesController : ControllerBase
    {
        private readonly ILogger<RepositoriesController> _logger;

        public RepositoriesController(ILogger<RepositoriesController> logger)
        {
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

            throw new NotImplementedException();
        }

        /// <summary>
        ///     Looks up a repository by id.
        /// </summary>
        /// <param name="id">id of the repository to lookup.</param>
        /// <returns>Full details of a repository with the id provided.</returns>
        /// <response code="404">Returns 404 when a repository for id was not found.</response>
        [HttpGet("[Controller]/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RepositorySnapshot), 200)]
        public Task<ActionResult<RepositorySnapshot>> GetRepositoryById(long id)
        {
            _logger.LogDebug($"Invoking GET by id with id=${id} on {typeof(RepositoriesController).Name}");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Look up file details by repository and file id.
        /// </summary>
        /// <param name="repositoryId">Id of the repository that contains the file.</param>
        /// <param name="fileId">Id of the file.</param>
        /// <returns>File with the provided id.</returns>
        /// <response code="404">Returns 404 when a file for the provided id was not found.</response>
        [HttpGet("[Controller]/{repositoryId}/files/{fileId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(FileDetails), 200)]
        public Task<ActionResult<FileDetails>> GetFileById(long repositoryId, long fileId)
        {
            throw new NotImplementedException();
        }
    }
}
