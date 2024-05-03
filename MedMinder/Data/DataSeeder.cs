using MedMinder.Models;

namespace MedMinder.Data
{
    public class DataSeeder
    {
        private readonly DataContext _context;

        public DataSeeder(DataContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            var patient = new Patient("0", "Xander", "Ford", "Pasay City", true);

            _context.Patients.Add(patient);
            _context.SaveChanges();
        }
    }
}
