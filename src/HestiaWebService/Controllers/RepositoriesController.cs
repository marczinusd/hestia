using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hestia.DAL;
using Hestia.DAL.DummyData;
using Hestia.DAL.Entities;
using Hestia.DAL.Extensions;
using Hestia.Model;
using LanguageExt;
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
            if (_context.Repositories == null || !_context.Repositories.Any())
            {
                _context.Setup();
            }

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
                                      .Select(r => r.MapEntityToModel()
                                                    .AsRepositoryIdentifier());

            return new ActionResult<IEnumerable<RepositoryIdentifier>>(identifiers);
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
        public async Task<ActionResult<RepositorySnapshot>> GetRepositoryById(long id)
        {
            _logger.LogDebug($"Invoking GET by id with id=${id} on {typeof(RepositoriesController).Name}");
            var repository = await _context.Repositories.FindAsync(id);
            if (repository == null)
            {
                // _context.Add(DataRepository.DummyRepository);
                // _context.SaveChanges();
                _logger.LogDebug("Repository with {id} not found.");
                return NotFound();
            }

            return repository.MapEntityToModel();
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
        public async Task<ActionResult<FileDetails>> GetFileById(long repositoryId, long fileId)
        {
            var repository = await _context.Repositories.FindAsync(repositoryId);

            return FindFileById(repository, fileId)
                   .Some(f => new ActionResult<FileDetails>(f.MapEntityToModel()
                                                             .AsFileDetails()))
                   .None(() => NotFound());
        }

        private Option<FileEntity> FindFileById(RepositoryEntity repository, long fileId)
        {
            return repository.RootDirectory.Files.First(f => f.Id == fileId);
        }
    }
}
