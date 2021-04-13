using System;
using System.Collections.Generic;
using System.Text;

namespace ExPrep01.Models
{
    public class ShortUrlClass
    {
        public string Url { get; set; }
        public string ShortCode { get; set; }
        public string ShortUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public int Visits { get; set; }
    }
}
