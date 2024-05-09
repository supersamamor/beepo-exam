using BMT.CMS.Infrastructure.Data;
using MediatR;
using BMT.Common.Utility.Extensions;
using Microsoft.EntityFrameworkCore;
using LanguageExt;
using BMT.Common.Data;
using BMT.CMS.Web.Areas.Admin.Models;
namespace BMT.CMS.Web.Areas.Admin.Queries.AuditTrail;
public record GetAuditLogsByPrimaryKeyQuery(string Id) : IRequest<IList<AuditLogViewModel>>;
public class GetAuditLogsByPrimaryKeyQueryHandler(ApplicationContext context) : IRequestHandler<GetAuditLogsByPrimaryKeyQuery, IList<AuditLogViewModel>>
{
    public async Task<IList<AuditLogViewModel>> Handle(GetAuditLogsByPrimaryKeyQuery request, CancellationToken cancellationToken = default)
    {
        return await context.Set<Audit>()
            .AsNoTracking().Where(l => l.PrimaryKey == request.Id).Select(e => new AuditLogViewModel()
            {
                Id = e.Id,
                UserId = e.UserId,
                Type = e.Type,
                TableName = e.TableName,
                DateTime = e.DateTime,
                PrimaryKey = e.PrimaryKey,
                TraceId = e.TraceId,
            })
            .OrderByDescending(e => e.DateTime).ToListAsync(cancellationToken: cancellationToken);
    }
}
