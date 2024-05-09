using BMT.Common.Core.Queries;
using BMT.Common.Utility.Models;
using BMT.CMS.Core.CMS;
using BMT.CMS.Infrastructure.Data;
using MediatR;
using BMT.Common.Utility.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BMT.CMS.Application.Features.CMS.Report.Queries;

public record GetReportQuery : BaseQuery, IRequest<PagedListResponse<ReportState>>;

public class GetReportQueryHandler : BaseQueryHandler<ApplicationContext, ReportState, GetReportQuery>, IRequestHandler<GetReportQuery, PagedListResponse<ReportState>>
{
    public GetReportQueryHandler(ApplicationContext context) : base(context)
    {
    }
		
}
