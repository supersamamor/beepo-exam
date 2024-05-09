using AutoMapper;
using BMT.Common.Core.Mapping;
using BMT.CMS.Application.Features.CMS.Approval.Commands;
using BMT.CMS.Core.CMS;
using BMT.CMS.Application.Features.CMS.Report.Commands;
using BMT.CMS.Application.Features.CMS.ContactInformation.Commands;



namespace BMT.CMS.Application.Features.CMS;

public class CMSProfile : Profile
{
    public CMSProfile()
    {
		CreateMap<AddReportCommand, ReportState>();
        CreateMap<EditReportCommand, ReportState>().IgnoreBaseEntityProperties();
		CreateMap<AddReportAnalyticsCommand, ReportAnalyticsState>();
		
        CreateMap<AddContactInformationCommand, ContactInformationState>();
		CreateMap <EditContactInformationCommand, ContactInformationState>().IgnoreBaseEntityProperties();
		
		CreateMap<EditApproverSetupCommand, ApproverSetupState>().IgnoreBaseEntityProperties();
		CreateMap<AddApproverSetupCommand, ApproverSetupState>().IgnoreBaseEntityProperties();
		CreateMap<ApproverAssignmentState, ApproverAssignmentState>().IgnoreBaseEntityProperties();
    }
}
