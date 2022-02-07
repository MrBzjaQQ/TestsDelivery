using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.BL.Mediators.Subjects;
using TestsDelivery.BL.Models.Subject;
using TestsDelivery.DAL.Exceptions.Subjects;

namespace AdminPanel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectsMediator _subjectsMediator;

        public SubjectsController(ISubjectsMediator subjectsMediator)
        {
            _subjectsMediator = subjectsMediator;
        }

        [HttpGet("{id}", Name = "GetSubject")]
        [Authorize]
        public IActionResult GetSubject(long id)
        {
            if (id < 1)
                throw new ArgumentException(nameof(id));

            try
            {
                var subjectReadModel = _subjectsMediator.GetSubject(id);

                return Ok(subjectReadModel);
            }
            catch (SubjectNotFoundException exception)
            {
                ModelState.TryAddModelException("SubjectNotFound", exception);
                return BadRequest(ModelState);
            }

        }

        [HttpPut]
        [Authorize]
        public IActionResult EditSubject(SubjectEditModel model)
        {
            if (ModelState.IsValid)
            {
                _subjectsMediator.EditSubject(model);

                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateSubject(SubjectCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var subject = _subjectsMediator.CreateSubject(model);

                return CreatedAtRoute(nameof(GetSubject), new { Id = subject.Id }, subject);
            }

            return BadRequest(ModelState);
        }
    }
}
