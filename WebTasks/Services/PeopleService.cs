using System.Collections.Generic;
using AutoMapper;
using WebTasks.Areas.User.Models.ViewModels;
using WebTasks.Models;
using System.Linq;
using WebTasks.Models.ViewModels;
using WebTasks.Models.EntityModels;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System;
using System.Threading.Tasks;
using WebTasks.Areas.Admin.Models.ViewModels;

namespace WebTasks.Services
{
    public class PeopleService : Service
    {
        public PeopleService()
            : base(10)
        {

        }

        internal async Task<PersonDetailedAdminVm> GetPersonDetailedAdminVm(string userId)
        {
            ApplicationUser user = await this.UserManager.FindByIdAsync(userId);
            PersonDetailedAdminVm vm = Mapper.Map<PersonDetailedAdminVm>(user);
            return vm;
        }

        internal async Task<IEnumerable<PersonVm>> GetAllUsersAync(string filter, int page)
        {
            var users = await this.UserManager.Users.Where(x => x.UserName.Contains(filter))
                .OrderBy(x => x.UserName).Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();
            IEnumerable<PersonVm> vm = Mapper.Instance.Map<IEnumerable<ApplicationUser>, IEnumerable<PersonVm>>(users);
            return vm;
        }

        internal async Task<PersonDetailedVm> GetPersonDetailedUserVm(string userId)
        {
            ApplicationUser user = await this.UserManager.FindByIdAsync(userId);
            PersonDetailedVm vm = Mapper.Map<PersonDetailedVm>(user);
            return vm;
        }

        internal async System.Threading.Tasks.Task<IEnumerable<PersonVm>> GetAllUsersByUserName(string filter, int page)
        {
            List<ApplicationUser> users = 
                await this.UserManager.Users
                .Where(x => x.UserName.Contains(filter))
                .OrderBy(x => x.UserName)
                .Skip((page - 1) * PageSize)
                .Take(PageSize).ToListAsync();
            
            IEnumerable<PersonVm> vm = Mapper.Instance.Map<IEnumerable<ApplicationUser>, IEnumerable<PersonVm>>(users);

            return vm;
        }
    }
}