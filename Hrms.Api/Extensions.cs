using Hrms.Api.HostedServices;
using Hrms.Core.Abstractions;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Abstractions.Services;
using Hrms.Core.Managers;
using Hrms.Core.Utilities;
using Hrms.Infrastructure.Data;
using Hrms.Infrastructure.Data.Repositories;
using Hrms.Infrastructure.Provider;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;

namespace Hrms.Api
{
    public static class Extensions
    {
        public static IEnumerable<string> GetErrorList(this ModelStateDictionary modelState)
        {
            return modelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage.Replace("'", "")));
        }

        public static IEnumerable<string> GetErrorList(this IdentityResult identityResult)
        {
            return identityResult.Errors.Select(x => x.Description.Replace("'", ""));
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IStorageService, StorageService>();
        }

        public static void ConfigureBackgroundHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<LeaveService>();
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddScoped<IAttendanceLogRepository, AttendanceLogRepository>();
            services.AddScoped<IAttendanceRuleRepository, AttendanceRuleRepository>();
            services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
            services.AddScoped<IJobApplicantSkillsRepository, JobApplicantSkillsRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDesignationRepository, DesignationsRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ILeaveRepository, LeaveRepository>();
            services.AddScoped<ILeaveRuleRepository, LeaveRuleRepository>();
            services.AddScoped<ILeaveLogRepository, LeaveLogRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<ISeedRepository, SeedRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFamilyRepository, FamilyRepository>();
            services.AddScoped<IRelationshipRepository, RelationshipRepository>();
            services.AddScoped<IEmployeeManagerRepository, EmployeeManagerRepository>();
            services.AddScoped<IEducationRepository, EducationRepository>();
            services.AddScoped<IQualificationTypeRepository, QualificationTypeRepository>();
            services.AddScoped<ICourseTypeRepository, CourseTypeRepository>();
            services.AddScoped<IEmployeeAttendanceRuleRepository, EmployeeAttendanceRuleRepository>();
            services.AddScoped<IHolidayRepository, HolidayRepository>();
            services.AddScoped<IWorkHistroyRepository, WorkHistroyRepository>();
            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IAssetTypeRepository, AssetTypeRepository>();
            services.AddScoped<IVariantRepository, VariantRepository>();
            services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
            services.AddScoped<IAssetAllocationRepository, AssetAllocationRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IInterviewRepository, InterviewRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<ISkillRepository, SkillRespository>();
            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<IReimbursementRepository, ReimbursementRepository>();
        }

        public static void ConfigureManagers(this IServiceCollection services)
        {
            services.AddScoped<IAttendanceManager, AttendanceManager>();
            services.AddScoped<IAttendanceLogManager, AttendanceLogManager>();
            services.AddScoped<IAttendanceRuleManager, AttendanceRuleManager>();
            services.AddScoped<ICompanyManager, CompanyManager>();
            services.AddScoped<ICityManager, CityManager>();
            services.AddScoped<ICountryManager, CountryManager>();
            services.AddScoped<IDepartmentsManager, DepartmentManager>();
            services.AddScoped<IDesignationManager, DesignationManager>();
            services.AddScoped<IEmployeeManager, EmployeeManager>();
            services.AddScoped<ILeaveManager, LeaveManager>();
            services.AddScoped<ILeaveRuleManager, LeaveRuleManager>();
            services.AddScoped<ILeaveLogManager, LeaveLogManager>();
            services.AddScoped<IStateManager, StateManager>();
            services.AddScoped<ISeedManager, SeedManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IFamilyManager, FamilyManager>();
            services.AddScoped<IRelationshipManager, RelationshipManager>();
            services.AddScoped<ITeamManager, TeamManager>();
            services.AddScoped<IEducationManager, EducationManager>();
            services.AddScoped<ICourseTypeManager, CourseTypeManager>();
            services.AddScoped<IQualificationTypeManager, QualificationTypeManager>();
            services.AddScoped<IEmployeeAttendanceRuleManager, EmployeeAttendanceRuleManager>();
            services.AddScoped<IHolidayManager, HolidayManager>();
            services.AddScoped<IWorkHistoryManager, WorkHistoryManager>();
            services.AddScoped<IAssetManager, AssetManager>();
            services.AddScoped<IAssetTypeManager, AssetTypeManager>();
            services.AddScoped<IVariantManager, VariantManager>();
            services.AddScoped<IManufacturerManager, ManufacturerManager>();
            services.AddScoped<IAssetAllocationManager, AssetAllocationManager>();
            services.AddScoped<IJobApplicationManager, JobApplicationManager>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IMiscellaneousManager, MiscellaneousManager>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IJobApplicationManager, JobApplicationManager>();
            services.AddScoped<IInterviewManager, InterviewManager>();
            services.AddScoped<IDocumentManager, DocumentManager>();
            services.AddScoped<IUserRoleManager, UserRoleManager>();
            services.AddScoped<ISkillManager, SkillManager>();
            services.AddScoped<INoteManager, NoteManager>();
            services.AddScoped<IReimbursementManager, ReimbursementManager>();
        }

        public static void LoadAppSettings(this WebApplicationBuilder builder)
        {
            var appSettings = builder.Configuration.GetSection("AppSettings");

            AppSettings.AppBaseUrl = appSettings.GetValue<string>("AppBaseUrl");

            var jwt = builder.Configuration.GetSection("Jwt");
            AppSettings.Jwt.Secret = jwt.GetValue<string>("Secret");
            AppSettings.Jwt.Audience = jwt.GetValue<string>("Audience");
            AppSettings.Jwt.Issuer = jwt.GetValue<string>("Issuer");

            var sendGrid = builder.Configuration.GetSection("SendGrid");
            AppSettings.SendGrid.ApiKey = sendGrid.GetValue<string>("ApiKey");
            AppSettings.SendGrid.SenderEmail = sendGrid.GetValue<string>("SenderEmail");
            AppSettings.SendGrid.SenderName = sendGrid.GetValue<string>("SenderName");
            AppSettings.SendGrid.DefaultReceiver = sendGrid.GetValue<string>("DefaultReceiver");
        }

        public static void EnsureDbIsUpToDate(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var scopedProvider = scope.ServiceProvider;
                var logger = scopedProvider.GetRequiredService<ILogger<Program>>();

                try
                {
                    var dataContext = scopedProvider.GetRequiredService<DataContext>();

                    if (dataContext.Database.GetPendingMigrations().Any())
                    {
                        dataContext.Database.Migrate();
                    }

                    var seedManager = scope.ServiceProvider.GetRequiredService<ISeedManager>();

                    seedManager.InitializeAsync().Wait();

                    logger.LogInformation("DB seeding completed successfully");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }
        }

        public static int GetUserId(this IPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return Convert.ToInt32(claim?.Value);
        }
    }
}
