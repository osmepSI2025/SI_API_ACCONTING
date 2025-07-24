// File: Service/TBudgetMappingService.cs
using SI_ACC.Entities;
using SI_ACC.Models;
using SI_ACC.Repository;
using SI_ACC.Services;
using System.Text.Json;

public class TBudgetMappingService
{
    private readonly TBudgetMappingRepository _repository;
    private readonly ICallAPIService _serviceApi;
    private readonly IApiInformationRepository _repositoryApi;
    private readonly string _FlagDev;


    public TBudgetMappingService(TBudgetMappingRepository repository, IConfiguration configuration, ICallAPIService serviceApi, IApiInformationRepository repositoryApi)

    {
        _repository = repository;
        _serviceApi = serviceApi;
        _repositoryApi = repositoryApi;
        _FlagDev = configuration["Devlopment:FlagDev"] ?? throw new ArgumentNullException("FlagDev is missing in appsettings.json");


    }

    public async Task<BudgetMappingResponse> GetAllAsync()
    {
        try
        {
            var entities = await _repository.GetAllAsync();
            if (entities == null || !entities.Any())
            {
                await BatchEndOfDay_BudgetMapping();
                entities = await _repository.GetAllAsync();
                if (entities == null || !entities.Any())
                {
                    return new BudgetMappingResponse { Value = new List<BudgetMappingItem>() };
                }
                else
                {
                    var response = new BudgetMappingResponse
                    {
                        Value = entities.Select(e => new BudgetMappingItem
                        {

                            BudgetLevel = e.BudgetLevel,
                            BudgetId = e.BudgetId,
                            BudgetName = e.BudgetName,
                            MappingCode = e.MappingCode,
                            MappingName = e.MappingName,
                            MappingParentCode = e.MappingParentCode,
                            AuxiliaryIndex1 = e.AuxiliaryIndex1
                        }).ToList()
                    };
                    return response;
                }

            }
            else
            {
                var response = new BudgetMappingResponse
                {
                    Value = entities.Select(e => new BudgetMappingItem
                    {

                        BudgetLevel = e.BudgetLevel,
                        BudgetId = e.BudgetId,
                        BudgetName = e.BudgetName,
                        MappingCode = e.MappingCode,
                        MappingName = e.MappingName,
                        MappingParentCode = e.MappingParentCode,
                        AuxiliaryIndex1 = e.AuxiliaryIndex1
                    }).ToList()
                };
                return response;
            }
        }
        catch (Exception ex)
        {
            return new BudgetMappingResponse { Value = new List<BudgetMappingItem>() };
        }
    }

    public async Task BatchEndOfDay_BudgetMapping()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };
        var BudgetMappingResponse = new BudgetMappingResponse();
        var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "budgetmapping" });
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
        var result = JsonSerializer.Deserialize<BudgetMappingResponse>(apiResponse, options);

        BudgetMappingResponse = result ?? new BudgetMappingResponse();
        if (BudgetMappingResponse.Value != null)
        {
            foreach (var item in BudgetMappingResponse.Value)
            {
                try
                {
                    // Use BudgetId, MappingCode, and MappingParentCode as key for GetByIdAsync
                    var existing = await _repository.GetByIdAsync(item.BudgetId, item.MappingCode, item.AuxiliaryIndex1);

                    if (existing == null)
                    {
                        // Create new record
                        var newData = new TBudgetMapping
                        {
                            BudgetLevel = item.BudgetLevel,
                            BudgetId = item.BudgetId,
                            BudgetName = item.BudgetName,
                            MappingCode = item.MappingCode,
                            MappingName = item.MappingName,
                            MappingParentCode = item.MappingParentCode,
                            AuxiliaryIndex1 = item.AuxiliaryIndex1
                        };

                        await _repository.AddAsync(newData);
                        Console.WriteLine($"[INFO] Created new TBudgetMapping with BudgetCode {newData.BudgetId}");
                    }
                    else
                    {
                        // Update existing record
                        existing.BudgetLevel = item.BudgetLevel;
                        existing.BudgetId = item.BudgetId;
                        existing.BudgetName = item.BudgetName;
                        existing.MappingCode = item.MappingCode;
                        existing.MappingName = item.MappingName;
                        existing.MappingParentCode = item.MappingParentCode;
                        existing.AuxiliaryIndex1 = item.AuxiliaryIndex1;

                        await _repository.UpdateAsync(existing);
                        Console.WriteLine($"[INFO] Updated TBudgetMapping with BudgetCode {existing.BudgetId}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to process TBudgetMapping BudgetCode {item.BudgetId}: {ex.Message}");
                }
            }
        }

    }
    public Task<TBudgetMapping?> GetByIdAsync(string budgetid, string mappingCode, string mappingparentcode) => _repository.GetByIdAsync(budgetid, mappingCode, mappingparentcode);
    public Task<TBudgetMapping?> AddAsync(TBudgetMapping entity) => _repository.AddAsync(entity);
    public Task<bool> UpdateAsync(TBudgetMapping entity) => _repository.UpdateAsync(entity);
    public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
}
