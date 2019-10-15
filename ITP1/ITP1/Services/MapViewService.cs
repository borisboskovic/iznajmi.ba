using ITP1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITP1.Services
{
    public class MapViewService:IMapView
    {
        private readonly ApplicationDbContext _db;

        public MapViewService(ApplicationDbContext db)
        {
            _db = db;
        }


    }
}
