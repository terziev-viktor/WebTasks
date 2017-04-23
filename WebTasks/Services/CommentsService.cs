using System;
using System.Linq;
using AutoMapper;
using WebTasks.Models.EntityModels;
using System.Collections.Generic;
using System.Data.Entity;
using WebTasks.Models.BindingModels;
using WebTasks.Models.ViewModels;

namespace WebTasks.Services
{
    public class CommentsService : Service
    {
        public CommentsService()
        {
            Mapper.Initialize(x =>
            {
                x.CreateMap<Comment, CommentVm>().AfterMap((a, b) =>
                {
                    b.Author = a.Author.Email;
                });
            });
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

        internal bool AddComment(Comment c, int fortask)
        {
            var dt = this.Context.DailyTasks.Find(fortask);
            if(dt != null)
            {
                dt.Comments.Add(c);
                return true;
            }
            var p = this.Context.Projects.Find(fortask);
            if(p != null)
            {
                p.Comments.Add(c);
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

        internal Comment CreateComment(int forTask, string content, string name)
        {
            Comment c = new Comment()
            {
                Content = content,
                PublishDate = DateTime.Now,
                Author = this.UserManager.Users.First(x => x.UserName == name)
            };

            return c;
        }
    }
}