using System;
using System.Collections.Generic;

namespace SI_ACC.Entities;

public partial class MBudgetavailable
{
    public int Id { get; set; }

    public int? BudgetLevel { get; set; }

    public string? BudgetId { get; set; }

    public string? BudgetName { get; set; }

    public string? BudgetStatus { get; set; }

    public DateTime? BudgetStartDate { get; set; }

    public DateTime? BudgetExpireDate { get; set; }

    public decimal? InitialAmount { get; set; }

    public decimal? OutsourceFund { get; set; }

    public decimal? AvailableAmount { get; set; }

    public decimal? BudgetReceive { get; set; }

    public decimal? BudgetReturn { get; set; }

    public string? ReferenceBudgetCode { get; set; }

    public string? BudgetLv1Code { get; set; }

    public string? BudgetLv2Code { get; set; }

    public string? BudgetLv3Code { get; set; }

    public string? BudgetLv4Code { get; set; }

    public string? BudgetLv5Code { get; set; }

    public string? BudgetLv6Code { get; set; }

    public decimal? DetailInitialAmount { get; set; }

    public decimal? BudgetAmount { get; set; }

    public decimal? AllocateAmount { get; set; }

    public decimal? ReservedAmount { get; set; }

    public decimal? AdvanceAmount { get; set; }

    public decimal? ActualAmount { get; set; }

    public decimal? VoucherPaymentAmt { get; set; }
}
