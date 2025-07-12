namespace Hrms.Core.Abstractions.Repositories
{
    public interface ISeedRepository
    {
        Task SeedRolesAsync();
        Task SeedAdminAsync();
        Task SeedCountryAsync();
        Task SeedCompanyAsync();
        Task SeedRelationshipAsync();
        Task SeedQualificationTypeAsync();
        Task SeedCourseTypeAsync();
        Task SeedAssetTypeAsync();
        Task SeedSkillsAsync();

    }
}
