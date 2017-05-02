using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebTasks.Models.EntityModels;

namespace WebTasks.Mocks
{
    public class MockedCommentsDbSet : MockedDbSet<Comment>
    {
        public override Comment Find(params object[] keyValues)
        {
            return this.Set.Find(x => x.Id == (int)keyValues[0]);
        }

        public override Task<Comment> FindAsync(params object[] keyValues)
        {
            return new Task<Comment>(() =>
            {
                return this.Set.FirstOrDefault(x => x.Id == (int)keyValues[0]);
            });
        }
    }
}