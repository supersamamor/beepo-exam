using AutoMapper;
using BMT.Common.Core.Commands;
using BMT.Common.Utility.Validators;
using BMT.CMS.Core.CMS;
using BMT.CMS.Infrastructure.Data;
using FluentValidation;
using LanguageExt;
using LanguageExt.Common;
using MediatR;
using static LanguageExt.Prelude;

namespace BMT.CMS.Application.Features.CMS.Report.Commands;

public record AddReportAnalyticsCommand : ReportAnalyticsState, IRequest<Validation<Error, ReportAnalyticsState>>;

public class AddReportAnalyticsCommandHandler(ApplicationContext context,
                                IMapper mapper,
                                CompositeValidator<AddReportAnalyticsCommand> validator) : BaseCommandHandler<ApplicationContext, ReportState, AddReportAnalyticsCommand>(context, mapper, validator), IRequestHandler<AddReportAnalyticsCommand, Validation<Error, ReportAnalyticsState>>
{
    public async Task<Validation<Error, ReportAnalyticsState>> Handle(AddReportAnalyticsCommand request, CancellationToken cancellationToken) =>
		await Validators.ValidateTAsync(request, cancellationToken).BindT(
			async request => await AddReport(request, cancellationToken));
	public async Task<Validation<Error, ReportAnalyticsState>> AddReport(AddReportAnalyticsCommand request, CancellationToken cancellationToken)
	{
        ReportAnalyticsState entity = Mapper.Map<ReportAnalyticsState>(request);			
        _ = await Context.AddAsync(entity, cancellationToken);	
		_ = await Context.SaveChangesAsync(cancellationToken);
		return Success<Error, ReportAnalyticsState>(entity);
	}	
}

public class AddReportAnalyticsCommandValidator : AbstractValidator<AddReportAnalyticsCommand>
{
    public AddReportAnalyticsCommandValidator()
    {
      
    }
}
