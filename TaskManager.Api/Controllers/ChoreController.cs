using ChoreManager.Application.UseCases.ChoreUseCases;
using ChoreManager.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ChoreManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChoreController : ControllerBase
    {
        private readonly CreateChore _createChore;
        private readonly DeleteChore _deleteChore;
        private readonly SelectAllChores _selectAllChores;
        private readonly SelectChoreById _selectChoreById;

        public ChoreController(CreateChore createChore, DeleteChore deleteChore, SelectAllChores selectAllChores, SelectChoreById selectChoreById)
        {
            _createChore = createChore;
            _deleteChore = deleteChore;
            _selectAllChores = selectAllChores;
            _selectChoreById = selectChoreById;
        }

        [HttpPost]
        public async Task<IActionResult> CreateChore([FromBody] Chore chore)
        {
            await _createChore.ExecuteAsync(chore);
            return Ok(chore);
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteChore([FromBody] Chore chore)
        {
            await _deleteChore.ExecuteAsync(chore);
            return Ok(chore);
        }

        [HttpGet]
        public async Task<IActionResult> SelectAllChores()
        {
            try
            {
                var chores = await _selectAllChores.ExecuteAsync();
                return Ok(chores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving chores.", Details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> SelectChoreById([FromRoute] Guid id)
        {
            try
            {
                var chore = await _selectChoreById.ExecuteAsync(id);
                return Ok(chore);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving chores.", Details = ex.Message });
            }
        }
    }
}
