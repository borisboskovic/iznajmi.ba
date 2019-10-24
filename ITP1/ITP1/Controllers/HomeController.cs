using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ITP1.Models;
using ITP1.Data.Models;
using ITP1.Data;
using System.Globalization;

namespace ITP1.Controllers
{
    public class HomeController : Controller
    {
        private readonly INekretnina _nekretnina;
        public HomeController(INekretnina nekretnina)
        {
            _nekretnina = nekretnina;
        }

        public IActionResult Index(PocetnaModel pModel)
        {
            Pager pager;
            IEnumerable<Nekretnina> nekretnine;
            if (pModel.Filter != null || pModel.NaciniIznajmljivanja != null || pModel.SviTipovi != null || (!string.IsNullOrWhiteSpace(pModel.SearchString)))
            {


                DateTime dtOd = DateTime.MinValue;
                DateTime dtDo = DateTime.MaxValue;
                DateTime.TryParseExact(pModel.Filter.DostupnoOdString, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtOd);
                pModel.Filter.DostupnoOd = dtOd;
                DateTime.TryParseExact(pModel.Filter.DostupnoDoString, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtDo);
                pModel.Filter.DostupnoDo = dtDo;

                pModel.SearchString = pModel.SearchString == null ? "" : pModel.SearchString;

                pager = new Pager(_nekretnina.CountNekretnineWithFilters(pModel), pModel.CurrPage );
                if (pModel.CurrPage == 0)
                    pager.CurrentPage = pager.EndPage;

                nekretnine = _nekretnina.GetAllNekretnineWithFilters(pager.CurrentPage, pager.PageSize, pModel);

            }
            else 
            {
                pager = new Pager(_nekretnina.CountNekretnine(), pModel.CurrPage);
                if (pModel.CurrPage == 0)
                    pager.CurrentPage = pager.EndPage;
                
                nekretnine = _nekretnina.GetNekretnine(pager.CurrentPage, pager.PageSize);              
            }

            List<NekretninaItem> nekretnine_item = new List<NekretninaItem>();
            foreach (var item in nekretnine)
            {
                NekretninaItem nekretnina_item = new NekretninaItem()
                {
                    Id = item.Id,
                    Cijena = item.Cijena,
                    Naslov = item.Naslov,
                    Korisik = item.Korisnik,
                    Lokacija = item.Lokacija,
                    Povrsina = item.Povrsina,
                    DostupnoOd = item.DostupnoOd,
                    DostupnoDo = item.DostupnoDo,
                    CoverImgUrl = item.CoverImg == null ? null : item.CoverImg.Url,
                    Tip = new TipModel()
                    {
                        Id = item.Tip == null ? 0 : item.Tip.Id,
                        ImeTipa = item.Tip == null ? null : item.Tip.ImeTipa
                    },
                    NacinIznajmljivanja = new NacinIznajmljivanjaModel()
                    {
                        Id = item.NacinIznajmljivanja == null ? 0 : item.NacinIznajmljivanja.Id,
                        Naziv = item.NacinIznajmljivanja == null ? null : item.NacinIznajmljivanja.Naziv,
                    },
                };
                nekretnine_item.Add(nekretnina_item);
            }

            IEnumerable<Tip> tipovi = _nekretnina.GetAllTipoviFiltera();
            List<TipModel> tipoviModel = new List<TipModel>();
            foreach (var item in tipovi)
            {
                var tipModel = new TipModel()
                {
                    Id = item.Id,
                    ImeTipa = item.ImeTipa,
                    Selected = pModel.SviTipovi == null ? true : pModel.SviTipovi.Where(st => st.Id == item.Id).FirstOrDefault().Selected,
                };

                tipoviModel.Add(tipModel);
            }
            IEnumerable<NacinIznajmljivanja> naciniIzn = _nekretnina.GetAllNaciniIznajmljivanja();
            List<NacinIznajmljivanjaModel> naciniIznModel = new List<NacinIznajmljivanjaModel>();
            foreach (var item in naciniIzn)
            {
                var nacinIznMod = new NacinIznajmljivanjaModel()
                {
                    Id = item.Id,
                    Naziv = item.Naziv,
                    Selected = pModel.NaciniIznajmljivanja == null ? true : pModel.NaciniIznajmljivanja.Where(ni => ni.Id == item.Id).FirstOrDefault().Selected,
                };
                naciniIznModel.Add(nacinIznMod);
            }

            PocetnaModel pocetnaModel = new PocetnaModel()
            {
                Nekretnine = nekretnine_item,
                Pager = pager,
                SviTipovi = tipoviModel,
                NaciniIznajmljivanja = naciniIznModel,
            };


            return View(pocetnaModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
