using System.Data.Entity;

namespace P2PDelivery.Domain;

public class PageList<T>
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public List<T> Data { get; set; }= new List<T>();
    public PageList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        PageSize = pageSize;
        TotalCount = count;
        Data = items.ToList();
    }
    public static async Task<PageList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PageList<T>(items, count, pageNumber, pageSize);
    }
}

//public class PageListDTO<T> : PageList<T>
//{

//    public PageListDTO(IEnumerable<T> items, int count, int pageNumber, int pageSize) : base(items, count, pageNumber, pageSize)
//    {
//    }
//}
