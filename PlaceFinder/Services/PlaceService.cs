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
        private readonly string IndexName = "places";

        public PlaceService()
        {
            string searchServiceName = ConfigurationManager.AppSettings["SearchServiceName"];
            string apiKey = ConfigurationManager.AppSettings["SearchServiceApiKey"];

            // Create an HTTP reference to the catalog index
            _searchClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));
            _indexClient = _searchClient.Indexes.GetClient(IndexName);
        }

        public async Task<DocumentSearchResult<Place>> Search(string query, bool typeFacet)
        {
            SearchParameters sp = new SearchParameters()
            {
                SearchMode = SearchMode.Any,
                Top = 100,
                // Add count
                IncludeTotalResultCount = true,
                // Add search highlights
                //HighlightFields = new List<String>() { "Name_BG" },
                //HighlightPreTag = "<b>",
                //HighlightPostTag = "</b>"
            };

            if (typeFacet)
            {
                sp.Facets = new List<string> { "place_type" };
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

            return await _indexClient.Documents.SuggestAsync(searchText, "place-suggester", sp);
        }

    }
}