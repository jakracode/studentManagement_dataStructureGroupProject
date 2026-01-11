using System;
using System.Security.Cryptography;
using System.Text;
using StudentManagementSystem.DataStructures;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Services
{
    /// <summary>
    /// Authentication Service using Custom Hash Table
    /// DEMONSTRATES: O(1) lookup time for user authentication
    /// </summary>
    public class AuthenticationService
    {
        // Hash Table to store users by Username as key
        // WHY USE HASH TABLE?
        // - Login requires finding user by username quickly
        // - With List: O(n) - must check every user until found
        // - With Hash Table: O(1) - instant lookup by username
        private readonly MyHashTable<string, SystemAdmin> _adminHashTable;

        public AuthenticationService()
        {
            _adminHashTable = new MyHashTable<string, SystemAdmin>(32);
        }

        /// <summary>
        /// Register a new admin user
        /// TIME COMPLEXITY: O(1)
        /// - Hash password: O(1) - fixed size input/output
        /// - Insert into hash table: O(1) average case
        /// </summary>
        public bool Register(string fullName, string username, string password)
        {
            // Check if username already exists - O(1) operation!
            if (_adminHashTable.ContainsKey(username))
            {
                return false; // Username already taken
            }

            // Hash the password using SHA-256
            string hashedPassword = HashPassword(password);

            // Create new admin
            var newAdmin = new SystemAdmin
            {
                FullName = fullName,
                Username = username,
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.Now
            };

            // Insert into hash table - O(1) operation!
            _adminHashTable.Insert(username, newAdmin);

            return true;
        }

        /// <summary>
        /// Login authentication
        /// TIME COMPLEXITY: O(1) - This is the KEY advantage!
        /// 
        /// EXPLANATION FOR YOUR EXAM:
        /// Traditional approach (List/Array):
        /// - Store all users in a list
        /// - For login, loop through EVERY user comparing username
        /// - Time: O(n) where n = number of users
        /// - Example: 1000 users might require 1000 comparisons!
        /// 
        /// Hash Table approach (This implementation):
        /// - Store users in hash table with username as key
        /// - For login, compute hash and jump directly to user
        /// - Time: O(1) - constant time, regardless of user count!
        /// - Example: 1000 or 1,000,000 users = SAME lookup speed!
        /// </summary>
        public SystemAdmin Login(string username, string password)
        {
            try
            {
                // Search for user in hash table - O(1) operation!
                // This is instant lookup - no looping through all users!
                SystemAdmin admin = _adminHashTable.Search(username);

                // Verify password
                string hashedPassword = HashPassword(password);
                if (admin.PasswordHash == hashedPassword)
                {
                    return admin; // Login successful
                }

                return null; // Invalid password
            }
            catch (Exception)
            {
                return null; // User not found
            }
        }

        /// <summary>
        /// Load existing admins from database into hash table
        /// This is called once at application startup
        /// TIME COMPLEXITY: O(n) - must load all admins once
        /// But subsequent lookups are O(1)!
        /// </summary>
        public void LoadAdminsIntoHashTable(List<SystemAdmin> admins)
        {
            foreach (var admin in admins)
            {
                // Insert each admin into hash table - O(1) per insert
                _adminHashTable.Insert(admin.Username, admin);
            }
        }

        /// <summary>
        /// Get admin by username
        /// TIME COMPLEXITY: O(1)
        /// </summary>
        public SystemAdmin GetAdminByUsername(string username)
        {
            try
            {
                return _adminHashTable.Search(username);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Check if username exists
        /// TIME COMPLEXITY: O(1)
        /// </summary>
        public bool UsernameExists(string username)
        {
            return _adminHashTable.ContainsKey(username);
        }

        /// <summary>
        /// Get all admins
        /// TIME COMPLEXITY: O(n)
        /// </summary>
        public List<SystemAdmin> GetAllAdmins()
        {
            return _adminHashTable.GetAllValues();
        }

        /// <summary>
        /// Update admin information
        /// TIME COMPLEXITY: O(1)
        /// </summary>
        public bool UpdateAdmin(SystemAdmin admin)
        {
            try
            {
                _adminHashTable.Insert(admin.Username, admin); // Insert updates if key exists
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Delete admin
        /// TIME COMPLEXITY: O(1)
        /// </summary>
        public bool DeleteAdmin(string username)
        {
            return _adminHashTable.Delete(username);
        }

        /// <summary>
        /// Hash password using SHA-256
        /// </summary>
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Get hash table statistics for demonstration
        /// Shows the power of hash tables for your exam presentation
        /// </summary>
        public string GetHashTableStats()
        {
            return $"Total Admins in Hash Table: {_adminHashTable.Size}\n" +
                   $"Lookup Time Complexity: O(1)\n" +
                   $"Compared to List Search: O(n)\n" +
                   $"Performance Gain: {_adminHashTable.Size}x faster for worst case!";
        }
    }
}
