using System;
using System.Collections.Generic;
using System.Text;
using ITP1.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ITP1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Nekretnina> Nekretnine { get; set; }
        public DbSet<Tip> Tipovi { get; set; }
        public DbSet<Marker> Markeri { get; set; }
        public DbSet<Komentar> Komentari { get; set; }
        public DbSet<NekretninaImg> NekretninaImgs { get; set; }
        public DbSet<NacinIznajmljivanja> NacinIznajmljivnja { get; set; }
        public DbSet<Utisak> Utisci { get; set; }
    }
}
