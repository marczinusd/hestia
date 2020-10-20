using System.Collections.Generic;
using System.Linq;
using Hestia.DAL.Interfaces;
using Hestia.Model.Interfaces;
using Hestia.WebService.ServiceModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hestia.WebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SnapshotsController : ControllerBase
    {
        private readonly IFileRetrieval _fileRetrieval;
        private readonly ISnapshotRetrieval _snapshotRetrieval;

        public SnapshotsController(ISnapshotRetrieval snapshotRetrieval,
                                   IFileRetrieval fileRetrieval)
        {
            _snapshotRetrieval = snapshotRetrieval;
            _fileRetrieval = fileRetrieval;
        }

        /// <summary>
        ///     Gets all available repository ids and names.
        /// </summary>
        /// <returns>Full list of repository ids and names.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ISnapshotHeader>> GetAllSnapshots() =>
            Ok(_snapshotRetrieval.GetAllSnapshotsHeaders()
                                 .Select(SlimSnapshot.From));

        /// <summary>
        ///     Looks up a repository by id.
        /// </summary>
        /// <param name="id">id of the repository to lookup.</param>
        /// <returns>Full details of a repository with the id provided.</returns>
        /// <response code="404">Returns 404 when a repository for id was not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IRepositorySnapshotEntity), StatusCodes.Status200OK)]
        public ActionResult<IRepositorySnapshotEntity> GetSnapshotById(string id) =>
            _snapshotRetrieval.GetSnapshotById(id)
                              .Match<ActionResult>(Ok, NotFound);

        /// <summary>
        ///     Fetches complete file details by id.
        /// </summary>
        /// <param name="id">Id of the snapshot that contains the file.</param>
        /// <param name="fileId">Id of the file within the snapshot.</param>
        /// <returns>Returns 404 if file was not found.</returns>
        [HttpGet("{id}/files/{fileId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IFileEntity), StatusCodes.Status200OK)]
        public ActionResult<IFileEntity> GetFileDetailsById(string id, string fileId) =>
            _fileRetrieval.GetFileDetails(fileId, id)
                          .Match<ActionResult>(Ok, NotFound);

        [HttpGet("{id}/files/{fileId}/lines")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IFileEntity), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ILineEntity>> GetLinesForFile(string fileId) =>
            _snapshotRetrieval.FileExistsWithId(fileId)
                ? Ok(_snapshotRetrieval.GetFileContent(fileId))
                : NotFound() as ActionResult;

        /// <summary>
        ///     Fetches all file headers for given snapshot.
        /// </summary>
        /// <param name="id">Snapshot id.</param>
        /// <returns>Returns 404 if snapshot id is invalid.</returns>
        [HttpGet("{id}/files")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IEnumerable<IFileHeader>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<IFileHeader>> GetAllFileHeaders(string id) =>
            _snapshotRetrieval.GetSnapshotById(id)
                              .Match<ActionResult>(_ => Ok(_snapshotRetrieval.GetAllFilesForSnapshot(id)), NotFound());
    }
}
