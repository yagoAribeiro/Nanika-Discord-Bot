using Microsoft.AspNetCore.Mvc;
using NanikaAPI.Authorization;
using NanikaAPI.Models;
using NanikaAPI.Utils;

namespace NanikaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GachaController : ControllerBase
    {
        private readonly ILogger<GachaController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorization _authorization;

        public GachaController(ILogger<GachaController> logger, IHttpContextAccessor httpContext, IAuthorization authorization)
        {
            _logger = logger;
            _httpContextAccessor = httpContext;
            _authorization = authorization;
        }

        [HttpGet("pull")]
        public Task<ActionResult<string>> Get()
        {
            if (_authorization.IsAuthenticated(_httpContextAccessor))
            {
                return Task.FromResult(new ActionResult<string>("Unauthorized"));
            }
            RngCalculator<string> test = new(new List<RngSample<string>>() {
                new("Common", 500000, 0),
                new("Uncommon", 320000, 0),
                new("Rare", 100000, 0),
                new("Epic", 55000, 0.1),
                new("Exotic", 22000, 0.25),
                new("Legendary", 3000, 1.0)
            }, true);
            return Task.FromResult(new ActionResult<string>($"{test.Pull().Value}, {_httpContextAccessor.HttpContext?.Connection.RemoteIpAddress}"));
        }
    }
}
