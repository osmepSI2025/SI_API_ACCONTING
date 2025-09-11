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
        public async Task<IActionResult> Get_Budgetavailable(string? filter)
        {
            var result = await _budgetService.GetAllAsync(filter);
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
        public async Task<IActionResult> Get_MBudgetentire(string? filter)
        {
            var result = await _budgetentireService.GetAllAsync(filter);
            return Ok(result);
        }
        [HttpGet("/api/SYS-ACCOUNT/batch-Budgetentire")]
        public async Task<IActionResult> Batch_MBudgetentire()
        {
            // year thai
            int currentYearThai = DateTime.Now.Year + 543;
            for (int yy = currentYearThai; yy <= currentYearThai + 1; yy++)
            {
                string yearth = (yy % 100).ToString();
                for (int mm = 1; mm <= 12; mm++)
                {
                    string bdy = "budgetyear eq '" + yearth + "/" + mm.ToString()+ "'";
                 //   string bdy = "budgetyear eq '68/2' and currentbudgetstatus ne 'Cancel' and budgetlevel eq 6";


                    await _budgetentireService.BatchEndOfDay_BudgetEntireBySearch(bdy);
                }

            }
            return Ok();
        }

        // GET: /<BudgetMapping>/
        [HttpGet("BudgetMapping")]
        public async Task<IActionResult> Get_BudgetMapping(string? filter)
        {
            var result = await _budgetMappingService.GetAllAsync(filter);
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
