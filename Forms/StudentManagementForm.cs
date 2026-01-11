using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Forms
{
    /// <summary>
    /// Student Management Form - Demonstrates O(1) CRUD operations using Hash Table
    /// KEY FOR EXAM: This form shows practical application of hash tables!
    /// </summary>
    public partial class StudentManagementForm : Form
    {
        private readonly StudentManager _studentManager;
        private DataGridView dgvStudents;
        private TextBox txtStudentID;
        private TextBox txtStudentName;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private ComboBox cmbGender;
        private DateTimePicker dtpBirthDate;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnFind;
        private Button btnLoadAll;
        private Button btnClear;
        private Button btnPerformance;
        private Label lblComplexity;
        private Label lblStatus;

        public StudentManagementForm()
        {
            _studentManager = new StudentManager();
            InitializeComponents();
            LoadSampleData(); // Load sample students into hash table
        }

        private void InitializeComponents()
        {
            // Form settings
            this.Text = "Student Management";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(236, 240, 241);

            // Title Label
            var lblTitle = new Label
            {
                Text = "Student Management",
                Location = new Point(20, 20),
                Size = new Size(1150, 35),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Subtitle
            lblComplexity = new Label
            {
                Text = "Add, Update, Delete, and Search Student Records",
                Location = new Point(20, 60),
                Size = new Size(1150, 25),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(127, 140, 141),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Input Panel
            var panelInput = new Panel
            {
                Location = new Point(20, 100),
                Size = new Size(1150, 180),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Student ID
            var lblID = new Label { Text = "Student ID:", Location = new Point(20, 20), Size = new Size(100, 20), Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            txtStudentID = new TextBox { Location = new Point(20, 45), Size = new Size(150, 25), Font = new Font("Segoe UI", 10) };

            // Student Name
            var lblName = new Label { Text = "Student Name:", Location = new Point(190, 20), Size = new Size(100, 20), Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            txtStudentName = new TextBox { Location = new Point(190, 45), Size = new Size(250, 25), Font = new Font("Segoe UI", 10) };

            // Gender
            var lblGender = new Label { Text = "Gender:", Location = new Point(460, 20), Size = new Size(100, 20), Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            cmbGender = new ComboBox { Location = new Point(460, 45), Size = new Size(120, 25), Font = new Font("Segoe UI", 10), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbGender.Items.AddRange(new object[] { "Male", "Female", "Other" });

            // Date of Birth
            var lblDOB = new Label { Text = "Date of Birth:", Location = new Point(600, 20), Size = new Size(100, 20), Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            dtpBirthDate = new DateTimePicker { Location = new Point(600, 45), Size = new Size(200, 25), Font = new Font("Segoe UI", 10) };

            // Phone
            var lblPhone = new Label { Text = "Phone:", Location = new Point(20, 85), Size = new Size(100, 20), Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            txtPhone = new TextBox { Location = new Point(20, 110), Size = new Size(200, 25), Font = new Font("Segoe UI", 10) };

            // Email
            var lblEmail = new Label { Text = "Email:", Location = new Point(240, 85), Size = new Size(100, 20), Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            txtEmail = new TextBox { Location = new Point(240, 110), Size = new Size(300, 25), Font = new Font("Segoe UI", 10) };

            // Buttons
            btnAdd = CreateButton("Add Student", new Point(850, 20), Color.FromArgb(46, 204, 113));
            btnAdd.Click += BtnAdd_Click;

            btnFind = CreateButton("Find by ID", new Point(850, 60), Color.FromArgb(52, 152, 219));
            btnFind.Click += BtnFind_Click;

            btnUpdate = CreateButton("Update Student", new Point(850, 100), Color.FromArgb(241, 196, 15));
            btnUpdate.Click += BtnUpdate_Click;

            btnDelete = CreateButton("Delete Student", new Point(1010, 20), Color.FromArgb(231, 76, 60));
            btnDelete.Click += BtnDelete_Click;

            btnClear = CreateButton("Clear Form", new Point(1010, 60), Color.FromArgb(149, 165, 166));
            btnClear.Click += (s, e) => ClearForm();

            panelInput.Controls.AddRange(new Control[]
            {
                lblID, txtStudentID, lblName, txtStudentName, lblGender, cmbGender,
                lblDOB, dtpBirthDate, lblPhone, txtPhone, lblEmail, txtEmail,
                btnAdd, btnFind, btnUpdate, btnDelete, btnClear
            });

            // DataGridView for displaying students
            dgvStudents = new DataGridView
            {
                Location = new Point(20, 340),
                Size = new Size(1150, 270),
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                Font = new Font("Segoe UI", 9)
            };
            dgvStudents.CellClick += DgvStudents_CellClick;

            // Load All Button
            btnLoadAll = new Button
            {
                Text = "📋 Load All Students",
                Location = new Point(20, 300),
                Size = new Size(200, 30),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLoadAll.Click += BtnLoadAll_Click;

            // Status Label
            lblStatus = new Label
            {
                Text = "Ready. Total Students: 0",
                Location = new Point(20, 620),
                Size = new Size(1150, 25),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(52, 73, 94)
            };

            // Add all controls to form
            this.Controls.AddRange(new Control[]
            {
                lblTitle, lblComplexity, panelInput, btnLoadAll, dgvStudents, lblStatus
            });
        }

        private Button CreateButton(string text, Point location, Color color)
        {
            return new Button
            {
                Text = text,
                Location = location,
                Size = new Size(140, 35),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        /// <summary>
        /// Add Student - O(1) operation
        /// </summary>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            var student = new Student
            {
                StudentID = int.Parse(txtStudentID.Text),
                StudentName = txtStudentName.Text,
                Gender = cmbGender.Text,
                DateOfBirth = dtpBirthDate.Value,
                Phone = txtPhone.Text,
                Email = txtEmail.Text
            };

            // O(1) insertion into hash table!
            bool success = _studentManager.AddStudent(student);

            if (success)
            {
                MessageBox.Show(
                    "Student added successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                UpdateStatus();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Student ID already exists!",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Find Student by ID - O(1) operation (KEY DEMO!)
        /// This is THE most important feature for your exam!
        /// </summary>
        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStudentID.Text))
            {
                MessageBox.Show("Please enter Student ID to search.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int studentId = int.Parse(txtStudentID.Text);

            // O(1) hash table lookup - instant retrieval!
            var student = _studentManager.FindStudentById(studentId);

            if (student != null)
            {
                // Populate form with found student
                txtStudentName.Text = student.StudentName;
                cmbGender.Text = student.Gender;
                dtpBirthDate.Value = student.DateOfBirth ?? DateTime.Now;
                txtPhone.Text = student.Phone;
                txtEmail.Text = student.Email;

                MessageBox.Show(
                    $"Student found!\n\n" +
                    $"Name: {student.StudentName}\n" +
                    $"ID: {student.StudentID}",
                    "Student Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Student not found!",
                    "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Update Student - O(1) operation
        /// </summary>
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            var student = new Student
            {
                StudentID = int.Parse(txtStudentID.Text),
                StudentName = txtStudentName.Text,
                Gender = cmbGender.Text,
                DateOfBirth = dtpBirthDate.Value,
                Phone = txtPhone.Text,
                Email = txtEmail.Text
            };

            // O(1) update in hash table!
            bool success = _studentManager.UpdateStudent(student);

            if (success)
            {
                MessageBox.Show("Student updated successfully!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateStatus();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Student not found!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Delete Student - O(1) operation
        /// </summary>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStudentID.Text))
            {
                MessageBox.Show("Please enter Student ID to delete.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to delete this student?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int studentId = int.Parse(txtStudentID.Text);

                // O(1) deletion from hash table!
                bool success = _studentManager.DeleteStudent(studentId);

                if (success)
                {
                    MessageBox.Show("Student deleted successfully!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateStatus();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Student not found!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Load all students into DataGridView - O(n)
        /// </summary>
        private void BtnLoadAll_Click(object sender, EventArgs e)
        {
            var students = _studentManager.GetAllStudents();

            dgvStudents.DataSource = null;
            dgvStudents.DataSource = students;
            UpdateStatus();
        }

        private void DgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvStudents.Rows[e.RowIndex];
                txtStudentID.Text = row.Cells["StudentID"].Value?.ToString();
                txtStudentName.Text = row.Cells["StudentName"].Value?.ToString();
                cmbGender.Text = row.Cells["Gender"].Value?.ToString();
                txtPhone.Text = row.Cells["Phone"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();

                if (row.Cells["DateOfBirth"].Value != null)
                    dtpBirthDate.Value = Convert.ToDateTime(row.Cells["DateOfBirth"].Value);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtStudentID.Text) ||
                string.IsNullOrWhiteSpace(txtStudentName.Text))
            {
                MessageBox.Show("Student ID and Name are required!", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtStudentID.Text, out _))
            {
                MessageBox.Show("Student ID must be a number!", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtStudentID.Clear();
            txtStudentName.Clear();
            cmbGender.SelectedIndex = -1;
            txtPhone.Clear();
            txtEmail.Clear();
            dtpBirthDate.Value = DateTime.Now;
            txtStudentID.Focus();
        }

        private void UpdateStatus()
        {
            int count = _studentManager.GetStudentCount();
            lblStatus.Text = $"Total Students: {count}";
        }

        /// <summary>
        /// Load sample data for demonstration
        /// </summary>
        private void LoadSampleData()
        {
            var sampleStudents = new List<Student>
            {
                new Student { StudentID = 1, StudentName = "Sok Pisey", Gender = "Female", DateOfBirth = new DateTime(2000, 5, 15), Phone = "012345678", Email = "pisey@email.com" },
                new Student { StudentID = 2, StudentName = "Chan Dara", Gender = "Male", DateOfBirth = new DateTime(1999, 8, 20), Phone = "012345679", Email = "dara@email.com" },
                new Student { StudentID = 3, StudentName = "Meng Sreynit", Gender = "Female", DateOfBirth = new DateTime(2001, 3, 10), Phone = "012345680", Email = "sreynit@email.com" },
                new Student { StudentID = 4, StudentName = "Vann Sokha", Gender = "Male", DateOfBirth = new DateTime(2000, 11, 25), Phone = "012345681", Email = "sokha@email.com" },
                new Student { StudentID = 5, StudentName = "Keo Malina", Gender = "Female", DateOfBirth = new DateTime(1999, 12, 5), Phone = "012345682", Email = "malina@email.com" }
            };

            _studentManager.LoadStudentsIntoHashTable(sampleStudents);
            UpdateStatus();
        }
    }
}
