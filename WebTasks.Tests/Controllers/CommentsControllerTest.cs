using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTasks.Controllers;
using WebTasks.Services;

namespace WebTasks.Tests.Controllers
{
    public class CommentsControllerTest : TestClass
    {
        protected CommentsService service;
        protected CommentsController controller;
        public void Init()
        {
            this.InitContext();
            this.service = new CommentsService(context);
            this.controller = new CommentsController(service);
            
        }
    }
}
