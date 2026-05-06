using System;
using System.Collections.Generic;

namespace Add_Celendar_Appointment.Models
{
    public class GroupMeeting : Appointment
    {
        public string Organizer { get; set; } = string.Empty;

        public int MaxParticipants { get; set; } = 0;

        public string Agenda { get; set; } = string.Empty;

        public string JoinLink { get; set; } = string.Empty;

        public GroupMeeting()
        {
            IsGroupMeeting = true;
            Participants   = new List<string>();
        }

        public bool HasAvailableSlot =>
            MaxParticipants == 0 || Participants.Count < MaxParticipants;

        public bool AddParticipant(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            if (Participants.Contains(name))    return false;
            if (!HasAvailableSlot)               return false;
            Participants.Add(name);
            return true;
        }

        public bool RemoveParticipant(string name) =>
            Participants.Remove(name);

        public string ParticipantSummary =>
            MaxParticipants == 0
                ? $"{Participants.Count} người"
                : $"{Participants.Count}/{MaxParticipants} người";
    }
}
