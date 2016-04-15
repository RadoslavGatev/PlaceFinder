using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using PlaceFinder.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PlaceFinder.Services
{
    public class PlaceService
    {
        private readonly SearchIndexClient _indexClient;
        private readonly SearchServiceClient _searchClient;

        public PlaceService()
        {
            string searchServiceName = ConfigurationManager.AppSettings["SearchServiceName"];
            string apiKey = ConfigurationManager.AppSettings["SearchServiceApiKey"];
            string indexName = ConfigurationManager.AppSettings["SearchIndex"];

            // Create an HTTP reference to the catalog index
            _searchClient = new SearchServiceClient(searchServiceName,
                new SearchCredentials(apiKey));
            _indexClient = _searchClient.Indexes.GetClient(indexName);
        }

        public async Task<DocumentSearchResult<Place>> Search(string query, bool typeFacet,
            string typeFilter)
        {
            SearchParameters sp = new SearchParameters()
            {
                SearchMode = SearchMode.Any,
                Top = 100,
                // Add count
                IncludeTotalResultCount = true
            };

            if (typeFilter != null)
            {
                sp.Filter = "placeType eq '" + typeFilter + "'";
            }

            if (typeFacet)
            {
                sp.Facets = new List<string> { "placeType" };
            }

            var results = await _indexClient.Documents.SearchAsync<Place>(query, sp);
            return results;
        }

        public async Task<DocumentSuggestResult> Suggest(string searchText, bool fuzzy)
        {
            // Execute search based on query string
            SuggestParameters sp = new SuggestParameters()
            {
                UseFuzzyMatching = fuzzy,
                Top = 8
            };

            return await _indexClient.Documents.SuggestAsync(searchText,
                "place-suggester", sp);
        }
    }
}