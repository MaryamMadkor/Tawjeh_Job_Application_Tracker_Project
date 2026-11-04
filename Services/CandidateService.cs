using JobApplicationTracker.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace JobApplicationTracker.Services
{
    internal class CandidateService
    {
        // add, update data by id, read all, read by id, remove
        public static DataStore<Candidate> candidateList = new();
        public static HashSet<string> candidatesEmail = new(StringComparer.OrdinalIgnoreCase);

        public Constants.TriggerAlert Add(string fullName, string email, string phoneNumber, int yearsOfExp) 
        {
            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phoneNumber) ||
                yearsOfExp < 0)
                return new Constants.TriggerAlert(false, "Invalid input.");
            if (!Regex.IsMatch(Constants.emailPattern, email)) return new Constants.TriggerAlert(false, "Not a valid Email.");
            if (candidatesEmail.Contains(email)) return new Constants.TriggerAlert(false, "This email already exist. A unique email is required.");

            Candidate candidate = new Candidate(fullName, email, phoneNumber, yearsOfExp);
            candidateList.Add(candidate);
            candidatesEmail.Add(email);
            return new Constants.TriggerAlert(true, "Added Successfully.");
        }

        public Constants.TriggerAlert UpdateDataById(int id, string? name, string? email, string? phone, int? yearsOfExp)
        {
            if (id <= 0) return new Constants.TriggerAlert(false, "not a valid id.");
            var cand = candidateList.ReadById(id);
            if (cand == null) return new Constants.TriggerAlert(false, "No candidate matches this id.");
            
            if (name == null) name = cand.FullName;
            if (email == null)
            {
                email = cand.Email;
            }
            else if (email != cand.Email)
            {
                if (!Regex.IsMatch(Constants.emailPattern, email))
                    return new Constants.TriggerAlert(false, "Not a valid email format.");
                
                if (candidatesEmail.Contains(email))
                    return new Constants.TriggerAlert(false, "This email already belongs to another candidate.");

                candidatesEmail.Remove(cand.Email);
                candidatesEmail.Add(email);
            }
            if (phone == null) phone = cand.Phone;
            if (yearsOfExp.HasValue && yearsOfExp.Value < 0 || yearsOfExp < cand.YearsOfExperience)
                return new Constants.TriggerAlert(false, "Years of experience not valid (can't be zero or less than old stated years of experience).");
            // meaning that if a user doesn't enter new data, keep original data and just update the changed data.
            cand.FullName = name;
            cand.Email = email; 
            cand.Phone = phone;
            cand.YearsOfExperience = yearsOfExp ?? cand.YearsOfExperience;
            candidateList.Update(cand);
            return new Constants.TriggerAlert(true, "Successfully Updated.");
        }

        public Candidate ReadById(int id)
        {
            if(id <= 0) throw new ArgumentOutOfRangeException("id is not valid.");
            var cand = candidateList.ReadById(id);
            if (cand == null) throw new ArgumentException("No candidate matches this id.");
            return cand;
        }

        public List<Candidate> ReadAll()
        {
            return candidateList.ReadAll();
        }

        public Constants.TriggerAlert RemoveById(int id)
        {
            if (id <= 0) return new Constants.TriggerAlert(false, "Not a valid id.");
            var cand = candidateList.ReadById(id);
            if (cand == null) return new Constants.TriggerAlert(false, "No candidate matches this id.");
            candidateList.RemoveById(id);
            candidatesEmail.Remove(cand.Email);
            // also all applications done by this candidate should be deleted? since we don't have a flag for active or inactive candidate.
            // keep it simple stupid for now.
            return new Constants.TriggerAlert(true, "Successfully Removed");
        }
    }
}
