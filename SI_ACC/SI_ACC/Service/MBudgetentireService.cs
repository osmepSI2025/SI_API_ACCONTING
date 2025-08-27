// File: Service/MBudgetentireService.cs
using SI_ACC.Models;
using SI_ACC.Entities;
using SI_ACC.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using SI_ACC.Services;

public class MBudgetentireService
{
    private readonly MBudgetentireRepository _repository;
    private readonly ICallAPIService _serviceApi;
    private readonly IApiInformationRepository _repositoryApi;
    private readonly string _FlagDev;


    public MBudgetentireService(MBudgetentireRepository repository, IConfiguration configuration, ICallAPIService serviceApi, IApiInformationRepository repositoryApi)

    {
        _repository = repository;
        _serviceApi = serviceApi;
        _repositoryApi = repositoryApi;
        _FlagDev = configuration["Devlopment:FlagDev"] ?? throw new ArgumentNullException("FlagDev is missing in appsettings.json");


    }

    public async Task<BudgetEntireResponse> GetAllAsync(string? filter)
    {
        try
        {
            var entities = await _repository.GetAllAsync(filter);
            if (entities == null || !entities.Any())
            {
                await BatchEndOfDay_BudgetEntire();
                entities = await _repository.GetAllAsync(filter);
                if (entities == null || !entities.Any())
                {
                    return new BudgetEntireResponse { Value = new List<BudgetEntireItem>() };
                }
                else
                {
                    var response = new BudgetEntireResponse
                    {
                        Value = entities.Select(e => new BudgetEntireItem
                        {
                            EntryNo = e.EntryNo,
                            BudgetLevel = e.BudgetLevel,
                            BudgetCode = e.BudgetCode,
                            BudgetName = e.BudgetName,
                            BudgetYear = e.BudgetYear,
                            BudgetPlan = e.BudgetPlan,
                            BudgetStrategies = e.BudgetStrategies,
                            BudgetActivites = e.BudgetActivites,
                            PostingDate = e.PostingDate,
                            DocumentNo = e.DocumentNo,
                            OriginalAmount = e.OriginalAmount,
                            TransDescription = e.TransDescription,
                            TransDescription2 = e.TransDescription2,
                            TransferToLevel = e.TransferToLevel,
                            TransferToCode = e.TransferToCode,
                            ReferenceBudgetCode = e.ReferenceBudgetCode,
                            CurrentBudgetStatus = e.CurrentBudgetStatus,
                            ReservedAmount = e.ReservedAmount,
                            ActualAmount = e.ActualAmount,
                            AdvanceAmount = e.AdvanceAmount,
                            VoucherAdvAmt = e.VoucherAdvAmt,
                            VoucherReceiveAmt = e.VoucherReceiveAmt,
                            VoucherPaymentAmt = e.VoucherPaymentAmt,
                            RemainingAmount = e.RemainingAmount,
                            ActivityOutsourceFund = e.ActivityOutsourceFund,
                            BudgetDepartment = e.BudgetDepartment,
                            BudgetProject = e.BudgetProject,
                            ApiMappingId = e.ApiMappingId,
                            AuxiliaryIndex1 = e.AuxiliaryIndex1,
                            AuxiliaryIndex2 = e.AuxiliaryIndex2,
                            AuxiliaryIndex3 = e.AuxiliaryIndex3,
                            AuxiliaryIndex4 = e.AuxiliaryIndex4
                        }).ToList()
                    };
                    return response;
                }

            }
            else
            {
                var response = new BudgetEntireResponse
                {
                    Value = entities.Select(e => new BudgetEntireItem
                    {
                        EntryNo = e.EntryNo,
                        BudgetLevel = e.BudgetLevel,
                        BudgetCode = e.BudgetCode,
                        BudgetName = e.BudgetName,
                        BudgetYear = e.BudgetYear,
                        BudgetPlan = e.BudgetPlan,
                        BudgetStrategies = e.BudgetStrategies,
                        BudgetActivites = e.BudgetActivites,
                        PostingDate = e.PostingDate,
                        DocumentNo = e.DocumentNo,
                        OriginalAmount = e.OriginalAmount,
                        TransDescription = e.TransDescription,
                        TransDescription2 = e.TransDescription2,
                        TransferToLevel = e.TransferToLevel,
                        TransferToCode = e.TransferToCode,
                        ReferenceBudgetCode = e.ReferenceBudgetCode,
                        CurrentBudgetStatus = e.CurrentBudgetStatus,
                        ReservedAmount = e.ReservedAmount,
                        ActualAmount = e.ActualAmount,
                        AdvanceAmount = e.AdvanceAmount,
                        VoucherAdvAmt = e.VoucherAdvAmt,
                        VoucherReceiveAmt = e.VoucherReceiveAmt,
                        VoucherPaymentAmt = e.VoucherPaymentAmt,
                        RemainingAmount = e.RemainingAmount,
                        ActivityOutsourceFund = e.ActivityOutsourceFund,
                        BudgetDepartment = e.BudgetDepartment,
                        BudgetProject = e.BudgetProject,
                        ApiMappingId = e.ApiMappingId,
                        AuxiliaryIndex1 = e.AuxiliaryIndex1,
                        AuxiliaryIndex2 = e.AuxiliaryIndex2,
                        AuxiliaryIndex3 = e.AuxiliaryIndex3,
                        AuxiliaryIndex4 = e.AuxiliaryIndex4
                    }).ToList()
                };
                return response;
            }
        } catch (Exception ex) 
        {
        return new BudgetEntireResponse { Value = new List<BudgetEntireItem>() };
        }
       
       
    }
    public async Task BatchEndOfDay_BudgetEntire()
{
    var options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };
    var BudgetEntireResponse = new BudgetEntireResponse();
        var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "budgetentires" });
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
        var result = JsonSerializer.Deserialize<BudgetEntireResponse>(apiResponse, options);

        BudgetEntireResponse = result ?? new BudgetEntireResponse();

        if (BudgetEntireResponse.Value != null)
        {
            foreach (var item in BudgetEntireResponse.Value)
            {
                try
                {
                    // Use both EntryNo and BudgetCode as key for GetByIdAsync
                    var existing = await _repository.GetByIdAsync(item.EntryNo?.ToString(), item.BudgetCode);

                    if (existing == null)
                    {
                        // Create new record
                        var newData = new MBudgetentire
                        {
                            EntryNo = item.EntryNo,
                            BudgetLevel = item.BudgetLevel,
                            BudgetCode = item.BudgetCode,
                            BudgetName = item.BudgetName,
                            BudgetYear = item.BudgetYear,
                            BudgetPlan = item.BudgetPlan,
                            BudgetStrategies = item.BudgetStrategies,
                            BudgetActivites = item.BudgetActivites,
                            PostingDate = item.PostingDate,
                            DocumentNo = item.DocumentNo,
                            OriginalAmount = item.OriginalAmount,
                            TransDescription = item.TransDescription,
                            TransDescription2 = item.TransDescription2,
                            TransferToLevel = item.TransferToLevel,
                            TransferToCode = item.TransferToCode,
                            ReferenceBudgetCode = item.ReferenceBudgetCode,
                            CurrentBudgetStatus = item.CurrentBudgetStatus,
                            ReservedAmount = item.ReservedAmount,
                            ActualAmount = item.ActualAmount,
                            AdvanceAmount = item.AdvanceAmount,
                            VoucherAdvAmt = item.VoucherAdvAmt,
                            VoucherReceiveAmt = item.VoucherReceiveAmt,
                            VoucherPaymentAmt = item.VoucherPaymentAmt,
                            RemainingAmount = item.RemainingAmount,
                            ActivityOutsourceFund = item.ActivityOutsourceFund,
                            BudgetDepartment = item.BudgetDepartment,
                            BudgetProject = item.BudgetProject,
                            ApiMappingId = item.ApiMappingId,
                            AuxiliaryIndex1 = item.AuxiliaryIndex1,
                            AuxiliaryIndex2 = item.AuxiliaryIndex2,
                            AuxiliaryIndex3 = item.AuxiliaryIndex3,
                            AuxiliaryIndex4 = item.AuxiliaryIndex4
                        };

                        await _repository.AddAsync(newData);
                        Console.WriteLine($"[INFO] Created new MBudgetentire with BudgetCode {newData.BudgetCode}");
                    }
                    else
                    {
                        // Update existing record
                        existing.EntryNo = item.EntryNo;
                        existing.BudgetLevel = item.BudgetLevel;
                        existing.BudgetCode = item.BudgetCode;
                        existing.BudgetName = item.BudgetName;
                        existing.BudgetYear = item.BudgetYear;
                        existing.BudgetPlan = item.BudgetPlan;
                        existing.BudgetStrategies = item.BudgetStrategies;
                        existing.BudgetActivites = item.BudgetActivites;
                        existing.PostingDate = item.PostingDate;
                        existing.DocumentNo = item.DocumentNo;
                        existing.OriginalAmount = item.OriginalAmount;
                        existing.TransDescription = item.TransDescription;
                        existing.TransDescription2 = item.TransDescription2;
                        existing.TransferToLevel = item.TransferToLevel;
                        existing.TransferToCode = item.TransferToCode;
                        existing.ReferenceBudgetCode = item.ReferenceBudgetCode;
                        existing.CurrentBudgetStatus = item.CurrentBudgetStatus;
                        existing.ReservedAmount = item.ReservedAmount;
                        existing.ActualAmount = item.ActualAmount;
                        existing.AdvanceAmount = item.AdvanceAmount;
                        existing.VoucherAdvAmt = item.VoucherAdvAmt;
                        existing.VoucherReceiveAmt = item.VoucherReceiveAmt;
                        existing.VoucherPaymentAmt = item.VoucherPaymentAmt;
                        existing.RemainingAmount = item.RemainingAmount;
                        existing.ActivityOutsourceFund = item.ActivityOutsourceFund;
                        existing.BudgetDepartment = item.BudgetDepartment;
                        existing.BudgetProject = item.BudgetProject;
                        existing.ApiMappingId = item.ApiMappingId;
                        existing.AuxiliaryIndex1 = item.AuxiliaryIndex1;
                        existing.AuxiliaryIndex2 = item.AuxiliaryIndex2;
                        existing.AuxiliaryIndex3 = item.AuxiliaryIndex3;
                        existing.AuxiliaryIndex4 = item.AuxiliaryIndex4;

                        await _repository.UpdateAsync(existing);
                        Console.WriteLine($"[INFO] Updated MBudgetentire with BudgetCode {existing.BudgetCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to process MBudgetentire BudgetCode {item.BudgetCode}: {ex.Message}");
                }
            }
        }



    }
    public async Task BatchEndOfDay_BudgetEntireBySearch(string filter)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };
        var BudgetEntireResponse = new BudgetEntireResponse();
        var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "budgetentires" });
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
        var result = JsonSerializer.Deserialize<BudgetEntireResponse>(apiResponse, options);

        BudgetEntireResponse = result ?? new BudgetEntireResponse();

        if (BudgetEntireResponse.Value != null)
        {
            foreach (var item in BudgetEntireResponse.Value)
            {
                try
                {
                    // Use both EntryNo and BudgetCode as key for GetByIdAsync
                    var existing = await _repository.GetByIdAsync(item.EntryNo?.ToString(), item.BudgetCode);

                    if (existing == null)
                    {
                        // Create new record
                        var newData = new MBudgetentire
                        {
                            EntryNo = item.EntryNo,
                            BudgetLevel = item.BudgetLevel,
                            BudgetCode = item.BudgetCode,
                            BudgetName = item.BudgetName,
                            BudgetYear = item.BudgetYear,
                            BudgetPlan = item.BudgetPlan,
                            BudgetStrategies = item.BudgetStrategies,
                            BudgetActivites = item.BudgetActivites,
                            PostingDate = item.PostingDate,
                            DocumentNo = item.DocumentNo,
                            OriginalAmount = item.OriginalAmount,
                            TransDescription = item.TransDescription,
                            TransDescription2 = item.TransDescription2,
                            TransferToLevel = item.TransferToLevel,
                            TransferToCode = item.TransferToCode,
                            ReferenceBudgetCode = item.ReferenceBudgetCode,
                            CurrentBudgetStatus = item.CurrentBudgetStatus,
                            ReservedAmount = item.ReservedAmount,
                            ActualAmount = item.ActualAmount,
                            AdvanceAmount = item.AdvanceAmount,
                            VoucherAdvAmt = item.VoucherAdvAmt,
                            VoucherReceiveAmt = item.VoucherReceiveAmt,
                            VoucherPaymentAmt = item.VoucherPaymentAmt,
                            RemainingAmount = item.RemainingAmount,
                            ActivityOutsourceFund = item.ActivityOutsourceFund,
                            BudgetDepartment = item.BudgetDepartment,
                            BudgetProject = item.BudgetProject,
                            ApiMappingId = item.ApiMappingId,
                            AuxiliaryIndex1 = item.AuxiliaryIndex1,
                            AuxiliaryIndex2 = item.AuxiliaryIndex2,
                            AuxiliaryIndex3 = item.AuxiliaryIndex3,
                            AuxiliaryIndex4 = item.AuxiliaryIndex4
                        };

                        await _repository.AddAsync(newData);
                        Console.WriteLine($"[INFO] Created new MBudgetentire with BudgetCode {newData.BudgetCode}");
                    }
                    else
                    {
                        // Update existing record
                        existing.EntryNo = item.EntryNo;
                        existing.BudgetLevel = item.BudgetLevel;
                        existing.BudgetCode = item.BudgetCode;
                        existing.BudgetName = item.BudgetName;
                        existing.BudgetYear = item.BudgetYear;
                        existing.BudgetPlan = item.BudgetPlan;
                        existing.BudgetStrategies = item.BudgetStrategies;
                        existing.BudgetActivites = item.BudgetActivites;
                        existing.PostingDate = item.PostingDate;
                        existing.DocumentNo = item.DocumentNo;
                        existing.OriginalAmount = item.OriginalAmount;
                        existing.TransDescription = item.TransDescription;
                        existing.TransDescription2 = item.TransDescription2;
                        existing.TransferToLevel = item.TransferToLevel;
                        existing.TransferToCode = item.TransferToCode;
                        existing.ReferenceBudgetCode = item.ReferenceBudgetCode;
                        existing.CurrentBudgetStatus = item.CurrentBudgetStatus;
                        existing.ReservedAmount = item.ReservedAmount;
                        existing.ActualAmount = item.ActualAmount;
                        existing.AdvanceAmount = item.AdvanceAmount;
                        existing.VoucherAdvAmt = item.VoucherAdvAmt;
                        existing.VoucherReceiveAmt = item.VoucherReceiveAmt;
                        existing.VoucherPaymentAmt = item.VoucherPaymentAmt;
                        existing.RemainingAmount = item.RemainingAmount;
                        existing.ActivityOutsourceFund = item.ActivityOutsourceFund;
                        existing.BudgetDepartment = item.BudgetDepartment;
                        existing.BudgetProject = item.BudgetProject;
                        existing.ApiMappingId = item.ApiMappingId;
                        existing.AuxiliaryIndex1 = item.AuxiliaryIndex1;
                        existing.AuxiliaryIndex2 = item.AuxiliaryIndex2;
                        existing.AuxiliaryIndex3 = item.AuxiliaryIndex3;
                        existing.AuxiliaryIndex4 = item.AuxiliaryIndex4;

                        await _repository.UpdateAsync(existing);
                        Console.WriteLine($"[INFO] Updated MBudgetentire with BudgetCode {existing.BudgetCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to process MBudgetentire BudgetCode {item.BudgetCode}: {ex.Message}");
                }
            }
        }



    }
    public Task<MBudgetentire?> GetByIdAsync(string entryNo, string budgetCode) => _repository.GetByIdAsync(entryNo, budgetCode);
    public Task<MBudgetentire?> AddAsync(MBudgetentire entity) => _repository.AddAsync(entity);
    public Task<bool> UpdateAsync(MBudgetentire entity) => _repository.UpdateAsync(entity);
    public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
}
