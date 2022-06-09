using AutoMapper;
using TestsDelivery.Domain.Lists;
using TestsDelivery.UserModels.ListFilters;

namespace TestsDelivery.BL.Mappings
{
    public class FiltersMapping : Profile
    {
        public FiltersMapping()
        {
            CreateMap<ListFilterModel, ListFilter>();
            CreateMap<QuestionsInSubjectListFilterModel, QuestionsInSubjectListFilter>();
        }
    }
}
