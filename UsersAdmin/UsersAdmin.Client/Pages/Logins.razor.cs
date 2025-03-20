using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using UserAdmin.Core.Model;
using UsersAdmin.Client.Services;

namespace UsersAdmin.Client.Pages
{
    public partial class LoginsComponent : ComponentBase
    {
        [Inject] private IJSRuntime JS { get; set; } = default!;
        [Inject] private AdminUserService AdminUserService { get; set; } = default!;

        public List<LogDTO> LogsList { get; private set; } = new();
        public LogDTO LogDTO { get; private set; } = new();
        public UserDTO? UserDTO { get; private set; }
        public LoginModel LoginModel { get; private set; } = new();
        public bool IsLoading { get; private set; }
        public bool ShowLoginDialog { get; private set; } = false;

        /// <summary>
        /// Initializes the component and loads logs on startup.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await LoadLogsAsync();
        }

        /// <summary>
        /// Closes the login dialog.
        /// </summary>
        protected void Cancel() => ShowLoginDialog = false;

        /// <summary>
        /// Handles user login and logs the attempt.
        /// </summary>
        protected async Task AddOrUpdateLogAsync()
        {
            IsLoading = true;

            bool loginSuccess = await AdminUserService.LoginAsync(LoginModel);
            if (!loginSuccess)
            { 
                IsLoading = false;

                await JS.InvokeVoidAsync("showAlert", "Greška prilikom logiranja");
                return;
            }

            await JS.InvokeVoidAsync("showAlert", "Korisnik uspješno logiran"); 

            // Retrieve user data after successful login
            UserDTO = await AdminUserService.GetUserByUserName(LoginModel.Login);
            if (UserDTO?.Id == null)
            {
                IsLoading = false;
                return;
            }

            // Create log entry
            LogDTO = new LogDTO
            {
                Browser = await JS.InvokeAsync<string>("getBrowserInfo"),
                Date = DateTime.UtcNow, // Use UTC for consistency
                UserId = UserDTO.Id.Value,
                User = UserDTO
            };

            await AdminUserService.SaveLogAsync(LogDTO);
            await LoadLogsAsync();
        }

        /// <summary>
        /// Loads logs from the server.
        /// </summary>
        private async Task LoadLogsAsync()
        {
            IsLoading = true;
            LogsList = await AdminUserService.GetLogsAsync();
            IsLoading = false;
        }
    }
}
