using System;
using System.Collections.Generic;

namespace Crawler.Entities
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

        public List<MeasuredLink> MeasuredLinks { get; set; }
    }
}
