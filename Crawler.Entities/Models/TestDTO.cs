using System;
using System.Collections.Generic;

namespace Crawler.Entities.Models
{
    public class TestDTO
    {
        public TestDTO()
        {
            Details = new List<DetailDTO>();
        }

        public int Id { get; set; }

        public string StartPageUrl { get; set; }

        public DateTime DateTime { get; set; }

        public ICollection<DetailDTO> Details { get; set; }

        public TestDTO(string startPageUrl, DateTime dateTime, ICollection<DetailDTO> details)
        {
            StartPageUrl = startPageUrl;
            DateTime = dateTime;
            Details = details;
        }

        public override bool Equals(object obj)
        {
            return obj is TestDTO result &&
                   Id == result.Id &&
                   StartPageUrl == result.StartPageUrl &&
                   DateTime == result.DateTime &&
                   EqualityComparer<ICollection<DetailDTO>>.Default.Equals(Details, result.Details);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, StartPageUrl, DateTime, Details);
        }
    }
}
