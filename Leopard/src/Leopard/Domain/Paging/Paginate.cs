namespace Leopard.Domain.Paging
{
    public class Paginate : IPaginate
    {
        public const int DefaultSize = 20;
        public const int DefaultIndex = 1;
        private int? pageIndex;
        private int? pageSize;

        public int PageSize
        {
            get { return this.pageSize ?? DefaultSize; }
            set { this.pageSize = value < 1 ? DefaultSize : value; }
        }

        public int PageIndex
        {
            get { return this.pageIndex ?? DefaultIndex; }
            set { this.pageIndex = value < 1 ? DefaultIndex : value; }
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", this.PageIndex, this.PageSize);
        }
    }
}