using MedMinder.Data;
using MedMinder.Models;
using MedMinder.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

//Add services to container
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddCors(options => {
    options.AddDefaultPolicy(builder => builder
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .Build());
});

services.AddDbContext<DataContext>(options => { options.UseInMemoryDatabase(databaseName: "MedMinder"); });

services.AddScoped<DataSeeder>();
services.AddScoped<IPatientRepository, PatientRepository>();

var app = builder.Build();

//Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/patients", async (IPatientRepository patientRepository) =>
{
    var result = await patientRepository.Get();

    if (result.Length < 1)
        return Results.Text("No Results Found");
    else
        return Results.Ok(result);
}).WithName("get patients");

app.MapGet("/search", async (string searchTerms, IPatientRepository patientRepository) =>
{
    return await patientRepository.GetAsync(searchTerms);
}).WithName("search patients");

app.MapPost("/create", async (Patient patient, IPatientRepository patientRepository) =>
{
    var created = await patientRepository.Create(patient);

    if (!created)
        return Results.Problem("ID Already Existing.");

    return Results.Ok(created);

}).WithName("create patient");

app.MapPost("/edit", async (Patient patient, IPatientRepository patientRepository) =>
{
    var result = await patientRepository.Update(patient);

    if (result is null)
        return Results.NotFound("No Results Found.");

    return Results.Ok(result);
}).WithName("edit patient");

app.MapPost("/remove", async (string patientId, IPatientRepository patientRepository) =>
{
    var result = await patientRepository.Delete(patientId);

    if (result == false)
        return Results.NotFound("No Results Found.");

    return Results.Ok();
}).WithName("remove patient by Id");

app.UseCors();

using (var scope = app.Services.CreateScope())
{
    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    dataSeeder.Seed();
}

app.Run();
