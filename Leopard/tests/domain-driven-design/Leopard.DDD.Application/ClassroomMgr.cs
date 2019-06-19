using Leopard.DDD.Domain;
using Leopard.Dependency;
using System;

namespace Leopard.DDD.Application
{
    public class ClassroomMgr : IScopeDependency
    {
        public Classroom Classroom { get; set; }

        public ClassroomMgr()
        {
            System.Diagnostics.Debug.WriteLine("初始化：ClassroomMgr");
        }

        public string GetClassroomInfo()
        {
            return $"Mgr_{Classroom.GetClassroomInfo()}";
        }
    }
}
