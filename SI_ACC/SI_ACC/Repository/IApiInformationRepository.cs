using SI_ACC.Entities;
using SI_ACC.Models;

namespace SI_ACC.Repository
{
    public interface IApiInformationRepository
    {
        Task<IEnumerable<MApiInformation>> GetAllAsync(MapiInformationModels param);
        Task<MApiInformation> GetByIdAsync(int id);
        Task AddAsync(MApiInformation service);
        Task UpdateAsync(MApiInformation service);
        Task DeleteAsync(int id);
        Task UpdateAllBearerTokensAsync(string newBearerToken);
    }
}
