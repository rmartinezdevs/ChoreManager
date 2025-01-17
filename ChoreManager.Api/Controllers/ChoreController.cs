using ChoreManager.Api.Common;
using ChoreManager.Application.DTOs;
using ChoreManager.Application.UseCases.ChoreUseCases.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ChoreManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChoreController : ControllerBase
    {
        private readonly ICreateChoreUseCase _createChore;
        private readonly IDeleteChoreUseCase _deleteChore;
        private readonly IUpdateChoreUseCase _updateChore;
        private readonly ISelectAllChoresUseCase _selectAllChores;
        private readonly ISelectChoreByIdUseCase _selectChoreById;

        public ChoreController(ICreateChoreUseCase createChore, IDeleteChoreUseCase deleteChore, IUpdateChoreUseCase updateChore, ISelectAllChoresUseCase selectAllChores, ISelectChoreByIdUseCase selectChoreById)
        {
            _createChore = createChore;
            _deleteChore = deleteChore;
            _updateChore = updateChore;
            _selectAllChores = selectAllChores;
            _selectChoreById = selectChoreById;
        }

        [HttpPost]
        public async Task<IActionResult> CreateChore([FromBody] CreateChoreDto createChoreDto)
        {
            await _createChore.ExecuteAsync(createChoreDto);
            return Ok(new ApiResponse<CreateChoreDto>
            {
                StatusCode = 200,
                Message = "Chore created successfully",
                Data = createChoreDto
            });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateChore([FromBody] ChoreDto updateChoreDto)
        {
            await _updateChore.ExecuteAsync(updateChoreDto);
            return Ok(new ApiResponse<ChoreDto>
            {
                StatusCode = 200,
                Message = "Chore updated successfully",
                Data = updateChoreDto
            });
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChore([FromRoute] Guid id)
        {
            try
            {
                await _deleteChore.ExecuteAsync(id);
                return Ok(new ApiResponse<object>
                {
                    StatusCode = 200,
                    Message = "Chore deleted successfully"
                });
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
                return Ok(new ApiResponse<List<ChoreDto>>
                {
                    StatusCode = 200,
                    Message = "Chores retrieved successfully",
                    Data = chores
                });
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
                if (chore == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        StatusCode = 404,
                        Message = "Chore not found"
                    });
                }

                return Ok(new ApiResponse<ChoreDto>
                {
                    StatusCode = 200,
                    Message = "Chore retrieved successfully",
                    Data = chore
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving chores.", Details = ex.Message });
            }
        }
    }
}
