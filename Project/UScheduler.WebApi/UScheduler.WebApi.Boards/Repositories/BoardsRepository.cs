using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UScheduler.WebApi.Boards.Data;
using UScheduler.WebApi.Boards.Data.Entities;
using UScheduler.WebApi.Boards.Interfaces;

namespace UScheduler.WebApi.Boards.Repositories
{
    public class BoardsRepository : IBoardsRepository
    {
        private readonly BoardsContext _context;

        public BoardsRepository(BoardsContext context)
        {
            _context = context;
        }

        public async Task<Board> GetBoardAsync(Expression<Func<Board, bool>> func)
            => await _context.Boards
                .AsNoTracking()
                .FirstOrDefaultAsync(func);

        public async Task<IEnumerable<Board>> GetBoardsAsync(Expression<Func<Board, bool>> func)
            => await _context.Boards
                .AsNoTracking()
                .Where(func)
                .ToListAsync();

        public async Task<Board> CreateBoardAsync(Board board)
        {
            var entry = await _context.Boards.AddAsync(board);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task DeleteAsync(Board board)
        {
            _context.Boards.Remove(board);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Board board)
        {
            _context.Boards.Update(board);
            await _context.SaveChangesAsync();
        }
    }
}
