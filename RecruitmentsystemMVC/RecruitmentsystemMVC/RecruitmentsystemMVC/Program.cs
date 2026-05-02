using RecruitmentsystemMVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 🚀 Configure HttpClient with SSL Bypass for Local Development
builder.Services.AddHttpClient("RecruitmentAPI").ConfigurePrimaryHttpMessageHandler(() => 
{
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
    };
});

// Register custom services with named HttpClient
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthService>(sp => 
    new AuthService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("RecruitmentAPI"), sp.GetRequiredService<IHttpContextAccessor>()));
builder.Services.AddScoped<UserService>(sp => 
    new UserService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("RecruitmentAPI"), sp.GetRequiredService<IHttpContextAccessor>()));
builder.Services.AddScoped<JobService>(sp => 
    new JobService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("RecruitmentAPI"), sp.GetRequiredService<IHttpContextAccessor>()));
builder.Services.AddScoped<ApplicationService>(sp => 
    new ApplicationService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("RecruitmentAPI"), sp.GetRequiredService<IHttpContextAccessor>()));
builder.Services.AddScoped<InterviewService>(sp => 
    new InterviewService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("RecruitmentAPI"), sp.GetRequiredService<IHttpContextAccessor>()));
builder.Services.AddScoped<CompanyService>(sp => 
    new CompanyService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("RecruitmentAPI"), sp.GetRequiredService<IHttpContextAccessor>()));
builder.Services.AddScoped<RoleService>(sp => 
    new RoleService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("RecruitmentAPI"), sp.GetRequiredService<IHttpContextAccessor>()));
builder.Services.AddScoped<InterviewRoundService>(sp => 
    new InterviewRoundService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("RecruitmentAPI"), sp.GetRequiredService<IHttpContextAccessor>()));
builder.Services.AddScoped<CandidateService>(sp => 
    new CandidateService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("RecruitmentAPI"), sp.GetRequiredService<IHttpContextAccessor>()));
builder.Services.AddScoped<RecruitmentsystemMVC.Helpers.RoleHelper>();

// 🛠 Generic HttpClient for DI
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("RecruitmentAPI"));

// 🔐 Session Configuration
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🔐 Use Session
app.UseSession();

app.UseAuthorization();

// 🚀 Default Routing (No Areas)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
