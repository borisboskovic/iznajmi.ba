using ITP1.Services.MapView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITP1.Models
{
    public class MapViewModel
    {
        public List<NekretninaListModel> Nekretnine { get; set; }
        public List<TipModel> SviTipovi { get; set; }
        public List<NacinIznajmljivanjaModel> NaciniIznajmljivanja { get; set; }
        public Filter Filter { get; set; }
        public string SearchString { get; set; }
    }
}
