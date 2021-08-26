using Crawler.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Repository
{
    public class RepositoryDataAccess
    {
        private readonly IRepository<Test> _repository;

        public RepositoryDataAccess(IRepository<Test> repository)
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
