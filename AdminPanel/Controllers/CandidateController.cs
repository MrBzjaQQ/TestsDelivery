using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.BL.Mediators.Candidate;
using TestsDelivery.UserModels.Candidate;
using TestsDelivery.DAL.Exceptions.Candidate;
using TestsDelivery.UserModels.Lists;

namespace AdminPanel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateMediator _mediator;

        public CandidateController(ICandidateMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}", Name = "GetCandidate")]
        public IActionResult GetCandidate(long id)
        {
            if (id < 1)
                throw new ArgumentException(nameof(id));

            try
            {
                var candidateReadModel = _mediator.GetCandidate(id);

                return Ok(candidateReadModel);
            }
            catch (CandidateException ex)
            {
                ModelState.TryAddModelException("CandidateNotFound", ex);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public IActionResult CreateCandidate(CandidateCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var candidate = _mediator.CreateCandidate(model);

                return CreatedAtRoute(nameof(GetCandidate), new {candidate.Id}, candidate);
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult EditCandidate(CandidateEditModel model)
        {
            if (ModelState.IsValid)
            {
                _mediator.EditCandidate(model);

                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        public IActionResult GetCandidatesList(ListModel model)
        {
            return Ok(_mediator.GetList(model));
        }
    }
}
