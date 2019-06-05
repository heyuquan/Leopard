using System.Collections.Generic;
using System.Linq;

namespace Leopard.Domain.Paging
{
    public class PagingResult<TEntity> : IPagingResult<TEntity>
    {
        /// <summary>
        /// 初始化分頁結果的新實例
        /// </summary>
        public PagingResult() { }


        /// <summary>
        /// 初始化分頁結果的新實例
        /// </summary>
        /// <param name="totalCount">符合查詢條件的種記錄數</param>
        /// <param name="entities">分頁查詢結果集</param>
        public PagingResult(int totalCount, IList<TEntity> entities)
        {
            this.TotalCount = totalCount;
            this.Entities = entities; 
        }

        /// <summary>
        /// 初始化分頁結果的新實例
        /// </summary>
        /// <param name="totalCount">符合查詢條件的種記錄數</param>
        /// <param name="entities">分頁查詢結果集</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PagingResult(int totalCount, int pageIndex, int pageSize, IList<TEntity> entities)
        {
            this.TotalCount = totalCount;
            this.Entities = entities; 
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }

        /// <summary>
        /// 获取或设置符合查詢條件的種記錄數
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 获取或设置分頁頁面大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 获取或设置分頁頁索引（從1開始）
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 获取或设置分頁查詢結果集
        /// </summary>
        public IList<TEntity> Entities { get; set; }
    }
}
