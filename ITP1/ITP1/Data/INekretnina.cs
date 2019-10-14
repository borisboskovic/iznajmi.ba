﻿using ITP1.Data.Models;
using ITP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITP1.Data
{
    public interface INekretnina
    {
        Task AddNekretnina(NekretninaInsertModel insertModel);
        void UpdateNekretnina(NekretninaUpadeModel nekretnina);
        void DeleteNekretnina(int id);
        Nekretnina GetNekretnina(int id);
        NekretninaUpadeModel GetNekretninaUpadeModel(int id);
        IEnumerable<Nekretnina> GetNekretnine(int pagenumber, int pagesize);
        int CountNekretnine();
        IEnumerable<Tip> GetAllTipoviFiltera();
        IEnumerable<NacinIznajmljivanja> GetAllNaciniIznajmljivanja();
        IEnumerable<Nekretnina> GetAllNekretnineWithFilters(int pagenumber, int pagesize, PocetnaModel pModel);
        int CountNekretnineWithFilters(PocetnaModel pModel);
        int CountNekretnineWithSearch(IEnumerable<Nekretnina> nekretninas, String searchString);
        IEnumerable<Nekretnina> SearchNekretnine(IEnumerable<Nekretnina> nekretninas, String searchString);
    }
}
