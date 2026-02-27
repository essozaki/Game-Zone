using GameZone.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);
//multy language
builder.Services.AddControllersWithViews().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.AddLocalization();
builder.Services.AddDistributedMemoryCache();

// Add services to the container.
var ConnectionString = builder.Configuration.GetConnectionString("DataConnection") ??
    throw new InvalidOperationException("No Connection String Founded");
builder.Services.AddDbContext<ApplicationDbContext>(Options =>
Options.UseSqlServer(ConnectionString));
builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
//add hangfire
//builder.Services.AddHangfire(x => x.UseSqlServerStorage(ConnectionString));
//builder.Services.AddHangfireServer();

//add services to repos
builder.Services.AddScoped<ICategoriesService,CategoriesService>();
builder.Services.AddScoped<IDevicesService, DevicesService>();
builder.Services.AddScoped<IGameService, GameService>();

//builder.Services.AddAutoMapper(typeof(Program));

//authontication & auuthorization
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//                .AddJwtBearer(o =>
//                {
//                    o.RequireHttpsMetadata = false;
//                    o.SaveToken = false;
//                    o.TokenValidationParameters = new TokenValidationParameters
//                    {
//                        ValidateIssuerSigningKey = true,
//                        ValidateIssuer = true,
//                        ValidateAudience = true,
//                        ValidateLifetime = true,
//                        ValidIssuer = builder.Configuration["JWT:Issuer"],
//                        ValidAudience = builder.Configuration["JWT:Audience"],
//                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
//                    };
//                });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//using Hangfire
//app.UseHangfireDashboard("/hangfire");


//multy language
var supportedCultures = new[] {"en-US","ar-SA"};
var localizationOptions = new RequestLocalizationOptions()
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
