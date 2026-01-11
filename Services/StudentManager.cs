using System;
using System.Collections.Generic;
using StudentManagementSystem.DataStructures;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Services
{
    /// <summary>
    /// Student Manager using Custom Hash Table
    /// DEMONSTRATES: Why Hash Tables are superior for data retrieval
    /// </summary>
    public class StudentManager
    {
        // Hash Table to store students by StudentID as key
        // KEY CONCEPT FOR EXAM:
        // StudentID is unique, making it perfect as a hash key
        // This enables O(1) lookup instead of O(n) search
        private readonly MyHashTable<int, Student> _studentHashTable;

        public StudentManager()
        {
            _studentHashTable = new MyHashTable<int, Student>(64);
        }

        /// <summary>
        /// Add a new student
        /// TIME COMPLEXITY: O(1)
        /// </summary>
        public bool AddStudent(Student student)
        {
            try
            {
                // Check if student already exists
                if (_studentHashTable.ContainsKey(student.StudentID))
                {
                    return false; // Student ID already exists
                }

                // Insert into hash table - O(1) operation
                _studentHashTable.Insert(student.StudentID, student);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Find student by ID
        /// TIME COMPLEXITY: O(1) vs O(n) for List
        /// 
        /// CRITICAL EXPLANATION FOR YOUR EXAM:
        /// 
        /// Scenario: Find student with ID 12345 among 10,000 students
        /// 
        /// Method 1 - Using List (Linear Search):
        /// ----------------------------------------
        /// foreach (var student in studentList)  // Must check each student
        /// {
        ///     if (student.StudentID == 12345)   // Compare with each ID
        ///         return student;
        /// }
        /// - Average: Check 5,000 students (half the list)
        /// - Worst case: Check all 10,000 students
        /// - Time Complexity: O(n) where n = 10,000
        /// - Operations: Up to 10,000 comparisons!
        /// 
        /// Method 2 - Using Hash Table (This Implementation):
        /// --------------------------------------------------
        /// int index = GetHash(12345);           // Calculate hash: O(1)
        /// return buckets[index];                // Direct access: O(1)
        /// 
        /// - Always: Check ~1-2 students (in same bucket)
        /// - Worst case: Check few students in one bucket
        /// - Time Complexity: O(1) constant time
        /// - Operations: Only 1-2 comparisons!
        /// 
        /// RESULT: Hash Table is ~5,000x faster on average!
        /// </summary>
        public Student FindStudentById(int studentId)
        {
            try
            {
                // Direct hash lookup - O(1) operation!
                // No looping through all students needed!
                return _studentHashTable.Search(studentId);
            }
            catch
            {
                return null; // Student not found
            }
        }

        /// <summary>
        /// Update student information
        /// TIME COMPLEXITY: O(1)
        /// </summary>
        public bool UpdateStudent(Student student)
        {
            try
            {
                // Check if student exists first
                if (!_studentHashTable.ContainsKey(student.StudentID))
                {
                    return false; // Student doesn't exist
                }

                // Update (Insert with existing key updates the value) - O(1)
                _studentHashTable.Insert(student.StudentID, student);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Delete student
        /// TIME COMPLEXITY: O(1)
        /// </summary>
        public bool DeleteStudent(int studentId)
        {
            // Delete from hash table - O(1) operation
            return _studentHashTable.Delete(studentId);
        }

        /// <summary>
        /// Get all students
        /// TIME COMPLEXITY: O(n) - must retrieve all students
        /// This is acceptable because we need all data
        /// The advantage is in single-student lookups!
        /// </summary>
        public List<Student> GetAllStudents()
        {
            return _studentHashTable.GetAllValues();
        }

        /// <summary>
        /// Check if student exists
        /// TIME COMPLEXITY: O(1)
        /// </summary>
        public bool StudentExists(int studentId)
        {
            return _studentHashTable.ContainsKey(studentId);
        }

        /// <summary>
        /// Load students from database into hash table
        /// Called once at application startup
        /// TIME COMPLEXITY: O(n) - one-time operation
        /// 
        /// IMPORTANT FOR EXAM:
        /// Yes, loading takes O(n) time initially
        /// BUT every subsequent search is O(1)!
        /// 
        /// Trade-off Analysis:
        /// - Load once: O(n) = 10,000 operations
        /// - Search 1000 times with List: O(n) × 1000 = 10,000,000 operations
        /// - Search 1000 times with Hash Table: O(1) × 1000 = 1,000 operations
        /// 
        /// Conclusion: Hash Table wins after just 2 searches!
        /// </summary>
        public void LoadStudentsIntoHashTable(List<Student> students)
        {
            foreach (var student in students)
            {
                _studentHashTable.Insert(student.StudentID, student);
            }
        }

        /// <summary>
        /// Search students by name (requires O(n) - must check all)
        /// NOTE: Hash tables are optimized for key-based lookup
        /// For non-key searches, we still need to check all entries
        /// </summary>
        public List<Student> SearchStudentsByName(string name)
        {
            var results = new List<Student>();
            var allStudents = _studentHashTable.GetAllValues();

            foreach (var student in allStudents)
            {
                if (student.StudentName.Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(student);
                }
            }

            return results;
        }

        /// <summary>
        /// Get students by gender
        /// TIME COMPLEXITY: O(n) - must check all students
        /// </summary>
        public List<Student> GetStudentsByGender(string gender)
        {
            var results = new List<Student>();
            var allStudents = _studentHashTable.GetAllValues();

            foreach (var student in allStudents)
            {
                if (student.Gender?.Equals(gender, StringComparison.OrdinalIgnoreCase) == true)
                {
                    results.Add(student);
                }
            }

            return results;
        }

        /// <summary>
        /// Get total student count
        /// TIME COMPLEXITY: O(1)
        /// </summary>
        public int GetStudentCount()
        {
            return _studentHashTable.Size;
        }

        /// <summary>
        /// Clear all students
        /// TIME COMPLEXITY: O(1)
        /// </summary>
        public void Clear()
        {
            _studentHashTable.Clear();
        }

        /// <summary>
        /// Get performance comparison for demonstration
        /// Use this in your exam presentation!
        /// </summary>
        public string GetPerformanceComparison()
        {
            int studentCount = _studentHashTable.Size;

            return $"=== PERFORMANCE COMPARISON ===\n\n" +
                   $"Total Students: {studentCount}\n\n" +
                   $"HASH TABLE (This Implementation):\n" +
                   $"- Find by ID: O(1) = ~1 operation\n" +
                   $"- Add Student: O(1) = ~1 operation\n" +
                   $"- Delete Student: O(1) = ~1 operation\n\n" +
                   $"LIST/ARRAY (Traditional Method):\n" +
                   $"- Find by ID: O(n) = {studentCount} operations (worst case)\n" +
                   $"- Add Student: O(1) = ~1 operation\n" +
                   $"- Delete Student: O(n) = {studentCount} operations (worst case)\n\n" +
                   $"CONCLUSION:\n" +
                   $"Hash Table is approximately {studentCount}x faster for searches!\n" +
                   $"For {studentCount} students, Hash Table checks ~1 entry vs {studentCount} entries for List.";
        }
    }
}
