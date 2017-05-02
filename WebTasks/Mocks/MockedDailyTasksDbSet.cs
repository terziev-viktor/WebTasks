using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebTasks.Models.EntityModels;

namespace WebTasks.Mocks
{
    public class MockedDailyTasksDbSet : MockedDbSet<DailyTask>
    {
        public override DailyTask Find(params object[] keyValues)
        {
            return this.Set.FirstOrDefault(x => x.Id == (int)keyValues[0]);
        }

        public override Task<DailyTask> FindAsync(params object[] keyValues)
        {
            return new Task<DailyTask>(() =>
            {
                return this.Set.FirstOrDefault(x => x.Id == (int)keyValues[0]);
            });
        }
    }
}