using Hrms.Core.Abstractions;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models.Company;
using Hrms.Core.Utilities;

namespace Hrms.Core.Managers
{
    public class CompanyManager : ICompanyManager
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompanyManager(ICompanyRepository companyRepository,
            IAddressRepository addressRepository,
            IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository;
            _addressRepository = addressRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CompanyModel> GetAsync(int id)
        {
            var companyDetail = await _companyRepository.GetAsync(id);
            companyDetail.RegisteredOffice = await _addressRepository.GetDetailAsync(companyDetail.Id, Constants.AddressType.Registered);
            companyDetail.CorporateOffice = await _addressRepository.GetDetailAsync(companyDetail.Id, Constants.AddressType.Corporate);

            return companyDetail;
        }

        public async Task UpdateAsync(CompanyModel model, int userId)
        {
            var company = await _companyRepository.GetByIdAsync(model.Id);

            company.RegisteredName = model.RegisteredName;
            company.BrandName = model.BrandName;
            company.Email = model.Email;
            company.FacebookUrl = model.FacebookUrl;
            company.TwitterUrl = model.TwitterUrl;
            company.Phone = model.Phone;
            company.LinkedInUrl = model.LinkedInUrl;
            company.WebsiteUrl = model.WebsiteUrl;
            await UpsertAddressAsync(model);
            _companyRepository.Update(company);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int?> GetIdByUserIdAsync(int userId)
        {
            return await _companyRepository.GetIdByUserIdAsync(userId);
        }


        #region private mathod

        private async Task UpsertAddressAsync(CompanyModel model)
        {
            if (model.RegisteredOffice != null)
            {
                if (model.RegisteredOffice.Id > 0)
                {
                    var address = await _addressRepository.FindAsync(model.RegisteredOffice.Id);
                    address.Line1 = model.RegisteredOffice.Line1;
                    address.Line2 = model.RegisteredOffice.Line2;
                    address.CityId = model.RegisteredOffice.CityId;
                    address.PinCode = model.RegisteredOffice.PinCode;
                    _addressRepository.Update(address);
                }
                else
                {
                    var address = new Address
                    {
                        IdentificationId = model.Id,
                        Line1 = model.RegisteredOffice.Line1,
                        Line2 = model.RegisteredOffice.Line2,
                        AddressType = Constants.AddressType.Registered,
                        CityId = model.RegisteredOffice.CityId,
                        PinCode = model.RegisteredOffice.PinCode
                    };
                    await _addressRepository.AddAsync(address);
                }
            }
            if (model.CorporateOffice != null)
            {
                if (model.CorporateOffice.Id > 0)
                {
                    var address = await _addressRepository.FindAsync(model.CorporateOffice.Id);
                    address.Line1 = model.CorporateOffice.Line1;
                    address.Line2 = model.CorporateOffice.Line2;
                    address.CityId = model.CorporateOffice.CityId;
                    address.PinCode = model.CorporateOffice.PinCode;
                    _addressRepository.Update(address);
                }
                else
                {
                    var address = new Address
                    {
                        IdentificationId = model.Id,
                        Line1 = model.CorporateOffice.Line1,
                        Line2 = model.CorporateOffice.Line2,
                        AddressType = Constants.AddressType.Corporate,
                        CityId = model.CorporateOffice.CityId,
                        PinCode = model.CorporateOffice.PinCode
                    };
                    await _addressRepository.AddAsync(address);
                }
            }

        }

        #endregion
    }
}
