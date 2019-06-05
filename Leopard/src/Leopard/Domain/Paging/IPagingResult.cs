
using System.Collections.Generic;

namespace Leopard.Domain.Paging
{
    public interface IPagingResult<TEntity> : IPaging
    {
        IList<TEntity> Entities { get; set; }
    }
}
