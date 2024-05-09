using BMT.CMS.Application.DTOs;
using BMT.CMS.Application.Features.CMS.Report.Commands;
using BMT.CMS.Application.Features.CMS.Report.Queries;
using BMT.CMS.Web.Areas.CMS.Models;
using BMT.CMS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMT.CMS.Web.Areas.CMS.Pages.Report
{
    [Authorize(Policy = Permission.Report.View)]
    public class IndexModel : BasePageModel<IndexModel>
    {
        [BindProperty]
        public ReportResultViewModel Report { get; set; } = new ReportResultViewModel();
        [BindProperty]
        public IList<ReportQueryFilterViewModel> Filters { get; set; } = new List<ReportQueryFilterViewModel>();
        [BindProperty]
        public string ReportId { get; set; } = "";
        public async Task<IActionResult> OnGet(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ReportId = id;
            var reportResult = new ReportResultModel();
            _ = (await Mediatr.Send(new GetReportSetupAndResultByIdQuery(id))).Select(l => reportResult = l);
            Mapper.Map(reportResult, Report);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var reportResult = new ReportResultModel();
            var query = new GetReportSetupAndResultByIdQuery(ReportId);
            Mapper.Map(Filters, query.Filters);
            _ = (await Mediatr.Send(query)).Select(l => reportResult = l);
            Mapper.Map(reportResult, Report);
            return Page();
        }
    }
}
