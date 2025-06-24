using System;
using System.Collections.Generic;

namespace SI_ACC.Entities;

public partial class MBudgetentire
{
    public int Id { get; set; }

    public int? EntryNo { get; set; }

    public int? BudgetLevel { get; set; }

    public string? BudgetCode { get; set; }

    public string? BudgetName { get; set; }

    public string? BudgetYear { get; set; }

    public string? BudgetPlan { get; set; }

    public string? BudgetStrategies { get; set; }

    public string? BudgetActivites { get; set; }

    public DateTime? PostingDate { get; set; }

    public string? DocumentNo { get; set; }

    public decimal? OriginalAmount { get; set; }

    public string? TransDescription { get; set; }

    public string? TransDescription2 { get; set; }

    public int? TransferToLevel { get; set; }

    public string? TransferToCode { get; set; }

    public string? ReferenceBudgetCode { get; set; }

    public string? CurrentBudgetStatus { get; set; }

    public decimal? ReservedAmount { get; set; }

    public decimal? ActualAmount { get; set; }

    public decimal? AdvanceAmount { get; set; }

    public decimal? VoucherAdvAmt { get; set; }

    public decimal? VoucherReceiveAmt { get; set; }

    public decimal? VoucherPaymentAmt { get; set; }

    public decimal? RemainingAmount { get; set; }

    public bool? ActivityOutsourceFund { get; set; }

    public string? BudgetDepartment { get; set; }

    public string? BudgetProject { get; set; }

    public string? ApiMappingId { get; set; }

    public int? AuxiliaryIndex1 { get; set; }

    public string? AuxiliaryIndex2 { get; set; }

    public int? AuxiliaryIndex3 { get; set; }

    public int? AuxiliaryIndex4 { get; set; }
}
