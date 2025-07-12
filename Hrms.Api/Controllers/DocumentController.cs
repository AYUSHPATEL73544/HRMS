using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.JobApplication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hrms.Api.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
   
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentManager _manager;

        public DocumentController(IDocumentManager manager)
        {
            _manager = manager;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FileDetailModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail([FromRoute] int id)
        {
            return Ok(await _manager.GetAsync(id));
        }


        [HttpPost("addProfileImage")]
        [ProducesResponseType(typeof(FileDetailModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddImage([FromBody] FileDetailModel model)
        {
            await _manager.AddImageAsync(model, User.GetUserId());
            return Ok();
        }

        [HttpPut("updateProfileImage")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateImage([FromBody] FileDetailModel model)
        {
            await _manager.UpdateImageAsync(model, User.GetUserId());
            return Ok();
        }

        [HttpGet("getProfileImageByUserId")]
        [ProducesResponseType(typeof(ImageDetailModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfileImageByUserId()
        {
            return Ok(await _manager.GetProfileImageByUserIdAsync(User.GetUserId()));
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            await _manager.DeleteAsync(id);
            return Ok();
        }

    }
}
