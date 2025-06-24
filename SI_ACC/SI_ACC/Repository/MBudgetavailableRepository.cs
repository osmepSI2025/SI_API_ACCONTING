// File: Repositories/MBudgetavailableRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SI_ACC.Entities;

public class MBudgetavailableRepository 
{
    private readonly SI_ACCDBContext _context;

    public MBudgetavailableRepository(SI_ACCDBContext context)
    {
        _context = context;
    }

    public async Task<List<MBudgetavailable>> GetAllAsync()
    {
        return await _context.MBudgetavailables.ToListAsync();
    }

    public async Task<MBudgetavailable?> GetByIdAsync(string budgetId)
    {
        return await _context.MBudgetavailables.FirstOrDefaultAsync(e=>e.BudgetId== budgetId);
    }

    public async Task<MBudgetavailable?> AddAsync(MBudgetavailable entity)
    {
        try
        {
            _context.MBudgetavailables.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch
        {
            // Optionally log the exception
            return null;
        }
    }

    public async Task<bool> UpdateAsync(MBudgetavailable entity)
    {
        _context.MBudgetavailables.Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.MBudgetavailables.FindAsync(id);
        if (entity == null) return false;
        _context.MBudgetavailables.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }
}
