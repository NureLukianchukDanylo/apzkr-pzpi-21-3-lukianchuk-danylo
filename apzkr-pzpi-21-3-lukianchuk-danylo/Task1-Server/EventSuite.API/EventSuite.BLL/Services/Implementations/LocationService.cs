﻿using EventSuite.Core.Extra;
using EventSuite.Core.Models;
using EventSuite.Core.Resources;
using EventSuite.DAL.Repositories.Interfaces;
using EventSuite.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSuite.BLL.Services.Implementations
{
    public class LocationService : ILocationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public LocationService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<Location> CreateLocationAsync(Location location)
        {
            if (location == null)
                throw new ArgumentException(Resources.Get("Invalid arguments"));
            var result = await _unitOfWork.Locations.AddAsync(location);
            await _unitOfWork.Locations.SaveAsync();
            return result;
        }

        public async Task<bool> DeleteLocationAsync(int id)
        {
            var location = await _unitOfWork.Locations.GetByIdAsync(id);
            var result = _unitOfWork.Locations.Delete(location);
            await _unitOfWork.Locations.SaveAsync();
            return result;
        }

        public async Task<IEnumerable<Location>> GetLocationsAsync(PageInfo pageInfo)
        {
            return await _unitOfWork.Locations.GetPageWithMultiplePredicatesAsync(null, pageInfo, EntitySelector.LocationSelector);
        }

        public async Task<Location> GetLocationByIdAsync(int id)
        {
            var result = await _unitOfWork.Locations.GetByConditionAsync(x => x.Id == id, EntitySelector.LocationSelector);
            return result.FirstOrDefault();
        }

        public async Task<Location> UpdateLocationAsync(int id, Location location)
        {
            if (location == null)
                throw new ArgumentException(Resources.Get("Invalid arguments"));
            var existingLocation = await _unitOfWork.Locations.GetByIdAsync(id);
            if (existingLocation == null)
                return null;
            existingLocation.Street = location.Street;
            existingLocation.StreetType = location.StreetType;
            existingLocation.City = location.City;
            existingLocation.BuildingNumber = location.BuildingNumber;
            existingLocation.Country = location.Country;
            existingLocation.DateUpdated = DateTime.Now;
            var result = _unitOfWork.Locations.Update(existingLocation);
            await _unitOfWork.Locations.SaveAsync();
            return result;
        }
    }
}
