using BMT.CMS.Application.Features.CMS.Report.Commands;
using BMT.CMS.Application.Features.CMS.Report.Queries;
using BMT.CMS.Web.Areas.CMS.Models;
using BMT.CMS.Web.Models;
using BMT.CMS.Web.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
namespace BMT.CMS.Web.Areas.CMS.Pages.Dashboard
{
    public class IndexModel(AIDataAnalyticsServices aiDataAnalyticsServices) : BasePageModel<IndexModel>
    {    
        [BindProperty]
        public IList<ReportResultViewModel> ReportList { get; set; } = new List<ReportResultViewModel>();
        [BindProperty]
        public string SelectedReportId { get; set; } = "";
        public async Task<IActionResult> OnGet()
        {
            Mapper.Map(await Mediatr.Send(new GetDashboardReportsQuery()), ReportList);
            return Page();
        }
        public async Task<IActionResult> OnPostDataAnalytics()
        {
            ModelState.Clear();
            var report = ReportList.Where(l => l.ReportId == SelectedReportId).FirstOrDefault();
            var chatGPTResult = await aiDataAnalyticsServices.AIDrivenAnalysis(report!.ReportName!, report.Results!, report.ColumnHeaders!, token: new CancellationToken());
            await Mediatr.Send(new AddReportAnalyticsCommand()
            { 
                ReportId = report.ReportId!,
                Input = $"Report Data : {report.Results} / Report Column Headers : {report.ColumnHeaders}",
                Output = chatGPTResult == null ? "" : chatGPTResult!,
            });
            JObject chatGPTJson = JObject.Parse(chatGPTResult!);
            return Partial("_DataAnalytics", chatGPTJson);
        }
    }
}
