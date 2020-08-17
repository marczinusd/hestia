using System.Collections.Generic;
using Hestia.DAL.Interfaces;
using Hestia.Model.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hestia.WebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SnapshotsController : ControllerBase
    {
        private readonly ISnapshotRetrieval _snapshotRetrieval;
        private readonly IFileRetrieval _fileRetrieval;

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
            Ok(_snapshotRetrieval.GetAllSnapshotsHeaders());

        /// <summary>
        ///     Looks up a repository by id.
        /// </summary>
        /// <param name="id">id of the repository to lookup.</param>
        /// <returns>Full details of a repository with the id provided.</returns>
        /// <response code="404">Returns 404 when a repository for id was not found.</response>
        [HttpGet("[Controller]/{id}")]
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
        [HttpGet("[Controller]/{id}/files/{fileId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IFileEntity), StatusCodes.Status200OK)]
        public ActionResult<IFileEntity> GetFileDetailsById(string id, string fileId) =>
            _fileRetrieval.GetFileDetails(fileId, id)
                          .Match<ActionResult>(Ok, NotFound);

        /// <summary>
        ///     Fetches all file headers for given snapshot.
        /// </summary>
        /// <param name="id">Snapshot id.</param>
        /// <returns>Returns 404 if snapshot id is invalid.</returns>
        [HttpGet("[Controller]/{id}/files")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IEnumerable<IFileHeader>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<IFileHeader>> GetAllFileHeaders(string id) =>
            _snapshotRetrieval.GetSnapshotById(id)
                              .Match<ActionResult>(file => Ok(file.Files),
                                                   NotFound);
    }
}
