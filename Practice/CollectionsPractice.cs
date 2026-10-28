using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplicationTracker.Practice
{
    internal class CollectionsPractice
    {
        private void ListPracticeNotes()
        {
            /*
             * Notes figured out on my own:
             *      1. var doesn't work outside a function, aka every class element must be explicitly determined.
             *      2. searched for copy types (shallow copy and deep copy) - almost all functions shallow copy data. (json serialization is deep copy)
             *      3. watched more videos regarding the list
                => Practice List<T>:
                    - adjustable on its own, shrinks and expands
                    - can be iterated upon using an iterator: for loop or a foreach loop for access or printing.
                    - .Add, .Count, .Remove takes out the first occurance only,
                    - .CopyTo(arrayPointerName, the index it starts from the list) copies all until one of their capacities end.
                    - .GetRange(index to start from, index to end at) copies the list to another list.
                    - case sensitive and can change any element normally with its index
                    - can declared with an old list and it copies all data to the next list
                    - .Sort(), .Reverse() ** reverses WITHOUT sorting it backwards. just reversed.
                    ---->>> without LINQ, to sort objects of a class it must inheret and implement the CompareTo interface inside it beforehand explicitly detailing what these objects are compared upon
                    - arrays are fixed sized data like a pic pixels, memory efficient and fast on the absolute contrary of list. flixable, slower and works with very large datasets that change regularly
                    - Enums can be cast into a list to using cast and toList or as IEnumrable after Enum.GetValues(typeof(EnumName))
            */
            //Add:
            List<string> countryList = new List<string>();
            List<string> employeesList = new List<string>() { "Manar Samy", "Somaia Kareem"};
            var employees = new List<string>() { "hello" };
            countryList.Add("USA");
            countryList.Add("Egypt");
            countryList.Add("Russia");
            countryList.Add("Japan");

            //Access Or Display
            Console.WriteLine("List Count = " + countryList.Count);
            Console.WriteLine("============================================");
            for(int i = 0; i < countryList.Count; i++ )
            {
                Console.WriteLine((i+1) + ". " + countryList[i]);
                Console.WriteLine("------");
            }
            Console.WriteLine("============================================");
            foreach(string country in countryList)
            {
                Console.WriteLine(country);
                Console.WriteLine("------");
            }

            //Remove
            countryList.Remove("USA"); // removes first occurance only
            // .Clear takes it all out.

            //Search
            for(int i = 0;i < countryList.Count;i++)
            {
                if(countryList[i] == "Egypt")
                {
                    Console.WriteLine("Egypt Found at: " + i);
                    break;
                }
                if(i  == countryList.Count-1 && countryList[i] != "Egypt")
                {
                    Console.WriteLine("Not Found.");
                }
            }
            countryList.Sort();
            foreach(string country in countryList)
            {
                if( country == "Egypt")
                {
                    Console.WriteLine("Found");
                    break;
                }
                else
                {
                    if (country[0] > 'E')
                    {
                        Console.WriteLine("Not Found.");
                        break;
                    }
                }
            }

            //Update
            countryList[2] = "China";
        }
        private void DictionaryPracticeNotes()
        {
            /*
             
            - Dictionary: generic storage for <key, value> pairs a list of them.
            - key must be unique, no duplicate keys, can not null
            - types must e identified and maintained throughout the code
            - capacity of the dictionary is the number of pairs it can hold.
             */

            // Declaration and Add and Access 
            Dictionary<string, string> capitals = new Dictionary<string, string>();
            Dictionary<int, string> employees = new Dictionary<int, string>()
            {
                {10, "Hanaa" },
                {23, "Samir" },
                {22, "Amira" }
            };
            var exchangeRate = new Dictionary<string, float>()
            {
                ["USD"] = 56.23f,
                ["EUR"] = 34.54f
            };
            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.Value}");
            }
            Console.WriteLine("===============================");
            capitals.Add("Egypt", "Cairo");
            capitals.Add("Germany", "Berlin");

            var newCapitals = new Dictionary<string, string>(capitals);
            var newEmployees = employees.ToDictionary(e => e.Key, e => e.Value);


            // Search and Update
            int index = Array.IndexOf(capitals.Keys.ToArray(), "Egypt");
            capitals["Egypt"] = "Al-Qaherah"; // can only be accessed by the key.. if ask by value gives error
            // if key doesn't exist = error (duh) => use tryGetKey()
            Console.WriteLine(capitals.TryGetValue("USA", out string? newCountry));
            // or check if key or value exists ContainsKey, ContainsValue : true or false
            Console.WriteLine(capitals.ContainsKey("USA"));

            // Remove and Clear
            employees.Remove(22); // key
            exchangeRate.Clear(); // delete all keys and values

            // Sort
            var sortedEmployees = employees.OrderBy(e => e.Key);
             // or OrderByDescending();

            //convert to string
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var capital in capitals)
            {
                stringBuilder.Append($"{capital.Key}: {capital.Value}\n");
            }
            string finalString = stringBuilder.ToString();

            Console.WriteLine(finalString);
        }
    }
}
