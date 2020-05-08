using Contoso.Spaces.Models;
using Contoso.Spaces.Web.Configuration;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contoso.Spaces.Web.Pages
{
    public class LocationsModel : PageModel
    {
        private readonly IOptions<ConnectionData> _connectionData;
        private readonly IOptions<ResourceData> _resourceData;

        public LocationsModel(IOptions<ConnectionData> connectionData, IOptions<ResourceData> resourceData)
        {
            _connectionData = connectionData;
            _resourceData = resourceData;
        }

        [BindProperty]
        public List<Location> AllLocations { get; set; }

        [BindProperty]
        public Location SpecificLocation { get; set; }

        [BindProperty]
        public string ImagePrefix { get; set; }

        public async Task OnGet(string id)
        {
            ImagePrefix = _resourceData.Value.BlobContainerUri;

            if (String.IsNullOrEmpty(id))
            {
                string api = _connectionData.Value.GetAllLocationsApiUrl;

                AllLocations = await api.GetJsonAsync<List<Location>>();
            }
            else
            {
                string api = _connectionData.Value.GetSpecificLocationApiUrl;

                SpecificLocation = await api.SetQueryParam("id", id).GetJsonAsync<Location>();
            }
        }
    }
}