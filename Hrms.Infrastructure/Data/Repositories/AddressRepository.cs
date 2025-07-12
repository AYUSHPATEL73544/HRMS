using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly DataContext _dataContext;
        public AddressRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(Address entity)
        {
            await _dataContext.AddAsync(entity);
        }

        public async Task<Address> GetByIdentificationIdAsync(int idenitificationId)
        {
            return await _dataContext.Addresses
                .Where(x => x.IdentificationId == idenitificationId)
                .SingleOrDefaultAsync();
        }

        public async Task<AddressModel> GetDetailAsync(int identificationId, Constants.AddressType addressType)
        {
            return await (from a in _dataContext.Addresses
                          join c in _dataContext.Cities on a.CityId equals c.Id
                          join s in _dataContext.States on c.StateId equals s.Id
                          where (a.IdentificationId == identificationId
                          && a.AddressType == addressType)
                          select new AddressModel
                          {
                              Id = a.Id,
                              IndentificationId = a.IdentificationId,
                              Line1 = a.Line1,
                              Line2 = a.Line2,
                              PinCode = a.PinCode,
                              CityId = c.Id,
                              CityName = c.Name,
                              StateId = s.Id,
                              StateName = s.Name
                          }).FirstOrDefaultAsync();
        }

        public async Task<Address> FindAsync(int id)
        {
            return await _dataContext.Addresses.FindAsync(id);
        }

        public void Update(Address entity)
        {
            _dataContext.Addresses.Update(entity);
        }


    }
}
