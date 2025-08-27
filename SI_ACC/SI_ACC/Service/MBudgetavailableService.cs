// File: Services/MBudgetavailableService.cs
using System.Linq;
using System.Threading.Tasks;
using SI_ACC.Models;
using SI_ACC.Entities;
using System.Collections.Generic;
using SI_ACC.Repository;
using SI_ACC.Services;
using System.Text.Json;

public class MBudgetavailableService 
{
    private readonly MBudgetavailableRepository _repository;
    private readonly ICallAPIService _serviceApi;
    private readonly IApiInformationRepository _repositoryApi;
    private readonly string _FlagDev;

    public MBudgetavailableService(MBudgetavailableRepository repository
        , IConfiguration configuration
    , ICallAPIService serviceApi, IApiInformationRepository repositoryApi)
    {
        _repository = repository;
        _serviceApi = serviceApi;
        _repositoryApi = repositoryApi;
        _FlagDev = configuration["Devlopment:FlagDev"] ?? throw new ArgumentNullException("FlagDev is missing in appsettings.json");

    }
    public async Task BatchEndOfDay_Budgetavailable()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };
        var BudgetAvailableResponse = new BudgetAvailableResponse();
       var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "budgetavailable" });
            var apiParam = LApi.Select(x => new MapiInformationModels
            {
                ServiceNameCode = x.ServiceNameCode,
                ApiKey = x.ApiKey,
                AuthorizationType = x.AuthorizationType,
                ContentType = x.ContentType,
                CreateDate = x.CreateDate,
                Id = x.Id,
                MethodType = x.MethodType,
                ServiceNameTh = x.ServiceNameTh,
                Urldevelopment = x.Urldevelopment,
                Urlproduction = x.Urlproduction,
                Username = x.Username,
                Password = x.Password,
                UpdateDate = x.UpdateDate,
                Bearer = x.Bearer,
                AccessToken = x.AccessToken,

            }).FirstOrDefault(); // Use FirstOrDefault to handle empty lists

            var apiResponse = await _serviceApi.GetDataApiAsync(apiParam, null);
            var result = JsonSerializer.Deserialize<BudgetAvailableResponse>(apiResponse, options);

            BudgetAvailableResponse = result ?? new BudgetAvailableResponse();

        if (BudgetAvailableResponse.Value != null)
        {
            foreach (var item in BudgetAvailableResponse.Value)
            {
                try
                {
                    var existing = await _repository.GetByIdAsync(item.BudgetId);

                    if (existing == null)
                    {
                        // Create new record
                        var newData = new MBudgetavailable
                        {
                            BudgetLevel = item.BudgetLevel,
                            BudgetId = item.BudgetId,
                            BudgetName = item.BudgetName,
                            BudgetStatus = item.BudgetStatus,
                            BudgetStartDate = item.BudgetStartDate,
                            BudgetExpireDate = item.BudgetExpireDate,
                            InitialAmount = item.InitialAmount,
                            OutsourceFund = item.OutsourceFund,
                            AvailableAmount = item.AvailableAmount,
                            BudgetReceive = item.BudgetReceive,
                            BudgetReturn = item.BudgetReturn,
                            ReferenceBudgetCode = item.ReferenceBudgetCode,
                            BudgetLv1Code = item.BudgetLv1Code,
                            BudgetLv2Code = item.BudgetLv2Code,
                            BudgetLv3Code = item.BudgetLv3Code,
                            BudgetLv4Code = item.BudgetLv4Code,
                            BudgetLv5Code = item.BudgetLv5Code,
                            BudgetLv6Code = item.BudgetLv6Code,
                            DetailInitialAmount = item.DetailInitialAmount,
                            BudgetAmount = item.BudgetAmount,
                            AllocateAmount = item.AllocateAmount,
                            ReservedAmount = item.ReservedAmount,
                            AdvanceAmount = item.AdvanceAmount,
                            ActualAmount = item.ActualAmount,
                            VoucherPaymentAmt = item.VoucherPaymentAmt
                        };

                        await _repository.AddAsync(newData);
                        Console.WriteLine($"[INFO] Created new MBudgetavailable with BudgetId {newData.BudgetId}");
                    }
                    else
                    {
                        // Update existing record
                        existing.BudgetLevel = item.BudgetLevel;
                        existing.BudgetName = item.BudgetName;
                        existing.BudgetStatus = item.BudgetStatus;
                        existing.BudgetStartDate = item.BudgetStartDate;
                        existing.BudgetExpireDate = item.BudgetExpireDate;
                        existing.InitialAmount = item.InitialAmount;
                        existing.OutsourceFund = item.OutsourceFund;
                        existing.AvailableAmount = item.AvailableAmount;
                        existing.BudgetReceive = item.BudgetReceive;
                        existing.BudgetReturn = item.BudgetReturn;
                        existing.ReferenceBudgetCode = item.ReferenceBudgetCode;
                        existing.BudgetLv1Code = item.BudgetLv1Code;
                        existing.BudgetLv2Code = item.BudgetLv2Code;
                        existing.BudgetLv3Code = item.BudgetLv3Code;
                        existing.BudgetLv4Code = item.BudgetLv4Code;
                        existing.BudgetLv5Code = item.BudgetLv5Code;
                        existing.BudgetLv6Code = item.BudgetLv6Code;
                        existing.DetailInitialAmount = item.DetailInitialAmount;
                        existing.BudgetAmount = item.BudgetAmount;
                        existing.AllocateAmount = item.AllocateAmount;
                        existing.ReservedAmount = item.ReservedAmount;
                        existing.AdvanceAmount = item.AdvanceAmount;
                        existing.ActualAmount = item.ActualAmount;
                        existing.VoucherPaymentAmt = item.VoucherPaymentAmt;

                        await _repository.UpdateAsync(existing);
                        Console.WriteLine($"[INFO] Updated MBudgetavailable with BudgetId {existing.BudgetId}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to process MBudgetavailable BudgetId {item.BudgetId}: {ex.Message}");
                }
            }
        }


    }
    public async Task BatchEndOfDay_BudgetavailableBySearch(string filter)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };
        var BudgetAvailableResponse = new BudgetAvailableResponse();
        var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "budgetavailable" });
        var apiParam = LApi.Select(x => new MapiInformationModels
        {
            ServiceNameCode = x.ServiceNameCode,
            ApiKey = x.ApiKey,
            AuthorizationType = x.AuthorizationType,
            ContentType = x.ContentType,
            CreateDate = x.CreateDate,
            Id = x.Id,
            MethodType = x.MethodType,
            ServiceNameTh = x.ServiceNameTh,
            Urldevelopment = x.Urldevelopment,
            Urlproduction = x.Urlproduction,
            Username = x.Username,
            Password = x.Password,
            UpdateDate = x.UpdateDate,
            Bearer = x.Bearer,
            AccessToken = x.AccessToken,

        }).FirstOrDefault(); // Use FirstOrDefault to handle empty lists

        var apiResponse = await _serviceApi.GetDataApiAsync(apiParam, filter);
        var result = JsonSerializer.Deserialize<BudgetAvailableResponse>(apiResponse, options);

        BudgetAvailableResponse = result ?? new BudgetAvailableResponse();

        if (BudgetAvailableResponse.Value != null)
        {
            foreach (var item in BudgetAvailableResponse.Value)
            {
                try
                {
                    var existing = await _repository.GetByIdAsync(item.BudgetId);

                    if (existing == null)
                    {
                        // Create new record
                        var newData = new MBudgetavailable
                        {
                            BudgetLevel = item.BudgetLevel,
                            BudgetId = item.BudgetId,
                            BudgetName = item.BudgetName,
                            BudgetStatus = item.BudgetStatus,
                            BudgetStartDate = item.BudgetStartDate,
                            BudgetExpireDate = item.BudgetExpireDate,
                            InitialAmount = item.InitialAmount,
                            OutsourceFund = item.OutsourceFund,
                            AvailableAmount = item.AvailableAmount,
                            BudgetReceive = item.BudgetReceive,
                            BudgetReturn = item.BudgetReturn,
                            ReferenceBudgetCode = item.ReferenceBudgetCode,
                            BudgetLv1Code = item.BudgetLv1Code,
                            BudgetLv2Code = item.BudgetLv2Code,
                            BudgetLv3Code = item.BudgetLv3Code,
                            BudgetLv4Code = item.BudgetLv4Code,
                            BudgetLv5Code = item.BudgetLv5Code,
                            BudgetLv6Code = item.BudgetLv6Code,
                            DetailInitialAmount = item.DetailInitialAmount,
                            BudgetAmount = item.BudgetAmount,
                            AllocateAmount = item.AllocateAmount,
                            ReservedAmount = item.ReservedAmount,
                            AdvanceAmount = item.AdvanceAmount,
                            ActualAmount = item.ActualAmount,
                            VoucherPaymentAmt = item.VoucherPaymentAmt
                        };

                        await _repository.AddAsync(newData);
                        Console.WriteLine($"[INFO] Created new MBudgetavailable with BudgetId {newData.BudgetId}");
                    }
                    else
                    {
                        // Update existing record
                        existing.BudgetLevel = item.BudgetLevel;
                        existing.BudgetName = item.BudgetName;
                        existing.BudgetStatus = item.BudgetStatus;
                        existing.BudgetStartDate = item.BudgetStartDate;
                        existing.BudgetExpireDate = item.BudgetExpireDate;
                        existing.InitialAmount = item.InitialAmount;
                        existing.OutsourceFund = item.OutsourceFund;
                        existing.AvailableAmount = item.AvailableAmount;
                        existing.BudgetReceive = item.BudgetReceive;
                        existing.BudgetReturn = item.BudgetReturn;
                        existing.ReferenceBudgetCode = item.ReferenceBudgetCode;
                        existing.BudgetLv1Code = item.BudgetLv1Code;
                        existing.BudgetLv2Code = item.BudgetLv2Code;
                        existing.BudgetLv3Code = item.BudgetLv3Code;
                        existing.BudgetLv4Code = item.BudgetLv4Code;
                        existing.BudgetLv5Code = item.BudgetLv5Code;
                        existing.BudgetLv6Code = item.BudgetLv6Code;
                        existing.DetailInitialAmount = item.DetailInitialAmount;
                        existing.BudgetAmount = item.BudgetAmount;
                        existing.AllocateAmount = item.AllocateAmount;
                        existing.ReservedAmount = item.ReservedAmount;
                        existing.AdvanceAmount = item.AdvanceAmount;
                        existing.ActualAmount = item.ActualAmount;
                        existing.VoucherPaymentAmt = item.VoucherPaymentAmt;

                        await _repository.UpdateAsync(existing);
                        Console.WriteLine($"[INFO] Updated MBudgetavailable with BudgetId {existing.BudgetId}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to process MBudgetavailable BudgetId {item.BudgetId}: {ex.Message}");
                }
            }
        }


    }
    public async Task<BudgetAvailableResponse> GetAllAsync(string filter)
    {
        try
        {
            var entities = await _repository.GetAllAsync(filter);
            if (entities == null || !entities.Any())
            {
                await BatchEndOfDay_BudgetavailableBySearch(filter);
                entities = await _repository.GetAllAsync(filter);

                if (entities == null || !entities.Any())
                {
                    return new BudgetAvailableResponse
                    {
                        Value = new List<BudgetAvailableItem>()
                    };
                }
            }

            var response = new BudgetAvailableResponse
            {
                Value = entities.Select(e => new BudgetAvailableItem
                {
                    BudgetLevel = e.BudgetLevel ?? 0,
                    BudgetId = e.BudgetId,
                    BudgetName = e.BudgetName,
                    BudgetStatus = e.BudgetStatus,
                    BudgetStartDate = e.BudgetStartDate,
                    BudgetExpireDate = e.BudgetExpireDate,
                    InitialAmount = e.InitialAmount,
                    OutsourceFund = e.OutsourceFund,
                    AvailableAmount = e.AvailableAmount,
                    BudgetReceive = e.BudgetReceive,
                    BudgetReturn = e.BudgetReturn,
                    ReferenceBudgetCode = e.ReferenceBudgetCode,
                    BudgetLv1Code = e.BudgetLv1Code,
                    BudgetLv2Code = e.BudgetLv2Code,
                    BudgetLv3Code = e.BudgetLv3Code,
                    BudgetLv4Code = e.BudgetLv4Code,
                    BudgetLv5Code = e.BudgetLv5Code,
                    BudgetLv6Code = e.BudgetLv6Code,
                    DetailInitialAmount = e.DetailInitialAmount,
                    BudgetAmount = e.BudgetAmount,
                    AllocateAmount = e.AllocateAmount,
                    ReservedAmount = e.ReservedAmount,
                    AdvanceAmount = e.AdvanceAmount,
                    ActualAmount = e.ActualAmount,
                    VoucherPaymentAmt = e.VoucherPaymentAmt
                }).ToList()
            };
            return response;
        }
        catch (Exception ex)
        {
            return new BudgetAvailableResponse
            {
                Value = new List<BudgetAvailableItem>()
            };
        }
    }


    public Task<MBudgetavailable?> GetByIdAsync(string id) => _repository.GetByIdAsync(id);

    public Task<MBudgetavailable?> AddAsync(MBudgetavailable entity) => _repository.AddAsync(entity);

    public Task<bool> UpdateAsync(MBudgetavailable entity) => _repository.UpdateAsync(entity);

    public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
}
