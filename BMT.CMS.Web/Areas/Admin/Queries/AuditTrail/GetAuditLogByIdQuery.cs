using BMT.CMS.Infrastructure.Data;
using BMT.Common.Data;
using LanguageExt;
using MediatR;

namespace BMT.CMS.Web.Areas.Admin.Queries.AuditTrail;

public record GetAuditLogByIdQuery(int Id) : IRequest<Option<Audit>>;

public class GetAuditLogByIdQueryHandler : IRequestHandler<GetAuditLogByIdQuery, Option<Audit>>
{
    readonly ApplicationContext _context;

    public GetAuditLogByIdQueryHandler(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Option<Audit>> Handle(GetAuditLogByIdQuery request, CancellationToken cancellationToken) =>
        await _context.GetSingle<Audit>(e => e.Id == request.Id, cancellationToken: cancellationToken);
}
