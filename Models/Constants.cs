using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplicationTracker.Models
{
    internal class Constants
    {
        public static readonly string emailPattern = "^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$";
        public record TriggerAlert(bool success, string message); // instead of throwing exceptions here and there, and i still get to know what is the problem and where.


    }
}
