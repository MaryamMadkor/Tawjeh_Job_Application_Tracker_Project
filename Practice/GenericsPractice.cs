using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

/*
 * Generics practice — constraints and IEntity
 *
 * where T : class    → T must be a reference type. Meaning it stops value types like
 *                      DataStore<int>.
 *
 * where T : IEntity   → T must implement IEntity (int Id { get; set; }).
 *                       This is the constraint that makes GetById and
 *                       auto-Id assignment possible without dynamic or
 *                       reflection. Every T is guaranteed to have an Id,
 *                       so the DataStore stays generic without losing type safety.
 *
 * Why IEntity specifically:
 *   - It is the minimum property that every entity has.
 *   - It keeps DataStore<T> generic and doesn't know of Company / Job / Candidate other properties.
 */

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
