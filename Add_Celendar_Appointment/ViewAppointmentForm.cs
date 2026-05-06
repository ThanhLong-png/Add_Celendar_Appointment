using Add_Celendar_Appointment.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Add_Celendar_Appointment
{
    public partial class ViewAppointmentForm : Form
    {
        private Appointment _appt;

        public ViewAppointmentForm(Appointment appt)
        {
            InitializeComponent();
            _appt = appt;
            LoadData();
        }

        private void LoadData()
        {
            lblTitle.Text    = _appt.Name;
            lblTime.Text     = $"🕐 {_appt.StartTime:dd/MM/yyyy HH:mm} – {_appt.EndTime:HH:mm}";
            lblLocation.Text = string.IsNullOrEmpty(_appt.Location)
                ? "📍 Không có địa điểm"
                : $"📍 {_appt.Location}";

            if (_appt is GroupMeeting gm)
            {
                lblType.Text      = "👥 Cuộc họp nhóm";
                lblType.ForeColor = Color.MediumBlue;

                panelGroupInfo.Visible = true;

                lblOrganizer.Text = string.IsNullOrEmpty(gm.Organizer)
                    ? "👤 Người tổ chức: (chưa có)"
                    : $"👤 Người tổ chức: {gm.Organizer}";

                lblParticipants.Text = $"👥 Người tham gia: {gm.ParticipantSummary}";

                if (!string.IsNullOrEmpty(gm.Agenda))
                    lblAgenda.Text = $"📋 Nội dung: {gm.Agenda}";
                else
                    lblAgenda.Text = "📋 Nội dung: (chưa có)";

                if (!string.IsNullOrEmpty(gm.JoinLink))
                    lblJoinLink.Text = $"🔗 {gm.JoinLink}";
                else
                    lblJoinLink.Visible = false;

                int btnY = panelGroupInfo.Bottom + 14;
                btnDelete.Location = new Point(25, btnY);
                btnClose.Location  = new Point(140, btnY);
                this.ClientSize    = new Size(400, btnY + 35 + 12);
            }
            else
            {
                lblType.Text      = "👤 Lịch cá nhân";
                lblType.ForeColor = Color.DarkGreen;

                panelGroupInfo.Visible = false;

                btnDelete.Location = new Point(25, 160);
                btnClose.Location  = new Point(140, 160);
                this.ClientSize    = new Size(400, 220);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa lịch hẹn này?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CalendarData.RemoveAppointment(_appt.Id);
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
