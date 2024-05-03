using MedMinder.Repositories;
using MedMinder.Data;
using MedMinder.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MedMinder.UnitTests.Repositories
{
    public class PatientRepositoryTests
    {
        private IPatientRepository _repository;
        const string databaseName = "MedMinder";

        [Test]
        public async Task PatientRepository_IfExists_ThenReturnRecord()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;

            using (var context = new DataContext(options))
            {
                context.Patients.Add(new Patient("0", "Xander", "Ford", "Pasay City", true));
                context.SaveChanges();

                _repository = new PatientRepository(context);

                var result = await _repository.GetAsync("xander");
                Assert.That(result.Count(), Is.EqualTo(1));
            }
        }

        [Test]
        public async Task PatientRepository_IfGet_ThenReturnAll()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;

            using (var context = new DataContext(options))
            {
                context.Patients.Add(new Patient("1", "Xander", "Ford", "Pasay City", true));
                context.Patients.Add(new Patient("2", "Marlou", "Arizala", "Pateros City", false));
                context.Patients.Add(new Patient("3", "Daniel", "Ford", "Taguig City", false));
                context.Patients.Add(new Patient("4", "Daniel", "Padilla", "Makati City", true));
                context.SaveChanges();

                _repository = new PatientRepository(context);
                var result = await _repository.Get();

                Assert.That(result.Count(), Is.EqualTo(5));
            }
        }
    }
}
