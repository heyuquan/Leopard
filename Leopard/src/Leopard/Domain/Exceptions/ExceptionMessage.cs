using System;
using System.Collections.Generic;
using System.Text;
using Leopard.Extensions;

namespace Leopard.Domain.Exceptions
{
    /// <summary>
    /// 异常信息封装类
    /// </summary>
    public class ExceptionMessage
    {
        #region 构造函数

        /// <summary>
        /// 以自定义用户信息和异常对象实例化一个异常信息对象
        /// </summary>
        /// <param name="e">异常对象</param>
        /// <param name="userMessage">自定义用户信息</param>
        /// <param name="isHideStackTrace">是否隐藏异常堆栈信息</param>
        public ExceptionMessage(Exception e, string userMessage = null, bool isHideStackTrace = false)
        {
            this.UserMessage = string.IsNullOrEmpty(userMessage) ? e.Message : userMessage;

            this.ExMessage = string.Empty;
            this.ErrorDetails = e.ToStrMessage();
        }

        #region 属性

        /// <summary>
        /// 用户信息，用于报告给用户的异常消息
        /// </summary>
        public string UserMessage { get; set; }

        /// <summary>
        /// 直接的Exception异常信息，即e.Message属性值
        /// </summary>
        public string ExMessage { get; private set; }

        /// <summary>
        /// 异常输出的详细描述，包含异常消息，规模信息，异常类型，异常源，引发异常的方法及内部异常信息
        /// </summary>
        public string ErrorDetails { get; private set; }

        #endregion

        #region 方法

        /// <summary>
        /// 返回表示当前 <see cref="T:System.Object"/> 的 <see cref="T:System.String"/>。
        /// </summary>
        /// <returns>
        /// <see cref="T:System.String"/>，表示当前的 <see cref="T:System.Object"/>。
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return this.ErrorDetails;
        }

        #endregion

        #endregion
    }
}
