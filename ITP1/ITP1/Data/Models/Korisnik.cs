﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITP1.Data.Models
{
    public class Korisnik
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Tel { get; set; }
        public string MailKontakt { get; set; }
        public string WebKontaktUrl { get; set; }
        public string EMailFromAuthentication { get; set; }
        public string AvatarImgUrl { get; set; }
        public int NumberOfRatings5 { get; set; }
        public int NumberOfRatings4 { get; set; }
        public int NumberOfRatings3 { get; set; }
        public int NumberOfRatings2 { get; set; }
        public int NumberOfRatings1 { get; set; }


        public string UserId { get; set; }
    }
}
