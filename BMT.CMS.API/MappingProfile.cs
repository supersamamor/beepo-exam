using AutoMapper;
using BMT.CMS.API.Controllers.v1;
using BMT.CMS.Application.Features.CMS.ContactInformation.Commands;


namespace BMT.CMS.API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ContactInformationViewModel, AddContactInformationCommand>();
		CreateMap <ContactInformationViewModel, EditContactInformationCommand>();
		
    }
}
