using AutoMapper;
using BMT.CMS.Web.Areas.Admin.Commands.Entities;
using BMT.CMS.Web.Areas.Admin.Models;
using BMT.Common.Data;
using Microsoft.AspNetCore.Identity;
using BMT.CMS.Core.Identity;

namespace BMT.CMS.Web.Areas.Admin.Mapping;

public class AdminProfile : Profile
{
    public AdminProfile()
    {
        CreateMap<Entity, EntityViewModel>().ReverseMap();
        CreateMap<EntityViewModel, AddOrEditEntityCommand>();
        CreateMap<AddOrEditEntityCommand, Entity>();

        CreateMap<ApplicationRole, RoleViewModel>().ReverseMap();

        CreateMap<Audit, AuditLogViewModel>();
        CreateMap<ApplicationUser, AuditLogUserViewModel>();
    }
}
