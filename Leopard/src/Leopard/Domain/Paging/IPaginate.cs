using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Domain.Paging
{
    public interface IPaginate
    {
        /// <summary>
        /// 页面大小
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        int PageIndex { get; set; }
    }
}
