using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Add_Celendar_Appointment.Models
{
    public static class CalendarData
    {
        public static List<Appointment> Appointments { get; private set; } = new List<Appointment>();

        private static int _nextId = 1;

        // ── File path: %LocalAppData%\CalendarAppointment\data.json ──
        private static readonly string _filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CalendarAppointment", "data.json");

        private static readonly JsonSerializerOptions _jsonOpts = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        // ── Load from disk ─────────────────────────────────────────
        public static void Load()
        {
            try
            {
                if (!File.Exists(_filePath)) return;

                string json = File.ReadAllText(_filePath);
                var saved   = JsonSerializer.Deserialize<SaveData>(json, _jsonOpts);
                if (saved == null) return;

                Appointments = saved.Appointments ?? new List<Appointment>();
                _nextId      = saved.NextId > 0 ? saved.NextId : ComputeNextId();
            }
            catch
            {
                // If file is corrupted, start fresh
                Appointments = new List<Appointment>();
                _nextId      = 1;
            }
        }

        // ── Save to disk ───────────────────────────────────────────
        public static void Save()
        {
            try
            {
                string dir = Path.GetDirectoryName(_filePath)!;
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var data = new SaveData { Appointments = Appointments, NextId = _nextId };
                string json = JsonSerializer.Serialize(data, _jsonOpts);
                File.WriteAllText(_filePath, json);
            }
            catch { /* ignore save errors silently */ }
        }

        // ── CRUD ───────────────────────────────────────────────────
        public static void AddAppointment(Appointment a)
        {
            a.Id = _nextId++;
            Appointments.Add(a);
            Save();
        }

        public static void RemoveAppointment(int id)
        {
            Appointments.RemoveAll(a => a.Id == id);
            Save();
        }

        public static Appointment FindConflict(DateTime start, DateTime end, int excludeId = -1)
        {
            foreach (var a in Appointments)
                if (a.Id != excludeId && a.StartTime < end && a.EndTime > start)
                    return a;
            return null;
        }

        public static Appointment FindGroupMeeting(string name, double durationMinutes)
        {
            foreach (var a in Appointments)
                if (a.IsGroupMeeting
                    && a.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                    && Math.Abs(a.DurationMinutes - durationMinutes) < 1)
                    return a;
            return null;
        }

        private static int ComputeNextId()
        {
            int max = 0;
            foreach (var a in Appointments)
                if (a.Id > max) max = a.Id;
            return max + 1;
        }

        // ── DTO for serialization ──────────────────────────────────
        private class SaveData
        {
            public List<Appointment> Appointments { get; set; } = new List<Appointment>();
            public int NextId { get; set; } = 1;
        }
    }
}