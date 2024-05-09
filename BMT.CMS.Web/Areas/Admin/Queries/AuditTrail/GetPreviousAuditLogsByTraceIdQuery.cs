using BMT.CMS.Infrastructure.Data;
using MediatR;
using BMT.Common.Utility.Extensions;
using Microsoft.EntityFrameworkCore;
using LanguageExt;
using BMT.Common.Data;
using BMT.CMS.Web.Areas.Admin.Models;
namespace BMT.CMS.Web.Areas.Admin.Queries.AuditTrail;
public record GetPreviousAuditLogsByTraceIdQuery(string TraceId, string MainRecordId, DateTime DateTime) : IRequest<IList<AuditLogViewModel>>;
public class GetPreviousAuditLogsByTraceIdQueryHandler(ApplicationContext context) : IRequestHandler<GetPreviousAuditLogsByTraceIdQuery, IList<AuditLogViewModel>>
{
    public async Task<IList<AuditLogViewModel>> Handle(GetPreviousAuditLogsByTraceIdQuery request, CancellationToken cancellationToken = default)
    {
        var traceId = await context.Set<Audit>()
            .AsNoTracking().Where(l => l.TraceId != request.TraceId && l.PrimaryKey == request.MainRecordId && l.DateTime < request.DateTime)
            .OrderByDescending(l=>l.DateTime).AsNoTracking().Select(l=>l.TraceId).FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return await context.Set<Audit>()
            .AsNoTracking().Where(l => l.TraceId == traceId && l.TraceId != null && l.PrimaryKey != request.MainRecordId).Select(e => new AuditLogViewModel()
            {
                Id = e.Id,
                UserId = e.UserId,
                Type = e.Type,
                TableName = e.TableName,
                DateTime = e.DateTime,
                PrimaryKey = e.PrimaryKey,
                TraceId = e.TraceId,
                OldValues = e.OldValues,
                NewValues = e.NewValues,
            })
            .OrderBy(e => e.TableName).ThenBy(e => e.Type).ToListAsync(cancellationToken: cancellationToken);
    }
}
