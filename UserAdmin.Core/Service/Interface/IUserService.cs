using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAdmin.Core.Model;
using UserAdmin.Infrastructure.Model;

namespace UserAdmin.Core.Service.Interface
{
    public interface IUserService
    { 
        Task<UserDTO> GetById(Guid id);
        Task<UserDTO> GetByUsername(string username);
        Task<IEnumerable<UserDTO>> GetAll();  
        Task<bool> CreateOrUpdate(UserDTO user);
        Task<bool> DeleteUser(Guid id); 
        Task<bool> SaveLog(LogDTO log);
        Task<IEnumerable<LogDTO>> GetAllLogs();

    }
}
