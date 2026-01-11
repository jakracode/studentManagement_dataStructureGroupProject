-- Sample Data for Testing the Hash Table Implementation
-- Run this after creating the database schema

USE hashingStudents;
GO

-- Insert sample admins
INSERT INTO SystemAdmins (FullName, Username, PasswordHash)
VALUES 
    ('System Administrator', 'admin', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f'), -- Password: admin123
    ('John Doe', 'john', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918'), -- Password: admin
    ('Jane Smith', 'jane', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'); -- Password: 1234

-- Insert sample students
INSERT INTO Students (StudentName, Gender, DateOfBirth, Phone, Email)
VALUES 
    ('Sok Pisey', 'Female', '2000-05-15', '012345678', 'pisey@email.com'),
    ('Chan Dara', 'Male', '1999-08-20', '012345679', 'dara@email.com'),
    ('Meng Sreynit', 'Female', '2001-03-10', '012345680', 'sreynit@email.com'),
    ('Vann Sokha', 'Male', '2000-11-25', '012345681', 'sokha@email.com'),
    ('Keo Malina', 'Female', '1999-12-05', '012345682', 'malina@email.com'),
    ('Pich Dara', 'Male', '2001-07-18', '012345683', 'pich@email.com'),
    ('Lim Sothea', 'Female', '2000-02-28', '012345684', 'sothea@email.com'),
    ('Heng Ratanak', 'Male', '1999-09-14', '012345685', 'ratanak@email.com'),
    ('Chea Sophea', 'Female', '2001-04-22', '012345686', 'sophea@email.com'),
    ('Nguon Rith', 'Male', '2000-06-30', '012345687', 'rith@email.com');

-- Insert sample teachers
INSERT INTO Teachers (TeacherName, Email, Phone)
VALUES 
    ('Dr. Sok Vannak', 'vannak@school.edu', '012111222'),
    ('Prof. Mey Chenda', 'chenda@school.edu', '012111223'),
    ('Mr. Heng Dara', 'heng@school.edu', '012111224'),
    ('Ms. Lim Sreypeou', 'sreypeou@school.edu', '012111225');

-- Insert sample courses
INSERT INTO Courses (CourseName, Credits)
VALUES 
    ('Data Structures and Algorithms', 4),
    ('Database Management Systems', 3),
    ('Web Development', 3),
    ('Computer Networks', 4),
    ('Software Engineering', 3),
    ('Operating Systems', 4);

-- Insert sample classes
INSERT INTO Classes (CourseID, TeacherID, Semester, RoomNumber)
VALUES 
    (1, 1, 'Fall 2024', 'A101'),
    (2, 2, 'Fall 2024', 'B202'),
    (3, 3, 'Fall 2024', 'C303'),
    (4, 1, 'Spring 2025', 'A102'),
    (5, 2, 'Spring 2025', 'B203'),
    (6, 4, 'Spring 2025', 'D404');

-- Insert sample enrollments
INSERT INTO Enrollments (StudentID, ClassID, EnrollmentDate, Grade)
VALUES 
    (1, 1, '2024-09-01', 85.50),
    (1, 2, '2024-09-01', 90.00),
    (2, 1, '2024-09-01', 78.00),
    (2, 3, '2024-09-01', 88.50),
    (3, 1, '2024-09-01', 92.00),
    (3, 2, '2024-09-01', 87.50),
    (4, 2, '2024-09-01', 75.00),
    (4, 3, '2024-09-01', 82.00),
    (5, 1, '2024-09-01', 95.00),
    (5, 3, '2024-09-01', 91.50),
    (6, 4, '2025-01-15', NULL), -- Not graded yet
    (7, 5, '2025-01-15', NULL),
    (8, 6, '2025-01-15', NULL),
    (9, 4, '2025-01-15', NULL),
    (10, 5, '2025-01-15', NULL);

-- Verify data
SELECT 'Admins' AS TableName, COUNT(*) AS RecordCount FROM SystemAdmins
UNION ALL
SELECT 'Students', COUNT(*) FROM Students
UNION ALL
SELECT 'Teachers', COUNT(*) FROM Teachers
UNION ALL
SELECT 'Courses', COUNT(*) FROM Courses
UNION ALL
SELECT 'Classes', COUNT(*) FROM Classes
UNION ALL
SELECT 'Enrollments', COUNT(*) FROM Enrollments;

GO

-- Display sample data
SELECT '=== SAMPLE ADMINS ===' AS Info;
SELECT AdminID, FullName, Username, CreatedAt FROM SystemAdmins;

SELECT '=== SAMPLE STUDENTS ===' AS Info;
SELECT StudentID, StudentName, Gender, DateOfBirth FROM Students;

SELECT '=== SAMPLE COURSES ===' AS Info;
SELECT CourseID, CourseName, Credits FROM Courses;

GO
