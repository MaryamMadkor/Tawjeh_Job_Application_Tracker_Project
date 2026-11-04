using JobApplicationTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace JobApplicationTracker.Services
{
    internal class DataStore<T> where T : class, Models.IEntity
    {
        // Add
        // Update
        // Remove
        // Read by id
        // Read all
        // Exists? by id
        public Dictionary<int, T> EntityList = new Dictionary<int, T>();
        private int nextId = 1;
        public Constants.TriggerAlert Add(T entity)
        {
            if (entity == null) return new Constants.TriggerAlert(false, "Add Failed: Entity Inserted Doesn't Exist."); // maybe try to lessen the throw exceptions.
            if (entity.Id == 0) return new Constants.TriggerAlert(false, "Add Failed: Entity Inserted Is Empty.");
            if (entity.Id <= 0) return new Constants.TriggerAlert(false, "Add Failed: Id inserted not valid.");

            entity.Id = nextId++;
            EntityList.Add(entity.Id, entity);
            return new Constants.TriggerAlert(true, "Successfully Added!");
        }

        public Constants.TriggerAlert Update(T entity)
        {
            if (entity == null) return new Constants.TriggerAlert(false, "Update Failed: Entity Inserted Doesn't Exist.");
            if (entity.Id == 0) return new Constants.TriggerAlert(false, "Update Failed: Entity Inserted Is Empty.");
            if (entity.Id <= 0) return new Constants.TriggerAlert(false, "Update Failed: Id Is Not Valid.");
            if (!EntityList.ContainsKey(entity.Id)) return new Constants.TriggerAlert(false, "Update Failed: Entity with inserted id doesn't exist.");

            EntityList[entity.Id] = entity;
            return new Constants.TriggerAlert(true, "Successfully Updated!");
        }

        public Constants.TriggerAlert RemoveById(int id)
        {
            if (id <= 0) return new Constants.TriggerAlert(false, "Remove Failed: Inserted Id Is Not a Valid Id.");
            if (!EntityList.ContainsKey(id)) return new Constants.TriggerAlert(false, "Remove Failed: Entity with inserted id doesn't exist.");
            
            EntityList.Remove(id);
            return new Constants.TriggerAlert(true, "Successfully Removed!");
        }

        public void Clear() { EntityList.Clear(); }

        public T ReadById(int id)
        {
            if (id <= 0) throw new ArgumentException("Read By Id Failed: Inserted Id Is Not a Valid Id.");
            if (!EntityList.ContainsKey(id)) throw new ArgumentException("Read By Id Failed: Entity with inserted id doesn't exist.");
            return EntityList[id];
        }
        public List<T> ReadAll()
        {
            return EntityList.Values.ToList();
        }

        public bool Exists(int id)
        {
            if (id <= 0) throw new ArgumentException("Exist By Id Failed: Inserted Id Is Not a Valid Id.");
            if (!EntityList.ContainsKey(id)) return false;
            return true;
        }
    }
}
