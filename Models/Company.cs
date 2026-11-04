using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml;

namespace JobApplicationTracker.Models
{
    internal class Company : IEntity
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; } = "Not Spacified";
        public string Industry { get; set; } = "Not Specified";
        public string City { get; set; } = "Not Specified";
        public CompanyStatus Status { get; set; } = CompanyStatus.Pending;

        public Company(string name, string industry, string city)
        {
            if(name  == null) throw new ArgumentNullException("Name is required. Please insert a valid Name.");
            Name = name;
            Industry = industry;
            City = city;
        }


    }
}
