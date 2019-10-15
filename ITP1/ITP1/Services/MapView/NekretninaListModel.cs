using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITP1.Services.MapView
{
    public class NekretninaListModel
    {
        public int Id { get; set; }
        public string Naslov { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
