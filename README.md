# Job Application Tracker

## Tawjeh Spring 1 Project
- Delivered:
  Fully functioning console application that tracks job applications and validates data and shows data cleanly.
  Authorization and Authentication (smaller version of it but Still is auth functionalities)
  `DataStore<T> where T : class, IEntity` — dictionary-backed, auto-assigns Ids, owns file I/O. Serialization lives in each service because the service knows the entity's shape. Side collections (`Stack` for undo, `HashSet<string>` for emails, `HashSet<(int,int)>` for duplicate applications) stay in sync with their owning service. Errors are `TriggerAlert(bool, string)` at the service boundary, exceptions at the store boundary.
  Repo Created | Repo created | First commit | File Structure | Web GUI of README.md file then pull it. | add .gitignore locally then push again.

## Learned

- `HashSet` default comparer is case-sensitive: emails could get duplicated and not notice it until I added `OrdinalIgnoreCase` (didn't know about it at first).
- Undo stack must be pushed BEFORE mutation. 
- To remove a stack member you might have to take it out in a temp stack then reverse it in a third final stack in the right (origianl intended) order.
- Before rewriting data in files, we must ensure we load data into the lists intended for them.

## Acknowledgement

I acknowledge that this might not have been the best i have ever created, but I fully intend to learn from my mistakes and fix that up next time and every other time.


## Git Workflow

```bash
git init
git remote add origin <url>
git add .
git commit -m "Update Models: Finalizing Models."
git push -u origin main
git push
git pull
git log --oneline
git tag sprint-1 && git push origin sprint-1
```

## Running the Project

### Ensure to change directory in Models/Constants to one that matches YOUR machine.

- Open the solution in Visual Studio.
- Run Program.cs (or dotnet run if you're building from the CLI).
- First launch: seed data is created and saved to Data/ on exit.
- Subsequent launches: data is loaded from Data/ on startup. Delete the folder to reset.
- Data files live under Data/ at the project root — one pipe-delimited text file per entity:

Data/
├── Companies.txt
├── Recruiters.txt
├── Jobs.txt
├── Candidates.txt
└── Applications.txt

Data/ is in .gitignore — the app creates it on first save. Delete it to reset to initiate data.