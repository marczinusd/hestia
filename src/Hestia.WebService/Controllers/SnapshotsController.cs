using System.Collections.Generic;
using System.Linq;
using Hestia.DAL.Interfaces;
using Hestia.Model.Interfaces;
using Hestia.WebService.ServiceModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Hestia.WebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SnapshotsController : ControllerBase
    {
        private readonly ISnapshotRetrieval _snapshotRetrieval;
        private readonly ILogger _logger;

        public SnapshotsController(ISnapshotRetrieval snapshotRetrieval, ILogger logger)
        {
            _snapshotRetrieval = snapshotRetrieval;
            _logger = logger;
        }

        /// <summary>
        ///     Gets all available repository ids and names.
        /// </summary>
        /// <returns>Full list of repository ids and names.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ISnapshotHeader>> GetAllSnapshots()
        {
            _logger.Information("Invoked GET /snapshots");

            return Ok(_snapshotRetrieval.GetAllSnapshotsHeaders()
                                        .Select(SlimSnapshot.From));
        }

        /// <summary>
        ///     Looks up a repository by id.
        /// </summary>
        /// <param name="id">id of the repository to lookup.</param>
        /// <returns>Full details of a repository with the id provided.</returns>
        /// <response code="404">Returns 404 when a repository for id was not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IRepositorySnapshotEntity), StatusCodes.Status200OK)]
        public ActionResult<IRepositorySnapshotEntity> GetSnapshotById(string id)
        {
            _logger.Information($"Invoked GET /snapshots/${id}");

            return _snapshotRetrieval.GetSnapshotById(id)
                                     .Match<ActionResult>(Ok, NotFound);
        }

        /// <summary>
        ///     Fetches all file headers for given snapshot.
        /// </summary>
        /// <param name="id">Snapshot id.</param>
        /// <returns>Returns 404 if snapshot id is invalid.</returns>
        [HttpGet("{id}/files")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IEnumerable<IFileHeader>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<IFileHeader>> GetAllFileHeaders(string id)
        {
            _logger.Information($"Invoked GET /snapshots/{id}/files");

            return _snapshotRetrieval.GetSnapshotById(id)
                                     .Match<ActionResult>(_ => Ok(_snapshotRetrieval.GetAllFilesForSnapshot(id)),
                                                          NotFound());
        }
    }
}
