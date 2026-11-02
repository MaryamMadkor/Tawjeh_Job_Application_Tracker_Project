using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace JobApplicationTracker.Practice
{
    public class GenericClass<T> where T : class
    {
        public List<T> TypeList = new List<T>();
        public void Add(T obj)
        {
            if (obj == null) throw new ArgumentNullException("Not Found.");
            if (TypeList.Contains(obj))
            {
                throw new ArgumentException("Already Exists.");
            }
            else TypeList.Add(obj);
        }
    }
    internal class GenericsPractice
    {
        /*
            ==>> Generics
                - to maintain type safty, reusability and efficiency we use generics.
                - it accepts any data type or object and works with it the way it is assigned to.
         */

        public void print()
        {
            var names = new GenericClass<String>();
            names.Add("Hello, It's me!");
            names.Add("I was wondering if after all..");
            names.Add("Adele.");
            foreach (var name in names.TypeList)
            {
                Console.WriteLine(name);
            }
        }
    }
}
