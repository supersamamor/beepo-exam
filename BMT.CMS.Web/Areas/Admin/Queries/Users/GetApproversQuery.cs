using BMT.CMS.Core.Identity;
using BMT.CMS.Infrastructure.Data;
using BMT.Common.Core.Queries;
using BMT.Common.Utility.Extensions;
using BMT.Common.Utility.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace BMT.CMS.Web.Areas.Admin.Queries.Users;

public record GetApproversQuery(string CurrentSelectedApprover, IList<string> AllSelectedApprovers) : BaseQuery, IRequest<PagedListResponse<ApplicationUser>>
{
}

public class GetApproversQueryHandler : IRequestHandler<GetApproversQuery, PagedListResponse<ApplicationUser>>
{
    private readonly IdentityContext _context;

    public GetApproversQueryHandler(IdentityContext context)
    {
        _context = context;
    }

    public async Task<PagedListResponse<ApplicationUser>> Handle(GetApproversQuery request, CancellationToken cancellationToken)
    {
        var excludedUsers = request.AllSelectedApprovers.Where(l => l != request.CurrentSelectedApprover);
        var query = _context.Users.Where(l => !excludedUsers.Contains(l.Id)).AsNoTracking();
        return await query.ToPagedResponse(request.SearchColumns, request.SearchValue,
                                                       request.SortColumn, request.SortOrder,
                                                       request.PageNumber, request.PageSize, cancellationToken);
    }
}
