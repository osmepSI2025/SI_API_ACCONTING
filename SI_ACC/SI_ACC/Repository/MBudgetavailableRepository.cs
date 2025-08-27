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

    public async Task<List<MBudgetavailable>> GetAllAsync(string filter)
    {
        try
        {
            if (string.IsNullOrEmpty(filter))
            {
                return await _context.MBudgetavailables.ToListAsync();
            }
            else
            {
                var filters = filter.Split("and", StringSplitOptions.RemoveEmptyEntries);
                var query = _context.MBudgetavailables.AsQueryable();

                foreach (var f in filters)
                {
                    var parts = f.Split("eq", StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        var field = parts[0].Trim().ToUpper();
                        var value = parts[1].Trim().Trim('\'');

                        switch (field)
                        {
                            case "BUDGETID":
                                query = query.Where(b => b.BudgetId == value);
                                break;
                            case "BUDGETLEVEL":
                                if (int.TryParse(value, out int level))
                                    query = query.Where(b => b.BudgetLevel == level);
                                break;
                            case "BUDGETNAME":
                                query = query.Where(b => b.BudgetName == value);
                                break;
                            case "BUDGETSTATUS":
                                query = query.Where(b => b.BudgetStatus == value);
                                break;
                            case "BUDGETSTARTDATE":
                                if (DateTime.TryParse(value, out DateTime startDate))
                                    query = query.Where(b => b.BudgetStartDate == startDate);
                                break;
                            case "BUDGETEXPIREDATE":
                                if (DateTime.TryParse(value, out DateTime expireDate))
                                    query = query.Where(b => b.BudgetExpireDate == expireDate);
                                break;
                            case "INITIALAMOUNT":
                                if (decimal.TryParse(value, out decimal initialAmount))
                                    query = query.Where(b => b.InitialAmount == initialAmount);
                                break;
                            case "OUTSOURCEFUND":
                                if (decimal.TryParse(value, out decimal outsourceFund))
                                    query = query.Where(b => b.OutsourceFund == outsourceFund);
                                break;
                            case "AVAILABLEAMOUNT":
                                if (decimal.TryParse(value, out decimal availableAmount))
                                    query = query.Where(b => b.AvailableAmount == availableAmount);
                                break;
                            case "BUDGETRECEIVE":
                                if (decimal.TryParse(value, out decimal budgetReceive))
                                    query = query.Where(b => b.BudgetReceive == budgetReceive);
                                break;
                            case "BUDGETRETURN":
                                if (decimal.TryParse(value, out decimal budgetReturn))
                                    query = query.Where(b => b.BudgetReturn == budgetReturn);
                                break;
                            case "REFERENCEBUDGETCODE":
                                query = query.Where(b => b.ReferenceBudgetCode == value);
                                break;
                            case "BUDGETLV1CODE":
                                query = query.Where(b => b.BudgetLv1Code == value);
                                break;
                            case "BUDGETLV2CODE":
                                query = query.Where(b => b.BudgetLv2Code == value);
                                break;
                            case "BUDGETLV3CODE":
                                query = query.Where(b => b.BudgetLv3Code == value);
                                break;
                            case "BUDGETLV4CODE":
                                query = query.Where(b => b.BudgetLv4Code == value);
                                break;
                            case "BUDGETLV5CODE":
                                query = query.Where(b => b.BudgetLv5Code == value);
                                break;
                            case "BUDGETLV6CODE":
                                query = query.Where(b => b.BudgetLv6Code == value);
                                break;
                            case "DETAILINITIALAMOUNT":
                                if (decimal.TryParse(value, out decimal detailInitialAmount))
                                    query = query.Where(b => b.DetailInitialAmount == detailInitialAmount);
                                break;
                            case "BUDGETAMOUNT":
                                if (decimal.TryParse(value, out decimal budgetAmount))
                                    query = query.Where(b => b.BudgetAmount == budgetAmount);
                                break;
                            case "ALLOCATEAMOUNT":
                                if (decimal.TryParse(value, out decimal allocateAmount))
                                    query = query.Where(b => b.AllocateAmount == allocateAmount);
                                break;
                            case "RESERVEDAMOUNT":
                                if (decimal.TryParse(value, out decimal reservedAmount))
                                    query = query.Where(b => b.ReservedAmount == reservedAmount);
                                break;
                            case "ADVANCEAMOUNT":
                                if (decimal.TryParse(value, out decimal advanceAmount))
                                    query = query.Where(b => b.AdvanceAmount == advanceAmount);
                                break;
                            case "ACTUALAMOUNT":
                                if (decimal.TryParse(value, out decimal actualAmount))
                                    query = query.Where(b => b.ActualAmount == actualAmount);
                                break;
                            case "VOUCHERPAYMENTAMT":
                                if (decimal.TryParse(value, out decimal voucherPaymentAmt))
                                    query = query.Where(b => b.VoucherPaymentAmt == voucherPaymentAmt);
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
            return new List<MBudgetavailable>();
        }
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
