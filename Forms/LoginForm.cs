using System;
using System.Drawing;
using System.Windows.Forms;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Forms
{
    /// <summary>
    /// Login Form - Demonstrates O(1) authentication using Hash Table
    /// </summary>
    public partial class LoginForm : Form
    {
        private readonly AuthenticationService _authService;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;
        private Label lblTitle;
        private Label lblUsername;
        private Label lblPassword;
        private Label lblComplexity;
        private Panel panelMain;

        public LoginForm(AuthenticationService authService)
        {
            _authService = authService;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Form settings
            this.Text = "Student Management System - Login";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Main Panel
            panelMain = new Panel
            {
                Location = new Point(50, 30),
                Size = new Size(400, 320),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Title Label
            lblTitle = new Label
            {
                Text = "Student Management System",
                Location = new Point(50, 20),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Subtitle
            lblComplexity = new Label
            {
                Text = "Please login to continue",
                Location = new Point(50, 55),
                Size = new Size(300, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.FromArgb(127, 140, 141),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Username Label
            lblUsername = new Label
            {
                Text = "Username:",
                Location = new Point(50, 100),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 10)
            };

            // Username TextBox
            txtUsername = new TextBox
            {
                Location = new Point(50, 125),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 11)
            };

            // Password Label
            lblPassword = new Label
            {
                Text = "Password:",
                Location = new Point(50, 165),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 10)
            };

            // Password TextBox
            txtPassword = new TextBox
            {
                Location = new Point(50, 190),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 11),
                PasswordChar = '●'
            };

            // Login Button
            btnLogin = new Button
            {
                Text = "Login",
                Location = new Point(50, 240),
                Size = new Size(145, 40),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            // Register Button
            btnRegister = new Button
            {
                Text = "Register New Admin",
                Location = new Point(205, 240),
                Size = new Size(145, 40),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += BtnRegister_Click;

            // Add controls to panel
            panelMain.Controls.Add(lblTitle);
            panelMain.Controls.Add(lblComplexity);
            panelMain.Controls.Add(lblUsername);
            panelMain.Controls.Add(txtUsername);
            panelMain.Controls.Add(lblPassword);
            panelMain.Controls.Add(txtPassword);
            panelMain.Controls.Add(btnLogin);
            panelMain.Controls.Add(btnRegister);

            // Add panel to form
            this.Controls.Add(panelMain);

            // Set Enter key to login
            this.AcceptButton = btnLogin;
        }

        /// <summary>
        /// Login button click - Demonstrates O(1) hash table lookup
        /// KEY POINT FOR EXAM: This is where O(1) authentication happens!
        /// </summary>
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Show loading
            btnLogin.Enabled = false;
            btnLogin.Text = "Authenticating...";
            Application.DoEvents();

            // CRITICAL: O(1) authentication using hash table!
            // No loop through all users - direct hash lookup!
            var admin = _authService.Login(username, password);

            if (admin != null)
            {
                // Show success message
                MessageBox.Show(
                    $"Welcome, {admin.FullName}!",
                    "Login Successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Open main dashboard
                this.Hide();
                var dashboard = new MainDashboardForm(_authService, admin);
                dashboard.FormClosed += (s, args) => this.Close();
                dashboard.Show();
            }
            else
            {
                MessageBox.Show(
                    "Invalid username or password.\n\n" +
                    "Default credentials:\n" +
                    "Username: admin\n" +
                    "Password: admin123",
                    "Login Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                btnLogin.Enabled = true;
                btnLogin.Text = "Login";
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        /// <summary>
        /// Register button - Open registration form
        /// </summary>
        private void BtnRegister_Click(object sender, EventArgs e)
        {
            var registerForm = new RegisterForm(_authService);
            if (registerForm.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show(
                    "Registration successful!\n" +
                    "You can now login with your new credentials.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
    }
}
