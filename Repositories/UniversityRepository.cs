using AutoMapper;
using System.Text;
using System.Text.Json;
using UniversityDto;
using UniversityShared.Models;

namespace UniversityWeb.Repositories;


public interface IWebRepository
{
    public Task<PaginationResponseData> GetPaginatedData(PaginationFilter? filter = null);
}

public class StudentsRepository: IWebRepository
{
    private readonly HttpClient httpClient;

    private readonly JsonSerializerOptions options;

    private readonly IMapper mapper;

    public StudentsRepository(HttpClient client, IMapper mapper)
    {
        httpClient = client;
        options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        this.mapper = mapper;
    }

    public async Task<PaginationResponseData> GetPaginatedData(PaginationFilter? filter = null)
    {
        var url = "students";

        if (filter != null)
        {
            url += $"?pageSize={filter.PageSize}&currentIndex={filter.CurrentIndex}";
        }
        var response = await httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        
        return JsonSerializer.Deserialize<StudentPaginationData>(content, options)!;
    }

    public async Task RegisterStudent(StudentRegistrationData data)
    {
        var content = JsonSerializer.Serialize(data, options);
        var jsonContent = new StringContent(content, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("students", jsonContent);
    }
    
    public async Task GradeStudent(StudentGradingData data)
    {
        var content = JsonSerializer.Serialize(data, options);
        var jsonContent = new StringContent(content, Encoding.UTF8, "application/json");
        Console.WriteLine(await jsonContent.ReadAsStringAsync()) ;
        await httpClient.PostAsync("students/grade", jsonContent);
    }

    public async Task UpdateStudent(string id, StudentUpdateData data)
    {
        var content = JsonSerializer.Serialize(data, options);
        var jsonContent = new StringContent(content, Encoding.UTF8, "application/json");
        await httpClient.PutAsync($"students/{id}", jsonContent);
    }

    public async Task<Student> GetStudent(string id)
    {
        var response = await httpClient.GetAsync($"students/{id}");
        var content = await response.Content.ReadAsStringAsync();

        var studentResponse = JsonSerializer.Deserialize<StudentResponseData>(content, options)!;

        return mapper.Map<Student>(studentResponse);
    }

    public async Task<StudentResponseData> GetStudentData(string id)
    {
        var response = await httpClient.GetAsync($"students/{id}");
        var content = await response.Content.ReadAsStringAsync();

        var studentResponse = JsonSerializer.Deserialize<StudentResponseData>(content, options)!;
        return studentResponse;
    }

    public async Task DeleteStudent(string id)
    {
        await httpClient.DeleteAsync($"students/{id}");
    }
}


public class CoursesRepository : IWebRepository
{
    private readonly HttpClient httpClient;

    private readonly JsonSerializerOptions options;

    public CoursesRepository(HttpClient client)
    {
        httpClient = client;
        options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<PaginationResponseData> GetPaginatedData(PaginationFilter? filter = null)
    {
        var url = "courses";

        if(filter != null)
        {
            url += $"?pageSize={filter.PageSize}&currentIndex={filter.CurrentIndex}";
        }

        var response = await httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CoursePaginationData>(content, options)!;
    }

    public async Task<List<DepartmentResponseData>> GetDepartments()
    {
        var response = await httpClient.GetAsync("departments");
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<DepartmentResponseData>>(content, options)!;
    }

    public async Task CreateCourse(CourseCreationData data)
    {
        var content = JsonSerializer.Serialize(data, options);
        var jsonContent = new StringContent(content, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("courses", jsonContent);
    }

    public async Task<CourseResponseData> GetCourse(string id)
    {
        var response = await httpClient.GetAsync($"courses/{id}");
        var content = await response.Content.ReadAsStringAsync();

        var course = JsonSerializer.Deserialize<CourseResponseData>(content, options)!;
        return course;
    }

    public async Task UpdateCourse(string id, CourseUpdateDto data)
    {
        var content = JsonSerializer.Serialize(data, options);
        var jsonContent = new StringContent(content, Encoding.UTF8, "application/json");
        await httpClient.PutAsync($"courses/{id}", jsonContent);
    }

    public async Task DeleteCourse(string id)
    {
        await httpClient.DeleteAsync($"courses/{id}");
    }
}


public class InstructorsRepository : IWebRepository
{
    private readonly HttpClient httpClient;

    private readonly JsonSerializerOptions options;


    public InstructorsRepository(HttpClient client)
    {
        httpClient = client;
        options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<PaginationResponseData> GetPaginatedData(PaginationFilter? filter)
    {
        var url = "instructors";

        if (filter != null)
        {
            url += $"?pageSize={filter.PageSize}&currentIndex={filter.CurrentIndex}";
            
            if(!string.IsNullOrEmpty(filter.SearchString))
            {
                url += $"&searchString={filter.SearchString}";
            }
        }

        var response = await httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<InstructorPaginationData>(content, options)!;
    }

    public async Task CreateInstructor(InstructorRegistrationData data)
    {
        var content = JsonSerializer.Serialize(data, options);
        var jsonContent = new StringContent(content, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("instructors", jsonContent);
    }

    public async Task<InstructorDto> GetInstructor(string id)
    {
        var response = await httpClient.GetAsync($"instructors/{id}");
        var content = await response.Content.ReadAsStringAsync();

        var instructor = JsonSerializer.Deserialize<InstructorDto>(content, options)!;
        return instructor;
    }

    public async Task UpdateInstructor(string id, InstructorUpdateData data)
    {
        var content = JsonSerializer.Serialize(data, options);
        var jsonContent = new StringContent(content, Encoding.UTF8, "application/json");
        await httpClient.PutAsync($"instructors/{id}", jsonContent);
    }

    public async Task DeleteInstructor(string id)
    {
        await httpClient.DeleteAsync($"instructors/{id}");
    }
}