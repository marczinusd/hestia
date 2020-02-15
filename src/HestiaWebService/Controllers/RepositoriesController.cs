using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hestia.DAL;
using Hestia.Model;
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

        [HttpGet]
        public ActionResult<IEnumerable<Repository>> GetAllRepositories()
        {
            _logger.LogDebug($"Invoking GET on {typeof(RepositoriesController).Name}");

            return _context.Repositories.ToArray();
        }

        [HttpGet("{id}")]
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
