using System;
using System.Linq;
using AutoMapper;
using WebTasks.Models.EntityModels;
using System.Collections.Generic;
using System.Data.Entity;
using WebTasks.Models.BindingModels;
using WebTasks.Models.ViewModels;
using Microsoft.AspNet.Identity;
using WebTasks.Models.Interfaces;
using WebTasks.Services.Interfaces;
using WebTasks.Enums;
using WebTasks.Models;

namespace WebTasks.Services
{
    public class CommentsService : Service, ICommentsService
    {
        public async System.Threading.Tasks.Task Edit(CommentBm bm)
        {
            Comment c = this.Context.Comments.Find(bm.Id);
            c.Content = bm.Content;
            await this.SaveChangesAsync();
        }

        public CommentsService(IApplicationDbContext context)
            : base(context, 10)
        {   }

        public IEnumerable<Comment> GetAllComments()
        {
            return this.Context.Comments.ToList();
        }

        public Comment Find(int id)
        {
            return this.Context.Comments.Find(id);
        }

        public CommentVm GetVm(int id)
        {
            Comment c = this.Context.Comments.Find(id);
            return Mapper.Map<CommentVm>(c);
        }

        public CommentVm GetVm(Comment c)
        {
            return Mapper.Map<CommentVm>(c);
        }

        public Comment Add(string content, int fortask, int taskType, ApplicationUser author)
        {
            if (taskType == 0)
            {
                var dt = this.Context.DailyTasks.Find(fortask);
                Comment c = new Comment()
                {
                    Content = content,
                    PublishDate = DateTime.Now,
                    Author = author
                };
                dt.Comments.Add(c);
                this.SaveChanges();
                return c;
            }
            else if(taskType == 1)
            {
                var p = this.Context.Projects.Find(fortask);
                Comment c = new Comment()
                {
                    Content = content,
                    PublishDate = DateTime.Now,
                    Author = author
                };
                p.Comments.Add(c);
                this.SaveChanges();
                return c;
            }
            
            return null;
        }

        public bool Exists(int id)
        {
            return this.Context.Comments.Count(x => x.Id == id) > 0;
        }

        public void Delete(Comment c)
        {
            this.Context.Comments.Remove(c);
        }

        public async System.Threading.Tasks.Task<int> DeleteAsync(int id)
        {
            var c = this.Context.Comments.Find(id);
            this.Context.Comments.Remove(c);
            return await this.Context.SaveChangesAsync();
        }

        public Comment GetComment(int id)
        {
            return this.Context.Comments.Find(id);
        }
    }
}