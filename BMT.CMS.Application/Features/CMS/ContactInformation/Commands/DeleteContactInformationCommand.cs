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

namespace BMT.CMS.Application.Features.CMS.ContactInformation.Commands;

public record DeleteContactInformationCommand : BaseCommand, IRequest<Validation<Error, ContactInformationState>>;

public class DeleteContactInformationCommandHandler(ApplicationContext context,
                                   IMapper mapper,
                                   CompositeValidator<DeleteContactInformationCommand> validator) : BaseCommandHandler<ApplicationContext, ContactInformationState, DeleteContactInformationCommand>(context, mapper, validator), IRequestHandler<DeleteContactInformationCommand, Validation<Error, ContactInformationState>>
{ 
    public async Task<Validation<Error, ContactInformationState>> Handle(DeleteContactInformationCommand request, CancellationToken cancellationToken) =>
        await Validators.ValidateTAsync(request, cancellationToken).BindT(
            async request => await Delete(request.Id, cancellationToken));
}


public class DeleteContactInformationCommandValidator : AbstractValidator<DeleteContactInformationCommand>
{
    readonly ApplicationContext _context;

    public DeleteContactInformationCommandValidator(ApplicationContext context)
    {
        _context = context;
        RuleFor(x => x.Id).MustAsync(async (id, cancellation) => await _context.Exists<ContactInformationState>(x => x.Id == id, cancellationToken: cancellation))
                          .WithMessage("ContactInformation with id {PropertyValue} does not exists");
    }
}
