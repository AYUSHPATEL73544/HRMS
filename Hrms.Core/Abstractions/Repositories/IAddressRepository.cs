using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Utilities;

namespace Hrms.Core.Abstractions.Repositories
{
    public interface IAddressRepository
    {
        Task AddAsync(Address entity);
        Task<Address> GetByIdentificationIdAsync(int idenitificationId);
        Task<AddressModel> GetDetailAsync(int identificationId, Constants.AddressType addressType);
        Task<Address> FindAsync(int id);
        void Update(Address entity);
    }
}
