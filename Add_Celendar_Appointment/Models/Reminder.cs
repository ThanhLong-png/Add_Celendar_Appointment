namespace Add_Celendar_Appointment.Models
{
    public class Reminder
    {
        public int MinutesBefore { get; set; }
        public string Label
        {
            get
            {
                return MinutesBefore >= 60
                    ? (MinutesBefore / 60) + " giờ trước"
                    : MinutesBefore + " phút trước";
            }
        }
    }
}