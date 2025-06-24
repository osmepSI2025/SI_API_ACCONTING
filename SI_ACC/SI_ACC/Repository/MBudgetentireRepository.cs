// File: Repository/MBudgetentireRepository.cs
using SI_ACC.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MBudgetentireRepository
{
    private readonly SI_ACCDBContext _context;

    public MBudgetentireRepository(SI_ACCDBContext context)
    {
        _context = context;
    }

    public async Task<List<MBudgetentire>> GetAllAsync()
    {
        return await _context.MBudgetentires.ToListAsync();
    }

    public async Task<MBudgetentire?> GetByIdAsync(string entryNo,string budgetCode)
    {
        return await _context.MBudgetentires.FirstOrDefaultAsync(x => x.EntryNo.ToString() == entryNo
        
       && x.BudgetCode == budgetCode
        );
    }

    public async Task<MBudgetentire?> AddAsync(MBudgetentire entity)
    {
        _context.MBudgetentires.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(MBudgetentire entity)
    {
        _context.MBudgetentires.Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.MBudgetentires.FindAsync(id);
        if (entity == null) return false;
        _context.MBudgetentires.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }
}
