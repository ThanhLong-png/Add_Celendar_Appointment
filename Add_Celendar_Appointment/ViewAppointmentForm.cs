using Add_Celendar_Appointment.Models;
using System;
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
            lblTitle.Text = _appt.Name;
            lblTime.Text = $"{_appt.StartTime:dd/MM/yyyy HH:mm} - {_appt.EndTime:HH:mm}";
            lblLocation.Text = string.IsNullOrEmpty(_appt.Location) ? "📍 Không có địa điểm" : $"📍 {_appt.Location}";
            
            if (_appt.IsGroupMeeting)
            {
                lblType.Text = "👥 Họp nhóm";
                lblType.ForeColor = System.Drawing.Color.MediumBlue;
            }
            else
            {
                lblType.Text = "👤 Cá nhân";
                lblType.ForeColor = System.Drawing.Color.DarkGreen;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa lịch hẹn này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
