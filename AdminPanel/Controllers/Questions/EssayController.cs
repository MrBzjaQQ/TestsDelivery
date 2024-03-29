﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.BL.Mediators.Questions.Essay;
using TestsDelivery.UserModels.Marking.Questions;
using TestsDelivery.UserModels.Questions.Essay;

namespace AdminPanel.Controllers.Questions
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EssayController : QuestionControllerBase<EssayCreateModel, EssayEditModel, EssayReadModel>
    {
        public EssayController(IEssayMediator mediator)
            : base(mediator)
        {
        }
    }
}
