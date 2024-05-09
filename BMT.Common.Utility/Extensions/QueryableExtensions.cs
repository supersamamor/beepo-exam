using BMT.Common.Utility.Models;
using System.Linq.Dynamic.Core;
using X.PagedList;

namespace BMT.Common.Utility.Extensions;

/// <summary>
/// Extension methods for <see cref="IQueryable{T}"/>.
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Converts <see cref="IQueryable{T}"/> to <see cref="PagedListResponse{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="searchColumns">Columns to search</param>
    /// <param name="searchValue">Global search value</param>
    /// <param name="sortColumn">Column where sorting will be applied</param>
    /// <param name="sortOrder">Asc or Desc</param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<PagedListResponse<T>> ToPagedResponse<T>(this IQueryable<T> query,
                                                                      string[]? searchColumns, string? searchValue,
                                                                      string? sortColumn, string? sortOrder,
                                                                      int pageNumber, int pageSize,
                                                                      CancellationToken cancellationToken = default)
    {        
	    if (!string.IsNullOrWhiteSpace(searchValue) && searchColumns != null)
        {
			searchValue = searchValue.Replace("\\", "");
            var filter = string.Join(" OR ", searchColumns.Select(x => $"{x}.StartsWith(\"{searchValue}\")"));
            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(filter);
            }
        }
        if (!string.IsNullOrWhiteSpace(sortColumn) && !string.IsNullOrWhiteSpace(sortOrder))
        {
            query = query.OrderBy($"{sortColumn} {sortOrder}");
        }
		var totalCount = query.Count();
		if (pageSize == -1)
		{
			pageSize = totalCount;
			if (pageSize == 0)
			{
				pageSize = 1;
			}        
		}
		var data = await query.ToPagedListAsync(pageNumber, pageSize, null, cancellationToken);
		return new PagedListResponse<T>(data, totalCount);
    }
}