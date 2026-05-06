using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Add_Celendar_Appointment.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(Appointment),   typeDiscriminator: "appointment")]
    [JsonDerivedType(typeof(GroupMeeting),  typeDiscriminator: "group")]
    public class Appointment
    {
        public int      Id             { get; set; }
        public string   Name           { get; set; }
        public string   Location       { get; set; }
        public DateTime StartTime      { get; set; }
        public DateTime EndTime        { get; set; }
        public bool     IsGroupMeeting { get; set; }

        public List<string>   Participants { get; set; } = new List<string>();
        public List<Reminder> Reminders   { get; set; } = new List<Reminder>();

        [JsonIgnore]
        public double DurationMinutes => (EndTime - StartTime).TotalMinutes;

        public Appointment()
        {
            IsGroupMeeting = false;
            Participants   = new List<string>();
            Reminders      = new List<Reminder>();
        }
    }
}