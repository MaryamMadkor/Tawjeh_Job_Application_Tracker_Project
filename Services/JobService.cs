using JobApplicationTracker.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace JobApplicationTracker.Services
{
    internal class JobService
    {
        // add job, update job data, read by id, read by status (open or closed), read by company, read by recruiter, read all, remove by id

        public static DataStore<Job> Jobs = new();
        private readonly CompanyService companyService;
        private readonly RecruiterService recruiterService;
        private readonly ApplicationService applicationService;

        public JobService(CompanyService company, RecruiterService recruiter, ApplicationService application)
        {
            companyService = company;
            recruiterService = recruiter;
            applicationService = application;
        }

        private static string Serialize(Job j) =>
            $"{j.Id}|{j.Title}|{j.Description}|{j.Salary}|{j.Location}|{j.IsOpen}|{j.PostedDate:O}|{j.CompanyId}|{j.RecruiterId}";

        private static Job Deserialize(string line)
        {
            var p = line.Split('|');
            var job = new Job(
                p[1],
                p[2],
                double.Parse(p[3], System.Globalization.CultureInfo.InvariantCulture),
                p[4],
                int.Parse(p[7]),
                int.Parse(p[8])
            )
            {
                Id = int.Parse(p[0]),
                IsOpen = bool.Parse(p[5]),
                PostedDate = DateTime.Parse(p[6], System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.RoundtripKind)
            };
            return job;
        }

        public void SaveAll() =>
            Jobs.SaveToFile(Constants.JobFilePath, Serialize);

        public void LoadAll() =>
            Jobs.LoadFromFile(Constants.JobFilePath, Deserialize);

        public Constants.TriggerAlert Add(string title, string description, double salary, string location, int companyId, int recruiterId)
        {
            if (string.IsNullOrWhiteSpace(title) ||
                string.IsNullOrWhiteSpace(description) ||
                string.IsNullOrWhiteSpace(location) ||
                salary <= 0 || companyId <= 0 || recruiterId <= 0)
                return new Constants.TriggerAlert(false, "Invalid Input.");
            if (salary < 3000)
                return new Constants.TriggerAlert(false, "Insufficient salary input.");
            // should check company status, should ensure both company and recruiter ids exist and respond to genuine entities...
            // should check that the recruiter belongs to claimed company id.... Wait for file persistence... Done
            var company = companyService.ReadById(companyId);
            if (company == null)
                return new Constants.TriggerAlert(false, "Company does not exist.");

            var recruiter = recruiterService.ReadById(recruiterId);
            if (recruiter == null)
                return new Constants.TriggerAlert(false, "Recruiter does not exist.");

            if (recruiter.CompanyId != companyId)
                return new Constants.TriggerAlert(false, "Recruiter does not belong to this company.");

            if (company.Status != CompanyStatus.Approved)
                return new Constants.TriggerAlert(false,
                    $"Company is {company.Status}. Cannot post jobs.");

            var job = new Job(title, description, salary, location, companyId, recruiterId);
            Jobs.Add(job);
            return new Constants.TriggerAlert(true, "Added Successfully.");
        }

        public Constants.TriggerAlert UpdateById(int id, string? title, string? description, double? salary, bool? isOpen, string? location, int actingRecruiterId)
        {
            if (id <= 0) return new Constants.TriggerAlert(false, "not a valid id.");
            
            var job = Jobs.ReadById(id);
            if (job == null) return new Constants.TriggerAlert(false, "No job matches this id.");

            if (job.RecruiterId != actingRecruiterId) return new Constants.TriggerAlert(false, "Recruiter does not own this job.");

            if (title == null) title = job.Title;
            if (description == null) description = job.Description;
            if (location == null) location = job.Location;

            if (salary.HasValue)
            {
                if (salary <= 0 || salary < 3000)
                    return new Constants.TriggerAlert(false, "Invalid Salary Input.");
                job.Salary = salary.Value;
            }
            // meaning that if a user doesn't enter new data, keep original data and just update the changed data.

            job.Title = title;
            job.Description = description;
            job.Location = location;
            job.IsOpen = isOpen ?? job.IsOpen;
            Jobs.Update(job);
            return new Constants.TriggerAlert(true, "Updated successfully.");
        }

        public Job ReadById(int id)
        {
            if (id <= 0) throw new ArgumentException("not a valid id.");

            var job = Jobs.ReadById(id);
            if (job == null) throw new ArgumentException("No job matches this id.");
            return job;
        }

        public Job ReadById(int id, int actingRecruiterId)
        {
            if (id <= 0 || actingRecruiterId <= 0) throw new ArgumentException("not a valid id.");
            
            var job = Jobs.ReadById(id);
            if (job == null) throw new ArgumentException("No job matches this id.");
            return job;
        }

        public List<Job> ReadAll()
        {
            return Jobs.ReadAll();
        }

        public List<Job> ReadByStatus(bool status = true)
        {
            var jobs = Jobs.ReadAll();
            if (jobs == null) throw new ArgumentException("No jobs exist yet.");
            var result = new List<Job>();
            foreach (Job job in jobs)
                if (job.IsOpen == status) result.Add(job);
            return result;
        }

        public List<Job> ReadByCompany(int companyId)
        {
            if (companyId <= 0) throw new ArgumentException("not a valid id.");
            var result = new List<Job>();
            foreach (Job job in Jobs.ReadAll())
                if (job.CompanyId == companyId) result.Add(job);
            return result;
        }

        public List<Job> ReadByRecruiter(int recruiterId)
        {
            if (recruiterId <= 0) throw new ArgumentException("not a valid id.");
            var result = new List<Job>();
            foreach (Job job in Jobs.ReadAll())
                if (job.RecruiterId == recruiterId) result.Add(job);
            return result;
        }

        public Constants.TriggerAlert RemoveById(int id, int actingRecruiterId)
        {
            if (id <= 0 || actingRecruiterId <= 0)
                return new Constants.TriggerAlert(false, "Invalid Id.");

            var job = Jobs.ReadById(id);
            if (job == null)
                return new Constants.TriggerAlert(false, "No job matches this id.");

            if (job.RecruiterId != actingRecruiterId)
                return new Constants.TriggerAlert(false, "Recruiter does not own this job.");

            // remove all applications related to this job
            var apps = applicationService.ReadByJob(id);
            foreach (var app in apps)
                applicationService.RemoveById(app.Id, job.RecruiterId);

            Jobs.RemoveById(id);
            return new Constants.TriggerAlert(true, "Removed Successfully.");
        }
    }
} 

