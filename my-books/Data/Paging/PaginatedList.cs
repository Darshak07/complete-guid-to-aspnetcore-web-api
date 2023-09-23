namespace my_books.Data.Paging
{
    public class PaginatedList<T>:List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items,int count,int pageIndex,int pageSize)
        {
            PageIndex = pageIndex;
            //based on the number of items and page size we will get page number
            //this will always get igher fractional value
            TotalPages = (int)Math.Ceiling(count/(double)pageSize);
            this.AddRange(items);
        }
        public bool HasPreviuosPage
        {
            get 
            {
                return PageIndex > 1;
            }
        }
        public bool HasNextPage
        {
            get
            {
                return PageIndex < TotalPages;
            }
        }
        //IQueryble is suitable for quering data from out-memory(like remote,database,service)
        //IEnumerable is good for in Memory collection query.
        //While quering data from database, IQueryable executes "Select query on server-side with all filters"
        public static PaginatedList<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
        { 
            var count = source.Count();
            //if you are at the page 2 
            // skip items (2-1)*5 means skip first 5 items
            // Take next 5 items in Queue which is for 2nd page
            var items = source.Skip((pageIndex-1)*pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count,pageIndex, pageSize);

        }
    }
}
