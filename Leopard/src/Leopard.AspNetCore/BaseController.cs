using Leopard.Domain.Entities;
using Leopard.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace JadeFramework.Core.Mvc
{
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
