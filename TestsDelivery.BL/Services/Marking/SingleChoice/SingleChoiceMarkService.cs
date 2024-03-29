﻿using AutoMapper;
using TestsDelivery.DAL.Models.Marking;
using TestsDelivery.DAL.Repositories.Marking.Choice;
using TestsDelivery.Domain.Marking;

namespace TestsDelivery.BL.Services.Marking.SingleChoice
{
    public class SingleChoiceMarkService : MarkServiceBase<MarkedSingleChoice, ChoiceMark>, ISingleChoiceMarkService
    {
        public SingleChoiceMarkService(IChoiceMarkingRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
