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
        public void Add(T entity)
        {
            if (entity == null) throw new ArgumentException("Add Failed: Entity Inserted Doesn't Exist.");

            if (entity.Id == 0)
            {
                // new entity (empty) give it an id..
                entity.Id = nextId++;
            }
            else
            {
                // if id is given from user or by me .. check data validation..
                if (entity.Id < 0)
                    throw new ArgumentException("Add Failed: Id inserted not valid.");
                if (EntityList.ContainsKey(entity.Id))
                    throw new ArgumentException($"Add Failed: Id {entity.Id} already exists.");
                if (entity.Id >= nextId) // start from that new id..
                    nextId = entity.Id + 1;
            }

            EntityList.Add(entity.Id, entity);
        }

        public void Update(T entity)
        {
            if (entity == null) throw new ArgumentException("Update Failed: Entity Inserted Doesn't Exist.");
            if (entity.Id == 0) throw new ArgumentException("Update Failed: Entity Inserted Is Empty.");
            if (entity.Id <= 0) throw new ArgumentException("Update Failed: Id Is Not Valid.");
            if (!EntityList.ContainsKey(entity.Id)) throw new ArgumentException("Update Failed: Entity with inserted id doesn't exist.");

            EntityList[entity.Id] = entity;
        }

        public void RemoveById(int id)
        {
            if (id <= 0) throw new ArgumentException("Remove Failed: Inserted Id Is Not a Valid Id.");
            if (!EntityList.ContainsKey(id)) throw new ArgumentException("Remove Failed: Entity with inserted id doesn't exist.");

            EntityList.Remove(id);
        }

        public void Clear() { EntityList.Clear(); }

        public T? ReadById(int id)
        {
            if (id <= 0) return null;
            if (!EntityList.ContainsKey(id)) return null;
            return EntityList[id];
        }

        public List<T> ReadAll()
        {
            return EntityList.Values.ToList();
        }

        public bool Exists(int id)
        {
            if (id <= 0) return false;
            return EntityList.ContainsKey(id);
        }
        // no need for trigger alert here .. any error here is my problem ( the developer ) not the user...
        public void SaveToFile(string path, Func<T, string> serialize)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            using var writer = new StreamWriter(path);
            foreach (var item in EntityList.Values)
                writer.WriteLine(serialize(item));
        }

        public void LoadFromFile(string path, Func<string, T> deserialize)
        {
            if (!File.Exists(path)) return;
            using var reader = new StreamReader(path);
            string? line;
            while ((line = reader.ReadLine()) is not null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var entity = deserialize(line);
                EntityList[entity.Id] = entity;
                if (entity.Id >= nextId) nextId = entity.Id + 1;
            }
        }
    }
}