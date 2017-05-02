using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebTasks.Models;
using WebTasks.Models.BindingModels;
using WebTasks.Models.EntityModels;
using WebTasks.Models.ViewModels;

namespace WebTasks.Services.Interfaces
{
    public interface ICommentsService
    {
        IEnumerable<Comment> GetAllComments();

        Comment Find(int id);

        CommentVm GetVm(int id);

        CommentVm GetVm(Comment c);

        Comment Add(string content, int fortask, int taskType, ApplicationUser author);

        bool Exists(int id);

        System.Threading.Tasks.Task<int> DeleteAsync(int id);

        void Delete(Comment c);

        Models.Interfaces.IApplicationDbContext Context { get; set; }

        Comment GetComment(int id);

        System.Threading.Tasks.Task Edit(CommentBm bm);
    }
}