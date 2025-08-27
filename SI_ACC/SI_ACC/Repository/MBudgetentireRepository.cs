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

    public async Task<List<MBudgetentire>> GetAllAsync(string filter)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return await _context.MBudgetentires.ToListAsync();
            }

            var filters = filter.Split("and", StringSplitOptions.RemoveEmptyEntries);
            var query = _context.MBudgetentires.AsQueryable();

            foreach (var f in filters)
            {
                var parts = f.Split("eq", StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    var field = parts[0].Trim().ToUpper();
                    var value = parts[1].Trim().Trim('\'');

                    switch (field)
                    {
                      
                        case "ENTRYNO":
                            if (int.TryParse(value, out int entryNo))
                                query = query.Where(x => x.EntryNo == entryNo);
                            break;
                        case "BUDGETLEVEL":
                            if (int.TryParse(value, out int budgetLevel))
                                query = query.Where(x => x.BudgetLevel == budgetLevel);
                            break;
                        case "BUDGETCODE":
                            query = query.Where(x => x.BudgetCode == value);
                            break;
                        case "BUDGETNAME":
                            query = query.Where(x => x.BudgetName == value);
                            break;
                        case "BUDGETYEAR":
                            query = query.Where(x => x.BudgetYear == value);
                            break;
                        case "BUDGETPLAN":
                            query = query.Where(x => x.BudgetPlan == value);
                            break;
                        case "BUDGETSTRATEGIES":
                            query = query.Where(x => x.BudgetStrategies == value);
                            break;
                        case "BUDGETACTIVITES":
                            query = query.Where(x => x.BudgetActivites == value);
                            break;
                        case "POSTINGDATE":
                            if (DateTime.TryParse(value, out DateTime postingDate))
                                query = query.Where(x => x.PostingDate == postingDate);
                            break;
                        case "DOCUMENTNO":
                            query = query.Where(x => x.DocumentNo == value);
                            break;
                        case "ORIGINALAMOUNT":
                            if (decimal.TryParse(value, out decimal originalAmount))
                                query = query.Where(x => x.OriginalAmount == originalAmount);
                            break;
                        case "TRANSDESCRIPTION":
                            query = query.Where(x => x.TransDescription == value);
                            break;
                        case "TRANSDESCRIPTION2":
                            query = query.Where(x => x.TransDescription2 == value);
                            break;
                        case "TRANSFERTOLEVEL":
                            if (int.TryParse(value, out int transferToLevel))
                                query = query.Where(x => x.TransferToLevel == transferToLevel);
                            break;
                        case "TRANSFERTOCODE":
                            query = query.Where(x => x.TransferToCode == value);
                            break;
                        case "REFERENCEBUDGETCODE":
                            query = query.Where(x => x.ReferenceBudgetCode == value);
                            break;
                        case "CURRENTBUDGETSTATUS":
                            query = query.Where(x => x.CurrentBudgetStatus == value);
                            break;
                        case "RESERVEDAMOUNT":
                            if (decimal.TryParse(value, out decimal reservedAmount))
                                query = query.Where(x => x.ReservedAmount == reservedAmount);
                            break;
                        case "ACTUALAMOUNT":
                            if (decimal.TryParse(value, out decimal actualAmount))
                                query = query.Where(x => x.ActualAmount == actualAmount);
                            break;
                        case "ADVANCEAMOUNT":
                            if (decimal.TryParse(value, out decimal advanceAmount))
                                query = query.Where(x => x.AdvanceAmount == advanceAmount);
                            break;
                        case "VOUCHERADVAMT":
                            if (decimal.TryParse(value, out decimal voucherAdvAmt))
                                query = query.Where(x => x.VoucherAdvAmt == voucherAdvAmt);
                            break;
                        case "VOUCHERRECEIVEAMT":
                            if (decimal.TryParse(value, out decimal voucherReceiveAmt))
                                query = query.Where(x => x.VoucherReceiveAmt == voucherReceiveAmt);
                            break;
                        case "VOUCHERPAYMENTAMT":
                            if (decimal.TryParse(value, out decimal voucherPaymentAmt))
                                query = query.Where(x => x.VoucherPaymentAmt == voucherPaymentAmt);
                            break;
                        case "REMAININGAMOUNT":
                            if (decimal.TryParse(value, out decimal remainingAmount))
                                query = query.Where(x => x.RemainingAmount == remainingAmount);
                            break;
                        case "ACTIVITYOUTSOURCEFUND":
                            if (bool.TryParse(value, out bool activityOutsourceFund))
                                query = query.Where(x => x.ActivityOutsourceFund == activityOutsourceFund);
                            break;
                        case "BUDGETDEPARTMENT":
                            query = query.Where(x => x.BudgetDepartment == value);
                            break;
                        case "BUDGETPROJECT":
                            query = query.Where(x => x.BudgetProject == value);
                            break;
                        case "APIMAPPINGID":
                            query = query.Where(x => x.ApiMappingId == value);
                            break;
                        case "AUXILIARYINDEX1":
                            if (int.TryParse(value, out int aux1))
                                query = query.Where(x => x.AuxiliaryIndex1 == aux1);
                            break;
                        case "AUXILIARYINDEX2":
                            query = query.Where(x => x.AuxiliaryIndex2 == value);
                            break;
                        case "AUXILIARYINDEX3":
                            if (int.TryParse(value, out int aux3))
                                query = query.Where(x => x.AuxiliaryIndex3 == aux3);
                            break;
                        case "AUXILIARYINDEX4":
                            if (int.TryParse(value, out int aux4))
                                query = query.Where(x => x.AuxiliaryIndex4 == aux4);
                            break;
                            // Add more fields as needed
                    }
                }
            }
            return await query.ToListAsync();
        }
        catch (Exception)
        {
            return new List<MBudgetentire>();
        }
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
