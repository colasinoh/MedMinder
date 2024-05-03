using MedMinder.Models;

namespace MedMinder.Repositories
{
    public interface IPatientRepository
    {
        Task<Patient[]> Get();
        Task<bool> Create(Patient patient);
        Task<Patient?> Update(Patient patient);
        Task<Patient[]> GetAsync(string searchTerms);
        Task<bool> Delete(string patientId);
    }
}
