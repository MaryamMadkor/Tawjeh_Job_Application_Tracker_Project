using System;
using JobApplicationTracker.Models;
using JobApplicationTracker.Services;

namespace JobApplicationTracker
{
    internal class Program
    {
        private static CompanyService companyService = new();
        private static RecruiterService recruiterService = null!;
        private static CandidateService candidateService = new();
        private static JobService jobService = null!;
        private static ApplicationService applicationService = null!;

        static void Main(string[] args)
        {
            // there should have been a ui namespace but we're letting that go for now.

            // the menu includes the entities and a sub-menu for each entity (add, update, list, remove - when eligable with business rules and authorized)
            // status are added on a stack of concequtive numbers aka status
            // entities are saved in a dictionary.
            // connect related services.
            recruiterService = new RecruiterService(companyService);
            applicationService = new ApplicationService(candidateService, companyService, recruiterService, null!);
            jobService = new JobService(companyService, recruiterService, applicationService);
            applicationService = new ApplicationService(candidateService, companyService, recruiterService, jobService); 

            // main function should only call functions and handle loops but not data and not ui...... lets make the functions for it here for now....
            LoadData();
            try
            {
                RunMainMenu();
            }
            finally
            {
                SaveAll();
            }
        }
        // for initial data load....
        private static void LoadData()
        {
            // if the files already have real data load it first before rewriting data in file...
            companyService.LoadAll();
            recruiterService.LoadAll();
            candidateService.LoadAll();
            jobService.LoadAll();
            applicationService.LoadAll();
            if (companyService.ReadAll().Count > 0) return;

            // 3 companies: all start with pending status, then i change two companies to approaved and keep company three pending.
            companyService.Add("AP Tech", "Technology", "Cairo");
            companyService.Add("CK Finance", "Finance", "Giza");
            companyService.Add("HI Health", "Healthcare", "Alexandria");
            companyService.ChangeStatus(1, CompanyStatus.Approved);
            companyService.ChangeStatus(2, CompanyStatus.Approved);

            // 3 recruiters belong to different companies
            recruiterService.Add("Hanaa Samir", "hanaa@acme.com", 1);
            recruiterService.Add("Omar Khaled", "omar@beta.com", 2);
            recruiterService.Add("Layla Nour", "layla@gamma.com", 3);

            // 6 random jobs for all APPROVED companies
            jobService.Add("Backend Developer", "C# and .NET", 12000, "Cairo", 1, 1);
            jobService.Add("Frontend Developer", "React", 10000, "Cairo", 1, 1);
            jobService.Add("Accountant", "Finance reporting", 9000, "Giza", 2, 2);
            jobService.Add("Risk Analyst", "Market risk", 11000, "Giza", 2, 2);
            jobService.Add("DevOps Engineer", "CI/CD", 14000, "Remote", 1, 1);
            jobService.Add("Data Analyst", "SQL and Excel", 8000, "Cairo", 1, 1);
            
            // 5 candidates for jobs randomly
            candidateService.Add("Ali Hassan", "ali@mail.com", "01000000001", 3);
            candidateService.Add("Mona Adel", "mona@mail.com", "01000000002", 5);
            candidateService.Add("Youssef Tarek", "youssef@mail.com", "01000000003", 1);
            candidateService.Add("Salma Fouad", "salma@mail.com", "01000000004", 7);
            candidateService.Add("Karim Nabil", "karim@mail.com", "01000000005", 2);

            SaveAll(); // save the fresh seed once, right after creating it — not after reloading over it.
        }

        // after adds or updates in main application.
        private static void SaveAll()
        {
            companyService.SaveAll();
            recruiterService.SaveAll();
            candidateService.SaveAll();
            jobService.SaveAll();
            applicationService.SaveAll();
        }

