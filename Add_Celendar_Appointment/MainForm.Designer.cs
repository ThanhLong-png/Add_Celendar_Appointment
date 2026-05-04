using System;
using System.Drawing;
using System.Windows.Forms;

namespace Add_Celendar_Appointment
{
    partial class MainForm : Form
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelTop      = new Panel();
            btnViewMode   = new Button();
            panelLeft     = new Panel();
            panelMain     = new Panel();
            vScrollBar    = new VScrollBar();
            panelMiniCal  = new Panel();
            panelDayHdr   = new Panel();
            panelGrid     = new Panel();
            lblAppName    = new Label();
            lblMonthYear  = new Label();
            btnToday      = new Button();
            btnPrev       = new Button();
            btnNext       = new Button();
            btnCreate     = new Button();
            lblMyCals     = new Label();
            chkMyCal      = new CheckBox();
            chkGroupMeeting = new CheckBox();
            timerRefresh  = new Timer();

            panelTop.SuspendLayout();
            panelLeft.SuspendLayout();
            panelMain.SuspendLayout();
            SuspendLayout();

            // ── TOP BAR ──────────────────────────────────────────────
            panelTop.Dock      = DockStyle.Top;
            panelTop.Width     = 1100;
            panelTop.Height    = 64;
            panelTop.BackColor = Color.White;
            panelTop.Paint    += (s, e) =>
            {
                using var p = new System.Drawing.Pen(Color.FromArgb(218, 220, 224));
                e.Graphics.DrawLine(p, 0, panelTop.Height - 1, panelTop.Width, panelTop.Height - 1);
            };
            panelTop.Controls.AddRange(new Control[] { lblAppName, btnToday, btnPrev, btnNext, lblMonthYear, btnCreate, btnViewMode });

            lblAppName.Text      = "📅 Calendar";
            lblAppName.Font      = new Font("Segoe UI", 17);
            lblAppName.ForeColor = Color.FromArgb(95, 99, 104);
            lblAppName.AutoSize  = true;
            lblAppName.Location  = new Point(16, 14);

            btnToday.Text = "Hôm nay";
            btnToday.Location = new Point(230, 14);
            btnToday.Size = new Size(88, 36);
            btnToday.Cursor = Cursors.Hand;
            btnToday.Click += (s, e) => { _currentMonth = DateTime.Today; _selectedDate = DateTime.Today; RefreshAll(); };

            btnPrev.Text = "‹";
            btnPrev.Font = new Font("Segoe UI", 16);
            btnPrev.Location = new Point(328, 12);
            btnPrev.Size = new Size(38, 38);
            btnPrev.Cursor = Cursors.Hand;
            btnPrev.Click += new EventHandler(btnPrev_Click);

            btnNext.Text = "›";
            btnNext.Font = new Font("Segoe UI", 16);
            btnNext.Location = new Point(368, 12);
            btnNext.Size = new Size(38, 38);
            btnNext.Cursor = Cursors.Hand;
            btnNext.Click += new EventHandler(btnNext_Click);

            lblMonthYear.Text      = "";
            lblMonthYear.Font      = new Font("Segoe UI", 17);
            lblMonthYear.ForeColor = Color.FromArgb(60, 64, 67);
            lblMonthYear.AutoSize  = true;
            lblMonthYear.Location  = new Point(414, 16);

            btnCreate.Text = "+  Tạo";
            btnCreate.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnCreate.Size = new Size(100, 36);
            btnCreate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCreate.Location = new Point(860, 14);
            btnCreate.Cursor = Cursors.Hand;
            btnCreate.Click += new EventHandler(btnCreate_Click);

            btnViewMode.Text = "Tháng ▼";
            btnViewMode.Font = new Font("Segoe UI", 10);
            btnViewMode.Size = new Size(100, 36);
            btnViewMode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnViewMode.Location = new Point(975, 14);
            btnViewMode.Cursor = Cursors.Hand;

