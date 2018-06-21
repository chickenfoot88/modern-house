using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Registries.Schedules.Models;

namespace Domain.Registries.Schedules.Interfaces
{
    public interface ISchedulesService
    {
        List<ScheduleGetModel> GetAllModels();

        ScheduleGetModel GetModel(long id);

        void Create(ScheduleSaveModel scheduleModel);
        Task CreateAsync(ScheduleSaveModel scheduleModel);

        void Update(long id, ScheduleSaveModel scheduleModel);
        Task UpdateAsync(long id, ScheduleSaveModel scheduleModel);

        void Delete(long id);
        Task DeleteAsync(long id);
    }
}
