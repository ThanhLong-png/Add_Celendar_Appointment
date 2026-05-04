using System;
using System.Drawing;
using System.Windows.Forms;

namespace Add_Celendar_Appointment
{
    partial class ViewAppointmentForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.lblTime = new Label();
            this.lblLocation = new Label();
            this.lblType = new Label();
            this.btnDelete = new Button();
            this.btnClose = new Button();
            this.panelTop = new Panel();
            
            this.SuspendLayout();

            // panelTop
            this.panelTop.BackColor = Color.FromArgb(26, 115, 232);
            this.panelTop.Dock = DockStyle.Top;
            this.panelTop.Height = 8;
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(60, 64, 67);
            this.lblTitle.Location = new Point(20, 20);
            this.lblTitle.MaximumSize = new Size(360, 0);

            // lblTime
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new Font("Segoe UI", 11);
            this.lblTime.ForeColor = Color.FromArgb(95, 99, 104);
            this.lblTime.Location = new Point(22, 60);

            // lblLocation
            this.lblLocation.AutoSize = true;
            this.lblLocation.Font = new Font("Segoe UI", 10);
            this.lblLocation.ForeColor = Color.FromArgb(60, 64, 67);
            this.lblLocation.Location = new Point(22, 90);

            // lblType
            this.lblType.AutoSize = true;
            this.lblType.Font = new Font("Segoe UI", 10, FontStyle.Italic);
            this.lblType.Location = new Point(22, 120);

            // btnDelete
            this.btnDelete.BackColor = Color.FromArgb(230, 67, 60);
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.FlatStyle = FlatStyle.Flat;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            this.btnDelete.Location = new Point(25, 160);
            this.btnDelete.Size = new Size(100, 35);
            this.btnDelete.Text = "Xóa";
            this.btnDelete.Cursor = Cursors.Hand;
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);

            // btnClose
            this.btnClose.BackColor = Color.White;
            this.btnClose.ForeColor = Color.FromArgb(60, 64, 67);
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderColor = Color.FromArgb(218, 220, 224);
            this.btnClose.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            this.btnClose.Location = new Point(140, 160);
            this.btnClose.Size = new Size(100, 35);
            this.btnClose.Text = "Đóng";
            this.btnClose.Cursor = Cursors.Hand;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);

            // ViewAppointmentForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(400, 220);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Chi tiết lịch hẹn";
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Label lblTitle;
        private Label lblTime;
        private Label lblLocation;
        private Label lblType;
        private Button btnDelete;
        private Button btnClose;
        private Panel panelTop;
    }
}
