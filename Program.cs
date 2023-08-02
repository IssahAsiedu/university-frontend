using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UniversityShared.Mapping;
using UniversityWeb;
using UniversityWeb.Repositories;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7218/") });
builder.Services.AddScoped<StudentsRepository>();
builder.Services.AddScoped<CoursesRepository>();
builder.Services.AddAutoMapper(typeof(MapperConfig));

await builder.Build().RunAsync();