using System.Collections.Generic;
using System.Threading.Tasks;
using WebTasks.Areas.Admin.Models.ViewModels;
using WebTasks.Areas.User.Models.ViewModels;
using WebTasks.Models;
using WebTasks.Models.ViewModels;

namespace WebTasks.Services.Interfaces
{
    public interface IPeopleService
    {
        Task<PersonDetailedAdminVm> GetPersonDetailedAdminVm(string userId);

        Task<IEnumerable<PersonVm>> GetAllUsersAsync(string filter, int page);

        Task<IEnumerable<PersonVm>> GetAllUsersAsync(string filter);

        Task<PersonDetailedVm> GetPersonDetailedUserVm(string username);
        
        Models.Interfaces.IApplicationDbContext Context { get; set; }
    }
}