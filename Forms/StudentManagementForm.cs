using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;
using StudentManagementSystem.Helpers;
using StudentManagementSystem.Forms.Controls;

namespace StudentManagementSystem.Forms
{
    public partial class StudentManagementForm : Form
    {
        private readonly StudentManager _studentManager;
        
        // Layout Panels
        private Panel pnlLeft;  // List/Search
        private Panel pnlRight; // Details/Edit
        
        // Left Side Controls
        private TextBox txtSearch;
        private ComboBox cmbFilterGender;
        private DataGridView dgvStudents;
        private Label lblCount;
        private ProgressBar loadingBar;
        
        // Right Side Controls
        private Label lblRightTitle;
        private TextBox txtStudentID;
        private TextBox txtStudentName;
        private ComboBox cmbGender;
        private DateTimePicker dtpBirthDate;
        private TextBox txtPhone;
        private TextBox txtCourse;
        
        // Buttons
        private ModernButton btnSave; // Acts as Add or Update
        private ModernButton btnDelete;
        private ModernButton btnClear;

        // Custom Title Bar
        private Panel pnlTitleBar;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public StudentManagementForm()
        {
            _studentManager = new StudentManager();
            InitializeComponents();
            
            // Load from database is now done in OnLoad async to prevent UI freeze
            // _studentManager.LoadFromDatabase();
            
            SetupEvents();
        }

