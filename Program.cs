

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<A2DBContext>(options =>  options.UseSqlite(builder.Configuration["WebAPIConnection"]));
builder.Services.AddScoped<IA2Repo, A2Repo>();

builder.Services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, A2AuthHandler>("MyAuthentication", null);
builder.Services.AddAuthorization(options =>{
    options.AddPolicy("UserOnly",
    policy => policy.RequireClaim("UserName"));
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
