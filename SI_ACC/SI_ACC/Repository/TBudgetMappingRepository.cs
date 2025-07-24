// File: Repository/TBudgetMappingRepository.cs
using SI_ACC.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TBudgetMappingRepository
{
    private readonly SI_ACCDBContext _context;

    public TBudgetMappingRepository(SI_ACCDBContext context)
    {
        _context = context;
    }

    public async Task<List<TBudgetMapping>> GetAllAsync()
    {
        return await _context.TBudgetMappings.ToListAsync();
    }

    public async Task<TBudgetMapping?> GetByIdAsync(string budgetid,string mappingCode, string auxiliaryIndex1)
    {
        return await _context.TBudgetMappings.FirstOrDefaultAsync(x => x.BudgetId == budgetid

       && x.MappingCode == mappingCode && x.AuxiliaryIndex1 == auxiliaryIndex1
        );
    }

    public async Task<TBudgetMapping?> AddAsync(TBudgetMapping entity)
    {
        _context.TBudgetMappings.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(TBudgetMapping entity)
    {
        _context.TBudgetMappings.Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.TBudgetMappings.FindAsync(id);
        if (entity == null) return false;
        _context.TBudgetMappings.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }
}