        private void InitializeComponents()
        {
            // Form Setup
            this.Text = "Student Management";
            this.Size = new Size(1300, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = ModernTheme.BackColor;

            // --- Title Bar ---
            SetupTitleBar();

            // --- Split Layout ---
            pnlLeft = new Panel
            {
                Location = new Point(20, 50),
                Size = new Size(500, 730),
                BackColor = ModernTheme.SurfaceColor,
            };

            pnlRight = new Panel
            {
                Location = new Point(540, 50),
                Size = new Size(740, 730),
                BackColor = ModernTheme.SurfaceColor,
            };

            // --- Left Panel Content (Search & List) ---
            var lblSearch = new Label
            {
                Text = "Student Directory",
                Location = new Point(20, 20),
                Size = new Size(300, 30),
                Font = ModernTheme.HeaderFont,
                ForeColor = ModernTheme.PrimaryColor
            };

            // Search Box
            var pnlSearchBox = new Panel { Location = new Point(20, 60), Size = new Size(300, 35), BackColor = ModernTheme.InputBackColor };
            txtSearch = new TextBox
            {
                Location = new Point(10, 8),
                Size = new Size(280, 20),
                Font = new Font("Segoe UI", 10),
                BackColor = ModernTheme.InputBackColor,
                ForeColor = ModernTheme.InputForeColor,
                BorderStyle = BorderStyle.None,
                PlaceholderText = "Search by Name or ID..."
            };
            pnlSearchBox.Controls.Add(txtSearch);

            // Filter
            cmbFilterGender = new ComboBox
            {
                Location = new Point(330, 60),
                Size = new Size(150, 35),
                Font = new Font("Segoe UI", 11),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ModernTheme.InputBackColor,
                ForeColor = ModernTheme.InputForeColor,
                FlatStyle = FlatStyle.Flat
            };
            cmbFilterGender.Items.AddRange(new object[] { "All Genders", "Male", "Female", "Other" });
            cmbFilterGender.SelectedIndex = 0;

            // Grid
            dgvStudents = new DataGridView
            {
                Location = new Point(20, 110),
                Size = new Size(460, 550),
                BackgroundColor = ModernTheme.BackColor, // Slightly darker than surface
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false,
                Font = ModernTheme.BodyFont
            };
            StyleGrid(dgvStudents);

            lblCount = new Label
            {
                Text = "0 Students Found",
                Location = new Point(20, 680),
                Size = new Size(460, 20),
                Font = ModernTheme.SmallFont,
                ForeColor = ModernTheme.SubTextColor,
                TextAlign = ContentAlignment.MiddleRight
            };

            // Loading Bar
            loadingBar = new ProgressBar
            {
                Location = new Point(20, 98),
                Size = new Size(460, 5),
                Style = ProgressBarStyle.Marquee,
                Visible = false
            };

            pnlLeft.Controls.AddRange(new Control[] { lblSearch, pnlSearchBox, cmbFilterGender, dgvStudents, lblCount, loadingBar });


            // --- Right Panel Content (Details) ---
            lblRightTitle = new Label
            {
                Text = "Add New Student",
                Location = new Point(30, 20),
                Size = new Size(400, 30),
                Font = ModernTheme.HeaderFont,
                ForeColor = ModernTheme.PrimaryColor
            };

            var lblSubtitle = new Label
            {
                Text = "Enter student details below. ID must be unique.",
                Location = new Point(30, 55),
                Size = new Size(600, 20),
                Font = ModernTheme.BodyFont,
                ForeColor = ModernTheme.SubTextColor
            };

            // Form Fields Helper
            int startY = 100;
            int gapY = 80;
            
            // ID
            pnlRight.Controls.Add(CreateLabel("STUDENT ID", 30, startY));
            var pnlID = CreateInputPanel(30, startY + 25, 300, out txtStudentID);
            pnlRight.Controls.Add(pnlID);

            // Name
            pnlRight.Controls.Add(CreateLabel("FULL NAME", 360, startY));
            var pnlName = CreateInputPanel(360, startY + 25, 350, out txtStudentName);
            pnlRight.Controls.Add(pnlName);

            startY += gapY;

            // Gender
            pnlRight.Controls.Add(CreateLabel("GENDER", 30, startY));
            cmbGender = new ComboBox
            {
                Location = new Point(30, startY + 25),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 11),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ModernTheme.InputBackColor,
                ForeColor = ModernTheme.InputForeColor,
                FlatStyle = FlatStyle.Flat
            };
            cmbGender.Items.AddRange(new object[] { "Male", "Female", "Other" });
            pnlRight.Controls.Add(cmbGender);

            // DOB
            pnlRight.Controls.Add(CreateLabel("DATE OF BIRTH", 360, startY));
            dtpBirthDate = new DateTimePicker
            {
                Location = new Point(360, startY + 25),
                Size = new Size(350, 30),
                Font = new Font("Segoe UI", 11),
                CalendarMonthBackground = ModernTheme.InputBackColor,
                CalendarForeColor = ModernTheme.InputForeColor
                // MaxDate = DateTime.Now // Removed to prevent crash when loading invalid legacy data (e.g. Year 3009)
            };
            pnlRight.Controls.Add(dtpBirthDate);

            startY += gapY;

            // Phone
            pnlRight.Controls.Add(CreateLabel("PHONE NUMBER", 30, startY));
            var pnlPhone = CreateInputPanel(30, startY + 25, 300, out txtPhone);
            pnlRight.Controls.Add(pnlPhone);

            // Email
            pnlRight.Controls.Add(CreateLabel("COURSE", 360, startY));
            var pnlCourse = CreateInputPanel(360, startY + 25, 350, out txtCourse);
            pnlRight.Controls.Add(pnlCourse);

            startY += gapY + 20;

            // Action Buttons
            btnSave = new ModernButton
            {
                Text = "Save Student",
                Location = new Point(30, startY),
                Size = new Size(200, 45),
                BackColor = ModernTheme.PrimaryColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BorderRadius = 20,
                Cursor = Cursors.Hand
            };

            btnClear = new ModernButton
            {
                Text = "New / Clear",
                Location = new Point(250, startY),
                Size = new Size(150, 45),
                BackColor = ModernTheme.InputBackColor,
                ForeColor = ModernTheme.SubTextColor,
                Font = new Font("Segoe UI", 10),
                BorderRadius = 20,
                Cursor = Cursors.Hand
            };

            btnDelete = new ModernButton
            {
                Text = "Delete Student",
                Location = new Point(560, startY),
                Size = new Size(150, 45),
                BackColor = ModernTheme.ErrorColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BorderRadius = 20,
                Cursor = Cursors.Hand,
                Visible = false // Hidden by default
            };

            pnlRight.Controls.AddRange(new Control[] { lblRightTitle, lblSubtitle, btnSave, btnClear, btnDelete });

            // Add Panels to Form
            this.Controls.Add(pnlLeft);
            this.Controls.Add(pnlRight);
        }

