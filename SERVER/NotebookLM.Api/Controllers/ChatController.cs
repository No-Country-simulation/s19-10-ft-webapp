using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotebookLM.Api.Services;
using NotebookLM.Persistence.Services;
using System.Security.Claims;

namespace NotebookLM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {

        private readonly SemanticKernelChatService _chatHistorySerrvice;

        public ChatController(SemanticKernelChatService chatHistorySerrvice)
        {
            _chatHistorySerrvice = chatHistorySerrvice;
        }

        [Authorize(Roles = "User")]
        [HttpGet("GetNewChat")]
        public async Task<IActionResult> GetNewChat()
        {

            try
            {
                var chatId = await _chatHistorySerrvice.GetNewChatHistoryId();
                return Ok(chatId);

            }
            catch (Exception e )
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost("FetchUserChatById")]
        public async Task<IActionResult> FetchUserChatById(int chatId)
        {

            try
            {
                var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                
                var chats =  await _chatHistorySerrvice.GetChatsOfUser(Guid.Parse(userId));

                return Ok(chats);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost("AskFromFile")]
        public async Task<IActionResult> AskFromFile(string fileId, int chatId, string prompt)
        {
            try
            {
                var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

                var result = await _chatHistorySerrvice.AskFromFile(userId, chatId, prompt, fileId);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /*
        [HttpPost]
        public async Task<IActionResult> AskFromFiles(Guid[] fileIds, int chatId, string prompt)
        {

            try
            {

            }
            catch
            {

            }
            return Ok();


        }
        */
    }
        
}
