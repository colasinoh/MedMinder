using System.ComponentModel.DataAnnotations;

namespace MedMinder.Models
{
    public class Patient
    {
        public Patient(string id, string firstName, string lastName, string city, bool isActive)
        {
            Id = id;
            FirstName = firstName; 
            LastName = lastName;
            City = city;
            IsActive = isActive;
        }

        [Required(ErrorMessage = "Id is required")]
        public string Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "ActiveStatus is required")]
        public bool IsActive { get; set; }

    }
}
