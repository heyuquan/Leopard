using Leopard.DDD.Domain.IRepository;
using System;

namespace Leopard.DDD.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private string name = string.Empty;
        public CourseRepository()
        {
            System.Diagnostics.Debug.WriteLine("初始化：CourseRepository");
            this.name = "课件仓储";
        }

        public IRoomRepository RoomRepository { get; set; }

        public string GetName() => name;
    }
}
