using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplicationTracker.Models
{
    internal enum ApplicationStatus
    {
        Submitted = 0,
        UnderReview,
        Interview,
        Accepted,
        Rejected
    }
}
