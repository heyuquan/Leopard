namespace Leopard.Domain.Paging
{
    /// <summary>
    /// 分页接口
    /// </summary>
    public interface IPaging : IPaginate
    {
        /// <summary>
        /// 满足查询条件的总记录数
        /// </summary>
        int TotalCount { get; set; }
    }
}
