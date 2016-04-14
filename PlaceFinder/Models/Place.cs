using Microsoft.Azure.Search.Models;
using Microsoft.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlaceFinder.Models
{
    [SerializePropertyNamesAsCamelCase]
    public class Place
    {
        public string PlaceId { get; set; }
        public string NameBg { get; set; }
        public string NameEn { get; set; }
        public GeographyPoint Location { get; set; }
        public string PlaceType { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Note { get; set; }
    }
}