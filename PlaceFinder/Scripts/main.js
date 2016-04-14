var viewModel = {
    places: ko.observableArray([]),
    typeFacets: ko.observableArray([]),
    placesCount: ko.observable(0),
    searchParams: {
        query: ko.observable(""),
        placeType: ko.observable(null)
    }
};

var map = null;

function Search() {
    map.setView({
        center: { latitude: 42.69562247634483, longitude: 23.322418397495518 },
        zoom: 12
    });

    $.get("api/places/search",
       ko.toJS(viewModel.searchParams),
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



viewModel.searchParams.query.subscribe(function () {
    //reset it
    viewModel.searchParams.placeType(null);
});
viewModel.searchParams.placeType.subscribe(Search);

$(function () {
    ko.applyBindings(viewModel);

    map = new Microsoft.Maps.Map(document.getElementById("jobs-page-map"),
           {
               credentials: "AhcILuRioQUg100rFMWF6SFQLjkw-jEW5cmz29E2RZnMp7BbhPxrwE0U5fOyxGv2",
               center: { latitude: 42.69562247634483, longitude: 23.322418397495518 },
               zoom: 12
           });

    viewModel.places.subscribe(function (places) {
        map.entities.clear();
        for (var i = 0; i < places.length; i++) {
            var pushpin = new Microsoft.Maps.Pushpin(places[i].location, null);
            map.entities.push(pushpin);
        }
    });

    $("#q").autocomplete({
        source: "/api/places/suggest",
        minLength: 2,
        select: function () {
            $("#q").trigger("change");
        }
    });

    $("body").on("click", ".facet-filter", function (e) {
        e.preventDefault();
        viewModel.searchParams.placeType($(this).data("facet"));
    });

    $("body").on("click", ".show-on-map", function (e) {
        e.preventDefault();
        var index = $(this).data("place-index");
        map.setView({
            center: viewModel.places()[index].location,
            zoom: 18
        });
    });

    Search();
});
