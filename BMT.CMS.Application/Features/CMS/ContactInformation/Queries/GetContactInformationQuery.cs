using BMT.Common.Core.Queries;
using BMT.Common.Utility.Models;
using BMT.CMS.Core.CMS;
using BMT.CMS.Infrastructure.Data;
using MediatR;
using BMT.Common.Utility.Extensions;
using Microsoft.EntityFrameworkCore;
using BMT.CMS.Application.DTOs;
using LanguageExt;

namespace BMT.CMS.Application.Features.CMS.ContactInformation.Queries;

public record GetContactInformationQuery : BaseQuery, IRequest<PagedListResponse<ContactInformationListDto>>;

public class GetContactInformationQueryHandler(ApplicationContext context) : BaseQueryHandler<ApplicationContext, ContactInformationListDto, GetContactInformationQuery>(context), IRequestHandler<GetContactInformationQuery, PagedListResponse<ContactInformationListDto>>
{
	public override async Task<PagedListResponse<ContactInformationListDto>> Handle(GetContactInformationQuery request, CancellationToken cancellationToken = default)
	{
		
		var pagedList = await Context.Set<ContactInformationState>()
			.AsNoTracking().Select(e => new ContactInformationListDto()
			{
				Id = e.Id,
				LastModifiedDate = e.LastModifiedDate,
				FirstName = e.FirstName,
				LastName = e.LastName,
				CompanyName = e.CompanyName,
				Mobile = e.Mobile,
				Email = e.Email,
			})
			.ToPagedResponse(request.SearchColumns, request.SearchValue,
				request.SortColumn, request.SortOrder,
				request.PageNumber, request.PageSize,
				cancellationToken);
		foreach (var item in pagedList.Data)
		{
			item.StatusBadge = await Helpers.ApprovalHelper.GetApprovalStatus(Context, item.Id, cancellationToken);
		}
		return pagedList;	
	}
}
