using System.Collections.Generic;

namespace SI_ACC.Models;

public class BudgetMappingResponse
{
    public List<BudgetMappingItem> Value { get; set; } = new();
}

public class BudgetMappingItem
{
    public int? BudgetLevel { get; set; }
    public string? BudgetId { get; set; }
    public string? BudgetName { get; set; }
    public string? MappingCode { get; set; }
    public string? MappingName { get; set; }
    public string? MappingParentCode { get; set; }
    public int? AuxiliaryIndex1 { get; set; }
}