        private void SetupTitleBar()
        {
            pnlTitleBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 30,
                BackColor = ModernTheme.SurfaceColor
            };
            pnlTitleBar.MouseDown += (s, e) => { dragging = true; dragCursorPoint = Cursor.Position; dragFormPoint = this.Location; };
            pnlTitleBar.MouseMove += (s, e) => { if (dragging) { Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint)); this.Location = Point.Add(dragFormPoint, new Size(dif)); } };
            pnlTitleBar.MouseUp += (s, e) => dragging = false;

            var btnClose = new Button
            {
                Text = "✕",
                Dock = DockStyle.Right,
                Width = 40,
                FlatStyle = FlatStyle.Flat,
                ForeColor = ModernTheme.TextColor,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = ModernTheme.ErrorColor;
            btnClose.Click += (s, e) => this.Close();

            var btnMin = new Button
            {
                Text = "—",
                Dock = DockStyle.Right,
                Width = 40,
                FlatStyle = FlatStyle.Flat,
                ForeColor = ModernTheme.TextColor,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            btnMin.FlatAppearance.BorderSize = 0;
            btnMin.FlatAppearance.MouseOverBackColor = ModernTheme.SurfaceColor;
            btnMin.Click += (s, e) => this.WindowState = FormWindowState.Minimized;

            var lblTitle = new Label { Text = "  Student Management System", Dock = DockStyle.Left, AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, ForeColor = ModernTheme.SubTextColor, Font = new Font("Segoe UI", 9) };
            
            pnlTitleBar.Controls.Add(lblTitle);
            pnlTitleBar.Controls.Add(btnMin);
            pnlTitleBar.Controls.Add(btnClose);
            this.Controls.Add(pnlTitleBar);
        }

        private void SetupEvents()
        {
            // Search Logic
            txtSearch.TextChanged += async (s, e) => await PerformSearch();
            cmbFilterGender.SelectedIndexChanged += async (s, e) => await PerformSearch();

            // Grid Selection
            dgvStudents.SelectionChanged += (s, e) =>
            {
                if (dgvStudents.SelectedRows.Count > 0)
                {
                    var student = (Student)dgvStudents.SelectedRows[0].DataBoundItem;
                    PopulateForm(student);
                }
            };

            // Buttons
            btnSave.Click += BtnSave_Click;
            btnClear.Click += (s, e) => ClearForm();
            btnDelete.Click += BtnDelete_Click;
        }

        private async System.Threading.Tasks.Task PerformSearch()
        {
            loadingBar.Visible = true;
            try
            {
                string query = txtSearch.Text.Trim();
                string genderFilter = cmbFilterGender.SelectedItem?.ToString();
                
                List<Student> results;

                // 1. Filter by Text (Name or ID)
                if (string.IsNullOrEmpty(query))
                {
                     results = _studentManager.GetAllStudents();
                }
                else
                {
                    if (int.TryParse(query, out int id))
                    {
                        var student = _studentManager.FindStudentById(id);
                        results = student != null ? new List<Student> { student } : new List<Student>();
                    }
                    else
                    {
                        results = _studentManager.SearchStudentsByName(query);
                    }
                }

                // 2. Filter by Gender
                if (!string.IsNullOrEmpty(genderFilter) && genderFilter != "All Genders")
                {
                    results = results.FindAll(s => s.Gender != null && s.Gender.Equals(genderFilter, StringComparison.OrdinalIgnoreCase));
                }

                BindGrid(results);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Search Error: {ex.Message}");
            }
            finally
            {
                loadingBar.Visible = false;
            }
        }

        private void BindGrid(List<Student> students)
        {
            try
            {
                dgvStudents.DataSource = null;
                dgvStudents.DataSource = students;
                
                // Hide columns - Check existence safely
                if (dgvStudents.Columns.Contains("DateOfBirth")) dgvStudents.Columns["DateOfBirth"].Visible = false;
                if (dgvStudents.Columns.Contains("Phone")) dgvStudents.Columns["Phone"].Visible = false;
                
                lblCount.Text = $"{students.Count} Students Found";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Grid Binding Error: {ex.Message}");
            }
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            var student = new Student
            {
                StudentID = int.Parse(txtStudentID.Text),
                StudentName = txtStudentName.Text,
                Gender = cmbGender.Text,
                DateOfBirth = dtpBirthDate.Value,
                Phone = txtPhone.Text,
                Course = txtCourse.Text
            };

            if (_studentManager.StudentExists(student.StudentID))
            {
                // Update
                // If we are in "Edit Mode" (meaning we selected a row), and the ID matches, we update.
                // If user Changed ID to an existing one, that's technically an update on that legitimate ID.
                
                _studentManager.UpdateStudent(student);
                ToastForm.Show("Student updated successfully", ToastForm.ToastType.Success);
            }
            else
            {
                // Insert
                _studentManager.AddStudent(student);
                ToastForm.Show("Student added successfully", ToastForm.ToastType.Success);
            }

            await PerformSearch(); // Refresh grid
            ClearForm();
        }

        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            if(int.TryParse(txtStudentID.Text, out int id))
            {
                var confirmResult = MessageBox.Show(
                    $"Delete {txtStudentName.Text} (ID {id})?", 
                    "Confirm Delete", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    _studentManager.DeleteStudent(id);
                    ToastForm.Show("Student deleted successfully", ToastForm.ToastType.Success);
                    await PerformSearch();
                    ClearForm();
                }
            }
        }

        private void PopulateForm(Student s)
        {
            txtStudentID.Text = s.StudentID.ToString();
            txtStudentID.Enabled = false; // Cannot change ID during edit to prevent key issues for now
            txtStudentName.Text = s.StudentName;
            cmbGender.Text = s.Gender;
            dtpBirthDate.Value = s.DateOfBirth ?? DateTime.Now;
            txtPhone.Text = s.Phone;
            txtCourse.Text = s.Course;

            lblRightTitle.Text = "Edit Student";
            btnSave.Text = "Update Changes";
            btnDelete.Visible = true;
        }

        private void ClearForm()
        {
            txtStudentID.Enabled = true;
            txtStudentID.Clear();
            txtStudentName.Clear();
            cmbGender.SelectedIndex = -1;
            txtPhone.Clear();
            txtCourse.Clear();
            dtpBirthDate.Value = DateTime.Now;

            lblRightTitle.Text = "Add New Student";
            btnSave.Text = "Save Student";
            btnDelete.Visible = false;
            
            // Clear selection
            dgvStudents.ClearSelection();
        }

        // Helpers
        private Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = ModernTheme.SubTextColor
            };
        }

        private Panel CreateInputPanel(int x, int y, int width, out TextBox textBox)
        {
            var pnl = new Panel { Location = new Point(x, y), Size = new Size(width, 35), BackColor = ModernTheme.InputBackColor };
            textBox = new TextBox
            {
                Location = new Point(10, 8),
                Size = new Size(width - 20, 20),
                Font = new Font("Segoe UI", 10),
                BackColor = ModernTheme.InputBackColor,
                ForeColor = ModernTheme.InputForeColor,
                BorderStyle = BorderStyle.None
            };
            pnl.Controls.Add(textBox);
            
            // Focus effect
            textBox.Enter += (s, e) => pnl.BackColor = Color.FromArgb(60, 60, 60);
            textBox.Leave += (s, e) => pnl.BackColor = ModernTheme.InputBackColor;
            
            return pnl;
        }

        private void StyleGrid(DataGridView dgv)
        {
            dgv.ColumnHeadersDefaultCellStyle.BackColor = ModernTheme.InputBackColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = ModernTheme.SubTextColor;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dgv.ColumnHeadersHeight = 35;

            dgv.DefaultCellStyle.BackColor = ModernTheme.SurfaceColor;
            dgv.DefaultCellStyle.ForeColor = ModernTheme.TextColor;
            dgv.DefaultCellStyle.SelectionBackColor = ModernTheme.PrimaryColor;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dgv.GridColor = ModernTheme.InputBackColor;
            
            dgv.RowTemplate.Height = 35;
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtStudentID.Text) || string.IsNullOrWhiteSpace(txtStudentName.Text))
            {
                MessageBox.Show("ID and Name are required.", "Error");
                return false;
            }
            if (!int.TryParse(txtStudentID.Text, out int id))
            {
                MessageBox.Show("ID must be a number.", "Error");
                return false;
            }

            // 1. Validate Date of Birth
            if (dtpBirthDate.Value.Date > DateTime.Now.Date)
            {
                 MessageBox.Show("Date of Birth cannot be in the future.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 return false;
            }
            if (dtpBirthDate.Value.Year < 1900)
            {
                 MessageBox.Show("Date of Birth is too old. Please enter a year after 1900.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 return false;
            }

            // 2. Validate Phone Number Uniqueness
            // We check if the phone number is used by ANY OTHER student (excluding the current ID)
            if (!string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                if (_studentManager.IsPhoneNumberUsed(txtPhone.Text, id))
                {
                    MessageBox.Show("This phone number is already associated with another student.\nPhone numbers must be unique.", 
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }


        
        protected override async void OnLoad(EventArgs e)
        {
             base.OnLoad(e);
             
             loadingBar.Visible = true;
             try 
             {
                 await _studentManager.LoadFromDatabaseAsync();
                 int count = _studentManager.GetStudentCount();
                 // Debug Info
                 // MessageBox.Show($"Successfully connected to DB.\nLoaded {count} students.", "Debug Info");
             } 
             catch (Exception ex) 
             {
                 MessageBox.Show("Failed to load database: " + ex.Message);
             }
             
             await PerformSearch(); // Initial Grid Bind
             loadingBar.Visible = false;
        }
    }
}
