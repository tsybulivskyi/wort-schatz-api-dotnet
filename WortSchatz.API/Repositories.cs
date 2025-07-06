using Microsoft.EntityFrameworkCore;
using WordTranslationApp.Models;

namespace WordTranslationApp;

public class WordRepository
{
    private readonly WortSchatzDbContext _context;
    public WordRepository(WortSchatzDbContext context)
    {
        _context = context;
    }

    public async Task<List<Word>> GetAllAsync()
    {
        return await _context.Words.Include(w => w.Tags).ToListAsync();
    }

    public async Task<Word?> GetByIdAsync(int id)
    {
        return await _context.Words.Include(w => w.Tags).FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task AddAsync(Word word)
    {
        _context.Words.Add(word);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var word = await _context.Words.FindAsync(id);
        if (word != null)
        {
            _context.Words.Remove(word);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAllAsync()
    {
        _context.Words.RemoveRange(_context.Words);
        await _context.SaveChangesAsync();
    }
}

public class TagRepository
{
    private readonly WortSchatzDbContext _context;
    public TagRepository(WortSchatzDbContext context)
    {
        _context = context;
    }

    public async Task<List<Tag>> GetAllAsync()
    {
        return await _context.Tags.ToListAsync();
    }

    public async Task<Tag?> GetByIdAsync(int id)
    {
        return await _context.Tags.FindAsync(id);
    }

    public async Task AddAsync(Tag tag)
    {
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var tag = await _context.Tags.FindAsync(id);
        if (tag != null)
        {
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAllAsync()
    {
        _context.Tags.RemoveRange(_context.Tags);
        await _context.SaveChangesAsync();
    }
}
