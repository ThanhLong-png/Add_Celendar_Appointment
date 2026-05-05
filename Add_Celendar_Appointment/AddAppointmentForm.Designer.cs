using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Add_Celendar_Appointment
{
    partial class AddAppointmentForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelHeader   = new Panel();
            this.lblTitle      = new Label();
            this.lblSubtitle   = new Label();

            // Section: Basic info
            this.panelBasic    = new Panel();
            this.lblSecBasic   = new Label();
            this.lblName       = new Label();
            this.txtName       = new TextBox();
            this.lblLocation   = new Label();
            this.txtLocation   = new TextBox();

            // Section: Time
            this.panelTime     = new Panel();
            this.lblSecTime    = new Label();
            this.lblStart      = new Label();
            this.dtpStart      = new DateTimePicker();
            this.lblEnd        = new Label();
            this.dtpEnd        = new DateTimePicker();
            this.lblDuration   = new Label();

            // Section: Options
            this.panelOptions  = new Panel();
            this.lblSecOptions = new Label();
            this.lblReminder   = new Label();
            this.cboReminder   = new ComboBox();
            this.chkIsGroup    = new CheckBox();

            // Section: Group Info (hiện khi chkIsGroup = true)
            this.panelGroup      = new Panel();
            this.lblSecGroup     = new Label();
            this.lblOrganizer    = new Label();
            this.txtOrganizer    = new TextBox();
            this.lblMaxPart      = new Label();
            this.numMaxPart      = new NumericUpDown();
            this.lblAgenda       = new Label();
            this.txtAgenda       = new TextBox();
            this.lblJoinLink     = new Label();
            this.txtJoinLink     = new TextBox();

            // Buttons
            this.panelFooter   = new Panel();
            this.btnSave       = new Button();
            this.btnCancel     = new Button();

            this.panelHeader.SuspendLayout();
            this.panelBasic.SuspendLayout();
            this.panelTime.SuspendLayout();
            this.panelOptions.SuspendLayout();
            this.panelGroup.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();

            // ── Header ────────────────────────────────────────────
            this.panelHeader.Dock      = DockStyle.Top;
            this.panelHeader.Height    = 62;
            this.panelHeader.BackColor = Color.FromArgb(24, 48, 120);
            this.panelHeader.Controls.AddRange(new Control[] { this.lblTitle, this.lblSubtitle });
            this.panelHeader.Paint += (s, e) =>
            {
                using var br = new LinearGradientBrush(panelHeader.ClientRectangle,
                    Color.FromArgb(24, 48, 120), Color.FromArgb(49, 99, 215), LinearGradientMode.Horizontal);
                e.Graphics.FillRectangle(br, panelHeader.ClientRectangle);
            };

            this.lblTitle.Text      = "📅  Thêm lịch hẹn mới";
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Font      = new Font("Segoe UI", 13, FontStyle.Bold);
            this.lblTitle.AutoSize  = true;
            this.lblTitle.Location  = new Point(16, 10);
            this.lblTitle.BackColor = Color.Transparent;

            this.lblSubtitle.Text      = "Điền đầy đủ thông tin để thêm cuộc hẹn";
            this.lblSubtitle.ForeColor = Color.FromArgb(180, 210, 255);
            this.lblSubtitle.Font      = new Font("Segoe UI", 8);
            this.lblSubtitle.AutoSize  = true;
            this.lblSubtitle.Location  = new Point(19, 40);
            this.lblSubtitle.BackColor = Color.Transparent;

            // ── Section: Basic Info ───────────────────────────────
            this.panelBasic.Location  = new Point(15, 75);
            this.panelBasic.Size      = new Size(390, 160);
            this.panelBasic.BackColor = Color.White;
            this.panelBasic.Controls.AddRange(new Control[]
                { this.lblSecBasic, this.lblName, this.txtName, this.lblLocation, this.txtLocation });
            RoundPanel(this.panelBasic);

            MakeSectionLabel(this.lblSecBasic, "📌  Thông tin cơ bản", new Point(12, 10));

            MakeLabel(this.lblName, "Tên cuộc hẹn  *", new Point(12, 38));
            MakeTextBox(this.txtName, new Point(12, 58), new Size(366, 30));

            MakeLabel(this.lblLocation, "Địa điểm", new Point(12, 97));
            MakeTextBox(this.txtLocation, new Point(12, 117), new Size(366, 30));
            this.txtLocation.PlaceholderText = "Ví dụ: Phòng A101, Online...";

            // ── Section: Time ─────────────────────────────────────
            this.panelTime.Location  = new Point(15, 248);
            this.panelTime.Size      = new Size(390, 130);
            this.panelTime.BackColor = Color.White;
            this.panelTime.Controls.AddRange(new Control[]
                { this.lblSecTime, this.lblStart, this.dtpStart, this.lblEnd, this.dtpEnd, this.lblDuration });
            RoundPanel(this.panelTime);

            MakeSectionLabel(this.lblSecTime, "🕐  Thời gian", new Point(12, 10));

            MakeLabel(this.lblStart, "Giờ bắt đầu  *", new Point(12, 38));
            this.dtpStart.Format       = DateTimePickerFormat.Custom;
            this.dtpStart.CustomFormat = "dd/MM/yyyy   HH:mm";
            this.dtpStart.Font         = new Font("Segoe UI", 10);
            this.dtpStart.Location     = new Point(12, 57);
            this.dtpStart.Size         = new Size(176, 30);
            this.dtpStart.ValueChanged += new EventHandler(this.dtpStart_ValueChanged);

            MakeLabel(this.lblEnd, "Giờ kết thúc  *", new Point(202, 38));
            this.dtpEnd.Format       = DateTimePickerFormat.Custom;
            this.dtpEnd.CustomFormat = "dd/MM/yyyy   HH:mm";
            this.dtpEnd.Font         = new Font("Segoe UI", 10);
            this.dtpEnd.Location     = new Point(202, 57);
            this.dtpEnd.Size         = new Size(176, 30);
            this.dtpEnd.ValueChanged += new EventHandler(this.dtpEnd_ValueChanged);

            this.lblDuration.Text      = "";
            this.lblDuration.Font      = new Font("Segoe UI", 8, FontStyle.Italic);
            this.lblDuration.ForeColor = Color.FromArgb(100, 130, 200);
            this.lblDuration.AutoSize  = true;
            this.lblDuration.Location  = new Point(12, 97);

            // ── Section: Options ──────────────────────────────────
            this.panelOptions.Location  = new Point(15, 393);
            this.panelOptions.Size      = new Size(390, 105);
            this.panelOptions.BackColor = Color.White;
            this.panelOptions.Controls.AddRange(new Control[]
                { this.lblSecOptions, this.lblReminder, this.cboReminder, this.chkIsGroup });
            RoundPanel(this.panelOptions);

            MakeSectionLabel(this.lblSecOptions, "⚙️  Tùy chọn", new Point(12, 10));

            MakeLabel(this.lblReminder, "Nhắc nhở trước", new Point(12, 38));
            this.cboReminder.Font          = new Font("Segoe UI", 10);
            this.cboReminder.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboReminder.Location      = new Point(12, 57);
            this.cboReminder.Size          = new Size(180, 30);
            this.cboReminder.Items.AddRange(new object[]
                { "Không nhắc", "10 phút trước", "30 phút trước", "1 giờ trước" });
            this.cboReminder.SelectedIndex = 0;

            this.chkIsGroup.Text             = "Đây là cuộc họp nhóm";
            this.chkIsGroup.Font             = new Font("Segoe UI", 10);
            this.chkIsGroup.ForeColor        = Color.FromArgb(30, 40, 80);
            this.chkIsGroup.AutoSize         = true;
            this.chkIsGroup.Location         = new Point(210, 60);
            this.chkIsGroup.Cursor           = Cursors.Hand;
            this.chkIsGroup.CheckedChanged  += new EventHandler(this.chkIsGroup_CheckedChanged);

            // ── Section: Group Info ───────────────────────────────
            this.panelGroup.Location  = new Point(15, 511);
            this.panelGroup.Size      = new Size(390, 210);
            this.panelGroup.BackColor = Color.White;
            this.panelGroup.Visible   = false;   // ẩn mặc định
            this.panelGroup.Controls.AddRange(new Control[]
            {
                this.lblSecGroup,
                this.lblOrganizer, this.txtOrganizer,
                this.lblMaxPart,   this.numMaxPart,
                this.lblAgenda,    this.txtAgenda,
                this.lblJoinLink,  this.txtJoinLink
            });
            RoundPanel(this.panelGroup);

            MakeSectionLabel(this.lblSecGroup, "👥  Thông tin họp nhóm", new Point(12, 10));

            MakeLabel(this.lblOrganizer, "Người tổ chức", new Point(12, 38));
            MakeTextBox(this.txtOrganizer, new Point(12, 56), new Size(175, 28));
            this.txtOrganizer.PlaceholderText = "Tên người chủ trì...";

            MakeLabel(this.lblMaxPart, "Số người tối đa (0 = không giới hạn)", new Point(200, 38));
            this.numMaxPart.Location  = new Point(200, 56);
            this.numMaxPart.Size      = new Size(178, 28);
            this.numMaxPart.Font      = new Font("Segoe UI", 10);
            this.numMaxPart.Minimum   = 0;
            this.numMaxPart.Maximum   = 9999;
            this.numMaxPart.Value     = 0;

            MakeLabel(this.lblAgenda, "Chương trình / Nội dung", new Point(12, 93));
            this.txtAgenda.Location    = new Point(12, 111);
            this.txtAgenda.Size        = new Size(366, 48);
            this.txtAgenda.Font        = new Font("Segoe UI", 9);
            this.txtAgenda.Multiline   = true;
            this.txtAgenda.BorderStyle = BorderStyle.FixedSingle;
            this.txtAgenda.PlaceholderText = "Mô tả nội dung cuộc họp...";

            MakeLabel(this.lblJoinLink, "Link tham gia (online)", new Point(12, 167));
            MakeTextBox(this.txtJoinLink, new Point(12, 185), new Size(366, 28));
            this.txtJoinLink.PlaceholderText = "https://meet.google.com/... hoặc để trống";

            // ── Footer: Buttons ───────────────────────────────────
            this.panelFooter.Dock      = DockStyle.Bottom;
            this.panelFooter.Height    = 62;
            this.panelFooter.BackColor = Color.FromArgb(245, 247, 252);
            this.panelFooter.Controls.AddRange(new Control[] { this.btnSave, this.btnCancel });
            this.panelFooter.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(210, 218, 240));
                e.Graphics.DrawLine(pen, 0, 0, panelFooter.Width, 0);
            };

            this.btnSave.Text                       = "💾  Lưu lịch hẹn";
            this.btnSave.Font                       = new Font("Segoe UI", 10, FontStyle.Bold);
            this.btnSave.BackColor                  = Color.FromArgb(37, 99, 220);
            this.btnSave.ForeColor                  = Color.White;
            this.btnSave.FlatStyle                  = FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize  = 0;
            this.btnSave.Size                       = new Size(160, 38);
            this.btnSave.Location                   = new Point(15, 12);
            this.btnSave.Cursor                     = Cursors.Hand;
            this.btnSave.Click                     += new EventHandler(this.btnSave_Click);

            this.btnCancel.Text                     = "✖  Hủy";
            this.btnCancel.Font                     = new Font("Segoe UI", 10);
            this.btnCancel.BackColor                = Color.FromArgb(100, 110, 130);
            this.btnCancel.ForeColor                = Color.White;
            this.btnCancel.FlatStyle                = FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Size                     = new Size(100, 38);
            this.btnCancel.Location                 = new Point(185, 12);
            this.btnCancel.Cursor                   = Cursors.Hand;
            this.btnCancel.Click                   += new EventHandler(this.btnCancel_Click);

            // ── Form ──────────────────────────────────────────────
            this.Text            = "Thêm lịch hẹn";
            this.ClientSize      = new Size(420, 572);   // sẽ được điều chỉnh động
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition   = FormStartPosition.CenterParent;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.BackColor       = Color.FromArgb(238, 242, 250);
            this.AutoScroll      = true;

            this.Controls.Add(this.panelBasic);
            this.Controls.Add(this.panelTime);
            this.Controls.Add(this.panelOptions);
            this.Controls.Add(this.panelGroup);     // panel nhóm thêm vào sau options
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelFooter);

            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelBasic.ResumeLayout(false);
            this.panelBasic.PerformLayout();
            this.panelTime.ResumeLayout(false);
            this.panelTime.PerformLayout();
            this.panelOptions.ResumeLayout(false);
            this.panelOptions.PerformLayout();
            this.panelGroup.ResumeLayout(false);
            this.panelGroup.PerformLayout();
            this.panelFooter.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        // ── UI helpers ────────────────────────────────────────────
        private static void MakeSectionLabel(Label lbl, string text, Point loc)
        {
            lbl.Text      = text;
            lbl.Font      = new Font("Segoe UI", 9, FontStyle.Bold);
            lbl.ForeColor = Color.FromArgb(37, 99, 220);
            lbl.AutoSize  = true;
            lbl.Location  = loc;
        }

        private static void MakeLabel(Label lbl, string text, Point loc)
        {
            lbl.Text      = text;
            lbl.Font      = new Font("Segoe UI", 8, FontStyle.Bold);
            lbl.ForeColor = Color.FromArgb(80, 90, 120);
            lbl.AutoSize  = true;
            lbl.Location  = loc;
        }

        private static void MakeTextBox(TextBox txt, Point loc, Size size)
        {
            txt.Font        = new Font("Segoe UI", 10);
            txt.Location    = loc;
            txt.Size        = size;
            txt.BorderStyle = BorderStyle.FixedSingle;
        }

        private static void RoundPanel(Panel p)
        {
            p.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(220, 228, 245));
                e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
            };
        }

        // ── Controls ─────────────────────────────────────────────
        private Panel          panelHeader, panelBasic, panelTime, panelOptions, panelGroup, panelFooter;
        private Label          lblTitle, lblSubtitle;
        private Label          lblSecBasic, lblName, lblLocation;
        private Label          lblSecTime, lblStart, lblEnd, lblDuration;
        private Label          lblSecOptions, lblReminder;
        private Label          lblSecGroup, lblOrganizer, lblMaxPart, lblAgenda, lblJoinLink;
        private TextBox        txtName, txtLocation;
        private TextBox        txtOrganizer, txtAgenda, txtJoinLink;
        private DateTimePicker dtpStart, dtpEnd;
        private ComboBox       cboReminder;
        private CheckBox       chkIsGroup;
        private NumericUpDown  numMaxPart;
        private Button         btnSave, btnCancel;
    }
}