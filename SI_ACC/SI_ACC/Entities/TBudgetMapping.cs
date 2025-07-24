using System;
using System.Collections.Generic;

namespace SI_ACC.Entities;

public partial class TBudgetMapping
{
    public int Id { get; set; }

    public int? BudgetLevel { get; set; }

    public string? BudgetId { get; set; }

    public string? BudgetName { get; set; }

    public string? MappingCode { get; set; }

    public string? MappingName { get; set; }

    public string? MappingParentCode { get; set; }

    public string? AuxiliaryIndex1 { get; set; }
}
