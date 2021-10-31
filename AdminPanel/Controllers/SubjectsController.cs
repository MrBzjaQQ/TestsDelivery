using System;
using AdminPanel.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.BL.Mediators;
using TestsDelivery.BL.Models.Subject;
using TestsDelivery.DAL.Exceptions.Subjects;

namespace AdminPanel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectsMediator _subjectsMediator;
        private readonly IAppLogging<SubjectsController> _logger;
        private readonly IMapper _mapper;

        public SubjectsController(
            ISubjectsMediator subjectsMediator,
            IAppLogging<SubjectsController> logger,
            IMapper mapper)
        {
            _subjectsMediator = subjectsMediator;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetSubject")]
        [Authorize]
        public IActionResult GetSubject(int id)
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
