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

public record AddContactInformationCommand : ContactInformationState, IRequest<Validation<Error, ContactInformationState>>;

public class AddContactInformationCommandHandler(ApplicationContext context,
                                IMapper mapper,
                                CompositeValidator<AddContactInformationCommand> validator,
                                    IdentityContext identityContext) : BaseCommandHandler<ApplicationContext, ContactInformationState, AddContactInformationCommand>(context, mapper, validator), IRequestHandler<AddContactInformationCommand, Validation<Error, ContactInformationState>>
{
    public async Task<Validation<Error, ContactInformationState>> Handle(AddContactInformationCommand request, CancellationToken cancellationToken) =>
		await Validators.ValidateTAsync(request, cancellationToken).BindT(
			async request => await AddContactInformation(request, cancellationToken));


	public async Task<Validation<Error, ContactInformationState>> AddContactInformation(AddContactInformationCommand request, CancellationToken cancellationToken)
	{
		ContactInformationState entity = Mapper.Map<ContactInformationState>(request);
		_ = await Context.AddAsync(entity, cancellationToken);
		await Helpers.ApprovalHelper.AddApprovers(Context, identityContext, ApprovalModule.ContactInformation, entity.Id, cancellationToken);
		_ = await Context.SaveChangesAsync(cancellationToken);
		return Success<Error, ContactInformationState>(entity);
	}
	
}

public class AddContactInformationCommandValidator : AbstractValidator<AddContactInformationCommand>
{
    readonly ApplicationContext _context;

    public AddContactInformationCommandValidator(ApplicationContext context)
    {
        _context = context;

        RuleFor(x => x.Id).MustAsync(async (id, cancellation) => await _context.NotExists<ContactInformationState>(x => x.Id == id, cancellationToken: cancellation))
                          .WithMessage("ContactInformation with id {PropertyValue} already exists");
        
    }
}
