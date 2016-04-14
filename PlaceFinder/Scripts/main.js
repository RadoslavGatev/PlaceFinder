var viewModel = {
    places: ko.observableArray([]),
    typeFacets: ko.observableArray([]),
    placesCount: ko.observable(0)
}, map = null;

function Search() {
    var query = $("#q").val();

    $.get("api/places/search",
        {
            query: query,
            //facets filter
        },
        function (data) {
            var places = $.map(data.results, function (result) {
                return result.document;
            });

            viewModel.places(places);
            viewModel.placesCount(data.count);

            if (data.facets != null) {
                var typeFacets = data.facets.place_type;
                typeFacets = $.map(typeFacets, function (facet) {
                    facet.text = facet.value + " (" + facet.count + ")";
                    return facet;
                });

                viewModel.typeFacets(typeFacets);
            }
        });
}

viewModel.places.subscribe(function (places) {
    map.entities.clear();
    for (var i = 0; i < places.length; i++) {
        var pushpin = new Microsoft.Maps.Pushpin(places[i].location, null);
        map.entities.push(pushpin);
    }
});

$(function () {
    ko.applyBindings(viewModel);

    $("#q").autocomplete({
        source: "/api/places/suggest",
        minLength: 2,
        select: function (event, ui) {
            Search();
        }
    });

    map = new Microsoft.Maps.Map(document.getElementById("jobs-page-map"),
           {
               credentials: "AhcILuRioQUg100rFMWF6SFQLjkw-jEW5cmz29E2RZnMp7BbhPxrwE0U5fOyxGv2",
               center: { latitude: 42.69562247634483, longitude: 23.322418397495518 },
               zoom: 12
           });
});
