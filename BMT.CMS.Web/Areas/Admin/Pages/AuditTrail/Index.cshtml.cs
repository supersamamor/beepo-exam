using BMT.CMS.Web.Areas.Admin.Models;
using BMT.CMS.Web.Areas.Admin.Queries.AuditTrail;
using BMT.CMS.Web.Models;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BMT.CMS.Application.Helpers;

namespace BMT.CMS.Web.Areas.Admin.Pages.AuditTrail;

[Authorize(Policy = Permission.AuditTrail.View)]
public class IndexModel : BasePageModel<IndexModel>
{
    [DataTablesRequest]
    public DataTablesRequest? DataRequest { get; set; }

    public AuditLogViewModel AuditLog { get; set; } = new();

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostListAllAsync()
    {
        var result = await Mediatr.Send(DataRequest!.ToQuery<GetAuditLogsQuery>());
        return new JsonResult(result.Data
            .Select(e => new
            {
                e.Id,
                DateTime = e.DateTime.ApplyTimeOffset().ToString("MM/dd/yyyy hh:mm:ss tt"),
				TimeStamp = e.DateTime.ApplyTimeOffset(),
                e.PrimaryKey,
                e.TableName,
                e.Type,
                e.UserId,
                e.TraceId
            })
            .ToDataTablesResponse(DataRequest, result.TotalCount, result.MetaData.TotalItemCount));
    }
}
