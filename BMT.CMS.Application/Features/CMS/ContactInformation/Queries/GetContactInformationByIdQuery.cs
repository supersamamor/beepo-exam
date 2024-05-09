using BMT.Common.Core.Queries;
using BMT.CMS.Core.CMS;
using BMT.CMS.Infrastructure.Data;
using LanguageExt;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMT.CMS.Application.Features.CMS.ContactInformation.Queries;

public record GetContactInformationByIdQuery(string Id) : BaseQueryById(Id), IRequest<Option<ContactInformationState>>;

public class GetContactInformationByIdQueryHandler(ApplicationContext context) : BaseQueryByIdHandler<ApplicationContext, ContactInformationState, GetContactInformationByIdQuery>(context), IRequestHandler<GetContactInformationByIdQuery, Option<ContactInformationState>>
{
		
}
