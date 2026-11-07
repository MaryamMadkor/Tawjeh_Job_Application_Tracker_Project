using JobApplicationTracker.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace JobApplicationTracker.Services
{
    internal class RecruiterService
    {
        // add, update, read by id, read by company, read all, remove
        public static DataStore<Recruiter> recruiters = new DataStore<Recruiter>();
        public static HashSet<string> recruitersEmail = new(StringComparer.OrdinalIgnoreCase);

        private readonly CompanyService companyService;
        public RecruiterService(CompanyService company)
        {
            companyService = company;
        }

        private static string Serialize(Recruiter r) =>
                $"{r.Id}|{r.Name}|{r.Email}|{r.CompanyId}";

        private static Recruiter Deserialize(string line)
        {
            var p = line.Split('|');
            var recruiter = new Recruiter
            (
                p[1],
                p[2],
                int.Parse(p[3])
            )
            {
                Id = int.Parse(p[0])
            };
            return recruiter;
        }

        public void SaveAll() =>
            recruiters.SaveToFile(Constants.RecruiterFilePath, Serialize);

        public void LoadAll() =>
            recruiters.LoadFromFile(Constants.RecruiterFilePath, Deserialize);

        public Constants.TriggerAlert Add(string name, string email, int companyId)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || companyId <= 0) return new Constants.TriggerAlert(false, "invalid Input.");
            if (!Regex.IsMatch(email, Constants.emailPattern)) return new Constants.TriggerAlert(false, "Not A Valid Email Format.");
            var company = companyService.ReadById(companyId);
            if (company == null) return new Constants.TriggerAlert(false, "No company matches this id.");
            if (recruitersEmail.Contains(email)) return new Constants.TriggerAlert(false, "This email already exist. A unique email is required.");


            var rec = new Recruiter(name, email, companyId);
            recruiters.Add(rec);
            recruitersEmail.Add(email);
            return new Constants.TriggerAlert(true, "Added successfully.");
        }
        public Constants.TriggerAlert UpdateById(int id, string? name, string? email)
        {
            if (id <= 0) return new Constants.TriggerAlert(false, "not a valid id.");
            var rec = recruiters.ReadById(id);
            if (rec == null) return new Constants.TriggerAlert(false, "No recruiter matches this id.");

            if (name == null) name = rec.Name;
            if (email == null)
            {
                email = rec.Email;
            }
            else if (email != rec.Email)
            {
                if (!Regex.IsMatch(email, Constants.emailPattern))
                    return new Constants.TriggerAlert(false, "Not a valid email format.");

                if (recruitersEmail.Contains(email))
                    return new Constants.TriggerAlert(false, "This email already belongs to another candidate.");

                recruitersEmail.Remove(rec.Email);
                recruitersEmail.Add(email);
            }
            // meaning that if a user doesn't enter new data, keep original data and just update the changed data.
            rec.Name = name;
            rec.Email = email;

            recruiters.Update(rec);
            return new Constants.TriggerAlert(true, "Updated Successfully.");
        }
        public Recruiter ReadById(int id)
        {
            if (id <= 0) throw new ArgumentException("not a valid id.");
            var recruiter = recruiters.ReadById(id);
            if (recruiter == null) throw new ArgumentException("No recruiter matches this id.");
            return recruiter;
        }

        public List<Recruiter> ReadAll()
        {
            return recruiters.ReadAll();
        }

        public List<Recruiter> ReadByCompany(int companyId)
        {
            if (companyId <= 0) throw new ArgumentException("not a valid id.");
            var result = new List<Recruiter>();
            foreach (Recruiter r in recruiters.ReadAll())
                if (r.CompanyId == companyId) result.Add(r);
            return result;
        }

        public Constants.TriggerAlert RemoveById(int id)
        {
            if (id <= 0) return new Constants.TriggerAlert(false, "Not a valid id.");
            var recruiter = recruiters.ReadById(id);
            if (recruiter == null) return new Constants.TriggerAlert(false, "No recruiter matches this id.");
            // company has other recruiters that could manage related jobs.... should i reassign jobs then? update jobs with this recruiter's id to different recruiter of the same company? or just nullify jobs recruiterId? inconsistency...
            // but what if the company only has one recruiter? (realisticly, this shouldn't happen but never say never)
            // keep it as is for now... simple stupid
            recruitersEmail.Remove(recruiter.Email);
            recruiters.RemoveById(id);
            return new Constants.TriggerAlert(true, "Removed Successfully.");
        }
    }
}
