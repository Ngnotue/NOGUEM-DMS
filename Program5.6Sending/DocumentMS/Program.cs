using DocumentMS.ConHelper;
using DocumentMS.Data;
using DocumentMS.Helpers;
using DocumentMS.Models;
using DocumentMS.Models.CommonViewModel;
using DocumentMS.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Services.SignalRService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();


builder.Services.AddScoped<ApplicationDbContext>();
var _ApplicationInfo = builder.Configuration.GetSection("ApplicationInfo").Get<ApplicationInfo>();
string _GetConnStringName = ControllerExtensions.GetConnectionString(builder.Configuration);
if (_ApplicationInfo.DBConnectionStringName == ConnectionStrings.connMySQL)
{
    builder.Services.AddDbContextPool<ApplicationDbContext>(options => options.UseMySql(_GetConnStringName, ServerVersion.AutoDetect(_GetConnStringName)));
}
else if (_ApplicationInfo.DBConnectionStringName == ConnectionStrings.connMSSQLDeployed) // For deployed remote database
{
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(_GetConnStringName));
}
else if (_ApplicationInfo.DBConnectionStringName == ConnectionStrings.connPostgreSQL)
{
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(_GetConnStringName));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(_GetConnStringName));
}

builder.Services.AddSignalR();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});

builder.Services.AddAuthentication(IISDefaults.AuthenticationScheme);


//Set Identity Options
var _IServiceScopeFactory = builder.Services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();
var _CreateScope = _IServiceScopeFactory.CreateScope();
var _ServiceProvider = _CreateScope.ServiceProvider;
var _context = _ServiceProvider.GetRequiredService<ApplicationDbContext>();
bool IsDBCanConnect = _context.Database.CanConnect();
if (IsDBCanConnect && _context.DefaultIdentityOptions.Count() > 0)
{
    var _DefaultIdentityOptions = _context.DefaultIdentityOptions.Where(x => x.Id == 1).SingleOrDefault();
    AddIdentityOptions.SetOptions(builder.Services, _DefaultIdentityOptions);
}
else
{
    IConfigurationSection _IConfigurationSection = builder.Configuration.GetSection("IdentityDefaultOptions");
    builder.Services.Configure<DefaultIdentityOptions>(_IConfigurationSection);
    var _DefaultIdentityOptions = _IConfigurationSection.Get<DefaultIdentityOptions>();
    AddIdentityOptions.SetOptions(builder.Services, _DefaultIdentityOptions);
}

//AddJWTOptions.SetOptions(builder.Services, builder.Configuration);

//Get Super Admin Default options
builder.Services.Configure<SuperAdminDefaultOptions>(builder.Configuration.GetSection("SuperAdminDefaultOptions"));
builder.Services.Configure<ApplicationInfo>(builder.Configuration.GetSection("ApplicationInfo"));

builder.Services.AddTransient<ICommon, Common>();
builder.Services.AddTransient<IDBOperation, DBOperation>();
builder.Services.AddTransient<IAccount, Account>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<IRoles, Roles>();
builder.Services.AddTransient<IFunctional, Functional>();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = _ApplicationInfo.FullName, Version = "v1" });
    c.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme.ToLowerInvariant(),
        In = ParameterLocation.Header,
        Name = "Authorization",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.OperationFilter<AuthResponsesOperationFilter>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});


builder.Services.Configure<FormOptions>(x =>
{
    x.ValueCountLimit = int.MaxValue;
});

builder.Services.AddWebOptimizer();

var app = builder.Build();

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AMS v1"));
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    
}

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.UseWebOptimizer();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseSession();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseCors("Open");

app.UseEndpoints(endpoints =>
    {
        endpoints.MapHub<SignalServer>("/notify");
    });

app.MapControllerRoute(name: "default", pattern: "{controller=Dashboard}/{action=Index}");

ProgramTaskExtension.SeedingData(app);

app.Run();
