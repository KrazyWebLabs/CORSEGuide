using ShareProject.Models;
using ShareProject.Models.DTOs.InputDTO;
using ShareProject.Models.DTOs.OutputDTO;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace UMAD_GUIDEE.Services;

public class HttpService
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _serializerOptions;
    private string? _authorizationKey;

    public string? AuthorizationKey { get => _authorizationKey; set => _authorizationKey =  value ; }

    public HttpService(HttpClient client)
    {
        _client = client;
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }

    /// <summary>
    /// Initializes the HttpClient with the provided email and password.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<bool> InitializeClient(string email, string password)
    {
        try
        {
            var response = await Login(email, password);
            if ( !response.IsSuccessStatusCode )
                return false;

            var contentStream = await response.Content.ReadAsStreamAsync();
            var token = await JsonSerializer.DeserializeAsync<UserTokenOutputDTO>(contentStream, _serializerOptions);

            _authorizationKey = token?.AccessToken;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationKey);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if ( !string.IsNullOrEmpty(_authorizationKey) )
            {
                await SecureStorage.SetAsync("accessToken", _authorizationKey);

                var user = await GetCurrentUser();

                if ( user != null )
                {
                    await SecureStorage.SetAsync("userName", $"{user.FirstName} {user.LastName}");
                    await SecureStorage.SetAsync("userRole", user.Role);
                    await SecureStorage.SetAsync("email", user.Email);
                }
            }

            return true;
        }

        catch ( Exception ex )
        {
            Console.WriteLine($"Error initializing client: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Logs in the user with the provided username and password.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<HttpResponseMessage> Login(string username, string password)
    {
        var loginDto = new LogInInputDTO
        {
            Email = username,
            Password = password
        };

        return await _client.PostAsJsonAsync("/login", loginDto, _serializerOptions);
    }

    /// <summary>
    /// Get the current user.
    /// </summary>
    /// <returns></returns>
    public async Task<LoggedInOutputDTO?> GetCurrentUser()
    {
        try
        {
            var response = await _client.GetAsync("User");
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var user = await JsonSerializer.DeserializeAsync<LoggedInOutputDTO>(stream, _serializerOptions);

            return user;
        }
        catch ( Exception ex )
        {
            Console.WriteLine($"Error getting current user: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Get all notes.
    /// </summary>
    /// <returns></returns>
    public async Task<List<NoteOutputDTO>> GetNotes()
    {
        var response = await _client.GetAsync("Note");
        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync();
        var notes = await JsonSerializer.DeserializeAsync<List<NoteOutputDTO>>(stream, _serializerOptions);
        return notes ?? [];
    }

    /// <summary>
    /// Get all workers.
    /// </summary>
    /// <returns></returns>
    public async Task<List<WorkerOutputDTO>> GetAllWorkers()
    {
        try
        {
            var response = await _client.GetAsync("User/Worker");
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var workers = await JsonSerializer.DeserializeAsync<List<WorkerOutputDTO>>(stream, _serializerOptions);

            return workers ?? [];
        }
        catch ( Exception ex )
        {
            Console.WriteLine($"Error getting all workers: {ex.Message}");
            return [];
        }
    }

    /// <summary>
    /// Get all categories.
    /// </summary>
    /// <returns></returns>
    public async Task<List<Category>> GetAllCategories()
    {
        try
        {
            var response = await _client.GetAsync("Category");
            response.EnsureSuccessStatusCode();
            
            var stream = await response.Content.ReadAsStreamAsync();
            var categories = await JsonSerializer.DeserializeAsync<List<Category>>(stream, _serializerOptions);
            return categories ?? [];
        }
        catch ( Exception ex )
        {
            Console.WriteLine($"Error getting all categories: {ex.Message}");
            return [];
        }
    }


    public async Task<HttpResponseMessage> CreateNote(NoteInputDTO newNote) => await _client.PostAsJsonAsync("Note", newNote);

    public async Task<HttpResponseMessage> UpdateNote(NoteInputDTO updatedNote) => await _client.PutAsJsonAsync($"Note/{updatedNote.Id}", updatedNote);

    public async Task<HttpResponseMessage> DeleteNote(NoteInputDTO noteToDelete) => await _client.DeleteAsync($"Note/{noteToDelete.Id}");
}
