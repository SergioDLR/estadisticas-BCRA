using PrincipalesVariables.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<StatsBcraController>();
builder.Services.AddSingleton<SyncController>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.Services.GetRequiredService<StatsBcraController>().MapEndpoints(app);
app.Services.GetRequiredService<SyncController>().MapEndpoints(app);

app.Run();

