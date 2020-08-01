using System.Collections.Generic;
using Hestia.DAL.Interfaces;
using Hestia.Model.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hestia.WebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SnapshotsController : ControllerBase
    {
        private readonly ILogger<SnapshotsController> _logger;
        private readonly ISnapshotRetrieval _snapshotRetrieval;

        public SnapshotsController(ILogger<SnapshotsController> logger, ISnapshotRetrieval snapshotRetrieval)
        {
            _logger = logger;
            _snapshotRetrieval = snapshotRetrieval;
        }

        /// <summary>
        ///     Gets all available repository ids and names.
        /// </summary>
        /// <returns>Full list of repository ids and names.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ISnapshotHeader>> GetAllRepositories()
        {
            _logger.LogDebug($"Invoking GET on {nameof(SnapshotsController)}");

            return new ActionResult<IEnumerable<ISnapshotHeader>>(_snapshotRetrieval.GetAllSnapshotsHeaders());
        }

        /// <summary>
        ///     Looks up a repository by id.
        /// </summary>
        /// <param name="id">id of the repository to lookup.</param>
        /// <returns>Full details of a repository with the id provided.</returns>
        /// <response code="404">Returns 404 when a repository for id was not found.</response>
        [HttpGet("[Controller]/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IRepositorySnapshotEntity), StatusCodes.Status200OK)]
        public ActionResult<IRepositorySnapshotEntity> GetRepositoryById(string id)
        {
            _logger.LogDebug($"Invoking GET by id with id=${id} on {nameof(SnapshotsController)}");

            return new ActionResult<IRepositorySnapshotEntity>(_snapshotRetrieval.GetSnapshotById(id));
        }
    }
}
