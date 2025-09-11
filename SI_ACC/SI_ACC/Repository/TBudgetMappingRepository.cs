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
                var query = _context.TBudgetMappings.AsQueryable();
                var filters = filter.Split("and", StringSplitOptions.RemoveEmptyEntries);

                foreach (var f in filters)
                {
                    var filterStr = f.Trim();

                    // Handle 'or' for the same field (e.g., BudgetLevel eq 1 or BudgetLevel eq 2)
                    if (filterStr.Contains(" or "))
                    {
                        var orParts = filterStr.Split("or", StringSplitOptions.RemoveEmptyEntries);
                        var orPredicates = new List<System.Linq.Expressions.Expression<Func<TBudgetMapping, bool>>>();

                        foreach (var orPart in orParts)
                        {
                            var singlePredicate = BuildPredicate(orPart.Trim());
                            if (singlePredicate != null)
                                orPredicates.Add(singlePredicate);
                        }

                        if (orPredicates.Count > 0)
                        {
                            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(TBudgetMapping), "x");
                            System.Linq.Expressions.Expression? body = null;
                            foreach (var expr in orPredicates)
                            {
                                var replaced = System.Linq.Expressions.Expression.Invoke(expr, parameter);
                                body = body == null ? replaced : System.Linq.Expressions.Expression.OrElse(body, replaced);
                            }
                            if (body != null)
                            {
                                var lambda = System.Linq.Expressions.Expression.Lambda<Func<TBudgetMapping, bool>>(body, parameter);
                                query = query.Where(lambda);
                            }
                        }
                        continue;
                    }

                    // Handle normal filter
                    var predicate = BuildPredicate(filterStr);
                    if (predicate != null)
                    {
                        query = query.Where(predicate);
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

    // Helper to build predicate for a single filter string
    private static System.Linq.Expressions.Expression<Func<TBudgetMapping, bool>>? BuildPredicate(string filter)
    {
        var operators = new[] { " eq ", " ne ", " gt ", " lt ", " ge ", " le ", " in ", "startswith(", "endswith(", "contains(" };
        foreach (var op in operators)
        {
            if (filter.Contains(op))
            {
                if (op == " in ")
                {
                    // Example: BudgetLevel in (1,2,3)
                    var parts = filter.Split(" in ", StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        var field = parts[0].Trim().ToUpper();
                        var values = parts[1].Trim('(', ')', ' ').Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(v => v.Trim('\'', ' ')).ToArray();

                        switch (field)
                        {
                            case "BUDGETLEVEL":
                                var budgetLevels = values.Select(v => int.TryParse(v, out int n) ? n : (int?)null)
                                    .Where(n => n.HasValue).Select(n => n.Value).ToList();
                                return x => budgetLevels.Contains(x.BudgetLevel ?? 0);
                            case "AUXILIARYINDEX1":
                                var aux1s = values.Select(v => int.TryParse(v, out int n) ? n : (int?)null)
                                    .Where(n => n.HasValue).Select(n => n.Value).ToList();
                                return x => aux1s.Contains(x.AuxiliaryIndex1 ?? 0);
                            case "BUDGETID":
                                return x => values.Contains(x.BudgetId);
                            case "BUDGETNAME":
                                return x => values.Contains(x.BudgetName);
                            case "MAPPINGCODE":
                                return x => values.Contains(x.MappingCode);
                            case "MAPPINGNAME":
                                return x => values.Contains(x.MappingName);
                            case "MAPPINGPARENTCODE":
                                return x => values.Contains(x.MappingParentCode);
                        }
                    }
                }
                else if (op == "startswith(")
                {
                    var fieldAndValue = filter.Substring("startswith(".Length).TrimEnd(')');
                    var args = fieldAndValue.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    if (args.Length == 2)
                    {
                        var field = args[0].Trim().ToUpper();
                        var value = args[1].Trim().Trim('\'');
                        switch (field)
                        {
                            case "BUDGETID":
                                return x => x.BudgetId != null && x.BudgetId.StartsWith(value);
                            case "BUDGETNAME":
                                return x => x.BudgetName != null && x.BudgetName.StartsWith(value);
                            case "MAPPINGCODE":
                                return x => x.MappingCode != null && x.MappingCode.StartsWith(value);
                            case "MAPPINGNAME":
                                return x => x.MappingName != null && x.MappingName.StartsWith(value);
                            case "MAPPINGPARENTCODE":
                                return x => x.MappingParentCode != null && x.MappingParentCode.StartsWith(value);
                        }
                    }
                }
                else if (op == "endswith(")
                {
                    var fieldAndValue = filter.Substring("endswith(".Length).TrimEnd(')');
                    var args = fieldAndValue.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    if (args.Length == 2)
                    {
                        var field = args[0].Trim().ToUpper();
                        var value = args[1].Trim().Trim('\'');
                        switch (field)
                        {
                            case "BUDGETID":
                                return x => x.BudgetId != null && x.BudgetId.EndsWith(value);
                            case "BUDGETNAME":
                                return x => x.BudgetName != null && x.BudgetName.EndsWith(value);
                            case "MAPPINGCODE":
                                return x => x.MappingCode != null && x.MappingCode.EndsWith(value);
                            case "MAPPINGNAME":
                                return x => x.MappingName != null && x.MappingName.EndsWith(value);
                            case "MAPPINGPARENTCODE":
                                return x => x.MappingParentCode != null && x.MappingParentCode.EndsWith(value);
                        }
                    }
                }
                else if (op == "contains(")
                {
                    var fieldAndValue = filter.Substring("contains(".Length).TrimEnd(')');
                    var args = fieldAndValue.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    if (args.Length == 2)
                    {
                        var field = args[0].Trim().ToUpper();
                        var value = args[1].Trim().Trim('\'');
                        switch (field)
                        {
                            case "BUDGETID":
                                return x => x.BudgetId != null && x.BudgetId.Contains(value);
                            case "BUDGETNAME":
                                return x => x.BudgetName != null && x.BudgetName.Contains(value);
                            case "MAPPINGCODE":
                                return x => x.MappingCode != null && x.MappingCode.Contains(value);
                            case "MAPPINGNAME":
                                return x => x.MappingName != null && x.MappingName.Contains(value);
                            case "MAPPINGPARENTCODE":
                                return x => x.MappingParentCode != null && x.MappingParentCode.Contains(value);
                        }
                    }
                }
                else
                {
                    // Existing comparison logic (eq, ne, gt, lt, ge, le)
                    var parts = filter.Split(op, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        var field = parts[0].Trim().ToUpper();
                        var value = parts[1].Trim().Trim('\'');
                        switch (field)
                        {
                            case "BUDGETLEVEL":
                                if (int.TryParse(value, out int budgetLevel))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.BudgetLevel == budgetLevel,
                                        " ne " => x => x.BudgetLevel != budgetLevel,
                                        " gt " => x => x.BudgetLevel > budgetLevel,
                                        " lt " => x => x.BudgetLevel < budgetLevel,
                                        " ge " => x => x.BudgetLevel >= budgetLevel,
                                        " le " => x => x.BudgetLevel <= budgetLevel,
                                        _ => null
                                    };
                                }
                                break;
                            case "AUXILIARYINDEX1":
                                if (int.TryParse(value, out int aux1))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.AuxiliaryIndex1 == aux1,
                                        " ne " => x => x.AuxiliaryIndex1 != aux1,
                                        " gt " => x => x.AuxiliaryIndex1 > aux1,
                                        " lt " => x => x.AuxiliaryIndex1 < aux1,
                                        " ge " => x => x.AuxiliaryIndex1 >= aux1,
                                        " le " => x => x.AuxiliaryIndex1 <= aux1,
                                        _ => null
                                    };
                                }
                                break;
                            case "BUDGETID":
                                return op switch
                                {
                                    " eq " => x => x.BudgetId == value,
                                    " ne " => x => x.BudgetId != value,
                                    _ => null
                                };
                            case "BUDGETNAME":
                                return op switch
                                {
                                    " eq " => x => x.BudgetName == value,
                                    " ne " => x => x.BudgetName != value,
                                    _ => null
                                };
                            case "MAPPINGCODE":
                                return op switch
                                {
                                    " eq " => x => x.MappingCode == value,
                                    " ne " => x => x.MappingCode != value,
                                    _ => null
                                };
                            case "MAPPINGNAME":
                                return op switch
                                {
                                    " eq " => x => x.MappingName == value,
                                    " ne " => x => x.MappingName != value,
                                    _ => null
                                };
                            case "MAPPINGPARENTCODE":
                                return op switch
                                {
                                    " eq " => x => x.MappingParentCode == value,
                                    " ne " => x => x.MappingParentCode != value,
                                    _ => null
                                };
                        }
                    }
                }
            }
        }
        return null;
    }
}
