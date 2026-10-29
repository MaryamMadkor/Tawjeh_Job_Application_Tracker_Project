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
        [Required] public string Name { get; set; } = "Not Specified"; // would be handled in logic to ensure user inputs it.
        private readonly string emailPattern = "^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$";
        public string Email { get; set; } = "Not Specified";
        [Required] public int CompanyId { get; set; }

        public Recruiter(string name, string emailPattern, string email, int companyId)
        {
            Name = name;
            if(Regex.IsMatch(emailPattern, email))
            Email = email;
            else
            {
                throw new ArgumentException("Not a Valid Email Format.", nameof(Email));
            }
            CompanyId = companyId;
        }
    }
}
