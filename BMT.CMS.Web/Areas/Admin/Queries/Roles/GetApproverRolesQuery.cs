using BMT.CMS.Infrastructure.Data;
using BMT.Common.Core.Queries;
using BMT.Common.Utility.Extensions;
using BMT.Common.Utility.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BMT.CMS.Core.Identity;

namespace BMT.CMS.Web.Areas.Admin.Queries.Roles;

public record GetApproverRolesQuery(string CurrentSelectedApprover, IList<string> AllSelectedApprovers) : BaseQuery, IRequest<PagedListResponse<ApplicationRole>>
{
}

public class GetApproverRolesQueryHandler : IRequestHandler<GetApproverRolesQuery, PagedListResponse<ApplicationRole>>
{
    private readonly IdentityContext _context;

    public GetApproverRolesQueryHandler(IdentityContext context)
    {
        _context = context;
    }

    public async Task<PagedListResponse<ApplicationRole>> Handle(GetApproverRolesQuery request, CancellationToken cancellationToken)
    {
        var excludedUsers = request.AllSelectedApprovers.Where(l => l != request.CurrentSelectedApprover);
        var query = _context.Roles.Where(l => !excludedUsers.Contains(l.Id)).AsNoTracking();
        return await query.ToPagedResponse(request.SearchColumns, request.SearchValue,
                                                       request.SortColumn, request.SortOrder,
                                                       request.PageNumber, request.PageSize, cancellationToken);
    }
}
