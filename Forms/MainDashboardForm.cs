using System;
using System.Drawing;
using System.Windows.Forms;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Forms
{
    public partial class MainDashboardForm : Form
    {
        private readonly AuthenticationService _authService;
        private readonly SystemAdmin _currentAdmin;
        private Label lblWelcome;
        private Label lblComplexity;
        private Button btnManageStudents;
        private Button btnManageTeachers;
        private Button btnManageCourses;
        private Button btnViewReports;
        private Button btnLogout;
        private Panel panelInfo;

        public MainDashboardForm(AuthenticationService authService, SystemAdmin admin)
        {
            _authService = authService;
            _currentAdmin = admin;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Form settings
            this.Text = "Student Management System - Dashboard";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(236, 240, 241);

            // Welcome Label
            lblWelcome = new Label
            {
                Text = $"Welcome, {_currentAdmin.FullName}!",
                Location = new Point(50, 30),
                Size = new Size(700, 40),
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };

            // Info Panel
            panelInfo = new Panel
            {
                Location = new Point(50, 90),
                Size = new Size(700, 80),
                BackColor = Color.FromArgb(52, 152, 219),
                BorderStyle = BorderStyle.None
            };

            var lblInfoTitle = new Label
            {
                Text = "Welcome to Student Management System",
                Location = new Point(20, 15),
                Size = new Size(660, 30),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblComplexity = new Label
            {
                Text = "Manage students, courses, and academic records efficiently",
                Location = new Point(20, 45),
                Size = new Size(660, 25),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter
            };

            panelInfo.Controls.Add(lblInfoTitle);
            panelInfo.Controls.Add(lblComplexity);

            // Manage Students Button
            btnManageStudents = new Button
            {
                Text = "📚 Manage Students",
                Location = new Point(70, 200),
                Size = new Size(180, 100),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnManageStudents.FlatAppearance.BorderSize = 0;
            btnManageStudents.Click += BtnManageStudents_Click;

            // Manage Teachers Button
            btnManageTeachers = new Button
            {
                Text = "👨‍🏫 Manage Teachers",
                Location = new Point(270, 200),
                Size = new Size(180, 100),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnManageTeachers.FlatAppearance.BorderSize = 0;
            btnManageTeachers.Click += BtnManageTeachers_Click;

            // Manage Courses Button
            btnManageCourses = new Button
            {
                Text = "📖 Manage Courses",
                Location = new Point(470, 200),
                Size = new Size(180, 100),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(155, 89, 182),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnManageCourses.FlatAppearance.BorderSize = 0;
            btnManageCourses.Click += BtnManageCourses_Click;

            // View Reports Button
            btnViewReports = new Button
            {
                Text = "📊 View Reports",
                Location = new Point(270, 320),
                Size = new Size(180, 100),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(230, 126, 34),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnViewReports.FlatAppearance.BorderSize = 0;
            btnViewReports.Click += BtnViewReports_Click;

            // Logout Button
            btnLogout = new Button
            {
                Text = "Logout",
                Location = new Point(650, 500),
                Size = new Size(100, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += (s, e) => this.Close();

            // Add all controls
            this.Controls.AddRange(new Control[]
            {
                lblWelcome, panelInfo, btnManageStudents, btnManageTeachers, 
                btnManageCourses, btnViewReports, btnLogout
            });
        }

        private void BtnManageStudents_Click(object sender, EventArgs e)
        {
            var studentForm = new StudentManagementForm();
            studentForm.Show();
        }

        private void BtnManageTeachers_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Teacher Management module coming soon!\n\n" +
                "This will allow you to:\n" +
                "• Add new teachers\n" +
                "• Update teacher information\n" +
                "• View all teachers\n" +
                "• Delete teachers",
                "Teachers",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void BtnManageCourses_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Course Management module coming soon!\n\n" +
                "This will allow you to:\n" +
                "• Add new courses\n" +
                "• Update course details\n" +
                "• View all courses\n" +
                "• Assign teachers to courses",
                "Courses",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void BtnViewReports_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Reports module coming soon!\n\n" +
                "Available reports:\n" +
                "• Student enrollment statistics\n" +
                "• Course popularity\n" +
                "• Teacher workload\n" +
                "• Academic performance",
                "Reports",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
