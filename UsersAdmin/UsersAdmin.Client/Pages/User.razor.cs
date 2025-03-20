using Microsoft.AspNetCore.Components;
using UsersAdmin.Client.Services;
using UserAdmin.Core.Model;
using Microsoft.JSInterop;

namespace UsersAdmin.Client.Pages
{
    public partial class UserComponent : ComponentBase
    {
        [Inject] private AdminUserService AdminUserService { get; set; } = default!;

        [Inject] private IJSRuntime JS { get; set; } = default!;

        public List<UserDTO> UserList { get; private set; } = new();
        public UserDTO UserModel { get; private set; } = new();
        public bool IsLoading { get; private set; }
        public bool ShowUserDialog { get; private set; } = false;

        /// <summary>
        /// Initializes the component and loads users.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await LoadUsersAsync();
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        protected async Task DeleteUserAsync(Guid id)
        {
            if (id == Guid.Empty) return; // Prevent invalid GUID usage

            bool success = await AdminUserService.DeleteAsync(id);
            if (success)
            {
                await JS.InvokeVoidAsync("showAlert", "Uspješno obrisan korisnik");

                await LoadUsersAsync();
            }
            else
            {
                // Handle delete failure (optional: show error message)
            }
        }

        /// <summary>
        /// Closes the user dialog.
        /// </summary>
        protected void Cancel() => ShowUserDialog = false;

        /// <summary>
        /// Saves or updates user data.
        /// </summary>
        protected async Task SaveUserAsync()
        {
            if (UserModel == null) return;

            await AdminUserService.AddOrUpdateAsync(UserModel);

            await JS.InvokeVoidAsync("showAlert", "Podaci uspješno promijenjeni");
            ShowUserDialog = false;
            await LoadUsersAsync();
            ResetUserModel();
        }

        /// <summary>
        /// Prepares user form for adding or updating a user.
        /// </summary>
        protected async Task OpenUserDialogAsync(Guid? id = null)
        {
            ShowUserDialog = true;

            if (id.HasValue && id != Guid.Empty)
            {
                UserModel = await AdminUserService.GetUserById(id.Value) ?? new UserDTO();
            }
            else
            {
                ResetUserModel();
            }
        }

        /// <summary>
        /// Loads all users from the API.
        /// </summary>
        private async Task LoadUsersAsync()
        {
            IsLoading = true;
            UserList = await AdminUserService.GetUsersAsync();
            IsLoading = false;
        }

        /// <summary>
        /// Resets the user model for new entries.
        /// </summary>
        protected void ResetUserModel()
        {
            UserModel = new UserDTO();
        }
    }
}
