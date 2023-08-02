﻿using AutoMapper;
using System;
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

        Console.WriteLine(url);
        var response = await httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CoursePaginationData>(content, options)!;
    }
}