using MedMinder.Data;
using MedMinder.Models;
using Microsoft.EntityFrameworkCore;

namespace MedMinder.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DataContext _dataContext;

        public PatientRepository(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public Task<Patient[]> Get()
        {
            return _dataContext.Patients.ToArrayAsync();
        }

        public async Task<bool> Create(Patient patient)
        {
            if (_dataContext.Patients.FirstOrDefault().Id == patient.Id) 
            {
                return false;
            }

            await _dataContext.AddAsync(patient);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<Patient?> Update(Patient patient)
        {
            var existingRecord = await _dataContext.Patients.FirstOrDefaultAsync(x => x.Id == patient.Id);

            if (existingRecord == null)
                return null;

            existingRecord.FirstName = patient.FirstName;
            existingRecord.LastName = patient.LastName;
            existingRecord.City = patient.City;
            existingRecord.IsActive = patient.IsActive;

            await _dataContext.SaveChangesAsync();
            return existingRecord;
        }

        public Task<Patient[]> GetAsync(string searchTerms)
        {
            string keyword = searchTerms.Trim().ToLower();
            var result = _dataContext.Patients.Where(
                p => p.FirstName.ToLower().Equals(keyword) ||
                     p.LastName.ToLower().Equals(keyword) ||
                     p.City.ToLower().Equals(keyword)).ToArrayAsync();

            return result;
        }

        public async Task<bool> Delete(string patientId)
        {
            var existingRecord = await _dataContext.Patients.FirstOrDefaultAsync(p => p.Id == patientId);

            if (existingRecord == null) 
                return false;

            _dataContext.Patients.Remove(existingRecord);
            _dataContext.SaveChanges();
            return true;
        }
    }
}
