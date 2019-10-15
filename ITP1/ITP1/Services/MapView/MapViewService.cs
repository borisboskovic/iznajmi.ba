using ITP1.Data;
using ITP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITP1.Services.MapView
{
    public class MapViewService:IMapView
    {
        private readonly ApplicationDbContext _db;

        public MapViewService(ApplicationDbContext db)
        {
            _db = db;
        }

        public MapViewModel GetNekretninas(MapViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
