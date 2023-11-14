using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Services.Interfaces
{
    public interface IGenericIssuesService<CreateTDto, ExportTDto, UpdateTDto>
    {
        Task<ExportTDto> CreateAsync(CreateTDto createDto);
        Task<ExportTDto> GetByIdAsync(string id);
        Task<ICollection<ExportTDto>> GetByMunicipalityAsync(string municipalityId);
        Task<ExportTDto> UpdateAsync(string id, UpdateTDto updateDto);
        Task<bool> DeleteAsync(string id);
        Task<ICollection<ExportTDto>> GetRequestsAsync(int page, int count);
        Task<int> GetRequestsCountAsync();
        Task<ICollection<ExportTDto>> GetByUserIdAsync(string userId);
    }
}
