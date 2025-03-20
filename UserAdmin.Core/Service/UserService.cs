using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAdmin.Core.Model;
using UserAdmin.Core.Service.Interface;
using UserAdmin.Infrastructure.Model;

namespace UserAdmin.Core.Service
{
    public class UserService : IUserService
    {
        private readonly AdminUserDbContext _dbContext;

        public UserService(AdminUserDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<bool> CreateOrUpdate(UserDTO user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            try
            {
                var userModel = await _dbContext.Users.FindAsync(user.Id) ?? new User();

                userModel.FirstName = user.FirstName;
                userModel.LastName = user.LastName;
                userModel.UserName = user.UserName;
                userModel.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                if (userModel.Id == Guid.Empty)
                    await _dbContext.Users.AddAsync(userModel);
                else
                    _dbContext.Users.Update(userModel);

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            { 
                return false;
            }
        }
        public async Task CreateLogs(Logs log)
        {
            if (log == null) throw new ArgumentNullException(nameof(log));

            await _dbContext.Logs.AddAsync(log);
            await _dbContext.SaveChangesAsync();
        }
 
        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            return await _dbContext.Users
             .AsNoTracking()
             .Select(u => new UserDTO
             {
                 Id = u.Id,
                 FirstName = u.FirstName,
                 LastName = u.LastName,
                 LoginCount = u.LoginCount,
                 UserName = u.UserName
             })
             .ToListAsync();
        }


        public async Task<UserDTO> GetById(Guid id)
        {
            UserDTO? user = await _dbContext.Users
            .Where(u => u.Id == id)
            .Select(u => new UserDTO
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                LoginCount = u.LoginCount,
                UserName = u.UserName
            })
            .FirstOrDefaultAsync();

            return user;

        }
        public async Task<UserDTO> GetByUsername(string username)
        {
            UserDTO? user = await _dbContext.Users
            .Where(u => u.UserName == username)
            .Select(u => new UserDTO
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                LoginCount = u.LoginCount,
                UserName = u.UserName,
                Password = u.Password
            })
            .FirstOrDefaultAsync();
            return user;
        }
        public async Task<bool> DeleteUser(Guid id)
        {
            User? user = await _dbContext.Users.Include(i => i.Logs).FirstOrDefaultAsync(i => i.Id == id);
            if (user == null)
                return false;
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return true;
        }

      
        public async Task<bool> SaveLog(LogDTO log)
        {
            if (log == null) throw new ArgumentNullException(nameof(log));

            try
            {
                var user = await _dbContext.Users.FindAsync(log.UserId);
                if (user == null)
                { 
                    return false;
                }

                var logs = new Logs
                {
                    Date = log.Date,
                    Browser = log.Browser,
                    UserId = log.UserId,
                    User = user
                };

                user.LoginCount += 1;
                await _dbContext.Logs.AddAsync(logs);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            { 
                return false;
            }
        }

        public async Task<IEnumerable<LogDTO>> GetAllLogs()
        {
            return await _dbContext.Logs
           .Include(l => l.User)
           .Select(l => new LogDTO
           {
               Id = l.Id,
               Browser = l.Browser,
               Date = l.Date,
               UserId = l.UserId,
               User = new UserDTO
               {
                   FirstName = l.User.FirstName,
                   LastName = l.User.LastName,
                   LoginCount = l.User.LoginCount,
                   UserName = l.User.UserName
               }
           })
           .ToListAsync();
        }
    }
}
