using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITP1.Data.Models;

namespace ITP1.Data
{
    public interface INekretnina
    {
        void AddNekretnina(Nekretnina nekretnina);
        void UpdateNekretnina(Nekretnina nekretnina);
        Nekretnina GetNekretnina(int id);
    }
}
