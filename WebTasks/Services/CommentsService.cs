using System;
using System.Linq;
using AutoMapper;
using WebTasks.Models.EntityModels;
using System.Collections.Generic;
using System.Data.Entity;
using WebTasks.Models.BindingModels;
using WebTasks.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace WebTasks.Services
{
    public class CommentsService : Service
    {
        public CommentsService()
            : base(10)
        {

        }

        internal IEnumerable<Comment> GetAllComments()
        {
            return this.Context.Comments.ToList();
        }

        internal Comment FindComment(int id)
        {
            return this.Context.Comments.Find(id);
        }

        internal CommentVm GetCommentVm(int id)
        {
            Comment c = this.Context.Comments.Find(id);
            return Mapper.Map<CommentVm>(c);
        }

        internal CommentVm GetCommentVm(Comment c)
        {
            return Mapper.Map<CommentVm>(c);
        }

        internal void UpdateTask(Comment comment, EntityState es)
        {
            this.Context.Entry(comment).State = es;
        }

        internal Comment MapComment(CommentBm bm)
        {
            return Mapper.Instance.Map<CommentBm, Comment>(bm);
        }

        internal bool AddComment(Comment c, int fortask, int taskType)
        {
            if (taskType == 0)
            {
                var dt = this.Context.DailyTasks.Find(fortask);
                dt.Comments.Add(c);
                this.SaveChanges();
                return true;
            }
            else if(taskType == 1)
            {
                var p = this.Context.Projects.Find(fortask);
                p.Comments.Add(c);
                this.SaveChanges();
                return true;
            }
            
            return false;
        }

        internal void RemoveComment(Comment c)
        {
            this.Context.Comments.Remove(c);
        }

        internal bool Exists(int id)
        {
            return this.Context.Comments.Count(x => x.Id == id) > 0;
        }

        internal async System.Threading.Tasks.Task<int> DeleteCommentAsync(int id)
        {
            var c = this.Context.Comments.Find(id);
            this.Context.Comments.Remove(c);
            return await this.Context.SaveChangesAsync();
        }

        internal Comment CreateComment(int forTask, string content, string id)
        {
            Comment c = new Comment()
            {
                Content = content,
                PublishDate = DateTime.Now,
                Author = this.UserManager.FindById(id)
            };

            return c;
        }
    }
}