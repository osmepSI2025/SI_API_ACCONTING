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

            var query = _context.MBudgetentires.AsQueryable();

            // Split by 'and' for multiple filters
            var filters = filter.Split("and", StringSplitOptions.RemoveEmptyEntries);

            // ... inside GetAllAsync(string filter) method ...

            foreach (var f in filters)
            {
                var filterStr = f.Trim();

                // Handle 'or' for the same field (e.g., EntryNo eq 610 or EntryNo eq 612)
                if (filterStr.Contains(" or "))
                {
                    var orParts = filterStr.Split("or", StringSplitOptions.RemoveEmptyEntries);
                    var orPredicates = new List<System.Linq.Expressions.Expression<Func<MBudgetentire, bool>>>();

                    foreach (var orPart in orParts)
                    {
                        var singlePredicate = BuildPredicate(orPart.Trim());
                        if (singlePredicate != null)
                            orPredicates.Add(singlePredicate);
                    }

                    if (orPredicates.Count > 0)
                    {
                        var parameter = System.Linq.Expressions.Expression.Parameter(typeof(MBudgetentire), "x");
                        System.Linq.Expressions.Expression? body = null;
                        foreach (var expr in orPredicates)
                        {
                            var replaced = System.Linq.Expressions.Expression.Invoke(expr, parameter);
                            body = body == null ? replaced : System.Linq.Expressions.Expression.OrElse(body, replaced);
                        }
                        if (body != null)
                        {
                            var lambda = System.Linq.Expressions.Expression.Lambda<Func<MBudgetentire, bool>>(body, parameter);
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
        catch (Exception)
        {
            return new List<MBudgetentire>();
        }
    }

    // Helper to build predicate for a single filter string
    private static System.Linq.Expressions.Expression<Func<MBudgetentire, bool>>? BuildPredicate(string filter)
    {
        // Supported operators
        var operators = new[] { " eq ", " ne ", " gt ", " lt ", " ge ", " le ", " in ", "startswith(", "endswith(", "contains(" };
        foreach (var op in operators)
        {
            if (filter.Contains(op))
            {
                if (op == " in ")
                {
                    // Example: EntryNo in (610, 612, 614)
                    var parts = filter.Split(" in ", StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        var field = parts[0].Trim().ToUpper();
                        var values = parts[1].Trim('(', ')', ' ').Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(v => v.Trim('\'', ' ')).ToArray();

                        switch (field)
                        {
                            case "ENTRYNO":
                                var entryNos = values.Select(v => int.TryParse(v, out int n) ? n : (int?)null)
                                    .Where(n => n.HasValue).Select(n => n.Value).ToList();
                                return x => entryNos.Contains(x.EntryNo ?? 0);
                            case "BUDGETLEVEL":
                                var budgetLevels = values.Select(v => int.TryParse(v, out int n) ? n : (int?)null)
                                    .Where(n => n.HasValue).Select(n => n.Value).ToList();
                                return x => budgetLevels.Contains(x.BudgetLevel ?? 0);
                            case "BUDGETCODE":
                                return x => values.Contains(x.BudgetCode);
                            case "BUDGETNAME":
                                return x => values.Contains(x.BudgetName);
                            case "BUDGETYEAR":
                                return x => values.Contains(x.BudgetYear);
                            case "BUDGETPLAN":
                                return x => values.Contains(x.BudgetPlan);
                            case "BUDGETSTRATEGIES":
                                return x => values.Contains(x.BudgetStrategies);
                            case "BUDGETACTIVITES":
                                return x => values.Contains(x.BudgetActivites);
                            case "POSTINGDATE":
                                var postingDates = values.Select(v => DateTime.TryParse(v, out DateTime dt) ? dt : (DateTime?)null)
                                    .Where(dt => dt.HasValue).Select(dt => dt.Value).ToList();
                                return x => postingDates.Contains(x.PostingDate ?? DateTime.MinValue);
                            case "DOCUMENTNO":
                                return x => values.Contains(x.DocumentNo);
                            case "ORIGINALAMOUNT":
                                var originalAmounts = values.Select(v => decimal.TryParse(v, out decimal d) ? d : (decimal?)null)
                                    .Where(d => d.HasValue).Select(d => d.Value).ToList();
                                return x => originalAmounts.Contains(x.OriginalAmount ?? 0);
                            case "TRANSDESCRIPTION":
                                return x => values.Contains(x.TransDescription);
                            case "TRANSDESCRIPTION2":
                                return x => values.Contains(x.TransDescription2);
                            case "TRANSFERTOLEVEL":
                                var transferToLevels = values.Select(v => int.TryParse(v, out int n) ? n : (int?)null)
                                    .Where(n => n.HasValue).Select(n => n.Value).ToList();
                                return x => transferToLevels.Contains(x.TransferToLevel ?? 0);
                            case "TRANSFERTOCODE":
                                return x => values.Contains(x.TransferToCode);
                            case "REFERENCEBUDGETCODE":
                                return x => values.Contains(x.ReferenceBudgetCode);
                            case "CURRENTBUDGETSTATUS":
                                return x => values.Contains(x.CurrentBudgetStatus);
                            case "RESERVEDAMOUNT":
                                var reservedAmounts = values.Select(v => decimal.TryParse(v, out decimal d) ? d : (decimal?)null)
                                    .Where(d => d.HasValue).Select(d => d.Value).ToList();
                                return x => reservedAmounts.Contains(x.ReservedAmount ?? 0);
                            case "ACTUALAMOUNT":
                                var actualAmounts = values.Select(v => decimal.TryParse(v, out decimal d) ? d : (decimal?)null)
                                    .Where(d => d.HasValue).Select(d => d.Value).ToList();
                                return x => actualAmounts.Contains(x.ActualAmount ?? 0);
                            case "ADVANCEAMOUNT":
                                var advanceAmounts = values.Select(v => decimal.TryParse(v, out decimal d) ? d : (decimal?)null)
                                    .Where(d => d.HasValue).Select(d => d.Value).ToList();
                                return x => advanceAmounts.Contains(x.AdvanceAmount ?? 0);
                            case "VOUCHERADVAMT":
                                var voucherAdvAmts = values.Select(v => decimal.TryParse(v, out decimal d) ? d : (decimal?)null)
                                    .Where(d => d.HasValue).Select(d => d.Value).ToList();
                                return x => voucherAdvAmts.Contains(x.VoucherAdvAmt ?? 0);
                            case "VOUCHERRECEIVEAMT":
                                var voucherReceiveAmts = values.Select(v => decimal.TryParse(v, out decimal d) ? d : (decimal?)null)
                                    .Where(d => d.HasValue).Select(d => d.Value).ToList();
                                return x => voucherReceiveAmts.Contains(x.VoucherReceiveAmt ?? 0);
                            case "VOUCHERPAYMENTAMT":
                                var voucherPaymentAmts = values.Select(v => decimal.TryParse(v, out decimal d) ? d : (decimal?)null)
                                    .Where(d => d.HasValue).Select(d => d.Value).ToList();
                                return x => voucherPaymentAmts.Contains(x.VoucherPaymentAmt ?? 0);
                            case "REMAININGAMOUNT":
                                var remainingAmounts = values.Select(v => decimal.TryParse(v, out decimal d) ? d : (decimal?)null)
                                    .Where(d => d.HasValue).Select(d => d.Value).ToList();
                                return x => remainingAmounts.Contains(x.RemainingAmount ?? 0);
                            case "ACTIVITYOUTSOURCEFUND":
                                var bools = values.Select(v => bool.TryParse(v, out bool b) ? b : (bool?)null)
                                    .Where(b => b.HasValue).Select(b => b.Value).ToList();
                                return x => bools.Contains(x.ActivityOutsourceFund ?? false);
                            case "BUDGETDEPARTMENT":
                                return x => values.Contains(x.BudgetDepartment);
                            case "BUDGETPROJECT":
                                return x => values.Contains(x.BudgetProject);
                            case "APIMAPPINGID":
                                return x => values.Contains(x.ApiMappingId);
                            case "AUXILIARYINDEX1":
                                var aux1s = values.Select(v => int.TryParse(v, out int n) ? n : (int?)null)
                                    .Where(n => n.HasValue).Select(n => n.Value).ToList();
                                return x => aux1s.Contains(x.AuxiliaryIndex1 ?? 0);
                            case "AUXILIARYINDEX2":
                                return x => values.Contains(x.AuxiliaryIndex2);
                            case "AUXILIARYINDEX3":
                                var aux3s = values.Select(v => int.TryParse(v, out int n) ? n : (int?)null)
                                    .Where(n => n.HasValue).Select(n => n.Value).ToList();
                                return x => aux3s.Contains(x.AuxiliaryIndex3 ?? 0);
                            case "AUXILIARYINDEX4":
                                var aux4s = values.Select(v => int.TryParse(v, out int n) ? n : (int?)null)
                                    .Where(n => n.HasValue).Select(n => n.Value).ToList();
                                return x => aux4s.Contains(x.AuxiliaryIndex4 ?? 0);
                                // Add more fields as needed
                        }
                    }
                }
                else if (op == " eq " || op == " ne " || op == " gt " || op == " lt " || op == " ge " || op == " le ")
                {
                    var parts = filter.Split(op, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        var field = parts[0].Trim().ToUpper();
                        var value = parts[1].Trim().Trim('\'');

                        switch (field)
                        {
                            case "ENTRYNO":
                                if (int.TryParse(value, out int entryNo))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.EntryNo == entryNo,
                                        " ne " => x => x.EntryNo != entryNo,
                                        " gt " => x => x.EntryNo > entryNo,
                                        " lt " => x => x.EntryNo < entryNo,
                                        " ge " => x => x.EntryNo >= entryNo,
                                        " le " => x => x.EntryNo <= entryNo,
                                        _ => null
                                    };
                                }
                                break;
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
                            case "BUDGETCODE":
                                return op switch
                                {
                                    " eq " => x => x.BudgetCode == value,
                                    " ne " => x => x.BudgetCode != value,
                                    _ => null
                                };
                            case "BUDGETNAME":
                                return op switch
                                {
                                    " eq " => x => x.BudgetName == value,
                                    " ne " => x => x.BudgetName != value,
                                    _ => null
                                };
                            case "BUDGETYEAR":
                                return op switch
                                {
                                    " eq " => x => x.BudgetYear == value,
                                    " ne " => x => x.BudgetYear != value,
                                    _ => null
                                };
                            case "BUDGETPLAN":
                                return op switch
                                {
                                    " eq " => x => x.BudgetPlan == value,
                                    " ne " => x => x.BudgetPlan != value,
                                    _ => null
                                };
                            case "BUDGETSTRATEGIES":
                                return op switch
                                {
                                    " eq " => x => x.BudgetStrategies == value,
                                    " ne " => x => x.BudgetStrategies != value,
                                    _ => null
                                };
                            case "BUDGETACTIVITES":
                                return op switch
                                {
                                    " eq " => x => x.BudgetActivites == value,
                                    " ne " => x => x.BudgetActivites != value,
                                    _ => null
                                };
                            case "POSTINGDATE":
                                if (DateTime.TryParse(value, out DateTime postingDate))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.PostingDate == postingDate,
                                        " ne " => x => x.PostingDate != postingDate,
                                        " gt " => x => x.PostingDate > postingDate,
                                        " lt " => x => x.PostingDate < postingDate,
                                        " ge " => x => x.PostingDate >= postingDate,
                                        " le " => x => x.PostingDate <= postingDate,
                                        _ => null
                                    };
                                }
                                break;
                            case "DOCUMENTNO":
                                return op switch
                                {
                                    " eq " => x => x.DocumentNo == value,
                                    " ne " => x => x.DocumentNo != value,
                                    _ => null
                                };
                            case "ORIGINALAMOUNT":
                                if (decimal.TryParse(value, out decimal originalAmount))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.OriginalAmount == originalAmount,
                                        " ne " => x => x.OriginalAmount != originalAmount,
                                        " gt " => x => x.OriginalAmount > originalAmount,
                                        " lt " => x => x.OriginalAmount < originalAmount,
                                        " ge " => x => x.OriginalAmount >= originalAmount,
                                        " le " => x => x.OriginalAmount <= originalAmount,
                                        _ => null
                                    };
                                }
                                break;
                            case "TRANSDESCRIPTION":
                                return op switch
                                {
                                    " eq " => x => x.TransDescription == value,
                                    " ne " => x => x.TransDescription != value,
                                    _ => null
                                };
                            case "TRANSDESCRIPTION2":
                                return op switch
                                {
                                    " eq " => x => x.TransDescription2 == value,
                                    " ne " => x => x.TransDescription2 != value,
                                    _ => null
                                };
                            case "TRANSFERTOLEVEL":
                                if (int.TryParse(value, out int transferToLevel))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.TransferToLevel == transferToLevel,
                                        " ne " => x => x.TransferToLevel != transferToLevel,
                                        " gt " => x => x.TransferToLevel > transferToLevel,
                                        " lt " => x => x.TransferToLevel < transferToLevel,
                                        " ge " => x => x.TransferToLevel >= transferToLevel,
                                        " le " => x => x.TransferToLevel <= transferToLevel,
                                        _ => null
                                    };
                                }
                                break;
                            case "TRANSFERTOCODE":
                                return op switch
                                {
                                    " eq " => x => x.TransferToCode == value,
                                    " ne " => x => x.TransferToCode != value,
                                    _ => null
                                };
                            case "REFERENCEBUDGETCODE":
                                return op switch
                                {
                                    " eq " => x => x.ReferenceBudgetCode == value,
                                    " ne " => x => x.ReferenceBudgetCode != value,
                                    _ => null
                                };
                            case "CURRENTBUDGETSTATUS":
                                return op switch
                                {
                                    " eq " => x => x.CurrentBudgetStatus == value,
                                    " ne " => x => x.CurrentBudgetStatus != value,
                                    _ => null
                                };
                            case "RESERVEDAMOUNT":
                                if (decimal.TryParse(value, out decimal reservedAmount))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.ReservedAmount == reservedAmount,
                                        " ne " => x => x.ReservedAmount != reservedAmount,
                                        " gt " => x => x.ReservedAmount > reservedAmount,
                                        " lt " => x => x.ReservedAmount < reservedAmount,
                                        " ge " => x => x.ReservedAmount >= reservedAmount,
                                        " le " => x => x.ReservedAmount <= reservedAmount,
                                        _ => null
                                    };
                                }
                                break;
                            case "ACTUALAMOUNT":
                                if (decimal.TryParse(value, out decimal actualAmount))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.ActualAmount == actualAmount,
                                        " ne " => x => x.ActualAmount != actualAmount,
                                        " gt " => x => x.ActualAmount > actualAmount,
                                        " lt " => x => x.ActualAmount < actualAmount,
                                        " ge " => x => x.ActualAmount >= actualAmount,
                                        " le " => x => x.ActualAmount <= actualAmount,
                                        _ => null
                                    };
                                }
                                break;
                            case "ADVANCEAMOUNT":
                                if (decimal.TryParse(value, out decimal advanceAmount))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.AdvanceAmount == advanceAmount,
                                        " ne " => x => x.AdvanceAmount != advanceAmount,
                                        " gt " => x => x.AdvanceAmount > advanceAmount,
                                        " lt " => x => x.AdvanceAmount < advanceAmount,
                                        " ge " => x => x.AdvanceAmount >= advanceAmount,
                                        " le " => x => x.AdvanceAmount <= advanceAmount,
                                        _ => null
                                    };
                                }
                                break;
                            case "VOUCHERADVAMT":
                                if (decimal.TryParse(value, out decimal voucherAdvAmt))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.VoucherAdvAmt == voucherAdvAmt,
                                        " ne " => x => x.VoucherAdvAmt != voucherAdvAmt,
                                        " gt " => x => x.VoucherAdvAmt > voucherAdvAmt,
                                        " lt " => x => x.VoucherAdvAmt < voucherAdvAmt,
                                        " ge " => x => x.VoucherAdvAmt >= voucherAdvAmt,
                                        " le " => x => x.VoucherAdvAmt <= voucherAdvAmt,
                                        _ => null
                                    };
                                }
                                break;
                            case "VOUCHERRECEIVEAMT":
                                if (decimal.TryParse(value, out decimal voucherReceiveAmt))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.VoucherReceiveAmt == voucherReceiveAmt,
                                        " ne " => x => x.VoucherReceiveAmt != voucherReceiveAmt,
                                        " gt " => x => x.VoucherReceiveAmt > voucherReceiveAmt,
                                        " lt " => x => x.VoucherReceiveAmt < voucherReceiveAmt,
                                        " ge " => x => x.VoucherReceiveAmt >= voucherReceiveAmt,
                                        " le " => x => x.VoucherReceiveAmt <= voucherReceiveAmt,
                                        _ => null
                                    };
                                }
                                break;
                            case "VOUCHERPAYMENTAMT":
                                if (decimal.TryParse(value, out decimal voucherPaymentAmt))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.VoucherPaymentAmt == voucherPaymentAmt,
                                        " ne " => x => x.VoucherPaymentAmt != voucherPaymentAmt,
                                        " gt " => x => x.VoucherPaymentAmt > voucherPaymentAmt,
                                        " lt " => x => x.VoucherPaymentAmt < voucherPaymentAmt,
                                        " ge " => x => x.VoucherPaymentAmt >= voucherPaymentAmt,
                                        " le " => x => x.VoucherPaymentAmt <= voucherPaymentAmt,
                                        _ => null
                                    };
                                }
                                break;
                            case "REMAININGAMOUNT":
                                if (decimal.TryParse(value, out decimal remainingAmount))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.RemainingAmount == remainingAmount,
                                        " ne " => x => x.RemainingAmount != remainingAmount,
                                        " gt " => x => x.RemainingAmount > remainingAmount,
                                        " lt " => x => x.RemainingAmount < remainingAmount,
                                        " ge " => x => x.RemainingAmount >= remainingAmount,
                                        " le " => x => x.RemainingAmount <= remainingAmount,
                                        _ => null
                                    };
                                }
                                break;
                            case "ACTIVITYOUTSOURCEFUND":
                                if (bool.TryParse(value, out bool activityOutsourceFund))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.ActivityOutsourceFund == activityOutsourceFund,
                                        " ne " => x => x.ActivityOutsourceFund != activityOutsourceFund,
                                        _ => null
                                    };
                                }
                                break;
                            case "BUDGETDEPARTMENT":
                                return op switch
                                {
                                    " eq " => x => x.BudgetDepartment == value,
                                    " ne " => x => x.BudgetDepartment != value,
                                    _ => null
                                };
                            case "BUDGETPROJECT":
                                return op switch
                                {
                                    " eq " => x => x.BudgetProject == value,
                                    " ne " => x => x.BudgetProject != value,
                                    _ => null
                                };
                            case "APIMAPPINGID":
                                return op switch
                                {
                                    " eq " => x => x.ApiMappingId == value,
                                    " ne " => x => x.ApiMappingId != value,
                                    _ => null
                                };
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
                            case "AUXILIARYINDEX2":
                                return op switch
                                {
                                    " eq " => x => x.AuxiliaryIndex2 == value,
                                    " ne " => x => x.AuxiliaryIndex2 != value,
                                    _ => null
                                };
                            case "AUXILIARYINDEX3":
                                if (int.TryParse(value, out int aux3))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.AuxiliaryIndex3 == aux3,
                                        " ne " => x => x.AuxiliaryIndex3 != aux3,
                                        " gt " => x => x.AuxiliaryIndex3 > aux3,
                                        " lt " => x => x.AuxiliaryIndex3 < aux3,
                                        " ge " => x => x.AuxiliaryIndex3 >= aux3,
                                        " le " => x => x.AuxiliaryIndex3 <= aux3,
                                        _ => null
                                    };
                                }
                                break;
                            case "AUXILIARYINDEX4":
                                if (int.TryParse(value, out int aux4))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.AuxiliaryIndex4 == aux4,
                                        " ne " => x => x.AuxiliaryIndex4 != aux4,
                                        " gt " => x => x.AuxiliaryIndex4 > aux4,
                                        " lt " => x => x.AuxiliaryIndex4 < aux4,
                                        " ge " => x => x.AuxiliaryIndex4 >= aux4,
                                        " le " => x => x.AuxiliaryIndex4 <= aux4,
                                        _ => null
                                    };
                                }
                                break;
                                // Add more all fields as needed
                        }
                    }
                }
                else if (op == "startswith(")
                {
                    // Example: startswith(BudgetCode, 'ABC')
                    var fieldAndValue = filter.Substring("startswith(".Length).TrimEnd(')');
                    var args = fieldAndValue.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    if (args.Length == 2)
                    {
                        var field = args[0].Trim().ToUpper();
                        var value = args[1].Trim().Trim('\'');
                        switch (field)
                        {
                            case "ENTRYNO":
                                if (int.TryParse(value, out int entryNo))
                                {
                                    return x => x.EntryNo != null && x.EntryNo == entryNo;
                                }
                                break;
                                case "BUDGETLEVEL":
                                    if (int.TryParse(value, out int budgetLevel))
                                {
                                    return x => x.BudgetLevel != null && x.BudgetLevel == budgetLevel;
                                    }break;
                                 
                            case "BUDGETCODE":
                                return x => x.BudgetCode != null && x.BudgetCode.StartsWith(value);
                            case "BUDGETNAME":
                                return x => x.BudgetName != null && x.BudgetName.StartsWith(value);
                            case "BUDGETYEAR":
                                return x => x.BudgetYear != null && x.BudgetYear.StartsWith(value);
                            case "BUDGETPLAN":
                                return x => x.BudgetPlan != null && x.BudgetPlan.StartsWith(value);
                            case "BUDGETSTRATEGIES":
                                return x => x.BudgetStrategies != null && x.BudgetStrategies.StartsWith(value);
                            case "BUDGETACTIVITES":
                                return x => x.BudgetActivites != null && x.BudgetActivites.StartsWith(value);

                            case "DOCUMENTNO":
                                return x => x.DocumentNo != null && x.DocumentNo.StartsWith(value);
                            case "ORIGINALAMOUNT":
                                if (decimal.TryParse(value, out decimal originalAmount))
                                {
                                    return x => x.OriginalAmount != null && x.OriginalAmount == originalAmount;
                                }
                                break;
                            case "TRANSFERTOLEVEL":
                                if (int.TryParse(value, out int transferToLevel))
                                {
                                    return x => x.TransferToLevel != null && x.TransferToLevel == transferToLevel;
                                }
                                break;

                            case "TRANSDESCRIPTION":
                                return x => x.TransDescription != null && x.TransDescription.StartsWith(value);
                            case "TRANSDESCRIPTION2":
                                return x => x.TransDescription2 != null && x.TransDescription2.StartsWith(value);
                            case "TRANSFERTOCODE":
                                return x => x.TransferToCode != null && x.TransferToCode.StartsWith(value);
                            case "RESERVEDAMOUNT":
                                if (decimal.TryParse(value, out decimal reservedAmount))
                                {
                                    return x => x.ReservedAmount != null && x.ReservedAmount == reservedAmount;
                                }
                                break;
                            case "ACTUALAMOUNT":
                                if (decimal.TryParse(value, out decimal actualAmount))
                                {
                                    return x => x.ActualAmount != null && x.ActualAmount == actualAmount;
                                }
                                break;
                            case "ADVANCEAMOUNT":
                                if (decimal.TryParse(value, out decimal advanceAmount))
                                {
                                    return x => x.AdvanceAmount != null && x.AdvanceAmount == advanceAmount;
                                }
                                break;
                            case "VOUCHERADVAMT":
                                if (decimal.TryParse(value, out decimal voucherAdvAmt))
                                {
                                    return x => x.VoucherAdvAmt != null && x.VoucherAdvAmt == voucherAdvAmt;
                                }
                                break;
                            case "VOUCHERRECEIVEAMT":
                                if (decimal.TryParse(value, out decimal voucherReceiveAmt))
                                {
                                    return x => x.VoucherReceiveAmt != null && x.VoucherReceiveAmt == voucherReceiveAmt;
                                }
                                break;
                            case "VOUCHERPAYMENTAMT":
                                if (decimal.TryParse(value, out decimal voucherPaymentAmt))
                                {
                                    return x => x.VoucherPaymentAmt != null && x.VoucherPaymentAmt == voucherPaymentAmt;
                                }
                                break;
                            case "REMAININGAMOUNT":
                                if (decimal.TryParse(value, out decimal remainingAmount))
                                {
                                    return x => x.RemainingAmount != null && x.RemainingAmount == remainingAmount;
                                }
                                break;
                            case "REFERENCEBUDGETCODE":
                                return x => x.ReferenceBudgetCode != null && x.ReferenceBudgetCode.StartsWith(value);
                            case "CURRENTBUDGETSTATUS":
                                return x => x.CurrentBudgetStatus != null && x.CurrentBudgetStatus.StartsWith(value);
                            case "BUDGETDEPARTMENT":
                                return x => x.BudgetDepartment != null && x.BudgetDepartment.StartsWith(value);
                            case "BUDGETPROJECT":
                                return x => x.BudgetProject != null && x.BudgetProject.StartsWith(value);
                            case "APIMAPPINGID":
                                return x => x.ApiMappingId != null && x.ApiMappingId.StartsWith(value);
                            case "AUXILIARYINDEX2":
                                return x => x.AuxiliaryIndex2 != null && x.AuxiliaryIndex2.StartsWith(value);
                                // Add more all fields as needed
                        }
                    }
                }
                else if (op == "endswith(")
                {
                    // Example: endswith(BudgetCode, 'XYZ')
                    var fieldAndValue = filter.Substring("endswith(".Length).TrimEnd(')');
                    var args = fieldAndValue.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    if (args.Length == 2)
                    {
                        var field = args[0].Trim().ToUpper();
                        var value = args[1].Trim().Trim('\'');
                        switch (field)
                        {
                            case "ENTRYNO":
                                if (int.TryParse(value, out int entryNo))
                                {
                                    return x => x.EntryNo != null && x.EntryNo == entryNo;
                                }
                                break;
                                case "BUDGETLEVEL":
                                    if (int.TryParse(value, out int budgetLevel))
                                {
                                    return x => x.BudgetLevel != null && x.BudgetLevel == budgetLevel;
                                }break;
                                 
                            case "BUDGETCODE":
                                return x => x.BudgetCode != null && x.BudgetCode.EndsWith(value);
                            case "BUDGETNAME":
                                return x => x.BudgetName != null && x.BudgetName.EndsWith(value);
                            case "BUDGETYEAR":
                                return x => x.BudgetYear != null && x.BudgetYear.EndsWith(value);
                            case "BUDGETPLAN":
                                return x => x.BudgetPlan != null && x.BudgetPlan.EndsWith(value);
                            case "BUDGETSTRATEGIES":
                                return x => x.BudgetStrategies != null && x.BudgetStrategies.EndsWith(value);
                            case "BUDGETACTIVITES":
                                return x => x.BudgetActivites != null && x.BudgetActivites.EndsWith(value);
                            case "DOCUMENTNO":
                                return x => x.DocumentNo != null && x.DocumentNo.EndsWith(value);
                                case "ORIGINALAMOUNT":
                                    if (decimal.TryParse(value, out decimal originalAmount))
                                {
                                    return x => x.OriginalAmount != null && x.OriginalAmount == originalAmount;
                                }break;
                                case "TRANSFERTOLEVEL":
                                    if (int.TryParse(value, out int transferToLevel))
                                {
                                    return x => x.TransferToLevel != null && x.TransferToLevel == transferToLevel;
                                    }break;
                                
                            case "TRANSDESCRIPTION":
                                return x => x.TransDescription != null && x.TransDescription.EndsWith(value);
                            case "TRANSDESCRIPTION2":
                                return x => x.TransDescription2 != null && x.TransDescription2.EndsWith(value);
                            case "TRANSFERTOCODE":
                                return x => x.TransferToCode != null && x.TransferToCode.EndsWith(value);
                                 case "RESERVEDAMOUNT":
                                if (decimal.TryParse(value, out decimal reservedAmount))
                                {
                                    return x => x.ReservedAmount != null && x.ReservedAmount == reservedAmount;
                                    }break;
                                case "ACTUALAMOUNT": 
                                    if (decimal.TryParse(value, out decimal actualAmount))
                                {
                                    return x => x.ActualAmount != null && x.ActualAmount == actualAmount;
                                    }break;
                                case "ADVANCEAMOUNT":
                                    if (decimal.TryParse(value, out decimal advanceAmount))
                                {
                                    return x => x.AdvanceAmount != null && x.AdvanceAmount == advanceAmount;
                                    }break;
                                case "VOUCHERADVAMT":
                                    if (decimal.TryParse(value, out decimal voucherAdvAmt))
                                {
                                    return x => x.VoucherAdvAmt != null && x.VoucherAdvAmt == voucherAdvAmt;
                                    }break;
                                case "VOUCHERRECEIVEAMT":
                                    if (decimal.TryParse(value, out decimal voucherReceiveAmt))
                                {
                                    return x => x.VoucherReceiveAmt != null && x.VoucherReceiveAmt == voucherReceiveAmt;
                                    }break;
                                case "VOUCHERPAYMENTAMT":
                                    if (decimal.TryParse(value, out decimal voucherPaymentAmt))
                                {
                                    return x => x.VoucherPaymentAmt != null && x.VoucherPaymentAmt == voucherPaymentAmt;
                                    }break;
                                case "REMAININGAMOUNT":
                                    if (decimal.TryParse(value, out decimal remainingAmount))
                                {
                                    return x => x.RemainingAmount != null && x.RemainingAmount == remainingAmount;
                                    }break;

                            case "REFERENCEBUDGETCODE":
                                return x => x.ReferenceBudgetCode != null && x.ReferenceBudgetCode.EndsWith(value);
                            case "CURRENTBUDGETSTATUS":
                                return x => x.CurrentBudgetStatus != null && x.CurrentBudgetStatus.EndsWith(value);
                            case "BUDGETDEPARTMENT":
                                return x => x.BudgetDepartment != null && x.BudgetDepartment.EndsWith(value);
                            case "BUDGETPROJECT":
                                return x => x.BudgetProject != null && x.BudgetProject.EndsWith(value);
                            case "APIMAPPINGID":
                                return x => x.ApiMappingId != null && x.ApiMappingId.EndsWith(value);
                            case "AUXILIARYINDEX2":
                                return x => x.AuxiliaryIndex2 != null && x.AuxiliaryIndex2.EndsWith(value);
                              
                                // Add more all fields as needed
                        }
                    }
                }
                else if (op == "contains(")
                {
                    // Example: contains(BudgetName, 'ABC')
                    var fieldAndValue = filter.Substring("contains(".Length).TrimEnd(')');
                    var args = fieldAndValue.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    if (args.Length == 2)
                    {
                        var field = args[0].Trim().ToUpper();
                        var values = args[1].Trim().Trim('\'');
                        switch (field)
                        {
                            case "ENTRYNO":

                                return x => x.EntryNo != null && x.EntryNo.Value.ToString().Contains(values);
                            case "BUDGETLEVEL":
                                return x => x.BudgetLevel != null && x.BudgetLevel.Value.ToString().Contains(values);
                            case "BUDGETCODE":
                                return x => x.BudgetCode.Contains(values);
                            case "BUDGETNAME":
                                return x => x.BudgetName.Contains(values);
                            case "BUDGETYEAR":
                                return x => x.BudgetYear.Contains(values);
                            case "BUDGETPLAN":
                                return x => x.BudgetPlan.Contains(values);
                            case "BUDGETSTRATEGIES":
                                return x => x.BudgetStrategies.Contains(values);
                            case "BUDGETACTIVITES":
                                return x => x.BudgetActivites.Contains(values);
                            case "DOCUMENTNO":
                                return x => x.DocumentNo.Contains(values);
                            case "TRANSDESCRIPTION":
                                return x => x.TransDescription.Contains(values);
                            case "TRANSDESCRIPTION2":
                                return x => x.TransDescription2.Contains(values);
                            case "TRANSFERTOCODE":
                                return x => x.TransferToCode.Contains(values);
                            case "REFERENCEBUDGETCODE":
                                return x => x.ReferenceBudgetCode.Contains(values);
                            case "CURRENTBUDGETSTATUS":
                                return x => x.CurrentBudgetStatus.Contains(values);
                            case "BUDGETDEPARTMENT":
                                return x => x.BudgetDepartment.Contains(values);
                            case "BUDGETPROJECT":
                                return x => x.BudgetProject.Contains(values);
                            case "APIMAPPINGID":
                                return x => x.ApiMappingId.Contains(values);
                            case "AUXILIARYINDEX2":
                                return x => x.AuxiliaryIndex2.Contains(values);
                                //// Numeric fields
                                case "TRANSFERTOLEVEL":
                                return x => x.TransferToLevel != null && values.Contains(x.TransferToLevel.Value.ToString());
                            case "AUXILIARYINDEX1":
          
                                return x => x.AuxiliaryIndex1 != null && values.Contains(x.AuxiliaryIndex1.Value.ToString());

                            case "AUXILIARYINDEX3":
                               
                                return x => x.AuxiliaryIndex3 != null && values.Contains(x.AuxiliaryIndex3.Value.ToString());

                            case "AUXILIARYINDEX4":
                          
                                return x => x.AuxiliaryIndex4 != null && values.Contains(x.AuxiliaryIndex4.Value.ToString());

                            // Decimal fields
                            case "ORIGINALAMOUNT":
                  
                                return x => x.OriginalAmount != null && values.Contains(x.OriginalAmount.Value.ToString());

                            case "RESERVEDAMOUNT":
                
                                return x => x.ReservedAmount != null && values.Contains(x.ReservedAmount.Value.ToString());

                            case "ACTUALAMOUNT":
                     
                                return x => x.ActualAmount != null && values.Contains(x.ActualAmount.Value.ToString());

                            case "ADVANCEAMOUNT":
                   
                                return x => x.AdvanceAmount != null && values.Contains(x.AdvanceAmount.Value.ToString());

                            case "VOUCHERADVAMT":
                         
                                return x => x.VoucherAdvAmt != null && values.Contains(x.VoucherAdvAmt.Value.ToString());

                            case "VOUCHERRECEIVEAMT":
                   
                                return x => x.VoucherReceiveAmt != null && values.Contains(x.VoucherReceiveAmt.Value.ToString());

                            case "VOUCHERPAYMENTAMT":
                  
                                return x => x.VoucherPaymentAmt != null && values.Contains(x.VoucherPaymentAmt.Value.ToString());

                            case "REMAININGAMOUNT":
                    
                                return x => x.RemainingAmount != null && values.Contains(x.RemainingAmount.Value.ToString());

                            // DateTime fields
                            case "POSTINGDATE":
                     
                                return x => x.PostingDate != null && values.Contains(x.PostingDate.Value.ToString());

                            // Boolean fields
                            case "ACTIVITYOUTSOURCEFUND":
                         
                                return x => x.ActivityOutsourceFund != null && values.Contains(x.ActivityOutsourceFund.Value.ToString());

                        }
                    }
                }
            }
        }
        return null;
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
