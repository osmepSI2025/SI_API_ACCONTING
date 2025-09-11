// File: Repositories/MBudgetavailableRepository.cs
using Microsoft.EntityFrameworkCore;
using SI_ACC.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                    var filterStr = f.Trim();

                    // Handle 'or' for the same field (e.g., EntryNo eq 610 or EntryNo eq 612)
                    if (filterStr.Contains(" or "))
                    {
                        var orParts = filterStr.Split("or", StringSplitOptions.RemoveEmptyEntries);
                        var orPredicates = new List<System.Linq.Expressions.Expression<Func<MBudgetavailable, bool>>>();

                        foreach (var orPart in orParts)
                        {
                            var singlePredicate = BuildPredicate(orPart.Trim());
                            if (singlePredicate != null)
                                orPredicates.Add(singlePredicate);
                        }

                        if (orPredicates.Count > 0)
                        {
                            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(MBudgetavailable), "x");
                            System.Linq.Expressions.Expression? body = null;
                            foreach (var expr in orPredicates)
                            {
                                var replaced = System.Linq.Expressions.Expression.Invoke(expr, parameter);
                                body = body == null ? replaced : System.Linq.Expressions.Expression.OrElse(body, replaced);
                            }
                            if (body != null)
                            {
                                var lambda = System.Linq.Expressions.Expression.Lambda<Func<MBudgetavailable, bool>>(body, parameter);
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
            return new List<MBudgetavailable>();
        }
    }
    private static System.Linq.Expressions.Expression<Func<MBudgetavailable, bool>>? BuildPredicate(string filter)
    {
        var operators = new[] { " eq ", " ne ", " gt ", " lt ", " ge ", " le ", " in ", "startswith(", "endswith(", "contains(" };
        foreach (var op in operators)
        {
            if (filter.Contains(op))
            {
                if (op == " in ")
                {
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
                            case "BUDGETID":
                                return x => values.Contains(x.BudgetId);
                            case "BUDGETNAME":
                                return x => values.Contains(x.BudgetName);
                            case "BUDGETSTATUS":
                                return x => values.Contains(x.BudgetStatus);
                            case "REFERENCEBUDGETCODE":
                                return x => values.Contains(x.ReferenceBudgetCode);
                            case "BUDGETLV1CODE":
                                return x => values.Contains(x.BudgetLv1Code);
                            case "BUDGETLV2CODE":
                                return x => values.Contains(x.BudgetLv2Code);
                            case "BUDGETLV3CODE":
                                return x => values.Contains(x.BudgetLv3Code);
                            case "BUDGETLV4CODE":
                                return x => values.Contains(x.BudgetLv4Code);
                            case "BUDGETLV5CODE":
                                return x => values.Contains(x.BudgetLv5Code);
                            case "BUDGETLV6CODE":
                                return x => values.Contains(x.BudgetLv6Code);
                            case "BUDGETSTARTDATE":
                                var startDates = values.Select(v => DateTime.TryParse(v, out DateTime dt) ? dt : (DateTime?)null)
                                    .Where(dt => dt.HasValue).Select(dt => dt.Value).ToList();
                                return x => startDates.Contains(x.BudgetStartDate ?? DateTime.MinValue);
                            case "BUDGETEXPIREDATE":
                                var expireDates = values.Select(v => DateTime.TryParse(v, out DateTime dt) ? dt : (DateTime?)null)
                                    .Where(dt => dt.HasValue).Select(dt => dt.Value).ToList();
                                return x => expireDates.Contains(x.BudgetExpireDate ?? DateTime.MinValue);
                            case "INITIALAMOUNT":
                                var initialAmounts = values.Select(v => decimal.TryParse(v, out decimal d) ? d : (decimal?)null)
                                    .Where(d => d.HasValue).Select(d => d.Value).ToList();
                                return x => initialAmounts.Contains(x.InitialAmount ?? 0);
                            case "OUTSOURCEFUND":
                                var outsourceFunds = values.Select(v => decimal.TryParse(v, out decimal d) ? d : (decimal?)null)
                                    .Where(d => d.HasValue).Select(d => d.Value).ToList();
                                return x => outsourceFunds.Contains(x.OutsourceFund ?? 0);
                            case "AVAILABLEAMOUNT":
                                var availableAmounts = values.Select(v => decimal.TryParse(v, out decimal d) ? d : (decimal?)null)
                                    .Where(d => d.HasValue).Select(d => d.Value).ToList();
                                return x => availableAmounts.Contains(x.AvailableAmount ?? 0);
                            case "BUDGETRECEIVE":
                                var budgetReceives = values.Select(v => decimal.TryParse(v, out decimal d) ? d : (decimal?)null)
                                    .Where(d => d.HasValue).Select(d => d.Value).ToList();
                                return x => budgetReceives.Contains(x.BudgetReceive ?? 0);
                            case "BUDGETRETURN":
                                var budgetReturns = values.Select(v => decimal.TryParse(v, out decimal d) ? d : (decimal?)null)
                                    .Where(d => d.HasValue).Select(d => d.Value).ToList();
                                return x => budgetReturns.Contains(x.BudgetReturn ?? 0);
                            case "DETAILINITIALAMOUNT":
                                var detailInitialAmounts = values.Select(v => decimal.TryParse(v, out decimal d) ? d : (decimal?)null)
                                    .Where(d => d.HasValue).Select(d => d.Value).ToList();
                                return x => detailInitialAmounts.Contains(x.DetailInitialAmount ?? 0);
                            case "BUDGETAMOUNT":
                                var budgetAmounts = values.Select(v => decimal.TryParse(v, out decimal d) ? d : (decimal?)null)
                                    .Where(d => d.HasValue).Select(d => d.Value).ToList();
                                return x => budgetAmounts.Contains(x.BudgetAmount ?? 0);
                            case "ALLOCATEAMOUNT":
                                var allocateAmounts = values.Select(v => decimal.TryParse(v, out decimal d) ? d : (decimal?)null)
                                    .Where(d => d.HasValue).Select(d => d.Value).ToList();
                                return x => allocateAmounts.Contains(x.AllocateAmount ?? 0);
                            case "RESERVEDAMOUNT":
                                var reservedAmounts = values.Select(v => decimal.TryParse(v, out decimal d) ? d : (decimal?)null)
                                    .Where(d => d.HasValue).Select(d => d.Value).ToList();
                                return x => reservedAmounts.Contains(x.ReservedAmount ?? 0);
                            case "ADVANCEAMOUNT":
                                var advanceAmounts = values.Select(v => decimal.TryParse(v, out decimal d) ? d : (decimal?)null)
                                    .Where(d => d.HasValue).Select(d => d.Value).ToList();
                                return x => advanceAmounts.Contains(x.AdvanceAmount ?? 0);
                            case "ACTUALAMOUNT":
                                var actualAmounts = values.Select(v => decimal.TryParse(v, out decimal d) ? d : (decimal?)null)
                                    .Where(d => d.HasValue).Select(d => d.Value).ToList();
                                return x => actualAmounts.Contains(x.ActualAmount ?? 0);
                            case "VOUCHERPAYMENTAMT":
                                var voucherPaymentAmts = values.Select(v => decimal.TryParse(v, out decimal d) ? d : (decimal?)null)
                                    .Where(d => d.HasValue).Select(d => d.Value).ToList();
                                return x => voucherPaymentAmts.Contains(x.VoucherPaymentAmt ?? 0);
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
                            case "BUDGETSTATUS":
                                return op switch
                                {
                                    " eq " => x => x.BudgetStatus == value,
                                    " ne " => x => x.BudgetStatus != value,
                                    _ => null
                                };
                            case "REFERENCEBUDGETCODE":
                                return op switch
                                {
                                    " eq " => x => x.ReferenceBudgetCode == value,
                                    " ne " => x => x.ReferenceBudgetCode != value,
                                    _ => null
                                };
                            case "BUDGETLV1CODE":
                                return op switch
                                {
                                    " eq " => x => x.BudgetLv1Code == value,
                                    " ne " => x => x.BudgetLv1Code != value,
                                    _ => null
                                };
                            case "BUDGETLV2CODE":
                                return op switch
                                {
                                    " eq " => x => x.BudgetLv2Code == value,
                                    " ne " => x => x.BudgetLv2Code != value,
                                    _ => null
                                };
                            case "BUDGETLV3CODE":
                                return op switch
                                {
                                    " eq " => x => x.BudgetLv3Code == value,
                                    " ne " => x => x.BudgetLv3Code != value,
                                    _ => null
                                };
                            case "BUDGETLV4CODE":
                                return op switch
                                {
                                    " eq " => x => x.BudgetLv4Code == value,
                                    " ne " => x => x.BudgetLv4Code != value,
                                    _ => null
                                };
                            case "BUDGETLV5CODE":
                                return op switch
                                {
                                    " eq " => x => x.BudgetLv5Code == value,
                                    " ne " => x => x.BudgetLv5Code != value,
                                    _ => null
                                };
                            case "BUDGETLV6CODE":
                                return op switch
                                {
                                    " eq " => x => x.BudgetLv6Code == value,
                                    " ne " => x => x.BudgetLv6Code != value,
                                    _ => null
                                };
                            case "BUDGETSTARTDATE":
                                if (DateTime.TryParse(value, out DateTime startDate))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.BudgetStartDate == startDate,
                                        " ne " => x => x.BudgetStartDate != startDate,
                                        " gt " => x => x.BudgetStartDate > startDate,
                                        " lt " => x => x.BudgetStartDate < startDate,
                                        " ge " => x => x.BudgetStartDate >= startDate,
                                        " le " => x => x.BudgetStartDate <= startDate,
                                        _ => null
                                    };
                                }
                                break;
                            case "BUDGETEXPIREDATE":
                                if (DateTime.TryParse(value, out DateTime expireDate))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.BudgetExpireDate == expireDate,
                                        " ne " => x => x.BudgetExpireDate != expireDate,
                                        " gt " => x => x.BudgetExpireDate > expireDate,
                                        " lt " => x => x.BudgetExpireDate < expireDate,
                                        " ge " => x => x.BudgetExpireDate >= expireDate,
                                        " le " => x => x.BudgetExpireDate <= expireDate,
                                        _ => null
                                    };
                                }
                                break;
                            case "INITIALAMOUNT":
                                if (decimal.TryParse(value, out decimal initialAmount))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.InitialAmount == initialAmount,
                                        " ne " => x => x.InitialAmount != initialAmount,
                                        " gt " => x => x.InitialAmount > initialAmount,
                                        " lt " => x => x.InitialAmount < initialAmount,
                                        " ge " => x => x.InitialAmount >= initialAmount,
                                        " le " => x => x.InitialAmount <= initialAmount,
                                        _ => null
                                    };
                                }
                                break;
                            case "OUTSOURCEFUND":
                                if (decimal.TryParse(value, out decimal outsourceFund))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.OutsourceFund == outsourceFund,
                                        " ne " => x => x.OutsourceFund != outsourceFund,
                                        " gt " => x => x.OutsourceFund > outsourceFund,
                                        " lt " => x => x.OutsourceFund < outsourceFund,
                                        " ge " => x => x.OutsourceFund >= outsourceFund,
                                        " le " => x => x.OutsourceFund <= outsourceFund,
                                        _ => null
                                    };
                                }
                                break;
                            case "AVAILABLEAMOUNT":
                                if (decimal.TryParse(value, out decimal availableAmount))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.AvailableAmount == availableAmount,
                                        " ne " => x => x.AvailableAmount != availableAmount,
                                        " gt " => x => x.AvailableAmount > availableAmount,
                                        " lt " => x => x.AvailableAmount < availableAmount,
                                        " ge " => x => x.AvailableAmount >= availableAmount,
                                        " le " => x => x.AvailableAmount <= availableAmount,
                                        _ => null
                                    };
                                }
                                break;
                            case "BUDGETRECEIVE":
                                if (decimal.TryParse(value, out decimal budgetReceive))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.BudgetReceive == budgetReceive,
                                        " ne " => x => x.BudgetReceive != budgetReceive,
                                        " gt " => x => x.BudgetReceive > budgetReceive,
                                        " lt " => x => x.BudgetReceive < budgetReceive,
                                        " ge " => x => x.BudgetReceive >= budgetReceive,
                                        " le " => x => x.BudgetReceive <= budgetReceive,
                                        _ => null
                                    };
                                }
                                break;
                            case "BUDGETRETURN":
                                if (decimal.TryParse(value, out decimal budgetReturn))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.BudgetReturn == budgetReturn,
                                        " ne " => x => x.BudgetReturn != budgetReturn,
                                        " gt " => x => x.BudgetReturn > budgetReturn,
                                        " lt " => x => x.BudgetReturn < budgetReturn,
                                        " ge " => x => x.BudgetReturn >= budgetReturn,
                                        " le " => x => x.BudgetReturn <= budgetReturn,
                                        _ => null
                                    };
                                }
                                break;
                            case "DETAILINITIALAMOUNT":
                                if (decimal.TryParse(value, out decimal detailInitialAmount))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.DetailInitialAmount == detailInitialAmount,
                                        " ne " => x => x.DetailInitialAmount != detailInitialAmount,
                                        " gt " => x => x.DetailInitialAmount > detailInitialAmount,
                                        " lt " => x => x.DetailInitialAmount < detailInitialAmount,
                                        " ge " => x => x.DetailInitialAmount >= detailInitialAmount,
                                        " le " => x => x.DetailInitialAmount <= detailInitialAmount,
                                        _ => null
                                    };
                                }
                                break;
                            case "BUDGETAMOUNT":
                                if (decimal.TryParse(value, out decimal budgetAmount))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.BudgetAmount == budgetAmount,
                                        " ne " => x => x.BudgetAmount != budgetAmount,
                                        " gt " => x => x.BudgetAmount > budgetAmount,
                                        " lt " => x => x.BudgetAmount < budgetAmount,
                                        " ge " => x => x.BudgetAmount >= budgetAmount,
                                        " le " => x => x.BudgetAmount <= budgetAmount,
                                        _ => null
                                    };
                                }
                                break;
                            case "ALLOCATEAMOUNT":
                                if (decimal.TryParse(value, out decimal allocateAmount))
                                {
                                    return op switch
                                    {
                                        " eq " => x => x.AllocateAmount == allocateAmount,
                                        " ne " => x => x.AllocateAmount != allocateAmount,
                                        " gt " => x => x.AllocateAmount > allocateAmount,
                                        " lt " => x => x.AllocateAmount < allocateAmount,
                                        " ge " => x => x.AllocateAmount >= allocateAmount,
                                        " le " => x => x.AllocateAmount <= allocateAmount,
                                        _ => null
                                    };
                                }
                                break;
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
                            case "BUDGETSTATUS":
                                return x => x.BudgetStatus != null && x.BudgetStatus.StartsWith(value);
                                case "BUDGETLEVEL":
                                    if (int.TryParse(value, out int budgetLevel))
                                {
                                    return x => x.BudgetLevel != null && x.BudgetLevel.ToString().StartsWith(value);
                                }
                                    break;
                                case "BUDGETSTARTDATE":
                                    if (DateTime.TryParse(value, out DateTime startDate))
                                {
                                    return x => x.BudgetStartDate != null && x.BudgetStartDate.Value.ToString("yyyy-MM-dd").StartsWith(value);
                                } break;
                                case "BUDGETEXPIREDATE":
                                    if (DateTime.TryParse(value, out DateTime expireDate))
                                {
                                    return x => x.BudgetExpireDate != null && x.BudgetExpireDate.Value.ToString("yyyy-MM-dd").StartsWith(value);
                                } break;
                                case "INITIALAMOUNT":
                                    if (decimal.TryParse(value, out decimal initialAmount))
                                {
                                    return x => x.InitialAmount != null && x.InitialAmount.Value.ToString().StartsWith(value);
                                } break;
                                case "OUTSOURCEFUND":
                                    if (decimal.TryParse(value, out decimal outsourceFund))
                                {
                                    return x => x.OutsourceFund != null && x.OutsourceFund.Value.ToString().StartsWith(value);
                                } break;
                                case "AVAILABLEAMOUNT":
                                    if (decimal.TryParse(value, out decimal availableAmount))
                                {
                                    return x => x.AvailableAmount != null && x.AvailableAmount.Value.ToString().StartsWith(value);
                                } break;
                                case "BUDGETRECEIVE":
                                    if (decimal.TryParse(value, out decimal budgetReceive))
                                {
                                    return x => x.BudgetReceive != null && x.BudgetReceive.Value.ToString().StartsWith(value);
                                } break;
                                case "BUDGETRETURN":
                                    if (decimal.TryParse(value, out decimal budgetReturn))
                                {
                                    return x => x.BudgetReturn != null && x.BudgetReturn.Value.ToString().StartsWith(value);
                                } break;
                                case "DETAILINITIALAMOUNT":
                                    if (decimal.TryParse(value, out decimal detailInitialAmount))
                                {
                                    return x => x.DetailInitialAmount != null && x.DetailInitialAmount.Value.ToString().StartsWith(value);
                                } break;
                                case "BUDGETAMOUNT":
                                    if (decimal.TryParse(value, out decimal budgetAmount))
                                {
                                    return x => x.BudgetAmount != null && x.BudgetAmount.Value.ToString().StartsWith(value);
                                } break;
                                case "ALLOCATEAMOUNT":
                                    if (decimal.TryParse(value, out decimal allocateAmount))
                                {
                                    return x => x.AllocateAmount != null && x.AllocateAmount.Value.ToString().StartsWith(value);
                                } break;
                                case "RESERVEDAMOUNT":
                                    if (decimal.TryParse(value, out decimal reservedAmount))
                                {
                                    return x => x.ReservedAmount != null && x.ReservedAmount.Value.ToString().StartsWith(value);
                                } break;
                                case "ADVANCEAMOUNT":
                                    if (decimal.TryParse(value, out decimal advanceAmount))
                                {
                                    return x => x.AdvanceAmount != null && x.AdvanceAmount.Value.ToString().StartsWith(value);
                                } break;
                                case "ACTUALAMOUNT":
                                    if (decimal.TryParse(value, out decimal actualAmount))
                                {
                                    return x => x.ActualAmount != null && x.ActualAmount.Value.ToString().StartsWith(value);
                                } break;
                                case "VOUCHERPAYMENTAMT":
                                    if (decimal.TryParse(value, out decimal voucherPaymentAmt))
                                {
                                    return x => x.VoucherPaymentAmt != null && x.VoucherPaymentAmt.Value.ToString().StartsWith(value);
                                } break;

                            case "REFERENCEBUDGETCODE":
                                return x => x.ReferenceBudgetCode != null && x.ReferenceBudgetCode.StartsWith(value);
                            case "BUDGETLV1CODE":
                                return x => x.BudgetLv1Code != null && x.BudgetLv1Code.StartsWith(value);
                            case "BUDGETLV2CODE":
                                return x => x.BudgetLv2Code != null && x.BudgetLv2Code.StartsWith(value);
                            case "BUDGETLV3CODE":
                                return x => x.BudgetLv3Code != null && x.BudgetLv3Code.StartsWith(value);
                            case "BUDGETLV4CODE":
                                return x => x.BudgetLv4Code != null && x.BudgetLv4Code.StartsWith(value);
                            case "BUDGETLV5CODE":
                                return x => x.BudgetLv5Code != null && x.BudgetLv5Code.StartsWith(value);
                            case "BUDGETLV6CODE":
                                return x => x.BudgetLv6Code != null && x.BudgetLv6Code.StartsWith(value);
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
                            case "BUDGETLEVEL":
                                if (int.TryParse(value, out int budgetLevel))
                                {
                                    return x => x.BudgetLevel != null && x.BudgetLevel.ToString().EndsWith(value);
                                }
                                break;
                            case "BUDGETID":
                                return x => x.BudgetId != null && x.BudgetId.EndsWith(value);
                            case "BUDGETNAME":
                                return x => x.BudgetName != null && x.BudgetName.EndsWith(value);
                            case "BUDGETSTATUS":
                                return x => x.BudgetStatus != null && x.BudgetStatus.EndsWith(value);
                                case "BUDGETSTARTDATE":
                                    if (DateTime.TryParse(value, out DateTime startDate))
                                {
                                    return x => x.BudgetStartDate != null && x.BudgetStartDate.Value.ToString("yyyy-MM-dd").EndsWith(value);
                                } break;
                                case "BUDGETEXPIREDATE":
                                    if (DateTime.TryParse(value, out DateTime expireDate))
                                {
                                    return x => x.BudgetExpireDate != null && x.BudgetExpireDate.Value.ToString("yyyy-MM-dd").EndsWith(value);
                                } break;    
                                case "INITIALAMOUNT":
                                    if (decimal.TryParse(value, out decimal initialAmount))
                                {
                                    return x => x.InitialAmount != null && x.InitialAmount.Value.ToString().EndsWith(value);
                                } break;
                                case "OUTSOURCEFUND":
                                    if (decimal.TryParse(value, out decimal outsourceFund))
                                {
                                    return x => x.OutsourceFund != null && x.OutsourceFund.Value.ToString().EndsWith(value);
                                } break;
                                case "AVAILABLEAMOUNT":
                                    if (decimal.TryParse(value, out decimal availableAmount))
                                {
                                    return x => x.AvailableAmount != null && x.AvailableAmount.Value.ToString().EndsWith(value);
                                } break;
                                case "BUDGETRECEIVE":
                                    if (decimal.TryParse(value, out decimal budgetReceive))
                                {
                                    return x => x.BudgetReceive != null && x.BudgetReceive.Value.ToString().EndsWith(value);
                                } break;
                                case "BUDGETRETURN":
                                    if (decimal.TryParse(value, out decimal budgetReturn))
                                {
                                    return x => x.BudgetReturn != null && x.BudgetReturn.Value.ToString().EndsWith(value);
                                } break;
                                case "DETAILINITIALAMOUNT":
                                    if (decimal.TryParse(value, out decimal detailInitialAmount))
                                {
                                    return x => x.DetailInitialAmount != null && x.DetailInitialAmount.Value.ToString().EndsWith(value);
                                } break;
                                case "BUDGETAMOUNT":
                                    if (decimal.TryParse(value, out decimal budgetAmount))
                                {
                                    return x => x.BudgetAmount != null && x.BudgetAmount.Value.ToString().EndsWith(value);
                                } break;
                                case "ALLOCATEAMOUNT":
                                    if (decimal.TryParse(value, out decimal allocateAmount))
                                {
                                    return x => x.AllocateAmount != null && x.AllocateAmount.Value.ToString().EndsWith(value);
                                } break;
                                case "RESERVEDAMOUNT":
                                    if (decimal.TryParse(value, out decimal reservedAmount))
                                {
                                    return x => x.ReservedAmount != null && x.ReservedAmount.Value.ToString().EndsWith(value);
                                } break;
                                case "ADVANCEAMOUNT":
                                    if (decimal.TryParse(value, out decimal advanceAmount))
                                {
                                    return x => x.AdvanceAmount != null && x.AdvanceAmount.Value.ToString().EndsWith(value);
                                } break;
                                case "ACTUALAMOUNT":
                                    if (decimal.TryParse(value, out decimal actualAmount))
                                {
                                    return x => x.ActualAmount != null && x.ActualAmount.Value.ToString().EndsWith(value);
                                } break;
                                case "VOUCHERPAYMENTAMT":
                                    if (decimal.TryParse(value, out decimal voucherPaymentAmt))
                                {
                                    return x => x.VoucherPaymentAmt != null && x.VoucherPaymentAmt.Value.ToString().EndsWith(value);
                                } break;


                            case "REFERENCEBUDGETCODE":
                                return x => x.ReferenceBudgetCode != null && x.ReferenceBudgetCode.EndsWith(value);
                            case "BUDGETLV1CODE":
                                return x => x.BudgetLv1Code != null && x.BudgetLv1Code.EndsWith(value);
                            case "BUDGETLV2CODE":
                                return x => x.BudgetLv2Code != null && x.BudgetLv2Code.EndsWith(value);
                            case "BUDGETLV3CODE":
                                return x => x.BudgetLv3Code != null && x.BudgetLv3Code.EndsWith(value);
                            case "BUDGETLV4CODE":
                                return x => x.BudgetLv4Code != null && x.BudgetLv4Code.EndsWith(value);
                            case "BUDGETLV5CODE":
                                return x => x.BudgetLv5Code != null && x.BudgetLv5Code.EndsWith(value);
                            case "BUDGETLV6CODE":
                                return x => x.BudgetLv6Code != null && x.BudgetLv6Code.EndsWith(value);
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
                        var values = args[1].Trim().Trim('\'');

                        switch (field)
                        {
                            case "BUDGETID":
                                return x => x.BudgetId != null && x.BudgetId.Contains(values);
                            case "BUDGETNAME":
                                // return x => x.BudgetName != null && values.Contains(x.BudgetName);
                                return x => x.BudgetName != null && x.BudgetName.Contains(values);
                            case "BUDGETSTATUS":
                                return x => x.BudgetStatus != null && values.Contains(x.BudgetStatus);
                                case "BUDGETLEVEL":
                                    if (int.TryParse(values, out int budgetLevel))
                                {
                                    return x => x.BudgetLevel != null && x.BudgetLevel.ToString().Contains(values);
                                }
                                    break;
                                case "BUDGETSTARTDATE":
                                    if (DateTime.TryParse(values, out DateTime startDate))
                                {
                                    return x => x.BudgetStartDate != null && x.BudgetStartDate.Value.ToString("yyyy-MM-dd").Contains(values);
                                } break;
                                case "BUDGETEXPIREDATE":
                                    if (DateTime.TryParse(values, out DateTime expireDate))
                                {
                                    return x => x.BudgetExpireDate != null && x.BudgetExpireDate.Value.ToString("yyyy-MM-dd").Contains(values);
                                } break;
                                case "INITIALAMOUNT":
                                    if (decimal.TryParse(values, out decimal initialAmount))
                                {
                                    return x => x.InitialAmount != null && x.InitialAmount.Value.ToString().Contains(values);
                                } break;
                                case "OUTSOURCEFUND":
                                    if (decimal.TryParse(values, out decimal outsourceFund))
                                {
                                    return x => x.OutsourceFund != null && x.OutsourceFund.Value.ToString().Contains(values);
                                } break;
                                case "AVAILABLEAMOUNT":
                                    if (decimal.TryParse(values, out decimal availableAmount))
                                {
                                    return x => x.AvailableAmount != null && x.AvailableAmount.Value.ToString().Contains(values);
                                } break;
                                case "BUDGETRECEIVE":
                                    if (decimal.TryParse(values, out decimal budgetReceive))
                                {
                                    return x => x.BudgetReceive != null && x.BudgetReceive.Value.ToString().Contains(values);
                                } break;
                                case "BUDGETRETURN":
                                    if (decimal.TryParse(values, out decimal budgetReturn))
                                {
                                    return x => x.BudgetReturn != null && x.BudgetReturn.Value.ToString().Contains(values);
                                } break;
                                case "DETAILINITIALAMOUNT":
                                    if (decimal.TryParse(values, out decimal detailInitialAmount))
                                {
                                    return x => x.DetailInitialAmount != null && x.DetailInitialAmount.Value.ToString().Contains(values);
                                } break;
                                case "BUDGETAMOUNT":
                                    if (decimal.TryParse(values, out decimal budgetAmount))
                                {
                                    return x => x.BudgetAmount != null && x.BudgetAmount.Value.ToString().Contains(values);
                                } break;
                                case "ALLOCATEAMOUNT":
                                    if (decimal.TryParse(values, out decimal allocateAmount))
                                {
                                    return x => x.AllocateAmount != null && x.AllocateAmount.Value.ToString().Contains(values);
                                } break;
                                case "RESERVEDAMOUNT":
                                    if (decimal.TryParse(values, out decimal reservedAmount))
                                {
                                    return x => x.ReservedAmount != null && x.ReservedAmount.Value.ToString().Contains(values);
                                } break;
                                case "ADVANCEAMOUNT":
                                    if (decimal.TryParse(values, out decimal advanceAmount))
                                {
                                    return x => x.AdvanceAmount != null && x.AdvanceAmount.Value.ToString().Contains(values);
                                } break;
                                case "ACTUALAMOUNT":
                                    if (decimal.TryParse(values, out decimal actualAmount))
                                {
                                    return x => x.ActualAmount != null && x.ActualAmount.Value.ToString().Contains(values);
                                } break;
                                case "VOUCHERPAYMENTAMT":
                                    if (decimal.TryParse(values, out decimal voucherPaymentAmt))
                                {
                                    return x => x.VoucherPaymentAmt != null && x.VoucherPaymentAmt.Value.ToString().Contains(values);
                                } break;

                            case "REFERENCEBUDGETCODE":
                                return x => x.ReferenceBudgetCode != null && x.ReferenceBudgetCode.Contains(values);
                            case "BUDGETLV1CODE":
                                return x => x.BudgetLv1Code != null && x.BudgetLv1Code.Contains(values);
                            case "BUDGETLV2CODE":
                                return x => x.BudgetLv2Code != null && x.BudgetLv2Code.Contains(values);
                            case "BUDGETLV3CODE":
                                return x => x.BudgetLv3Code != null && x.BudgetLv3Code.Contains(values);
                            case "BUDGETLV4CODE":
                                return x => x.BudgetLv4Code != null && x.BudgetLv4Code.Contains(values);
                            case "BUDGETLV5CODE":
                                return x => x.BudgetLv5Code != null && x.BudgetLv5Code.Contains(values);
                            case "BUDGETLV6CODE":
                                return x => x.BudgetLv6Code != null && x.BudgetLv6Code.Contains(values);
                        }
                    }
                }
            }
        }
        return null;
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
