using Contoso.Spaces.Models;
using Contoso.Spaces.Web.Configuration;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contoso.Spaces.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IOptions<ConnectionData> _connectionData;

        public IndexModel(IOptions<ConnectionData> connectionData)
        {
            _connectionData = connectionData;
        }

        [BindProperty]
        public List<Location> FeaturedLocations { get; set; }

        public async Task OnGetAsync()
        {
            string api = _connectionData.Value.GetFeaturedLocationsApiUrl;

            FeaturedLocations = await api.GetJsonAsync<List<Location>>();
        }
    }
}