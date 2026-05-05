using System;
using System.Collections.Generic;

namespace Add_Celendar_Appointment.Models
{
    /// <summary>
    /// Cuộc họp nhóm – kế thừa từ Appointment và bổ sung thông tin nhóm.
    /// </summary>
    public class GroupMeeting : Appointment
    {
        // ── Thông tin nhóm ────────────────────────────────────────
        /// <summary>Người tổ chức / chủ trì cuộc họp.</summary>
        public string Organizer { get; set; } = string.Empty;

        /// <summary>Số người tham gia tối đa (0 = không giới hạn).</summary>
        public int MaxParticipants { get; set; } = 0;

        /// <summary>Chương trình / nội dung cuộc họp.</summary>
        public string Agenda { get; set; } = string.Empty;

        /// <summary>Link tham gia (nếu họp online).</summary>
        public string JoinLink { get; set; } = string.Empty;

        // ── Constructor ───────────────────────────────────────────
        public GroupMeeting()
        {
            // GroupMeeting luôn là cuộc họp nhóm
            IsGroupMeeting = true;
            Participants   = new List<string>();
        }

        // ── Helpers ───────────────────────────────────────────────
        /// <summary>Kiểm tra còn chỗ cho người tham gia mới không.</summary>
        public bool HasAvailableSlot =>
            MaxParticipants == 0 || Participants.Count < MaxParticipants;

        /// <summary>Thêm người tham gia nếu còn chỗ, trả về true nếu thành công.</summary>
        public bool AddParticipant(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            if (Participants.Contains(name))    return false;   // đã có rồi
            if (!HasAvailableSlot)               return false;   // hết chỗ
            Participants.Add(name);
            return true;
        }

        /// <summary>Xóa một người tham gia.</summary>
        public bool RemoveParticipant(string name) =>
            Participants.Remove(name);

        /// <summary>Tóm tắt số người tham gia.</summary>
        public string ParticipantSummary =>
            MaxParticipants == 0
                ? $"{Participants.Count} người"
                : $"{Participants.Count}/{MaxParticipants} người";
    }
}
