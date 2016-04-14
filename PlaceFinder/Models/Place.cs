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
        public string Place_Id { get; set; }
        public string Name_BG { get; set; }
        public string Name_EN { get; set; }
        public GeographyPoint Location { get; set; }
        public string Place_Type { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Note { get; set; }
    }
}