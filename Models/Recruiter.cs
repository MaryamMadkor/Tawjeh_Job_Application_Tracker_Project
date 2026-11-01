using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;


namespace JobApplicationTracker.Models
{
    internal class Recruiter
    {
        public UniqueId id { get; } = new UniqueId();
        [Required] 
        public string Name { get; set; } = "Not Specified"; // would be handled in logic to ensure user inputs it.
        public string Email { get; set; } = "Not Specified";
        public int CompanyId { get; set; }

        public Recruiter(string name, string email, int companyId)
        {
            if(name == null) throw new ArgumentNullException("Recruiter name is required. Please insert a valid name.");
            Name = name;
            if(Regex.IsMatch(Constants.emailPattern, email))
            Email = email;
            else
            {
                throw new ArgumentException("The Email Provided Is Not In a Valid Email Format: ", nameof(Email));
            }
            CompanyId = companyId;
        }
    }
}
