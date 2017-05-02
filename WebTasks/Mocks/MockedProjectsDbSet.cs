using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebTasks.Models.EntityModels;

namespace WebTasks.Mocks
{
    public class MockedProjectsDbSet : MockedDbSet<Project>
    {
        public override Project Find(params object[] keyValues)
        {
            return this.Set.FirstOrDefault(x => x.Id == keyValues[0] as int?);
        }

        public override Task<Project> FindAsync(params object[] keyValues)
        {
            return new Task<Project>(() =>
            {
                return this.Set.FirstOrDefault(x => x.Id == (int)keyValues[0]);
            });
        }
    }
}