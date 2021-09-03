using System;
using System.Collections.Generic;

namespace Crawler.Entities.Models
{
    public class TestResult
    {
        public TestResult()
        {
            TestDetails = new List<TestDetail>();  
        }

        public int Id { get; set; }

        public string StartPageUrl { get; set; }

        public DateTime DateTime { get; set; }

        public ICollection<TestDetail> TestDetails { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TestResult result &&
                   Id == result.Id &&
                   StartPageUrl == result.StartPageUrl &&
                   DateTime == result.DateTime &&
                   EqualityComparer<ICollection<TestDetail>>.Default.Equals(TestDetails, result.TestDetails);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, StartPageUrl, DateTime, TestDetails);
        }
    }
}
