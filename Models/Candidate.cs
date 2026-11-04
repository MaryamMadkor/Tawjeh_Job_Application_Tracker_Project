using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace JobApplicationTracker.Models
{
    internal class Candidate : IEntity
    {
        public int Id { get; set; }
        [Required] 
        public string FullName { get; set; } = "Not Specified.";
        public string Email { get; set; } = "Not Specified.";
        public string Phone { get; set; } = "Not Specified.";
        public int YearsOfExperience { get; set; }

        public Candidate(string fullName, string email, string phone, int yearsOfExperience)
        {
            if (fullName == null) throw new ArgumentNullException("Name is required. Please insert a valid full name.");
            if (yearsOfExperience < 0) throw new ArgumentException("Years of Experience is not a valid number.");
            Email = email;
            FullName = fullName;
            Email = email;
            Phone = phone;
            YearsOfExperience = yearsOfExperience;
        }
    }
}
