using Leopard.DDD.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leopard.DDD.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private string name = string.Empty;
        public RoomRepository()
        {
            System.Diagnostics.Debug.WriteLine("初始化：RoomRepository");
            this.name = "教室仓储";
        }

        public string GetName() => name;
    }
}
