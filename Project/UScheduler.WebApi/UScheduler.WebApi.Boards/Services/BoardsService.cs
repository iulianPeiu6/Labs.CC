using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UScheduler.WebApi.Boards.Data.Entities;
using UScheduler.WebApi.Boards.Interfaces;
using UScheduler.WebApi.Boards.Models;
using UScheduler.WebApi.Boards.Statics;

namespace UScheduler.WebApi.Boards.Services
{
    public class BoardsService : IBoardsService
    {
        private readonly IBoardsRepository _repository;
        private readonly IWorkspacesAdapter _workspacesAdapter;
        private readonly IMapper _mapper;
        private readonly ILogger<BoardsService> _logger;

        public BoardsService(
            IBoardsRepository repository,
            IMapper mapper,
            ILogger<BoardsService> logger,
            IWorkspacesAdapter workspacesAdapter)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _workspacesAdapter = workspacesAdapter;
        }

        public async Task<(bool IsSuccess, BoardDto Board, string Error)> GetBoardAsync(Guid boardId)
        {
            try
            {
                var board = await _repository.GetBoardAsync(board => board.Id == boardId);
                if (board == null)
                {
                    return (false, null, ErrorMessage.BoardNotFound);
                }

                var boardDto = _mapper.Map<BoardDto>(board);
                return (true, boardDto, null);
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<BoardDto> Boards, string Error)> GetBoardsByWorkspaceIdAsync(Guid workspaceId)
        {
            try
            {
                var boards = await _repository
                    .GetBoardsAsync(board => board.WorkspaceId == workspaceId);

                var boardsDto = _mapper.Map<IEnumerable<BoardDto>>(boards);
                return (true, boardsDto, null);
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, BoardDto Board, string Error)> CreateBoardAsync(CreateBoardModel model, string createdBy)
        {
            try
            {
                //not working yet since there is no suitable endpoint for this check
                var (workspaceExists, error) = await _workspacesAdapter.WorkspaceExists(model.WorkspaceId);
                if (!workspaceExists)
                {
                    return (false, null, error);
                }

                var currentTime = DateTime.UtcNow;
                var board = _mapper.Map<Board>(model);

                board.CreatedBy = createdBy;
                board.UpdatedBy = createdBy;
                board.CreatedAt = currentTime;
                board.UpdatedAt = currentTime;

                var createdBoard = await _repository.CreateBoardAsync(board);
                var boardDto = _mapper.Map<BoardDto>(createdBoard);
                return (true, boardDto, null);
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, BoardDto Board, string Error)> UpdateAsync(Guid id, UpdateBoardModel model, string modifiedBy)
        {
            try
            {
                var board = await _repository.GetBoardAsync(board => board.Id == id);
                if (board == null)
                {
                    return (false, null, ErrorMessage.BoardNotFound);
                }

                board.UpdatedBy = modifiedBy;
                board.UpdatedAt = DateTime.UtcNow;
                board.Title = model.Title;
                board.Description = model.Description;

                await _repository.UpdateAsync(board);
                var boardDto = _mapper.Map<BoardDto>(board);
                return (true, boardDto, null);
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, BoardDto Board, string Error)> UpdateAsync(Guid id, JsonPatchDocument<Board> model, string modifiedBy)
        {
            try
            {
                var board = await _repository.GetBoardAsync(board => board.Id == id);
                if (board == null)
                {
                    return (false, null, ErrorMessage.BoardNotFound);
                }

                model.ApplyTo(board);

                board.UpdatedBy = modifiedBy;
                board.UpdatedAt = DateTime.UtcNow;

                await _repository.UpdateAsync(board);
                var boardDto = _mapper.Map<BoardDto>(board);
                return (true, boardDto, null);
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string Error)> DeleteBoardAsync(Guid boardId)
        {
            try
            {
                var board = await _repository.GetBoardAsync(board => board.Id == boardId);
                if (board == null)
                {
                    return (false, ErrorMessage.BoardNotFound);
                }

                await _repository.DeleteAsync(board);
                return (true, null);
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                return (false, ex.Message);
            }
        }
    }
}
