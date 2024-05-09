using AutoMapper;
using BMT.Common.Core.Commands;
using BMT.Common.Data;
using BMT.Common.Utility.Validators;
using BMT.CMS.Core.CMS;
using BMT.CMS.Infrastructure.Data;
using FluentValidation;
using LanguageExt;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;

namespace BMT.CMS.Application.Features.CMS.ContactInformation.Commands;

public record EditContactInformationCommand : ContactInformationState, IRequest<Validation<Error, ContactInformationState>>;

public class EditContactInformationCommandHandler(ApplicationContext context,
                                 IMapper mapper,
                                 CompositeValidator<EditContactInformationCommand> validator) : BaseCommandHandler<ApplicationContext, ContactInformationState, EditContactInformationCommand>(context, mapper, validator), IRequestHandler<EditContactInformationCommand, Validation<Error, ContactInformationState>>
{ 
    
public async Task<Validation<Error, ContactInformationState>> Handle(EditContactInformationCommand request, CancellationToken cancellationToken) =>
		await Validators.ValidateTAsync(request, cancellationToken).BindT(
			async request => await Edit(request, cancellationToken));
	
}

public class EditContactInformationCommandValidator : AbstractValidator<EditContactInformationCommand>
{
    readonly ApplicationContext _context;

    public EditContactInformationCommandValidator(ApplicationContext context)
    {
        _context = context;
		RuleFor(x => x.Id).MustAsync(async (id, cancellation) => await _context.Exists<ContactInformationState>(x => x.Id == id, cancellationToken: cancellation))
                          .WithMessage("ContactInformation with id {PropertyValue} does not exists");
        
    }
}
