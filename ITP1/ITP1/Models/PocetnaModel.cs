using ITP1.Data.Models;
using System;
using System.Collections.Generic;

namespace ITP1.Models
{
    public class PocetnaModel
    {
        public List<NekretninaItem> Nekretnine { get; set; }

        public Pager Pager { get; set; }

        public Filter Filter { get; set; }

        //Ovo sluzi kao i filter u isto vrijeme
        public List<TipModel> SviTipovi { get; set; }
        public List<NacinIznajmljivanjaModel> NaciniIznajmljivanja { get; set; }
        //

        public int CurrPage { get; set; }
        public String SearchString { get; set; }
    }
}
