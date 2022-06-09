using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.BL.Mediators.Test;
using TestsDelivery.UserModels.Test;
using TestsDelivery.DAL.Exceptions.Test;
using TestsDelivery.UserModels.ListFilters;

namespace AdminPanel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        private readonly ITestMediator _mediator;

        public TestController(ITestMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public IActionResult CreateTest(TestCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var test = _mediator.CreateTest(model);

                return CreatedAtRoute(nameof(GetTest), new {test.Id}, test);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("{id}", Name = "GetTest")]
        public IActionResult GetTest(long id)
        {
            if (id < 1)
            {
                // TODO: fix all get endpoints this way
                ModelState.AddModelError("IdOutOfRange", $"Id = {id} is out of range.");
                return BadRequest(ModelState);
            }

            try
            {
                var test = _mediator.GetTest(id);

                return Ok(test);
            }
            catch (TestException ex)
            {
                ModelState.AddModelError("TestException", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        public IActionResult EditTest(TestEditModel model)
        {
            if (ModelState.IsValid)
            {
                _mediator.EditTest(model);

                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpPost("GetList")]
        public IActionResult GetTestsList(ListFilterModel model)
        {
            return Ok(_mediator.GetList(model));
        }
    }
}
