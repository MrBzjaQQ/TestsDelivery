using AutoMapper;
using TestsDelivery.Domain.Lists;
using TestsDelivery.UserModels.Lists;

namespace TestsDelivery.BL.Mappings
{
    public class FiltersMapping : Profile
    {
        public FiltersMapping()
        {
            CreateMap<ListModel, ListFilter>();
        }
    }
}
