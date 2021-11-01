using System;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.BL.Models.Questions.SingleChoice;

namespace AdminPanel.Controllers.Questions
{
    [Route("api/[controller]")]
    [ApiController]
    public class SingleChoiceQuestionsController : ControllerBase
    {
        public SingleChoiceQuestionsController()
        {
            
        }

        [HttpGet]
        public IActionResult GetItem(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult CreateItem(ScqCreateModel model)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public IActionResult EditItem(ScqEditModel model)
        {
            throw new NotImplementedException();
        }
    }
}
