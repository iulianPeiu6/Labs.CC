using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UScheduler.WebApi.Boards.Data.Entities;

namespace UScheduler.WebApi.Boards.Interfaces
{
    public interface IBoardsRepository
    {
        Task<Board> GetBoardAsync(Expression<Func<Board, bool>> func);
        Task<IEnumerable<Board>> GetBoardsAsync(Expression<Func<Board, bool>> func);
        Task<Board> CreateBoardAsync(Board board);
        Task DeleteAsync(Board board);
        Task UpdateAsync(Board board);
    }
}
