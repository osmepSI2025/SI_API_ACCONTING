using Microsoft.AspNetCore.Mvc;

namespace SI_ACC.Controllers
{
    [ApiController]
    [Route("api/SYS-ACCOUNT")]
    public class AccountingController : ControllerBase
    {
        private readonly MBudgetavailableService _budgetService;
        private readonly MBudgetentireService _budgetentireService;
        private readonly TBudgetMappingService _budgetMappingService;
        public AccountingController(MBudgetavailableService budgetService
            , MBudgetentireService budgetentireService,
TBudgetMappingService budgetMappingService)
        {
            _budgetService = budgetService;
            _budgetentireService = budgetentireService;
            _budgetMappingService = budgetMappingService;
        }


        // GET: /<controller>/
        [HttpGet("Master/GetBudgetavailable")]
        public async Task<IActionResult> Get_Budgetavailable()
        {
            var result = await _budgetService.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("batch-Budgetavailable")]
        public async Task<IActionResult> Batch_Budgetavailable()
        {
            await _budgetService.BatchEndOfDay_Budgetavailable();
            return Ok();
        }

        // GET: /<Budgetentire>/
        [HttpGet("Budgetentire")]
        public async Task<IActionResult> Get_MBudgetentire()
        {
            var result = await _budgetentireService.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("/api/SYS-ACCOUNT/batch-Budgetentire")]
        public async Task<IActionResult> Batch_MBudgetentire()
        {
            await _budgetentireService.BatchEndOfDay_BudgetEntire();
            return Ok();
        }

        // GET: /<BudgetMapping>/
        [HttpGet("BudgetMapping")]
        public async Task<IActionResult> Get_BudgetMapping()
        {
            var result = await _budgetMappingService.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("batch-BudgetMapping")]
        public async Task<IActionResult> Batch_BudgetMapping()
        {
            await _budgetMappingService.BatchEndOfDay_BudgetMapping();
            return Ok();
        }
    }
    
}
