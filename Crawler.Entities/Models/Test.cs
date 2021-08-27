using System;
using System.Collections.Generic;

namespace Crawler.Entities.Models
{
    public class Test
    {
        public Test()
        {
            MeasuredLinks = new List<MeasuredLink>();  
        }

        public int Id { get; set; }

        public string HomePageUrl { get; set; }

        public DateTime DateTime { get; set; }

        public ICollection<MeasuredLink> MeasuredLinks { get; set; }
    }
}
