using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JobApplicationTracker.Models
{
    internal class Application : IEntity
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public int CandidateId { get; set; }
        public DateTime AppliedTime { get; set; } = DateTime.Now;
        public ApplicationStatus applictionStatus { get; set; } = ApplicationStatus.Submitted;

        public Application(int jobId, int candidateId)
        {
            JobId = jobId;
            CandidateId = candidateId;
        }
    }
}
