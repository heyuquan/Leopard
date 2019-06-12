//using Leopard.Domain.Entities;
//using Leopard.AspNetCore.Extensions;
//using Microsoft.AspNetCore.Mvc;

//namespace JadeFramework.Core.Mvc
//{

//    ControllerBase 类
//Web API 有一个或多个派生自 ControllerBase 的控制器类。 
//不要通过从 Controller 基类派生来创建 Web API 控制器。Controller 派生自 ControllerBase，并添加对视图的支持，因此它用于处理 Web 页面，而不是 Web API 请求。
//此规则有一个例外：如果打算为视图和 API 使用相同的控制器，则从 Controller 派生控制器。


//    /// <summary>
//    /// 控制器基类
//    /// </summary>
//    public class BaseController : Controller
//    {
//        public UserIdentity UserIdentity
//        {
//            get
//            {
//                return User.ToUserIdentity();
//            }
//        }
//    }
//}
