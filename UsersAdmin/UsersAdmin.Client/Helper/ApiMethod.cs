using System.Net.Http.Json;
using System.Net.Http;
using UsersAdmin.Client.Services;

namespace UsersAdmin.Client.Helper
{
    public class ApiMethod
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AdminUserService> _logger;
        public ApiMethod(HttpClient httpClient, ILogger<AdminUserService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Generic method for sending GET requests.
        /// </summary>
        internal async Task<T?> SendGetRequestAsync<T>(string endpoint)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
                return await HandleResponseAsync<T>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GET request to {Endpoint}", endpoint);
                return default;
            }
        }

        /// <summary>
        /// Generic method for sending POST requests.
        /// </summary>
        internal async Task<bool> SendPostRequestAsync<T>(string endpoint, T content)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(endpoint, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in POST request to {Endpoint}", endpoint);
                return false;
            }
        }

        /// <summary>
        /// Generic method for sending DELETE requests.
        /// </summary>
        internal async Task<bool> SendDeleteRequestAsync(string endpoint)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync(endpoint);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in DELETE request to {Endpoint}", endpoint);
                return false;
            }
        }

        /// <summary>
        /// Handles API responses and logs errors if necessary.
        /// </summary>
        internal async Task<T?> HandleResponseAsync<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            _logger.LogError("API Error: {StatusCode} - {ErrorMessage}", response.StatusCode, errorMessage);
            return default;
        }
    }
}
