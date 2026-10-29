using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml;

namespace JobApplicationTracker.Models
{
    internal class Company
    {
        public UniqueId Id { get; } = new UniqueId();
        [Required] public string Name { get; set; } = "Not Spacified";
        public string Industry { get; set; } = "Not Specified";
        public string City { get; set; } = "Not Specified";
        public CompanyStatus Status { get; set; } = CompanyStatus.Pending;

        public Company(string name, string industry, string city, CompanyStatus status)
        {
            Name = name;
            Industry = industry;
            City = city;
            Status = status;
        }


    }
}