            // ── LEFT SIDEBAR ─────────────────────────────────────────
            panelLeft.Dock      = DockStyle.Left;
            panelLeft.Width     = 256;
            panelLeft.BackColor = Color.FromArgb(248, 249, 250);
            panelLeft.Paint    += (s, e) =>
            {
                using var p = new System.Drawing.Pen(Color.FromArgb(218, 220, 224));
                e.Graphics.DrawLine(p, panelLeft.Width - 1, 0, panelLeft.Width - 1, panelLeft.Height);
            };
            panelLeft.Controls.AddRange(new Control[] { panelMiniCal, lblMyCals, chkMyCal, chkGroupMeeting });

            panelMiniCal.Location   = new Point(8, 8);
            panelMiniCal.Size       = new Size(240, 220);
            panelMiniCal.BackColor  = Color.FromArgb(248, 249, 250);
            panelMiniCal.Paint     += new PaintEventHandler(panelMiniCal_Paint);
            panelMiniCal.MouseClick+= new MouseEventHandler(panelMiniCal_MouseClick);

            lblMyCals.Text      = "Lịch của tôi";
            lblMyCals.Font      = new Font("Segoe UI", 9, FontStyle.Bold);
            lblMyCals.ForeColor = Color.FromArgb(60, 64, 67);
            lblMyCals.AutoSize  = true;
            lblMyCals.Location  = new Point(12, 238);

            chkMyCal.Text      = "Cá nhân";
            chkMyCal.Font      = new Font("Segoe UI", 10);
            chkMyCal.Checked   = true;
            chkMyCal.AutoSize  = true;
            chkMyCal.Location  = new Point(14, 260);
            chkMyCal.ForeColor = Color.FromArgb(60, 64, 67);
            chkMyCal.CheckedChanged += (s, e) => panelGrid.Invalidate();

            chkGroupMeeting.Text      = "Họp nhóm";
            chkGroupMeeting.Font      = new Font("Segoe UI", 10);
            chkGroupMeeting.Checked   = true;
            chkGroupMeeting.AutoSize  = true;
            chkGroupMeeting.Location  = new Point(14, 285);
            chkGroupMeeting.ForeColor = Color.FromArgb(60, 64, 67);
            chkGroupMeeting.CheckedChanged += (s, e) => panelGrid.Invalidate();

            // ── MAIN AREA ─────────────────────────────────────────────
            panelMain.Dock      = DockStyle.Fill;
            panelMain.BackColor = Color.White;
            panelMain.Controls.AddRange(new Control[] { panelGrid, vScrollBar, panelDayHdr });

            panelDayHdr.Dock      = DockStyle.Top;
            panelDayHdr.Height    = 32;
            panelDayHdr.BackColor = Color.White;
            panelDayHdr.Paint    += new PaintEventHandler(panelDayHdr_Paint);

            vScrollBar.Dock = DockStyle.Right;
            vScrollBar.Visible = false;
            vScrollBar.Scroll += (s, e) => panelGrid.Invalidate();

            panelGrid.Dock       = DockStyle.Fill;
            panelGrid.BackColor  = Color.White;
            panelGrid.Paint     += new PaintEventHandler(panelGrid_Paint);
            panelGrid.MouseClick+= new MouseEventHandler(panelGrid_MouseClick);

            timerRefresh.Interval = 60000;
            timerRefresh.Tick    += (s, e) => panelGrid.Invalidate();
            timerRefresh.Start();

            // ── FORM ──────────────────────────────────────────────────
            Text          = "Calendar";
            ClientSize    = new Size(1100, 700);
            MinimumSize   = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor     = Color.White;

            Controls.Add(panelMain);
            Controls.Add(panelLeft);
            Controls.Add(panelTop);

            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelLeft.ResumeLayout(false);
            panelLeft.PerformLayout();
            panelMain.ResumeLayout(false);
            ResumeLayout(false);
        }

        private CheckBox chkGroupMeeting;

        private Panel panelTop, panelLeft, panelMain, panelMiniCal, panelDayHdr, panelGrid;
        private Label lblAppName, lblMonthYear, lblMyCals;
        private Button btnToday, btnPrev, btnNext, btnCreate;
        private CheckBox chkMyCal;
        private Timer timerRefresh;
        private Button btnViewMode;
        private VScrollBar vScrollBar;
    }
}