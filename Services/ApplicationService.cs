using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using JobApplicationTracker.Models;

namespace JobApplicationTracker.Services
{
    internal class ApplicationService
    {
        // apply (add), read by id, read all, read by job, read by candidate,
        // change status, remove by id, remove by job (cascade), read by company

        public static DataStore<Application> Applications = new();
        public static HashSet<(int JobId, int CandidateId)> AppliedPairs = new();

        private readonly CandidateService candidateService;
        private readonly CompanyService companyService;
        private readonly RecruiterService recruiterService;
        private readonly JobService jobService;
        public ApplicationService(CandidateService candidate, CompanyService company, RecruiterService recruiter, JobService job)
        {
            candidateService = candidate;
            companyService = company;
            recruiterService = recruiter;
            jobService = job;
        }

        private static string Serialize(Application a) =>
            $"{a.Id}|{a.JobId}|{a.CandidateId}|{a.AppliedTime:O}|{a.applictionStatus}";

        private static Application Deserialize(string line)
        {
            var p = line.Split('|');
            var app = new Application(
                int.Parse(p[1]),
                int.Parse(p[2])
            )
            {
                Id = int.Parse(p[0]),
                AppliedTime = DateTime.Parse(p[3]),
                applictionStatus = Enum.Parse<ApplicationStatus>(p[4])
            };
            return app;
        }

        public void SaveAll() =>
            Applications.SaveToFile(Constants.ApplicationFilePath, Serialize);

        public void LoadAll()
        {
            Applications.LoadFromFile(Constants.ApplicationFilePath, Deserialize);
            // keep the application set..
            AppliedPairs.Clear();
            foreach (var app in Applications.ReadAll())
                AppliedPairs.Add((app.JobId, app.CandidateId));
        }

        public Constants.TriggerAlert Apply(int jobId, int candidateId)
        {
            if (jobId <= 0 || candidateId <= 0)
                return new Constants.TriggerAlert(false, "Invalid Id.");

            var job = jobService.ReadById(jobId);
            if (job == null)
                return new Constants.TriggerAlert(false, "Job does not exist.");

            var candidate = candidateService.ReadById(candidateId);
            if (candidate == null)
                return new Constants.TriggerAlert(false, "Candidate does not exist.");

            if (!job.IsOpen)
                return new Constants.TriggerAlert(false, "Cannot apply: job is closed.");

            var company = companyService.ReadById(job.CompanyId);
            if (company == null)
                return new Constants.TriggerAlert(false, "Company does not exist.");

            if (company.Status != CompanyStatus.Approved)
                return new Constants.TriggerAlert(false,
                    $"Cannot apply: company is {company.Status}.");

            if (!AppliedPairs.Add((jobId, candidateId)))
                return new Constants.TriggerAlert(false,
                    "Cannot apply: candidate already applied to this job.");

            var app = new Application(jobId, candidateId);
            Applications.Add(app);
            return new Constants.TriggerAlert(true, "Application submitted.");
        }

        public Constants.TriggerAlert ChangeStatus(int applicationId, ApplicationStatus newStatus, int actingRecruiterId)
        {
            if (applicationId <= 0 || actingRecruiterId <= 0)
                return new Constants.TriggerAlert(false, "Invalid Id.");

            var app = Applications.ReadById(applicationId);
            if (app == null)
                return new Constants.TriggerAlert(false, "No application matches this id.");

            var job = jobService.ReadById(app.JobId);
            if (job == null)
                return new Constants.TriggerAlert(false, "Related job does not exist.");

            if (job.RecruiterId != actingRecruiterId)
                return new Constants.TriggerAlert(false, "Recruiter does not own this job.");

            app.applictionStatus = newStatus;
            Applications.Update(app);
            return new Constants.TriggerAlert(true, "Status updated.");
        }

        public Application ReadById(int id)
        {
            if (id <= 0) throw new ArgumentException("not a valid id.");
            var app = Applications.ReadById(id);
            if (app == null) throw new ArgumentException("No application matches this id.");
            return app;
        }

        public List<Application> ReadAll()
        {
            return Applications.ReadAll();
        }

        public List<Application> ReadByJob(int jobId)
        {
            if (jobId <= 0) throw new ArgumentException("not a valid id.");
            var result = new List<Application>();
            foreach (Application a in Applications.ReadAll())
                if (a.JobId == jobId) result.Add(a);
            return result;
        }

        public List<Application> ReadByCandidate(int candidateId)
        {
            if (candidateId <= 0) throw new ArgumentException("not a valid id.");
            var result = new List<Application>();
            foreach (Application a in Applications.ReadAll())
                if (a.CandidateId == candidateId) result.Add(a);
            return result;
        }

        public List<Application> ReadByCompany(int companyId)
        {
            if (companyId <= 0) throw new ArgumentException("not a valid id.");
            var result = new List<Application>();
            foreach (Application a in Applications.ReadAll())
            {
                var job = jobService.ReadById(a.JobId);
                if (job != null && job.CompanyId == companyId)
                    result.Add(a);
            }
            return result;
        }

        public Constants.TriggerAlert RemoveById(int id, int actingRecruiterId)
        {
            if (id <= 0 || actingRecruiterId <= 0)
                return new Constants.TriggerAlert(false, "Invalid Id.");

            var app = Applications.ReadById(id);
            if (app == null)
                return new Constants.TriggerAlert(false, "No application matches this id.");

            var job = jobService.ReadById(app.JobId);
            if (job == null)
                return new Constants.TriggerAlert(false, "Related job does not exist.");

            if (job.RecruiterId != actingRecruiterId)
                return new Constants.TriggerAlert(false, "Recruiter does not own this job.");

            Applications.RemoveById(id);
            AppliedPairs.Remove((app.JobId, app.CandidateId));
            return new Constants.TriggerAlert(true, "Removed Successfully.");
        }
    }
}