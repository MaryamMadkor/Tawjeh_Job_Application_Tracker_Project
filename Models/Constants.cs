using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplicationTracker.Models
{
    internal class Constants
    {
        public static readonly string emailPattern = "^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$";
        public record TriggerAlert(bool success, string message); // instead of throwing exceptions here and there, and i still get to know what is the problem and where.

        public static readonly string CompanyFilePath = "M:\\JobApplicationTracker\\Models\\Data\\Companies.txt";
        public static readonly string CandidateFilePath = "M:\\JobApplicationTracker\\Models\\Data\\Candidates.txt";
        public static readonly string RecruiterFilePath = "M:\\JobApplicationTracker\\Models\\Data\\Recruiters.txt";
        public static readonly string JobFilePath = "M:\\JobApplicationTracker\\Models\\Data\\Jobs.txt";
        public static readonly string ApplicationFilePath = "M:\\JobApplicationTracker\\Models\\Data\\Applications.txt";
    }
}
