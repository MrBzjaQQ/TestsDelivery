using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AdminPanel.Controllers.Marking
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MarkingController : ControllerBase
    {
        [HttpPost(nameof(Start))]
        public void Start(long testId)
        {
            throw new NotImplementedException();
        }

        [HttpPost(nameof(Finish))]
        public void Finish(long testId)
        {
            throw new NotImplementedException();
        }
    }
}
