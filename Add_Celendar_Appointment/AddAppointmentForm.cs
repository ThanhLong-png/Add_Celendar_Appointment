using Add_Celendar_Appointment.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Add_Celendar_Appointment
{
    public partial class AddAppointmentForm : Form
    {
        private const int FormHeightNormal = 572;
        private const int FormHeightGroup  = 572 + 220;

        public AddAppointmentForm(DateTime defaultStart)
        {
            InitializeComponent();
            dtpStart.Value = defaultStart;
            dtpEnd.Value   = defaultStart.AddHours(1);
            UpdateDurationLabel();
        }

        private void chkIsGroup_CheckedChanged(object sender, EventArgs e)
        {
            bool isGroup       = chkIsGroup.Checked;
            panelGroup.Visible = isGroup;
            this.ClientSize    = new System.Drawing.Size(420, isGroup ? FormHeightGroup : FormHeightNormal);

            panelGroup.Location = new System.Drawing.Point(15,
                panelOptions.Bottom + 8);
        }

        private void UpdateDurationLabel()
        {
            var diff = dtpEnd.Value - dtpStart.Value;
            if (diff.TotalMinutes <= 0)
            {
                lblDuration.Text      = "⚠️  Giờ kết thúc phải sau giờ bắt đầu!";
                lblDuration.ForeColor = System.Drawing.Color.FromArgb(200, 40, 40);
            }
            else
            {
                string dur = diff.TotalHours >= 1
                    ? $"⏱  Thời lượng: {(int)diff.TotalHours} giờ {diff.Minutes} phút"
                    : $"⏱  Thời lượng: {(int)diff.TotalMinutes} phút";
                lblDuration.Text      = dur;
                lblDuration.ForeColor = System.Drawing.Color.FromArgb(60, 120, 200);
            }
        }

        private void dtpStart_ValueChanged(object sender, EventArgs e) => UpdateDurationLabel();
        private void dtpEnd_ValueChanged(object sender, EventArgs e)   => UpdateDurationLabel();

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Tên cuộc hẹn không được để trống!",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return;
            }

            DateTime start    = dtpStart.Value;
            DateTime end      = dtpEnd.Value;

            if (end <= start)
            {
                MessageBox.Show("Giờ kết thúc phải sau giờ bắt đầu!",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtpEnd.Focus();
                return;
            }

            string name     = txtName.Text.Trim();
            double duration = (end - start).TotalMinutes;

            Appointment conflict = CalendarData.FindConflict(start, end);
            if (conflict != null)
            {
                var choice = MessageBox.Show(
                    $"⚠️  Trùng lịch với:\n\n\"{conflict.Name}\"\n" +
                    $"({conflict.StartTime:HH:mm} – {conflict.EndTime:HH:mm})\n\n" +
                    "YES = Thay thế lịch cũ\nNO = Chọn giờ khác",
                    "Phát hiện trùng lịch", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (choice == DialogResult.No) return;
                CalendarData.RemoveAppointment(conflict.Id);
            }

            GroupMeeting existingGroup = CalendarData.FindGroupMeeting(name, duration);
            if (existingGroup != null)
            {
                string slotInfo = existingGroup.MaxParticipants == 0
                    ? $"({existingGroup.Participants.Count} người đã tham gia)"
                    : $"({existingGroup.Participants.Count}/{existingGroup.MaxParticipants} người)";

                var join = MessageBox.Show(
                    $"🔔  Đã có cuộc họp nhóm cùng tên:\n\n\"{existingGroup.Name}\" {slotInfo}\n\n" +
                    "YES = Tham gia nhóm họp này\nNO = Tạo lịch hẹn riêng",
                    "Tham gia họp nhóm?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (join == DialogResult.Yes)
                {
                    if (!existingGroup.HasAvailableSlot)
                    {
                        MessageBox.Show("❌  Cuộc họp này đã đủ người tham gia!",
                            "Không thể tham gia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    bool added = existingGroup.AddParticipant("Tôi");
                    CalendarData.Save();
                    MessageBox.Show(
                        $"✅  Đã tham gia \"{existingGroup.Name}\"\n" +
                        $"Số người: {existingGroup.ParticipantSummary}",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                    return;
                }
            }

            int[] minutesMap = { 0, 10, 30, 60 };
            int   minutes    = minutesMap[cboReminder.SelectedIndex];

            if (chkIsGroup.Checked)
            {
                var gm = new GroupMeeting
                {
                    Name           = name,
                    Location       = txtLocation.Text.Trim(),
                    StartTime      = start,
                    EndTime        = end,
                    Organizer      = txtOrganizer.Text.Trim(),
                    MaxParticipants = (int)numMaxPart.Value,
                    Agenda         = txtAgenda.Text.Trim(),
                    JoinLink       = txtJoinLink.Text.Trim(),
                    Participants   = new List<string> { "Tôi" }
                };
                if (minutes > 0)
                    gm.Reminders.Add(new Reminder { MinutesBefore = minutes });

                CalendarData.AddGroupMeeting(gm);
            }
            else
            {
                var appointment = new Appointment
                {
                    Name           = name,
                    Location       = txtLocation.Text.Trim(),
                    StartTime      = start,
                    EndTime        = end,
                    IsGroupMeeting = false
                };
                if (minutes > 0)
                    appointment.Reminders.Add(new Reminder { MinutesBefore = minutes });

                CalendarData.AddAppointment(appointment);
            }

            MessageBox.Show("✅  Đã thêm lịch hẹn thành công!",
                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}