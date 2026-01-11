using System;
using System.Drawing;
using System.Windows.Forms;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Forms
{
    public partial class RegisterForm : Form
    {
        private readonly AuthenticationService _authService;
        private TextBox txtFullName;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private Button btnRegister;
        private Button btnCancel;

        public RegisterForm(AuthenticationService authService)
        {
            _authService = authService;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Register New Admin";
            this.Size = new Size(450, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            var lblTitle = new Label
            {
                Text = "Register New Administrator",
                Location = new Point(50, 20),
                Size = new Size(350, 30),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(46, 204, 113)
            };

            var lblFullName = new Label
            {
                Text = "Full Name:",
                Location = new Point(50, 70),
                Size = new Size(100, 20)
            };

            txtFullName = new TextBox
            {
                Location = new Point(50, 95),
                Size = new Size(330, 25),
                Font = new Font("Segoe UI", 10)
            };

            var lblUsername = new Label
            {
                Text = "Username:",
                Location = new Point(50, 130),
                Size = new Size(100, 20)
            };

            txtUsername = new TextBox
            {
                Location = new Point(50, 155),
                Size = new Size(330, 25),
                Font = new Font("Segoe UI", 10)
            };

            var lblPassword = new Label
            {
                Text = "Password:",
                Location = new Point(50, 190),
                Size = new Size(100, 20)
            };

            txtPassword = new TextBox
            {
                Location = new Point(50, 215),
                Size = new Size(330, 25),
                Font = new Font("Segoe UI", 10),
                PasswordChar = '●'
            };

            var lblConfirmPassword = new Label
            {
                Text = "Confirm Password:",
                Location = new Point(50, 250),
                Size = new Size(150, 20)
            };

            txtConfirmPassword = new TextBox
            {
                Location = new Point(50, 275),
                Size = new Size(330, 25),
                Font = new Font("Segoe UI", 10),
                PasswordChar = '●'
            };

            btnRegister = new Button
            {
                Text = "Register",
                Location = new Point(50, 320),
                Size = new Size(160, 35),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnRegister.Click += BtnRegister_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(220, 320),
                Size = new Size(160, 35),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[]
            {
                lblTitle, lblFullName, txtFullName,
                lblUsername, txtUsername, lblPassword, txtPassword,
                lblConfirmPassword, txtConfirmPassword,
                btnRegister, btnCancel
            });
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("All fields are required!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Register using O(1) hash table insertion
            bool success = _authService.Register(
                txtFullName.Text.Trim(),
                txtUsername.Text.Trim(),
                txtPassword.Text);

            if (success)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Username already exists!\n\n" +
                    "Hash table detected duplicate in O(1) time!",
                    "Registration Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
