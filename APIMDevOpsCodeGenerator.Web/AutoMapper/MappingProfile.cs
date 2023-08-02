using APIMDevOpsCodeGenerator.Web.Models;
using AutoMapper;
using DevOpsResourceCreator.Model;

namespace APIMDevOpsCodeGenerator.Web.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Project, DevOpsProjectModel>();
            CreateMap<DevOpsAccount, DevOpsAccountModel>();
        }
    }
}
