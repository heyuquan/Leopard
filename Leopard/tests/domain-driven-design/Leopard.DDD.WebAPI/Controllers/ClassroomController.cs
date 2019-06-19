using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Leopard.DDD.Application;
using Leopard.DDD.Domain;
using Leopard.DDD.Domain.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Leopard.DDD.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassroomController : ControllerBase
    {
        public ClassroomMgr ClassroomMgr { get; set; }

        public Classroom Classroom { get; set; }

        public ICourseRepository CourseRepository { get; set; }

        public ClassroomController()
        {
            System.Diagnostics.Debug.WriteLine("初始化：ClassroomController");
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "success";
            //return ClassroomMgr.GetClassroomInfo();
        }
    }
}