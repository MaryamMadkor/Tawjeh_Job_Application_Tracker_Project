Workshop Requirements
Read each requirement before writing code. For every feature, discuss:
Actor
Inputs
Output
Validations
Business rules
At least 3 Given / When / Then acceptance criteria
Which collection would you choose, and why?
What is the expected time complexity?
________________________________________________________________________
Requirement 1 — Find a Job
A user can search for a job using its ID. If a job with that ID exists, the system returns its details. Otherwise, the system reports that the job was not found.
Actor: Any User
Inputs: job id
Output: job details or if doesn't exist "not found"
Validations: id > 0
Business rules: ..same..? i don't know ...?
At least 3 Given / When / Then acceptance criteria: 
	given i have three jobs, when i search 2, thenn it should return its data
Which collection would you choose, and why? : use dictionary because its faster in searching and getting value
What is the expected time complexity?: O(1)

________________________________________________________________________
Requirement 2 — List Companies by Status
A user can request companies with a particular status. The system returns all companies whose status matches the requested status. If none match, the result is empty.

Actor: Any user
Inputs: Company Status
Output: list of companies that match input status
Validations: status must be a valid one.
Business rules: can't choose one that is running currently.
At least 3 Given / When / Then acceptance criteria: Given five approved companies, when i list rejected, then the return list should be empty.
Which collection would you choose, and why?: dictionary and lists work but i choose to save data into a dictionay and return it in a list.
What is the expected time complexity?: search O(1) dictionay, access and indexing for looping O(1) for list.
_________________________________________________________________________
Requirement 3 — Apply to a Job
A candidate can apply to a job by providing their candidate ID and the job ID.
The candidate and job must exist. The job must be open, and its company must have Approved status. A candidate cannot apply to the same job more than once.
If any rule fails, the system must explain why the application was rejected. A rejected attempt must not be saved. If all rules pass, the application is saved and the system reports success.

Actor : Candidate
Inputs : Candidate id, job id
Output: TriggerAlert
Validations: candidate exists, job exists, job is open, company is approved, no duplicate applications.
Business rules: no duplicate applications, job must be open, company must be approved, rejected attemps are not saved.
At least 3 Given / When / Then acceptance criteria : Given a candidate applied to job 5, when he applies again, then it should be reject it.
Which collection would you choose, and why?: hashset for duplicate checking
What is the expected time complexity?: O(1)
_________________________________________________________________________
Requirement 4 — Check Email Availability

A user can check whether an email address is available for a new candidate. Email comparison is case-insensitive. The result is available only if no existing candidate uses that email.
Sample Data Notes
The sample data intentionally includes open and closed jobs, companies with different statuses, and an existing application. Use these cases to explore successful and unsuccessful scenarios.


Actor: Any user
Inputs: Email
Output: Available or not
Validations: no empty emails, valid format for emails (regex patterns)
Business rules: comparision is case-insensitive.
At least 3 Given / When / Then acceptance criteria: Given an email "fadyhady@mail.com" exists, when I check for "FADY@mail.com", then it should see it as a duplicate and reject the transaction.
Which collection would you choose, and why?: Hashset for uniqness. using ordinal ignore case for case-insensitivty.
What is the expected time complexity?: O(1)