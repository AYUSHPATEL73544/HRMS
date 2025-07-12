namespace Hrms.Core.Models.Company
{
    public class CompanyModel
    {
        public int Id { get; set; }
        public string RegisteredName { get; set; }
        public string WebsiteUrl { get; set; }
        public string BrandName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string TwitterUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string LinkedInUrl { get; set; }

        public List<DepartmentModel> Departments { get; set; }
        public AddressModel RegisteredOffice { get; set; }
        public AddressModel CorporateOffice { get; set; }
    }
}
