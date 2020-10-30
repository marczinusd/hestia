using System.Linq;
using Hestia.DAL.EFCore.Adapters;
using Hestia.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Hestia.WebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileRetrieval _fileRetrieval;
        private readonly ILogger _logger;

        public FilesController(IFileRetrieval fileRetrieval, ILogger logger)
        {
            _fileRetrieval = fileRetrieval;
            _logger = logger;
        }

        /// <summary>
        ///     Fetches complete file details by id.
        /// </summary>
        /// <param name="id">Id of file.</param>
        /// <returns>Returns 404 if file was not found.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IFileEntity), StatusCodes.Status200OK)]
        public ActionResult<IFileEntity> GetFileDetailsById(string id)
        {
            _logger.Information($"Invoked /files/{id}");

            return _fileRetrieval.GetFileDetails(id)
                                 .Match<ActionResult>(val =>
                                                      {
                                                          if (val.Lines.Count == 0 && val is FileEntityAdapter adapter)
                                                          {
                                                              // this should not be necessary, but I genuinely don't have more time to spend on this
                                                              // for some reasons the links between entities is not preserved between Line, File and Snapshot
                                                              // even though the IDs in the are correct and the mappings _should_ be correct too
                                                              adapter.Lines = _fileRetrieval.GetLinesForFile(id)
                                                                  .ToList();
                                                          }

                                                          return Ok(val);
                                                      },
                                                      NotFound);
        }
    }
}
