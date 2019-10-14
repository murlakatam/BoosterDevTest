using System.Collections.Generic;
using System.Threading.Tasks;
using Booster.BlazorApp.Server.Data;
using Booster.Shared.Domain;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Booster.BlazorApp.Server.Hubs
{
    public class LoremIpsumHub : Microsoft.AspNetCore.SignalR.Hub
    {
        protected override void Dispose(bool disposing)
        {
            if (_statService != null)
            {
                _statService.StatsHasChanged -= OnStatHasChanged;
            }
            base.Dispose(disposing);
        }

        private readonly StatService _statService;
        private readonly ILogger _logger;

        public LoremIpsumHub(StatService statService, LoggerFactory loggerFactory)
        {
           
            _statService = statService;
            _logger = loggerFactory.CreateLogger<LoremIpsumHub>();
            _logger.LogDebug("SignalR Hub is initializing");
            _statService.StatsHasChanged += OnStatHasChanged;
        }

        private async Task OnStatHasChanged(Stats e)
        {
            _logger.LogInformation("Stats has changed");
            var obj = JsonConvert.SerializeObject(e);
            
            await Clients.All.SendAsync(nameof(OnStatHasChanged),  obj);
        }

        public async Task UploadLoremIpsumStream(IAsyncEnumerable<string> stream)
        {
            _logger.LogInformation("Incoming text stream");
            await _statService.UpdateStats(stream);
        }
    }
}
