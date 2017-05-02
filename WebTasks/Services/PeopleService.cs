using System.Collections.Generic;
using AutoMapper;
using WebTasks.Areas.User.Models.ViewModels;
using WebTasks.Models;
using System.Linq;
using WebTasks.Models.ViewModels;
using System.Data.Entity;
using System.Threading.Tasks;
using WebTasks.Areas.Admin.Models.ViewModels;
using WebTasks.Models.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using WebTasks.Services.Interfaces;
using Microsoft.AspNet.Identity;
using System;

namespace WebTasks.Services
{
    public class PeopleService : Service, IPeopleService
    {
        private ApplicationUserManager userManager;

        public PeopleService(IApplicationDbContext context)
            : base(context, 10)
        {
            this.userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context as ApplicationDbContext));
        }

        public async Task<PersonDetailedAdminVm> GetPersonDetailedAdminVm(string username)
        {
            var user = await this.userManager.FindByNameAsync(username);
            PersonDetailedAdminVm vm = Mapper.Map<PersonDetailedAdminVm>(user);
            return vm;
        }


        public async Task<PersonDetailedVm> GetPersonDetailedUserVm(string username)
        {
            ApplicationUser user = await this.userManager.FindByNameAsync(username);
            PersonDetailedVm vm = Mapper.Map<PersonDetailedVm>(user);
            return vm;
        }

        public async Task<IEnumerable<PersonVm>> GetAllUsersAsync(string filter, int page)
        {
            var users = await this.userManager.Users.Where(x => x.UserName.Contains(filter))
                .OrderBy(x => x.UserName).Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();
            
            List<PersonVm> vm = Mapper.Instance.Map<IEnumerable<ApplicationUser>, IEnumerable<PersonVm>>(users).ToList();
            vm.ForEach(x =>
            {
                x.IsAdmin = userManager.IsInRole(x.Id, "Admin");
            });

            return vm;
        }

        public async Task<IEnumerable<PersonVm>> GetAllUsersAsync(int page)
        {
            var users = await this.userManager.Users
                .OrderBy(x => x.UserName).Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            List<PersonVm> vm = Mapper.Instance.Map<IEnumerable<ApplicationUser>, IEnumerable<PersonVm>>(users).ToList();
            vm.ForEach(x =>
            {
                x.IsAdmin = userManager.IsInRole(x.Id, "Admin");
            });

            return vm;
        }

        public async Task<IEnumerable<PersonVm>> GetAllUsersAsync(string filter)
        {
            var users = await this.userManager.Users.Where(x => x.UserName.Contains(filter))
                .OrderBy(x => x.UserName).ToListAsync();
            List<PersonVm> vm = Mapper.Instance.Map<IEnumerable<ApplicationUser>, IEnumerable<PersonVm>>(users).ToList();
            vm.ForEach(x =>
            {
                x.IsAdmin = userManager.IsInRole(x.Id, "Admin");
            });
            return vm;
        }
    }
}