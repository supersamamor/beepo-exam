using BMT.CMS.ExcelProcessor.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BMT.CMS.ExcelProcessor
{
    public static class ServiceExtensions
    {
        public static void AddExcelProcessor(this IServiceCollection services)
        {
            services.AddTransient<ExcelService>();
        }
    }
}
