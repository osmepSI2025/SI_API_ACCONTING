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

    public async Task<List<TBudgetMapping>> GetAllAsync(string? filter)
    {
        try
        {
            if (string.IsNullOrEmpty(filter))
            {
                return await _context.TBudgetMappings.ToListAsync();
            }
            else
            {
                var filters = filter.Split("and", StringSplitOptions.RemoveEmptyEntries);
                var query = _context.TBudgetMappings.AsQueryable();

                foreach (var f in filters)
                {
                    var parts = f.Split("eq", StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        var field = parts[0].Trim().ToUpper();
                        var value = parts[1].Trim().Trim('\'');

                        switch (field)
                        {
                         
                            case "BUDGETLEVEL":
                                if (int.TryParse(value, out int budgetLevel))
                                    query = query.Where(x => x.BudgetLevel == budgetLevel);
                                break;
                            case "BUDGETID":
                                query = query.Where(x => x.BudgetId == value);
                                break;
                            case "BUDGETNAME":
                                query = query.Where(x => x.BudgetName == value);
                                break;
                            case "MAPPINGCODE":
                                query = query.Where(x => x.MappingCode == value);
                                break;
                            case "MAPPINGNAME":
                                query = query.Where(x => x.MappingName == value);
                                break;
                            case "MAPPINGPARENTCODE":
                                query = query.Where(x => x.MappingParentCode == value);
                                break;
                            case "AUXILIARYINDEX1":
                                if (int.TryParse(value, out int aux1))
                                    query = query.Where(x => x.AuxiliaryIndex1 == aux1);
                                break;
                                // Add more fields as needed
                        }
                    }
                }
                return await query.ToListAsync();
            }
        }
        catch (Exception)
        {
            return new List<TBudgetMapping>();
        }
    }
    public async Task<TBudgetMapping?> GetByIdAsync(string budgetid, string mappingCode, int? auxiliaryIndex1)
    {
        try
        {
            return await _context.TBudgetMappings.FirstOrDefaultAsync(
                x => x.BudgetId == budgetid
                  && x.MappingCode == mappingCode
                  && x.AuxiliaryIndex1 == auxiliaryIndex1
            );
        }
        catch (Exception ex)
        {
            // Optionally log the exception here
            // e.g., _logger.LogError(ex, "Error in GetByIdAsync");
            return null;
        }
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
