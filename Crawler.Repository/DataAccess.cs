using Crawler.Entities;
using Crawler.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Crawler.Repository
{
    public class DataAccess
    {
        private readonly IRepository<Test> _repository;

        public DataAccess(IRepository<Test> repository)
        {
            _repository = repository;
        }

        public virtual async Task SaveTestResultAsync(string homePageUrl, IEnumerable<MeasuredLink> measuredLinks)
        {
            await _repository.AddAsync(new Test
            {
                HomePageUrl = homePageUrl,
                DateTime = DateTime.Now,
                MeasuredLinks = measuredLinks.ToList()
            });

            await _repository.SaveChangesAsync();
        }

        public virtual IEnumerable<int> GetAllTestIds()
        {
            var allIds = _repository.GetAll()
                .Select(t => t.Id)
                .OrderBy(i => i);

            return allIds;
        }

        public virtual Test GetTestsByHomePageUrl(string homePageUrl)
        {
            var test = _repository
                .Include(t => t.MeasuredLinks)
                .OrderBy(t => t.Id)
                .LastOrDefault(t => t.HomePageUrl == homePageUrl);

            return test;
        }
    }
}
