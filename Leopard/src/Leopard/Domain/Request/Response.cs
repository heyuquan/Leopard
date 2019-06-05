using Leopard.Domain.Paging;
using Leopard.Http;

namespace Leopard.Domain.Request
{

    /*
现在前端请求RestAPI后获得的errorcode，HBS, ABS, FBS, CRM都是没有统一的规范，导致可能例如errocode=200的情况在某系统RestAPI报错是一种不要处理普通错误，而在另一个系统RestAPI报错则是一种需要处理特殊错误的情况存在。为解决这个问题，经部门会议讨论，用下面方法来规范RestAPI中errorcode的定义。
如果接口成功返回，那么errorcode定义为0。
如果接口错误，则errorcode返回9位的错误码，规则分别是：1位接口状态码 + 2位业务系统编码 + 2位子系统编码 + 4位错误码。
第一位表示接口的状态信息，如下面附录中的“接口状态编码规定表”；
第2，3位是业务系统编码信息，如下面附录中的“业务系统编码规定表”；
第4，5位是子系统编码，请各业务子系统根据自身业务系统定义。默认为00，尽量跟业务系统编码保持一致。例如PBS的机票子系统用04表示，酒店子系统用03表示；
第6-9位是错误码，请接口开发者自定义，定义后请填写到Conf的ErrorCode集合中。

    2(状态码)02(PBS)01(FH)0001()

接口状态编码规定表:
接口状态码    描述
1	         接口正常错误，后端只需将错误信息记录到日志，前端不需做任何处理。 例如：各种日志接口问题等。
2	         接口正常錯誤，前端需要對此类errcode做特殊處理。 例如：创建接口出现“套票已满”，需要重新跳到资源页等情况。
4	         接口業務錯誤，前端直接提示errmessage信息。 例如：创建订单接口的信息不全引起的错误等。
5	         接口系統錯誤，前端不顯示errmessage，展示默认接口請求失敗頁。 例如：“供应商数据异常”或接口请求其他接口报错等一系列的未知错误。

业务系统编码规定表:
业务系统 编码号
TBS	     01
PBS	     02
HBS	     03
ABS	     04
Wireless 05
FBS    	 06
CRM	     07
CMS	     08
BANNER	 09

*/
    public class Response
    {
        /// <summary>
        /// 响应Id
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// 响应头
        /// </summary>
        public ResponseHeader Header { get; set; }

        /// <summary>
        /// 0成功，其它失败。
        /// 2xx|需要前端后续处理，4xx验证错|前端会原生显示errmessage，5xx程序错|前端会跳到通用错误页。
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 接口抛出的错误信息
        /// </summary>
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// 响应对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T> : Response where T : class
    {
        /// <summary>
        /// 用于返回接口返回的数据
        /// </summary>
        public T Data
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 響應頭
    /// </summary>
    public class ResponseHeader
    {
        /// <summary>
        /// 返回数据编码类型
        /// </summary>
        public FormatType ContentType { get; set; } = FormatType.JSON;
    }
}