        // ------------------------------------------------------------------
        // Main menu
        // ------------------------------------------------------------------
        private static void RunMainMenu()
        {
            // could do it using enums and so on.. no time ... leave out the unnecessary hustles for now.... 
            bool exit = false;
            while (!exit)
            {
                Console.Clear(); // start clean on the console everytime....
                Console.WriteLine("=== Job Application Tracker ===");
                Console.WriteLine("1. Companies");
                Console.WriteLine("2. Recruiters");
                Console.WriteLine("3. Jobs");
                Console.WriteLine("4. Candidates");
                Console.WriteLine("5. Applications");
                Console.WriteLine("0. Exit");
                Console.Write("Choice: ");

                switch (ReadInt())
                {
                    case 1: CompanyMenu(); break;
                    case 2: RecruiterMenu(); break;
                    case 3: JobMenu(); break;
                    case 4: CandidateMenu(); break;
                    case 5: ApplicationMenu(); break;
                    case 0: exit = true; break;
                    default: DisplayAlert("Invalid choice."); break;
                }
            }
        }

        // when choice is company... enter company menu
        private static void CompanyMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== Companies ===");
                Console.WriteLine("1. List all");
                Console.WriteLine("2. List by status");
                Console.WriteLine("3. Add");
                Console.WriteLine("4. Update");
                Console.WriteLine("5. Change status (approve / reject)");
                Console.WriteLine("6. Undo last status change");
                Console.WriteLine("7. Remove");
                Console.WriteLine("0. Back");
                Console.Write("Choice: ");

