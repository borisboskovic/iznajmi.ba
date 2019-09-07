using System;
using System.Collections.Generic;
using ITP1.Data.Models;


namespace ITP1.Models
{
    public class Filter
    {
        public DateTime DostupnoOd { get; set; }
        public DateTime DostupnoDo { get; set; }
        public Double CijenaMin { get; set; }
        public Double CijenaMax { get; set; }
        public Double PovrsinaMin { get; set; }
        public Double PovrsinaMax { get; set; }

    }
}
