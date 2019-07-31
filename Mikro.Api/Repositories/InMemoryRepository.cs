using System.Collections.Generic;

namespace Mikro.Api.Repositories
{
    public class InMemoryRepository : IRepository
    {
        private static readonly IDictionary<int,int> _results = new Dictionary<int,int>();
        public int? Get(int number)
        {
            if(_results.ContainsKey(number))
            {
                return _results[number];
            }
            return null;

        }

        public void Insert(int number, int result)
        {
            _results.Add(number,result);
        }

        

    }
}