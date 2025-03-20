using Microsoft.AspNetCore.Mvc;
using UserAdmin.Core.Model;
using UserAdmin.Core.Service.Interface;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    [HttpGet("getusers")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetAll();
        return Ok(users);
    }

    /// <summary>
    /// Retrieves a user by ID.
    /// </summary>
    [HttpGet("getbyid/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetById(id);
        return Ok(user); 
    }

    /// <summary>
    /// Retrieves a user by username.
    /// </summary>
    [HttpGet("getbyusername/{username}")]
    public async Task<IActionResult> GetByUsername(string username)
    {
        var user = await _userService.GetByUsername(username);
        return Ok(user);
    }

    /// <summary>
    /// Adds or updates a user.
    /// </summary>
    [HttpPost("addorupdate")]
    public async Task<IActionResult> AddOrUpdate([FromBody] UserDTO user)
    {
        var success = await _userService.CreateOrUpdate(user);
        return Ok(new { success });
    }

    /// <summary>
    /// Delete a user by ID.
    /// </summary>
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _userService.DeleteUser(id);
        return Ok(new { success });
    }

    /// <summary>
    /// Handles user login.
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginDto)
    {
        var userEntry = await _userService.GetByUsername(loginDto.Login);
        if (userEntry == null)
        {
            return Ok(false); 
        }

        bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, userEntry.Password);
        if (!isValidPassword)
        {
            return Ok(false); 
        }

        return Ok(true);  

    }

    /// <summary>
    /// Retrieves all user logs.
    /// </summary>
    [HttpGet("getlogs")]
    public async Task<IActionResult> GetLogs()
    {
        var logs = await _userService.GetAllLogs();
        return Ok(logs);
    }

    /// <summary>
    /// Saves a user log.
    /// </summary>
    [HttpPost("savelog")]
    public async Task<IActionResult> SaveLog([FromBody] LogDTO logmodel)
    {
        var success = await _userService.SaveLog(logmodel);
        return Ok(new { success });
    }
}
