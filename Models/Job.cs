using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text;
using System.Xml;

namespace JobApplicationTracker.Models
{
    internal class Job : IEntity
    { 
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = "Not Specified.";
        public string Description { get; set; } = "Not Specified.";
        public double Salary { get; set; }
        public string Location { get; set; } = "Not Specified.";
        public bool IsOpen { get; set; } = true;
        public DateTime PostedDate { get; set; } = DateTime.Now;
        public int CompanyId { get; set; }
        public int RecruiterId { get; set; }

        public Job(string title, string description, double salary, string location, int companyId, int recruiterId)
        {
            if (title == null) throw new ArgumentNullException("Title is Requied. Please insert a valid Title.");
            if (salary <= 0) throw new ArgumentException("Salary must be above threshold.");
            Title = title;
            Description = description;
            Location = location;
            Salary = salary;
            CompanyId = companyId;
            RecruiterId = recruiterId;
        }
    }
}
