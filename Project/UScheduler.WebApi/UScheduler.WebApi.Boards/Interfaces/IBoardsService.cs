using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UScheduler.WebApi.Boards.Data.Entities;
using UScheduler.WebApi.Boards.Models;

namespace UScheduler.WebApi.Boards.Interfaces
{
    public interface IBoardsService
    {
        Task<(bool IsSuccess, BoardDto Board, string Error)> GetBoardAsync(Guid boardId);
        Task<(bool IsSuccess, IEnumerable<BoardDto> Boards, string Error)> GetBoardsByWorkspaceIdAsync(Guid workspaceId);
        Task<(bool IsSuccess, BoardDto Board, string Error)> CreateBoardAsync(CreateBoardModel model, string createdBy);
        Task<(bool IsSuccess, BoardDto Board, string Error)> UpdateAsync(Guid id, UpdateBoardModel model, string modifiedBy);
        Task<(bool IsSuccess, BoardDto Board, string Error)> UpdateAsync(Guid id, JsonPatchDocument<Board> model, string modifiedBy);
        Task<(bool IsSuccess, string Error)> DeleteBoardAsync(Guid boardId);
    }
}
