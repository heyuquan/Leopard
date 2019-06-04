using Leopard.Domain.Entities;
using Leopard.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace JadeFramework.Core.Mvc
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    public class BaseController : Controller
    {
        public UserIdentity UserIdentity
        {
            get
            {
                return User.ToUserIdentity();
            }
        }
    }
}
