using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplicationTracker.Practice
{
    internal class CollectionsPractice
    {
        private void PracticeInsideAMethod()
        {
            /*
             * Notes figured out on my own:
             *      1. var doesn't work outside a function, aka every class element must be explicitly determined.
             *      2. searched for copy types (shallow copy and deep copy)
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
        
    }
}
