using Add_Celendar_Appointment.Models;
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
            this.lblTitle        = new Label();
            this.lblTime         = new Label();
            this.lblLocation     = new Label();
            this.lblType         = new Label();

            this.panelGroupInfo  = new Panel();
            this.lblGroupSep     = new Label();
            this.lblOrganizer    = new Label();
            this.lblParticipants = new Label();
            this.lblAgenda       = new Label();
            this.lblJoinLink     = new Label();

            this.btnDelete       = new Button();
            this.btnClose        = new Button();
            this.panelTop        = new Panel();

            this.SuspendLayout();

            this.panelTop.BackColor = Color.FromArgb(26, 115, 232);
            this.panelTop.Dock      = DockStyle.Top;
            this.panelTop.Height    = 8;

            this.lblTitle.AutoSize    = true;
            this.lblTitle.Font        = new Font("Segoe UI", 16, FontStyle.Bold);
            this.lblTitle.ForeColor   = Color.FromArgb(60, 64, 67);
            this.lblTitle.Location    = new Point(20, 20);
            this.lblTitle.MaximumSize = new Size(360, 0);

            this.lblTime.AutoSize  = true;
            this.lblTime.Font      = new Font("Segoe UI", 11);
            this.lblTime.ForeColor = Color.FromArgb(95, 99, 104);
            this.lblTime.Location  = new Point(22, 62);

            this.lblLocation.AutoSize  = true;
            this.lblLocation.Font      = new Font("Segoe UI", 10);
            this.lblLocation.ForeColor = Color.FromArgb(60, 64, 67);
            this.lblLocation.Location  = new Point(22, 90);

            this.lblType.AutoSize  = true;
            this.lblType.Font      = new Font("Segoe UI", 10, FontStyle.Italic);
            this.lblType.Location  = new Point(22, 118);

            this.panelGroupInfo.Location  = new Point(15, 148);
            this.panelGroupInfo.Size      = new Size(370, 150);
            this.panelGroupInfo.BackColor = Color.FromArgb(240, 245, 255);
            this.panelGroupInfo.Visible   = false;
            this.panelGroupInfo.Paint    += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(180, 200, 240));
                e.Graphics.DrawRectangle(pen, 0, 0, panelGroupInfo.Width - 1, panelGroupInfo.Height - 1);
            };

            this.lblGroupSep.Text      = "👥  Thông tin họp nhóm";
            this.lblGroupSep.Font      = new Font("Segoe UI", 9, FontStyle.Bold);
            this.lblGroupSep.ForeColor = Color.FromArgb(37, 99, 220);
            this.lblGroupSep.AutoSize  = true;
            this.lblGroupSep.Location  = new Point(10, 8);

            this.lblOrganizer.AutoSize  = true;
            this.lblOrganizer.Font      = new Font("Segoe UI", 9);
            this.lblOrganizer.ForeColor = Color.FromArgb(60, 64, 67);
            this.lblOrganizer.Location  = new Point(10, 30);

            this.lblParticipants.AutoSize  = true;
            this.lblParticipants.Font      = new Font("Segoe UI", 9);
            this.lblParticipants.ForeColor = Color.FromArgb(60, 64, 67);
            this.lblParticipants.Location  = new Point(10, 52);

            this.lblAgenda.AutoSize     = true;
            this.lblAgenda.Font         = new Font("Segoe UI", 9, FontStyle.Italic);
            this.lblAgenda.ForeColor    = Color.FromArgb(80, 90, 120);
            this.lblAgenda.Location     = new Point(10, 74);
            this.lblAgenda.MaximumSize  = new Size(350, 0);

            this.lblJoinLink.AutoSize   = true;
            this.lblJoinLink.Font       = new Font("Segoe UI", 9);
            this.lblJoinLink.ForeColor  = Color.FromArgb(26, 115, 232);
            this.lblJoinLink.Location   = new Point(10, 118);

            this.panelGroupInfo.Controls.AddRange(new Control[]
            {
                this.lblGroupSep, this.lblOrganizer,
                this.lblParticipants, this.lblAgenda, this.lblJoinLink
            });

            this.btnDelete.BackColor                = Color.FromArgb(230, 67, 60);
            this.btnDelete.ForeColor                = Color.White;
            this.btnDelete.FlatStyle                = FlatStyle.Flat;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.Font                     = new Font("Segoe UI", 9, FontStyle.Bold);
            this.btnDelete.Location                 = new Point(25, 170);
            this.btnDelete.Size                     = new Size(100, 35);
            this.btnDelete.Text                     = "🗑 Xóa";
            this.btnDelete.Cursor                   = Cursors.Hand;
            this.btnDelete.Click                   += new EventHandler(this.btnDelete_Click);

            this.btnClose.BackColor                 = Color.White;
            this.btnClose.ForeColor                 = Color.FromArgb(60, 64, 67);
            this.btnClose.FlatStyle                 = FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderColor = Color.FromArgb(218, 220, 224);
            this.btnClose.Font                      = new Font("Segoe UI", 9, FontStyle.Bold);
            this.btnClose.Location                  = new Point(140, 170);
            this.btnClose.Size                      = new Size(100, 35);
            this.btnClose.Text                      = "Đóng";
            this.btnClose.Cursor                    = Cursors.Hand;
            this.btnClose.Click                    += new EventHandler(this.btnClose_Click);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode       = AutoScaleMode.Font;
            this.BackColor           = Color.White;
            this.ClientSize          = new Size(400, 220);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.panelGroupInfo);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.StartPosition   = FormStartPosition.CenterParent;
            this.Text            = "Chi tiết lịch hẹn";

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Label   lblTitle, lblTime, lblLocation, lblType;
        private Label   lblGroupSep, lblOrganizer, lblParticipants, lblAgenda, lblJoinLink;
        private Panel   panelGroupInfo, panelTop;
        private Button  btnDelete, btnClose;
    }
}
