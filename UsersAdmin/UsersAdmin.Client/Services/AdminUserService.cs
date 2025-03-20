using System.Net.Http.Json;
using UserAdmin.Core.Model;
using UsersAdmin.Client.Helper;

namespace UsersAdmin.Client.Services
{
    public class AdminUserService :ApiMethod
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AdminUserService> _logger;

        public AdminUserService(HttpClient httpClient, ILogger<AdminUserService> logger):base(httpClient, logger)
        { 
        }

        /// <summary>
        /// Retrieves the list of users from the API.
        /// </summary>
        public async Task<List<UserDTO>> GetUsersAsync()
        {
            return await SendGetRequestAsync<List<UserDTO>>("users/getusers") ?? new List<UserDTO>();
        }

        /// <summary>
        /// Retrieves the list of logs from the API.
        /// </summary>
        public async Task<List<LogDTO>> GetLogsAsync()
        {
            return await SendGetRequestAsync<List<LogDTO>>("users/getlogs") ?? new List<LogDTO>();
        }

        /// <summary>
        /// Adds or updates a user.
        /// </summary>
        public async Task<bool> AddOrUpdateAsync(UserDTO userModel)
        {
            return await SendPostRequestAsync("users/addorupdate", userModel);
        }

        /// <summary>
        /// Retrieves a user by username.
        /// </summary>
        public async Task<UserDTO> GetUserByUserName(string username)
        {
            return await SendGetRequestAsync<UserDTO>($"users/getbyusername/{username}") ?? new UserDTO();
        }

        /// <summary>
        /// Retrieves a user by ID.
        /// </summary>
        public async Task<UserDTO> GetUserById(Guid id)
        {
            return await SendGetRequestAsync<UserDTO>($"users/getbyid/{id}") ?? new UserDTO();
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        public async Task<bool> DeleteAsync(Guid id)
        {
            return await SendDeleteRequestAsync($"users/delete/{id}");
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        public async Task<bool> LoginAsync(LoginModel login)
        {
            return await SendPostRequestAsync("users/login", login);
        }

        /// <summary>
        /// Saves a user log.
        /// </summary>
        public async Task<bool> SaveLogAsync(LogDTO logDto)
        {
            return await SendPostRequestAsync("users/savelog", logDto);
        }

     
    }
}
