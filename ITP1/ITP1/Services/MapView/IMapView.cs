using ITP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITP1.Services.MapView
{
    public interface IMapView
    {
        MapViewModel GetNekretninas(MapViewModel model);
    }
}
