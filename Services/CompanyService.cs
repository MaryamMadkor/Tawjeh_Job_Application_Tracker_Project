using JobApplicationTracker.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace JobApplicationTracker.Services
{
    internal class CompanyService
    {
        // add, update status by id, read by id, read all, read by status, remove by id.
        public static DataStore<Company> CompanyList = new();
        public static Stack<(int CompanyId, CompanyStatus companyStatus)> CompanyStatusHistory = new();
        private static string Serialize(Company c) =>
                $"{c.Id}|{c.Name}|{c.Industry}|{c.City}|{c.Status}";

        private static Company Deserialize(string line)
        {
            var p = line.Split('|');
            var company = new Company
            (
                p[1],
                p[2],
                p[3]
            )
            {
                Id = int.Parse(p[0]),
                Status = Enum.Parse<CompanyStatus>(p[4])
            };
            return company;
        }

        public void SaveAll() =>
            CompanyList.SaveToFile(Constants.CompanyFilePath, Serialize);

        public void LoadAll() =>
            CompanyList.LoadFromFile(Constants.CompanyFilePath, Deserialize);

        public Constants.TriggerAlert Add(string name, string industry, string city)
        {
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(industry) ||
                string.IsNullOrWhiteSpace(city))
                return new Constants.TriggerAlert(false, "Invalid input.");
            Company company = new Company(name, industry, city);
            CompanyList.Add(company);
            return new Constants.TriggerAlert(true, "Added Successfully.");
        }
        // update normal data not status
        public Constants.TriggerAlert UpdateById(int id, string? name, string? industry, string? city)
        {
            if (id <= 0) return new Constants.TriggerAlert(false, "not a valid id.");
            var comp = CompanyList.ReadById(id);
            if (comp == null) return new Constants.TriggerAlert(false, "No company matches this id.");
            if (name == null) name = comp.Name;
            if (industry == null) industry = comp.Industry;
            if (city == null ) city = comp.City;
            // update only changed data...
            comp.Name = name;
            comp.Industry = industry;
            comp.City = city;
            CompanyList.Update(comp);
            return new Constants.TriggerAlert(true, "Updataed Successfully.");
        }
        // update status...
        public Constants.TriggerAlert ChangeStatus(int id, CompanyStatus newStatus)
        {
            if (id <= 0) return new Constants.TriggerAlert(false, "Not a valid id.");

            var company = CompanyList.ReadById(id);
            if (company == null) return new Constants.TriggerAlert(false, "No company matches this id.");
            // user can't make it pending... its already pending  
            // what if user choose same status
            if (newStatus != CompanyStatus.Approved && newStatus != CompanyStatus.Rejected)
                return new Constants.TriggerAlert(false, "Status must be Approved or Rejected.");

            if (company.Status == newStatus)
                return new Constants.TriggerAlert(false, $"Company is already {newStatus}.");

            // push the old status BEFORE changing it
            CompanyStatusHistory.Push((id, company.Status));
            company.Status = newStatus;
            CompanyList.Update(company);

            return new Constants.TriggerAlert(true,
                $"Company {id} status changed from {CompanyStatusHistory.Peek().companyStatus} to {newStatus}.");
        }
        // undo last status
        public Constants.TriggerAlert UndoLastStatusChange()
        {
            if (CompanyStatusHistory.Count == 0)
                return new Constants.TriggerAlert(false, "Nothing to undo.");

            var (companyId, oldStatus) = CompanyStatusHistory.Pop();
            var company = CompanyList.ReadById(companyId);
            if (company == null) // .... doesn't exist? but it was pushed.... then removed after changing its state before..?
                return new Constants.TriggerAlert(false,
                    $"Company {companyId} no longer exists; cannot undo.");

            company.Status = oldStatus;
            CompanyList.Update(company);

            return new Constants.TriggerAlert(true,
                $"Company {companyId} status restored to {oldStatus}.");
        }
        public Company ReadById(int id)
        {
            if (id <= 0) throw new ArgumentException("Not a valid id.");
            var company = CompanyList.ReadById(id);
            if (company == null) throw new ArgumentException("No company matches this id.");
            return company;
        }
        public List<Company> ReadByStatus(CompanyStatus status)
        {
            var companies = CompanyList.ReadAll();
            var StatusComp = new List<Company>();
            foreach( Company company in companies )
            {
                if(company.Status == status) StatusComp.Add(company);
            }
            if(StatusComp.Count == 0) return new List<Company>();
            return StatusComp;
        }
        public List<Company> ReadAll()
        {
            return CompanyList.ReadAll();
        }

        public Constants.TriggerAlert RemoveById(int id)
        {
            if(id <= 0) return new Constants.TriggerAlert(false, "id is not valid.");
            var comp = CompanyList.ReadById(id);
            if (comp == null) return new Constants.TriggerAlert(false, "No company matches this id.");
            CompanyList.RemoveById(id);
            // should also remove all related recruiters, jobs and applications. but maybe should be triggered in the main or the acting class
            // Lets keep it simple stupid for now....
            // also remove all company status from stack .........
            var temp = new Stack<(int CompanyId, CompanyStatus Status)>();
            while (CompanyStatusHistory.Count > 0)
            {
                var entry = CompanyStatusHistory.Pop();
                if (entry.CompanyId != id) temp.Push(entry);
            }
            // now reverse the order of the remaining companies status
            while (temp.Count > 0)
                CompanyStatusHistory.Push(temp.Pop());
            return new Constants.TriggerAlert(true, "Removed Susseccfully.");

        }
    }
}
