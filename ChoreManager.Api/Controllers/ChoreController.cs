using ChoreManager.Application.DTOs;
using ChoreManager.Application.UseCases.ChoreUseCases;
using Microsoft.AspNetCore.Mvc;

namespace ChoreManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChoreController : ControllerBase
    {
        private readonly CreateChoreUseCase _createChore;
        private readonly DeleteChoreUseCase _deleteChore;
        private readonly SelectAllChoresUseCase _selectAllChores;
        private readonly SelectChoreByIdUseCase _selectChoreById;

        public ChoreController(CreateChoreUseCase createChore, DeleteChoreUseCase deleteChore, SelectAllChoresUseCase selectAllChores, SelectChoreByIdUseCase selectChoreById)
        {
            _createChore = createChore;
            _deleteChore = deleteChore;
            _selectAllChores = selectAllChores;
            _selectChoreById = selectChoreById;
        }

        [HttpPost]
        public async Task<IActionResult> CreateChore([FromBody] CreateChoreDto createChoreDto)
        {
            await _createChore.ExecuteAsync(createChoreDto);
            return Ok(createChoreDto);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChore([FromRoute] Guid id)
        {
            try
            {
                await _deleteChore.ExecuteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting chore.", Details = ex.Message });
            }
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
