using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.BL.Mediators.Marking;
using TestsDelivery.UserModels.Marking.FilterModels;
using TestsDelivery.UserModels.Marking.Questions;

namespace AdminPanel.Controllers.Marking
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MarkControllerBase<TMarkCreateModel, TMarkReadModel> : ControllerBase
        where TMarkCreateModel : MarkCreateModelBase
    {
        protected readonly IMarkMediatorBase<TMarkCreateModel, TMarkReadModel> Mediator;

        public MarkControllerBase(IMarkMediatorBase<TMarkCreateModel, TMarkReadModel> mediator)
        {
            Mediator = mediator;
        }

        [HttpGet]
        public IActionResult GetMark(GetMarkModel model)
        {
             if (ModelState.IsValid)
            {
                return Ok(Mediator.GetMark(model));
            }
             return BadRequest(ModelState);
        }

        [HttpPost]
        public IActionResult PostMark(TMarkCreateModel model)
        {
            if (ModelState.IsValid)
            {
                return Ok(Mediator.Create(model));
            }
            return BadRequest(ModelState);
        }
    }
}
