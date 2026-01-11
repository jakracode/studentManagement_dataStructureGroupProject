using System;
using System.Collections.Generic;

namespace StudentManagementSystem.DataStructures
{
    /// <summary>
    /// Custom Hash Table Implementation using Chaining for Collision Handling
    /// TIME COMPLEXITY EXPLANATION:
    /// - Average Case: O(1) for Insert, Search, Delete operations
    /// - Worst Case: O(n) when all keys hash to same bucket (very rare with good hash function)
    /// - Compare to Linear Search in Array/List: Always O(n)
    /// </summary>
    /// <typeparam name="K">Key type</typeparam>
    /// <typeparam name="V">Value type</typeparam>
    public class MyHashTable<K, V>
    {
        // Node class for Linked List implementation (Chaining)
        private class HashNode
        {
            public K Key { get; set; }
            public V Value { get; set; }
            public HashNode Next { get; set; }

            public HashNode(K key, V value)
            {
                Key = key;
                Value = value;
                Next = null;
            }
        }

        private HashNode[] buckets;
        private int capacity;
        private int size;
        private const double LOAD_FACTOR_THRESHOLD = 0.75;

        /// <summary>
        /// Initialize Hash Table with default capacity
        /// </summary>
        public MyHashTable(int initialCapacity = 16)
        {
            capacity = initialCapacity;
            buckets = new HashNode[capacity];
            size = 0;
        }

        /// <summary>
        /// Custom Hash Function
        /// EXPLANATION: This function converts any key into an array index
        /// Using a prime number (31) helps distribute keys uniformly across buckets
        /// Modulo operation ensures index stays within array bounds
        /// </summary>
        private int GetHash(K key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            // Get hashcode from the key object
            int hash = key.GetHashCode();

            // Make hash positive and map to bucket index
            // Using capacity ensures index is within array bounds
            return Math.Abs(hash) % capacity;
        }

        /// <summary>
        /// Insert or Update a key-value pair
        /// TIME COMPLEXITY: O(1) average case
        /// - Calculate hash: O(1)
        /// - Find bucket: O(1)
        /// - Traverse chain in bucket: O(1) on average (assuming uniform distribution)
        /// </summary>
        public void Insert(K key, V value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            // Check if we need to resize
            if ((double)size / capacity >= LOAD_FACTOR_THRESHOLD)
            {
                Resize();
            }

            int bucketIndex = GetHash(key);
            HashNode head = buckets[bucketIndex];

            // Check if key already exists (update scenario)
            HashNode current = head;
            while (current != null)
            {
                if (current.Key.Equals(key))
                {
                    // Key exists, update value
                    current.Value = value;
                    return;
                }
                current = current.Next;
            }

            // Key doesn't exist, insert new node at the beginning of chain
            HashNode newNode = new HashNode(key, value);
            newNode.Next = head;
            buckets[bucketIndex] = newNode;
            size++;
        }

        /// <summary>
        /// Search for a value by key
        /// TIME COMPLEXITY: O(1) average case
        /// WHY O(1)? 
        /// 1. Hash function computes index instantly: O(1)
        /// 2. Direct array access to bucket: O(1)
        /// 3. Chain traversal: O(1) on average (few items per bucket with good hash function)
        /// 
        /// COMPARE TO LINEAR SEARCH:
        /// - Array/List search: Must check EVERY element until found = O(n)
        /// - Hash Table search: Jump directly to correct bucket = O(1)
        /// 
        /// Example: Finding 1 student among 10,000 students
        /// - List: Might need to check all 10,000 entries (worst case)
        /// - Hash Table: Check only ~1-2 entries in the correct bucket!
        /// </summary>
        public V Search(K key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            int bucketIndex = GetHash(key);
            HashNode current = buckets[bucketIndex];

            // Traverse the chain to find the key
            while (current != null)
            {
                if (current.Key.Equals(key))
                {
                    return current.Value;
                }
                current = current.Next;
            }

            // Key not found
            throw new KeyNotFoundException($"Key '{key}' not found in hash table.");
        }

        /// <summary>
        /// Try to get value without throwing exception
        /// </summary>
        public bool TryGetValue(K key, out V value)
        {
            try
            {
                value = Search(key);
                return true;
            }
            catch (KeyNotFoundException)
            {
                value = default(V);
                return false;
            }
        }

        /// <summary>
        /// Delete a key-value pair
        /// TIME COMPLEXITY: O(1) average case
        /// </summary>
        public bool Delete(K key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            int bucketIndex = GetHash(key);
            HashNode current = buckets[bucketIndex];
            HashNode previous = null;

            // Traverse the chain to find and remove the key
            while (current != null)
            {
                if (current.Key.Equals(key))
                {
                    // Found the key, remove it
                    if (previous == null)
                    {
                        // Removing head of chain
                        buckets[bucketIndex] = current.Next;
                    }
                    else
                    {
                        // Removing from middle/end of chain
                        previous.Next = current.Next;
                    }
                    size--;
                    return true;
                }
                previous = current;
                current = current.Next;
            }

            return false; // Key not found
        }

        /// <summary>
        /// Check if key exists
        /// </summary>
        public bool ContainsKey(K key)
        {
            if (key == null)
                return false;

            try
            {
                Search(key);
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Get all values in the hash table
        /// TIME COMPLEXITY: O(n) - must visit all entries
        /// </summary>
        public List<V> GetAllValues()
        {
            List<V> values = new List<V>();

            for (int i = 0; i < capacity; i++)
            {
                HashNode current = buckets[i];
                while (current != null)
                {
                    values.Add(current.Value);
                    current = current.Next;
                }
            }

            return values;
        }

        /// <summary>
        /// Get all key-value pairs
        /// </summary>
        public List<KeyValuePair<K, V>> GetAllEntries()
        {
            List<KeyValuePair<K, V>> entries = new List<KeyValuePair<K, V>>();

            for (int i = 0; i < capacity; i++)
            {
                HashNode current = buckets[i];
                while (current != null)
                {
                    entries.Add(new KeyValuePair<K, V>(current.Key, current.Value));
                    current = current.Next;
                }
            }

            return entries;
        }

        /// <summary>
        /// Resize the hash table when load factor exceeds threshold
        /// This maintains O(1) performance by preventing long chains
        /// </summary>
        private void Resize()
        {
            int newCapacity = capacity * 2;
            HashNode[] oldBuckets = buckets;

            buckets = new HashNode[newCapacity];
            capacity = newCapacity;
            size = 0;

            // Rehash all existing entries
            for (int i = 0; i < oldBuckets.Length; i++)
            {
                HashNode current = oldBuckets[i];
                while (current != null)
                {
                    Insert(current.Key, current.Value);
                    current = current.Next;
                }
            }
        }

        /// <summary>
        /// Get current size of hash table
        /// </summary>
        public int Size => size;

        /// <summary>
        /// Check if hash table is empty
        /// </summary>
        public bool IsEmpty => size == 0;

        /// <summary>
        /// Clear all entries
        /// </summary>
        public void Clear()
        {
            buckets = new HashNode[capacity];
            size = 0;
        }
    }
}
