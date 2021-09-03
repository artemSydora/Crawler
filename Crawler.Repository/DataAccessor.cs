using Crawler.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Crawler.Repository
{
    public class DataAccessor
    {
        private readonly IRepository<TestResult> _repository;

        public DataAccessor(IRepository<TestResult> repository)
        {
            _repository = repository;
        }

        public virtual async Task<(int TotalCount, IList<TestResult> Result)> GetPageAsync(int pageNumber, int pageSize)
        {
            var tests = _repository.Include(t => t.TestDetails);
            var page = await _repository.GetPageAsync(tests, pageNumber, pageSize);

            return page;
        }

        public virtual async Task SaveTestResultsAsync(string homePageUrl, IEnumerable<TestDetail> testDetails)
        {
            await _repository.AddAsync(new TestResult
            {
                StartPageUrl = homePageUrl,
                DateTime = DateTime.Now,
                TestDetails = testDetails.ToList()
            });

            await _repository.SaveChangesAsync();
        }

        public virtual IEnumerable<TestResult> GetAllTests()
        {
            var allIds = _repository.GetAll()
                .OrderBy(i => i);

            return allIds;
        }

        public virtual TestResult GetTestById(int id)
        {
            var test = _repository
                .Include(t => t.TestDetails)
                .FirstOrDefault(t => t.Id == id);

            return test;
        }
    }
}
