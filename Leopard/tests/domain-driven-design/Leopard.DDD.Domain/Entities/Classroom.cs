using Leopard.DDD.Domain.IRepository;
using Leopard.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leopard.DDD.Domain
{
    public class Classroom : IScopeDependency
    {
        public ICourseRepository CourseRepository { get; set; }

        public IRoomRepository RoomRepository { get; set; }

        public Classroom()
        {
            System.Diagnostics.Debug.WriteLine("初始化：Classroom");
        }

        public string GetClassroomInfo()
        {
            return $"{CourseRepository.GetName()}_{RoomRepository.GetName()}";
        }
    }
}
