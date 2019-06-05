
namespace Leopard.Domain.Paging
{
    public class PagingBase : Paginate, IPaging
    {
        public int TotalCount { get; set; }
    }
}
