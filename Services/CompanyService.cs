using JobApplicationTracker.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JobApplicationTracker.Services
{
    internal class CompanyService
    {
        // add, update status by id, read by id, read all, read by status, remove by id.
        public static DataStore<Company> CompanyList = new();
        public static Stack<(int CompnayId, CompanyStatus companyStatus)> CompanyStatusHistory = new();

        public void Add(string name, string industry, string city)
        {
            Company company = new Company(name, city, industry);
            CompanyList.Add(company);
            CompanyStatusHistory.Push((company.Id, company.Status));
        }

        public void UpdatestatusById(int id, CompanyStatus status)
        {
            if(id <= 0) throw new ArgumentOutOfRangeException("id Is Invalid");

            var company = CompanyList.ReadById(id);
            if (company == null) throw new ArgumentException("Not found.");
            company.Status = status;
            CompanyList.Update(company);
            CompanyStatusHistory.Push((company.Id, status));
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

        public void RemoveById(int id)
        {
            if(id <= 0) throw new ArgumentOutOfRangeException("id is not valid.");
            CompanyList.RemoveById(id);
            // should also remove all related recruiters, jobs and applications. but maybe should be triggered in the main or the acting class
            // Lets keep it simple stupid for now....
            // also remove all company status from stack .........
            var temp = new Stack<(int CompanyId, CompanyStatus Status)>();
            while (CompanyStatusHistory.Count > 0)
            {
                var entry = CompanyStatusHistory.Pop();
                if (entry.CompnayId != id) temp.Push(entry);
            }
            // now reverse the order of the remaining companies status
            while (temp.Count > 0)
                CompanyStatusHistory.Push(temp.Pop());
        }
    }
}
