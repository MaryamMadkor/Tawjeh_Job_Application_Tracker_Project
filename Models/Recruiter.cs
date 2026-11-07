using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;


namespace JobApplicationTracker.Models
{
    internal class Recruiter: IEntity
    {
        public int Id { get; set; }
        [Required] 
        public string Name { get; set; } = "Not Specified"; // would be handled in logic to ensure user inputs it.
        public string Email { get; set; } = "Not Specified";
        public int CompanyId { get; set; }

        public Recruiter(string name, string email, int companyId)
        {
            Name = name;
            Email = email;
            CompanyId = companyId;
        }
    }
}