                switch (ReadInt())
                {
                    case 1: ListCompanies(companyService.ReadAll()); break;
                    case 2: ListCompaniesByStatus(); break;
                    case 3: AddCompany(); break;
                    case 4: UpdateCompany(); break;
                    case 5: ChangeCompanyStatus(); break;
                    case 6: UndoCompanyStatus(); break;
                    case 7: RemoveCompany(); break;
                    case 0: back = true; break;
                    default: DisplayAlert("Invalid choice."); break;
                }
            }
        }
        // ui spcified for company
        private static void ListCompanies(List<Company> list)
        {
            Console.Clear();
            Console.WriteLine($"{"Id",-4} {"Name",-20} {"Industry",-15} {"City",-15} {"Status",-10}");
            Console.WriteLine(new string('-', 70));
            foreach (var c in list)
                Console.WriteLine($"{c.Id,-4} {c.Name,-20} {c.Industry,-15} {c.City,-15} {c.Status,-10}");
            DisplayAlert("");
        }

        private static void ListCompaniesByStatus()
        {
            Console.Write("Status (0=Pending, 1=Approved, 2=Rejected): ");
            var s = ReadInt();
            if (s < 0 || s > 2) { DisplayAlert("Invalid status."); return; }
            ListCompanies(companyService.ReadByStatus((CompanyStatus)s));
        }

        private static void AddCompany()
        {
            Console.Clear();
            var name = ReadString("Name: ");
            var industry = ReadString("Industry: ");
            var city = ReadString("City: ");
            var res = companyService.Add(name, industry, city);
            DisplayAlert(res.message);
        }

        private static void UpdateCompany()
        {
            Console.Clear();
            var id = ReadInt("Company Id: ");
            var name = ReadString("New name (blank = keep): ");
            var industry = ReadString("New industry (blank = keep): ");
            var city = ReadString("New city (blank = keep): ");
            var res = companyService.UpdateById(
                id,
                string.IsNullOrWhiteSpace(name) ? null : name,
                string.IsNullOrWhiteSpace(industry) ? null : industry,
                string.IsNullOrWhiteSpace(city) ? null : city);
            DisplayAlert(res.message);
        }

        private static void ChangeCompanyStatus()
        {
            Console.Clear();
            var id = ReadInt("Company Id: ");
            Console.Write("New status (1=Approved, 2=Rejected): ");
            var s = ReadInt();
            if (s != 1 && s != 2) { DisplayAlert("Invalid status."); return; }
            var res = companyService.ChangeStatus(id, (CompanyStatus)s);
            DisplayAlert(res.message);
        }

        private static void UndoCompanyStatus()
        {
            Console.Clear();
            var res = companyService.UndoLastStatusChange();
            DisplayAlert(res.message);
        }

        private static void RemoveCompany()
        {
            Console.Clear();
            var id = ReadInt("Company Id: ");
            var res = companyService.RemoveById(id);
            DisplayAlert(res.message);
        }

        // then recruiter menu
        private static void RecruiterMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== Recruiters ===");
                Console.WriteLine("1. List all");
                Console.WriteLine("2. List by company");
                Console.WriteLine("3. Add");
                Console.WriteLine("4. Update");
                Console.WriteLine("5. Remove");
                Console.WriteLine("0. Back");
                Console.Write("Choice: ");

                switch (ReadInt())
                {
                    case 1: ListRecruiters(recruiterService.ReadAll()); break;
                    case 2:
                        ListRecruiters(recruiterService.ReadByCompany(ReadInt("Company Id: ")));
                        break;
                    case 3: AddRecruiter(); break;
                    case 4: UpdateRecruiter(); break;
                    case 5: RemoveRecruiter(); break;
                    case 0: back = true; break;
                    default: DisplayAlert("Invalid choice."); break;
                }
            }
        }

        private static void ListRecruiters(List<Recruiter> list)
        {
            Console.Clear();
            Console.WriteLine($"{"Id",-4} {"Name",-20} {"Email",-25} {"CompanyId",-10}");
            Console.WriteLine(new string('-', 62));
            foreach (var r in list)
                Console.WriteLine($"{r.Id,-4} {r.Name,-20} {r.Email,-25} {r.CompanyId,-10}");
            DisplayAlert("");
        }

        private static void AddRecruiter()
        {
            Console.Clear();
            var name = ReadString("Name: ");
            var email = ReadString("Email: ");
            var companyId = ReadInt("Company Id: ");
            var res = recruiterService.Add(name, email, companyId);
            DisplayAlert(res.message);
        }

        private static void UpdateRecruiter()
        {
            Console.Clear();
            var id = ReadInt("Recruiter Id: ");
            var name = ReadString("New name (blank = keep old): ");
            var email = ReadString("New email (blank = keep old): ");
            var res = recruiterService.UpdateById(
                id,
                string.IsNullOrWhiteSpace(name) ? null : name,
                string.IsNullOrWhiteSpace(email) ? null : email
                );
            DisplayAlert(res.message);
        }

        private static void RemoveRecruiter()
        {
            Console.Clear();
            var id = ReadInt("Recruiter Id: ");
            var res = recruiterService.RemoveById(id);
            DisplayAlert(res.message);
        }

        // Job menu
        private static void JobMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== Jobs ===");
                Console.WriteLine("1. List all");
                Console.WriteLine("2. List by status (open/closed)");
                Console.WriteLine("3. List by company");
                Console.WriteLine("4. List by recruiter");
                Console.WriteLine("5. Add (acting recruiter)");
                Console.WriteLine("6. Update (acting recruiter)");
                Console.WriteLine("7. Remove (acting recruiter)");
                Console.WriteLine("0. Back");
                Console.Write("Choice: ");

                switch (ReadInt())
                {
                    case 1: ListJobs(jobService.ReadAll()); break;
                    case 2: ListJobsByStatus(); break;
                    case 3: ListJobs(jobService.ReadByCompany(ReadInt("Company Id: "))); break;
                    case 4: ListJobs(jobService.ReadByRecruiter(ReadInt("Recruiter Id: "))); break;
                    case 5: AddJob(); break;
                    case 6: UpdateJob(); break;
                    case 7: RemoveJob(); break;
                    case 0: back = true; break;
                    default: DisplayAlert("Invalid choice."); break;
                }
            }
        }

        private static void ListJobs(List<Job> list)
        {
            Console.Clear();
            Console.WriteLine($"{"Id",-4} {"Title",-22} {"Salary",-10} {"Location",-12} {"Open",-6} {"Comp",-5} {"Rec",-4}");
            Console.WriteLine(new string('-', 75));
            foreach (var j in list)
                Console.WriteLine($"{j.Id,-4} {j.Title,-22} {j.Salary,-10} {j.Location,-12} {(j.IsOpen ? "Yes" : "No"),-6} {j.CompanyId,-5} {j.RecruiterId,-4}");
            DisplayAlert("");
        }

        private static void ListJobsByStatus()
        {
            Console.Write("Show open? (1=yes, 0=closed): ");
            var v = ReadInt();
            ListJobs(jobService.ReadByStatus(v == 1));
        }

        private static void AddJob()
        {
            Console.Clear();
            var title = ReadString("Title: ");
            var desc = ReadString("Description: ");
            var salary = ReadDouble("Salary: ");
            var location = ReadString("Location: ");
            var companyId = ReadInt("Company Id: ");
            var recruiterId = ReadInt("Acting Recruiter Id: ");
            var res = jobService.Add(title, desc, salary, location, companyId, recruiterId);
            DisplayAlert(res.message);
        }

        private static void UpdateJob()
        {
            Console.Clear();
            var id = ReadInt("Job Id: ");
            var recruiterId = ReadInt("Acting Recruiter Id: ");
            var title = ReadString("New title (blank = keep): ");
            var desc = ReadString("New description (blank = keep): ");
            var salaryRaw = ReadString("New salary (blank = keep): ");
            double? salary = null;
            if (!string.IsNullOrWhiteSpace(salaryRaw))
            {
                if (double.TryParse(salaryRaw, out var s)) salary = s;
                else { DisplayAlert("Invalid salary."); return; }
            }
            var openRaw = ReadString("Open? (1=yes, 0=no, blank = keep): ");
            bool? isOpen = null;
            if (openRaw == "1") isOpen = true;
            else if (openRaw == "0") isOpen = false;
            var location = ReadString("New location (blank = keep): ");

            var res = jobService.UpdateById(
                id,
                string.IsNullOrWhiteSpace(title) ? null : title,
                string.IsNullOrWhiteSpace(desc) ? null : desc,
                salary, isOpen,
                string.IsNullOrWhiteSpace(location) ? null : location,
                recruiterId);
            DisplayAlert(res.message);
        }

        private static void RemoveJob()
        {
            Console.Clear();
            var id = ReadInt("Job Id: ");
            var recruiterId = ReadInt("Acting Recruiter Id: ");
            var res = jobService.RemoveById(id, recruiterId);
            DisplayAlert(res.message);
        }

        // Candidate menu
        private static void CandidateMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== Candidates ===");
                Console.WriteLine("1. List all");
                Console.WriteLine("2. Add");
                Console.WriteLine("3. Update");
                Console.WriteLine("4. Remove");
                Console.WriteLine("0. Back");
                Console.Write("Choice: ");

                switch (ReadInt())
                {
                    case 1: ListCandidates(candidateService.ReadAll()); break;
                    case 2: AddCandidate(); break;
                    case 3: UpdateCandidate(); break;
                    case 4: RemoveCandidate(); break;
                    case 0: back = true; break;
                    default: DisplayAlert("Invalid choice."); break;
                }
            }
        }

        private static void ListCandidates(List<Candidate> list)
        {
            Console.Clear();
            Console.WriteLine($"{"Id",-4} {"FullName",-20} {"Email",-25} {"Phone",-15} {"YoE",-4}");
            Console.WriteLine(new string('-', 70));
            foreach (var c in list)
                Console.WriteLine($"{c.Id,-4} {c.FullName,-20} {c.Email,-25} {c.Phone,-15} {c.YearsOfExperience,-4}");
            DisplayAlert("");
        }

        private static void AddCandidate()
        {
            Console.Clear();
            var name = ReadString("Full name: ");
            var email = ReadString("Email: ");
            var phone = ReadString("Phone: ");
            var yoe = ReadInt("Years of experience: ");
            var res = candidateService.Add(name, email, phone, yoe);
            DisplayAlert(res.message);
        }

        private static void UpdateCandidate()
        {
            Console.Clear();
            var id = ReadInt("Candidate Id: ");
            var name = ReadString("New name (blank = keep): ");
            var email = ReadString("New email (blank = keep): ");
            var phone = ReadString("New phone (blank = keep): ");
            var yoeRaw = ReadString("New years of experience (blank = keep): ");
            int? yoe = null;
            if (!string.IsNullOrWhiteSpace(yoeRaw))
            {
                if (int.TryParse(yoeRaw, out var y)) yoe = y;
                else { DisplayAlert("Invalid YoE."); return; }
            }
            var res = candidateService.UpdateById(
                id,
                string.IsNullOrWhiteSpace(name) ? null : name,
                string.IsNullOrWhiteSpace(email) ? null : email,
                string.IsNullOrWhiteSpace(phone) ? null : phone,
                yoe);
            DisplayAlert(res.message);
        }

        private static void RemoveCandidate()
        {
            Console.Clear();
            var id = ReadInt("Candidate Id: ");
            var res = candidateService.RemoveById(id);
            DisplayAlert(res.message);
        }

        // Application menu
        private static void ApplicationMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== Applications ===");
                Console.WriteLine("1. List all");
                Console.WriteLine("2. List by job");
                Console.WriteLine("3. List by candidate");
                Console.WriteLine("4. List by company");
                Console.WriteLine("5. Apply");
                Console.WriteLine("6. Change status (acting recruiter)");
                Console.WriteLine("7. Remove (acting recruiter)");
                Console.WriteLine("0. Back");
                Console.Write("Choice: ");

                switch (ReadInt())
                {
                    case 1: ListApplications(applicationService.ReadAll()); break;
                    case 2: ListApplications(applicationService.ReadByJob(ReadInt("Job Id: "))); break;
                    case 3: ListApplications(applicationService.ReadByCandidate(ReadInt("Candidate Id: "))); break;
                    case 4: ListApplications(applicationService.ReadByCompany(ReadInt("Company Id: "))); break;
                    case 5: Apply(); break;
                    case 6: ChangeApplicationStatus(); break;
                    case 7: RemoveApplication(); break;
                    case 0: back = true; break;
                    default: DisplayAlert("Invalid choice."); break;
                }
            }
        }

        private static void ListApplications(List<Application> list)
        {
            Console.Clear();
            Console.WriteLine($"{"Id",-4} {"JobId",-6} {"CandId",-7} {"Applied",-20} {"Status",-14}");
            Console.WriteLine(new string('-', 55));
            foreach (var a in list)
                Console.WriteLine($"{a.Id,-4} {a.JobId,-6} {a.CandidateId,-7} {a.AppliedTime,-20:yyyy-MM-dd HH:mm} {a.applictionStatus,-14}");
            DisplayAlert("");
        }

        private static void Apply()
        {
            Console.Clear();
            var jobId = ReadInt("Job Id: ");
            var candidateId = ReadInt("Candidate Id: ");
            var res = applicationService.Apply(jobId, candidateId);
            DisplayAlert(res.message);
        }

        private static void ChangeApplicationStatus()
        {
            Console.Clear();
            var id = ReadInt("Application Id: ");
            var recruiterId = ReadInt("Acting Recruiter Id: ");
            Console.WriteLine("Status: 0=Submitted, 1=UnderReview, 2=Interview, 3=Accepted, 4=Rejected");
            var s = ReadInt("New status: ");
            if (s < 0 || s > 4) { DisplayAlert("Invalid status."); return; }
            var res = applicationService.ChangeStatus(id, (ApplicationStatus)s, recruiterId);
            DisplayAlert(res.message);
        }

        private static void RemoveApplication()
        {
            Console.Clear();
            var id = ReadInt("Application Id: ");
            var recruiterId = ReadInt("Acting Recruiter Id: ");
            var res = applicationService.RemoveById(id, recruiterId);
            DisplayAlert(res.message);
        }

        // those are input and output helper (should be private in a ui space but .. well)
        private static int ReadInt(string prompt = "")
        {
            while (true)
            {
                if (!string.IsNullOrEmpty(prompt)) Console.Write(prompt);
                var raw = Console.ReadLine();
                if (int.TryParse(raw, out var v)) return v;
                Console.WriteLine("Please enter a valid integer.");
            }
        }

        private static double ReadDouble(string prompt = "")
        {
            while (true)
            {
                if (!string.IsNullOrEmpty(prompt)) Console.Write(prompt);
                var raw = Console.ReadLine();
                if (double.TryParse(raw, out var v)) return v;
                Console.WriteLine("Please enter a valid number.");
            }
        }

        private static string ReadString(string prompt = "")
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? "";
        }

        private static void DisplayAlert(string message)
        {
            if (!string.IsNullOrEmpty(message)) Console.WriteLine(message);
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}