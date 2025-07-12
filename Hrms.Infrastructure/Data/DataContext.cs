using Hrms.Core.Entities;
using Hrms.Infrastructure.Data.EntityConfigs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hrms.Infrastructure.Data
{
    public class DataContext : IdentityDbContext<User, Role<int>, int>
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AttendanceRule> AttendanceRules { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<LeaveRule> LeaveRules { get; set; }
        public DbSet<LeaveLog> LeaveLogs { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<AttendanceLog> AttendanceLogs { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<Family> Families { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<QualificationType> QualificationTypes { get; set; }
        public DbSet<CourseType> CourseTypes { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<EmployeeAttendanceRule> EmployeeAttendanceRules { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<WorkHistory> WorkHistories { get; set; }
        public DbSet<AssetAllocation> AssetAllocation { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Variant> Variants { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<AssetType> AssetTypes { get; set; }
        public DbSet<AssetAllocation> AssetAllocations { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<ApplicantsSkill> ApplicantsSkills { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Reimbursement> Reimbursements {get; set;}

        public DataContext(DbContextOptions options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RoleConfig());
            builder.ApplyConfiguration(new RoleClaimConfig());
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new UserClaimConfig());
            builder.ApplyConfiguration(new UserRoleConfig());
            builder.ApplyConfiguration(new UserLoginConfig());
            builder.ApplyConfiguration(new UserTokenConfig());
            builder.ApplyConfiguration(new AddressConfig());
            builder.ApplyConfiguration(new AttendanceConfig());
            builder.ApplyConfiguration(new AttendanceLogConfig());
            builder.ApplyConfiguration(new AttendanceRuleConfig());
            builder.ApplyConfiguration(new CityConfig());
            builder.ApplyConfiguration(new CompanyConfig());
            builder.ApplyConfiguration(new CountryConfig());
            builder.ApplyConfiguration(new DepartmentConfig());
            builder.ApplyConfiguration(new DesignationConfig());
            builder.ApplyConfiguration(new EmployeeConfig());
            builder.ApplyConfiguration(new LeaveRuleConfig());
            builder.ApplyConfiguration(new LeaveLogConfig());
            builder.ApplyConfiguration(new LeaveConfig());
            builder.ApplyConfiguration(new StateConfig());
            builder.ApplyConfiguration(new RelationshipConfig());
            builder.ApplyConfiguration(new FamilyConfig());
            builder.ApplyConfiguration(new TeamConfig());
            builder.ApplyConfiguration(new QualificationTypeConfig());
            builder.ApplyConfiguration(new CourseTypeConfig());
            builder.ApplyConfiguration(new EducationConfig());
            builder.ApplyConfiguration(new EmployeeAttendanceRuleConfig());
            builder.ApplyConfiguration(new HolidayConfig());
            builder.ApplyConfiguration(new WorkHistoryConfig());
            builder.ApplyConfiguration(new AssetAllocationConfig());
            builder.ApplyConfiguration(new AssetConfig());
            builder.ApplyConfiguration(new VariantConfig());
            builder.ApplyConfiguration(new ManufacturerConfig());
            builder.ApplyConfiguration(new AssetTypeConfig());
            builder.ApplyConfiguration(new JobApplicationConfig());
            builder.ApplyConfiguration(new InterviewConfig());
            builder.ApplyConfiguration(new ApplicantsSkillConfig());
            builder.ApplyConfiguration(new Skillconfig());
            builder.ApplyConfiguration(new Documentconfig());
            builder.ApplyConfiguration(new NoteConfig());
            builder.ApplyConfiguration(new ReimbursementConfig());
        }
    }
}
