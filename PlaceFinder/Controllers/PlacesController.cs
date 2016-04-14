using Microsoft.Azure.Search.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PlaceFinder.Models;
using PlaceFinder.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PlaceFinder.Controllers
{
    [RoutePrefix("api/places")]
    public class PlacesController : ApiController
    {
        private readonly PlaceService _placesService;

        public PlacesController()
        {
            _placesService = new PlaceService();
        }

        [Route("search")]
        [HttpGet]
        public async Task<DocumentSearchResult<Place>> Search(string query, bool typeFacet = true)
        {
            var places = await _placesService.Search(query, typeFacet);
            return places;
        }

        [Route("Suggest")]
        [HttpGet]
        public async Task<List<string>> Suggest(string term, bool fuzzy = true)
        {
            // Call suggest query and return results
            var response = await _placesService.Suggest(term, fuzzy);
            List<string> suggestions = new List<string>();
            foreach (var result in response.Results)
            {
                suggestions.Add(result.Text);
            }

            // Only return unique suggestions
            var distinctSuggestions = (from w in suggestions
                                       select w).Distinct().ToList();
            return distinctSuggestions;
        }

    }

}
