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
        private readonly int pageSize = 10;

        public PeopleService()
            : base()
        {

        }

        internal async Task<PersonDetailedAdminVm> GetPersonDetailedAdminVm(string userId)
        {
            ApplicationUser user = await this.UserManager.FindByIdAsync(userId);
            PersonDetailedAdminVm vm = Mapper.Map<PersonDetailedAdminVm>(user);
            return vm;
        }

        internal async Task<IEnumerable<PersonAdminVm>> GetAllUsersAync()
        {
            var users = await this.UserManager.Users.ToListAsync();
            IEnumerable<PersonAdminVm> vm = Mapper.Instance.Map<IEnumerable<ApplicationUser>, IEnumerable<PersonAdminVm>>(users);
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
            List<ApplicationUser> users = new List<ApplicationUser>();
            if (filter != null)
            {
                users.AddRange(await this.UserManager.Users.Where(x => x.UserName.Contains(filter)).ToListAsync());
            }
            else
            {
                users.AddRange(await this.UserManager.Users.Take(pageSize).ToListAsync());
            }
            if (users.Count > pageSize)
            users.RemoveRange((page - 1) * pageSize, pageSize);

            IEnumerable<PersonVm> vm = Mapper.Instance.Map<IEnumerable<ApplicationUser>, IEnumerable<PersonVm>>(users);

            return vm;
        }
    }
}