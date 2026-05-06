using Add_Celendar_Appointment.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace Add_Celendar_Appointment
{
    public enum ViewMode { Day = 0, Week = 1, Month = 2, Year = 3 }

    public partial class MainForm : Form
    {
        private DateTime _currentMonth;
        private DateTime _selectedDate;
        private ViewMode _viewMode = ViewMode.Month;
        private List<Tuple<Rectangle, Appointment>> _hitTests = new List<Tuple<Rectangle, Appointment>>();
        private ContextMenuStrip ctxViewMode;

        private static readonly Color[] EventColors =
        {
            Color.FromArgb(26,115,232), Color.FromArgb(11,128,67),
            Color.FromArgb(230,67,60),  Color.FromArgb(249,171,0),
            Color.FromArgb(164,0,214),  Color.FromArgb(0,172,193),
            Color.FromArgb(255,113,0),
        };

        private static readonly string[] DayNames  = { "CN","T2","T3","T4","T5","T6","T7" };
        private static readonly string[] MonthNames =
        {
            "Tháng 1","Tháng 2","Tháng 3","Tháng 4","Tháng 5","Tháng 6",
            "Tháng 7","Tháng 8","Tháng 9","Tháng 10","Tháng 11","Tháng 12"
        };

        public MainForm()
        {
            InitializeComponent();
            
            var prop = typeof(Panel).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            prop?.SetValue(panelGrid, true, null);
            prop?.SetValue(panelMiniCal, true, null);
            prop?.SetValue(panelDayHdr, true, null);

            SetupRoundedButton(btnToday, Color.White, Color.FromArgb(60, 64, 67), 18, true);
            SetupRoundedButton(btnPrev, Color.White, Color.FromArgb(95, 99, 104), 18, false);
            SetupRoundedButton(btnNext, Color.White, Color.FromArgb(95, 99, 104), 18, false);
            SetupRoundedButton(btnCreate, Color.FromArgb(26, 115, 232), Color.White, 18, false);
            SetupRoundedButton(btnViewMode, Color.White, Color.FromArgb(60, 64, 67), 18, true);

            ctxViewMode = new ContextMenuStrip();
            ctxViewMode.Items.Add("Ngày", null, (s, e) => SetViewMode(ViewMode.Day));
            ctxViewMode.Items.Add("Tuần", null, (s, e) => SetViewMode(ViewMode.Week));
            ctxViewMode.Items.Add("Tháng", null, (s, e) => SetViewMode(ViewMode.Month));
            ctxViewMode.Items.Add("Năm", null, (s, e) => SetViewMode(ViewMode.Year));
            ctxViewMode.Font = new Font("Segoe UI", 10);
            
            btnViewMode.Click += (s, e) => ctxViewMode.Show(btnViewMode, new Point(0, btnViewMode.Height + 2));

            _currentMonth = DateTime.Today;
            _selectedDate = DateTime.Today;
            CalendarData.Load();
            RefreshAll();
            
            vScrollBar.Maximum = 24 * 60;
            vScrollBar.LargeChange = panelGrid.Height > 0 ? panelGrid.Height : 500;
        }

        private void SetupRoundedButton(Button b, Color bg, Color fg, int radius, bool border)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = Color.Transparent; 
            b.ForeColor = Color.Transparent; 
            
            b.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = new Rectangle(0, 0, b.Width - 1, b.Height - 1);
                
                Color drawBg = b.Tag is Color hoverColor ? hoverColor : bg;
                using var path = RoundedRect(rect, radius);
                using var brush = new SolidBrush(drawBg);
                e.Graphics.FillPath(brush, path);
                
                if (border) 
                {
                    using var p = new Pen(Color.FromArgb(218, 220, 224));
                    e.Graphics.DrawPath(p, path);
                }
                
                TextRenderer.DrawText(e.Graphics, b.Text, b.Font, rect, fg, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            
            b.MouseEnter += (s, e) => { 
                int amt = bg == Color.White ? -15 : 20;
                b.Tag = Color.FromArgb(Math.Clamp(bg.R + amt, 0, 255), Math.Clamp(bg.G + amt, 0, 255), Math.Clamp(bg.B + amt, 0, 255)); 
                b.Invalidate(); 
            };
            b.MouseLeave += (s, e) => { b.Tag = bg; b.Invalidate(); };
            b.MouseDown += (s, e) => { 
                int amt = bg == Color.White ? -30 : -20;
                b.Tag = Color.FromArgb(Math.Clamp(bg.R + amt, 0, 255), Math.Clamp(bg.G + amt, 0, 255), Math.Clamp(bg.B + amt, 0, 255)); 
                b.Invalidate(); 
            };
            b.MouseUp += (s, e) => { 
                int amt = bg == Color.White ? -15 : 20;
                b.Tag = Color.FromArgb(Math.Clamp(bg.R + amt, 0, 255), Math.Clamp(bg.G + amt, 0, 255), Math.Clamp(bg.B + amt, 0, 255)); 
                b.Invalidate(); 
            };
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (vScrollBar != null && panelGrid != null)
                vScrollBar.LargeChange = panelGrid.Height > 0 ? panelGrid.Height : 500;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if ((_viewMode == ViewMode.Day || _viewMode == ViewMode.Week) && vScrollBar.Visible)
            {
                int maxVal = Math.Max(0, vScrollBar.Maximum - vScrollBar.LargeChange + 1);
                int step = 60;
                int newValue = vScrollBar.Value + (e.Delta > 0 ? -step : step);
                newValue = Math.Clamp(newValue, 0, maxVal);
                
                if (vScrollBar.Value != newValue)
                {
                    vScrollBar.Value = newValue;
                    panelGrid.Invalidate();
                }
            }
        }

        private void SetViewMode(ViewMode mode)
        {
            _viewMode = mode;
            string modeName = mode switch { ViewMode.Day => "Ngày", ViewMode.Week => "Tuần", ViewMode.Month => "Tháng", ViewMode.Year => "Năm", _ => "" };
            btnViewMode.Text = $"{modeName} ▼";
            RefreshAll();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (_viewMode == ViewMode.Day) _selectedDate = _selectedDate.AddDays(-1);
            else if (_viewMode == ViewMode.Week) _selectedDate = _selectedDate.AddDays(-7);
            else if (_viewMode == ViewMode.Month) _currentMonth = _currentMonth.AddMonths(-1);
            else if (_viewMode == ViewMode.Year) _currentMonth = _currentMonth.AddYears(-1);
            
            if (_viewMode == ViewMode.Day || _viewMode == ViewMode.Week) 
                _currentMonth = new DateTime(_selectedDate.Year, _selectedDate.Month, 1);
                
            RefreshAll();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_viewMode == ViewMode.Day) _selectedDate = _selectedDate.AddDays(1);
            else if (_viewMode == ViewMode.Week) _selectedDate = _selectedDate.AddDays(7);
            else if (_viewMode == ViewMode.Month) _currentMonth = _currentMonth.AddMonths(1);
            else if (_viewMode == ViewMode.Year) _currentMonth = _currentMonth.AddYears(1);
            
            if (_viewMode == ViewMode.Day || _viewMode == ViewMode.Week) 
                _currentMonth = new DateTime(_selectedDate.Year, _selectedDate.Month, 1);
                
            RefreshAll();
        }

        private void RefreshAll()
        {
            if (_viewMode == ViewMode.Year) lblMonthYear.Text = $"{_currentMonth.Year}";
            else lblMonthYear.Text = $"{MonthNames[_currentMonth.Month - 1]} {_currentMonth.Year}";

            vScrollBar.Visible = (_viewMode == ViewMode.Day || _viewMode == ViewMode.Week);
            
            panelMiniCal.Invalidate();
            panelDayHdr.Invalidate();
            panelGrid.Invalidate();
        }


        private void panelDayHdr_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);
            
            using var sf  = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            using var pen = new Pen(Color.FromArgb(218,220,224));
            
            if (_viewMode == ViewMode.Month) 
            {
                int w = panelDayHdr.Width / 7;
                for (int c = 0; c < 7; c++)
                {
                    var r  = new Rectangle(c * w, 0, w, panelDayHdr.Height);
                    var fg = c == 0 ? Color.FromArgb(192,0,0) : c == 6 ? Color.FromArgb(70,70,200) : Color.FromArgb(95,99,104);
                    using var br  = new SolidBrush(fg);
                    g.DrawString(DayNames[c], new Font("Segoe UI",9,FontStyle.Bold), br, r, sf);
                }
            }
            else if (_viewMode == ViewMode.Week)
            {
                DateTime weekStart = _selectedDate.AddDays(-(int)_selectedDate.DayOfWeek);
                int timeWidth = 60;
                int colWidth = (panelDayHdr.Width - timeWidth) / 7;
                
                for (int c = 0; c < 7; c++)
                {
                    DateTime d = weekStart.AddDays(c);
                    var r  = new Rectangle(timeWidth + c * colWidth, 0, colWidth, panelDayHdr.Height);
                    var fg = c == 0 ? Color.FromArgb(192,0,0) : c == 6 ? Color.FromArgb(70,70,200) : Color.FromArgb(95,99,104);
                    using var br  = new SolidBrush(fg);
                    g.DrawString($"{DayNames[c]} {d:dd/MM}", new Font("Segoe UI",9,FontStyle.Bold), br, r, sf);
                }
            }
            else if (_viewMode == ViewMode.Day) 
            {
                string text = $"{DayNames[(int)_selectedDate.DayOfWeek]}, {_selectedDate:dd/MM/yyyy}";
                using var br  = new SolidBrush(Color.FromArgb(60,64,67));
                g.DrawString(text, new Font("Segoe UI",11,FontStyle.Bold), br, panelDayHdr.ClientRectangle, sf);
            }

            g.DrawLine(pen, 0, panelDayHdr.Height-1, panelDayHdr.Width, panelDayHdr.Height-1);
        }


        private void panelGrid_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode       = SmoothingMode.AntiAlias;
            g.TextRenderingHint   = TextRenderingHint.ClearTypeGridFit;
            g.Clear(Color.White);
            
            _hitTests.Clear();

            if (_viewMode == ViewMode.Month) DrawMonthView(g);
            else if (_viewMode == ViewMode.Day) DrawDayView(g);
            else if (_viewMode == ViewMode.Week) DrawWeekView(g);
            else if (_viewMode == ViewMode.Year) DrawYearView(g);
        }

        private void DrawMonthView(Graphics g)
        {
            int W = panelGrid.Width, H = panelGrid.Height;
            int cw = W / 7, rh = H / 6;
            DateTime start = GridStart();

            for (int row = 0; row < 6; row++)
            for (int col = 0; col < 7; col++)
            {
                DateTime day  = start.AddDays(row * 7 + col);
                var cell      = new Rectangle(col * cw, row * rh, cw, rh);
                bool curMonth = day.Month == _currentMonth.Month;
                bool isToday  = day.Date == DateTime.Today;

                Color bg = curMonth ? Color.White : Color.FromArgb(248,249,250);
                using (var br = new SolidBrush(bg)) g.FillRectangle(br, cell);

                using (var p = new Pen(Color.FromArgb(218,220,224)))
                {
                    g.DrawLine(p, cell.Right-1, cell.Top, cell.Right-1, cell.Bottom);
                    g.DrawLine(p, cell.Left, cell.Bottom-1, cell.Right, cell.Bottom-1);
                }

                string dayStr = day.Day.ToString();
                var numFont   = new Font("Segoe UI", 10, isToday ? FontStyle.Bold : FontStyle.Regular);
                Color numFg   = !curMonth ? Color.FromArgb(180,182,185)
                              : col == 0  ? Color.FromArgb(192,0,0)
                              : col == 6  ? Color.FromArgb(70,70,200)
                              : Color.FromArgb(60,64,67);

                if (isToday)
                {
                    int cx = cell.Left + 20, cy = cell.Top + 10;
                    using var todayBr = new SolidBrush(Color.FromArgb(26,115,232));
                    g.FillEllipse(todayBr, cx - 12, cy - 2, 24, 24);
                    using var wBr = new SolidBrush(Color.White);
                    using var sf2 = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    g.DrawString(dayStr, numFont, wBr, new RectangleF(cx - 12, cy - 2, 24, 24), sf2);
                }
                else
                {
                    using var br = new SolidBrush(numFg);
                    g.DrawString(dayStr, numFont, br, cell.Left + 8, cell.Top + 6);
                }
                numFont.Dispose();

                if (chkMyCal.Checked || chkGroupMeeting.Checked)
                    DrawEventChips(g, day, new Rectangle(cell.Left + 3, cell.Top + 32, cw - 6, rh - 36));
            }
        }

        private void DrawDayView(Graphics g)
        {
            int hourHeight = 60;
            int scrollY = vScrollBar.Value;
            int timeWidth = 60;
            int colWidth = panelGrid.Width - timeWidth - 10;
            
            using var font = new Font("Segoe UI", 9);
            using var pen = new Pen(Color.FromArgb(218, 220, 224));
            
            for (int i = 0; i <= 24; i++)
            {
                int y = i * hourHeight - scrollY;
                if (y < -hourHeight || y > panelGrid.Height) continue;
                
                string timeStr = i == 24 ? "0:00" : $"{i}:00";
                g.DrawString(timeStr, font, Brushes.Gray, 5, y - 8);
                g.DrawLine(pen, timeWidth, y, panelGrid.Width, y);
            }
            
            var appts = CalendarData.Appointments
                .Where(a => a.StartTime.Date <= _selectedDate.Date && a.EndTime >= _selectedDate.Date)
                .Where(a => (chkMyCal.Checked && !a.IsGroupMeeting) || (chkGroupMeeting.Checked && a.IsGroupMeeting))
                .OrderBy(a => a.StartTime)
                .ToList();
                
            foreach (var a in appts)
            {
                DateTime start = a.StartTime < _selectedDate.Date ? _selectedDate.Date : a.StartTime;
                DateTime end = a.EndTime > _selectedDate.Date.AddDays(1) ? _selectedDate.Date.AddDays(1) : a.EndTime;
                
                float startHour = start.Hour + start.Minute / 60f;
                float endHour = end.Hour + end.Minute / 60f;
                if (endHour <= startHour) endHour = startHour + 0.5f;
                
                int y = (int)(startHour * hourHeight) - scrollY;
                int h = (int)((endHour - startHour) * hourHeight);
                
                var rect = new Rectangle(timeWidth + 5, y + 1, colWidth, h - 2);
                _hitTests.Add(Tuple.Create(rect, a));
                
                Color c = EventColors[a.Id % EventColors.Length];
                using (var br = new SolidBrush(Color.FromArgb(200, c)))
                using (var path = RoundedRect(rect, 4)) g.FillPath(br, path);
                
                using var textBrush = new SolidBrush(Color.White);
                using var titleFont = new Font("Segoe UI", 9, FontStyle.Bold);
                using var descFont = new Font("Segoe UI", 8);
                
                g.DrawString(a.Name, titleFont, textBrush, new RectangleF(rect.X + 4, rect.Y + 2, rect.Width - 8, rect.Height - 4));
                if (h > 30)
                    g.DrawString($"{start:HH:mm} - {end:HH:mm}", descFont, textBrush, new RectangleF(rect.X + 4, rect.Y + 16, rect.Width - 8, rect.Height - 18));
            }
        }

        private void DrawWeekView(Graphics g)
        {
            int hourHeight = 60;
            int scrollY = vScrollBar.Value;
            int timeWidth = 60;
            int colWidth = (panelGrid.Width - timeWidth) / 7;
            
            using var font = new Font("Segoe UI", 9);
            using var pen = new Pen(Color.FromArgb(218, 220, 224));
            
            for (int i = 0; i <= 24; i++)
            {
                int y = i * hourHeight - scrollY;
                if (y < -hourHeight || y > panelGrid.Height) continue;
                
                string timeStr = i == 24 ? "0:00" : $"{i}:00";
                g.DrawString(timeStr, font, Brushes.Gray, 5, y - 8);
                g.DrawLine(pen, timeWidth, y, panelGrid.Width, y);
            }
            
            for (int c = 1; c < 7; c++)
            {
                int x = timeWidth + c * colWidth;
                g.DrawLine(pen, x, 0, x, panelGrid.Height);
            }

            DateTime weekStart = _selectedDate.Date.AddDays(-(int)_selectedDate.DayOfWeek);
            DateTime weekEnd = weekStart.AddDays(6);
            
            if (DateTime.Today >= weekStart && DateTime.Today <= weekEnd)
            {
                int c = (int)DateTime.Today.DayOfWeek;
                using var todayBg = new SolidBrush(Color.FromArgb(20, 26, 115, 232));
                g.FillRectangle(todayBg, timeWidth + c * colWidth, 0, colWidth, panelGrid.Height);
            }

            var appts = CalendarData.Appointments
                .Where(a => a.StartTime.Date <= weekEnd && a.EndTime >= weekStart)
                .Where(a => (chkMyCal.Checked && !a.IsGroupMeeting) || (chkGroupMeeting.Checked && a.IsGroupMeeting))
                .OrderBy(a => a.StartTime)
                .ToList();

            foreach (var a in appts)
            {
                DateTime renderStart = a.StartTime < weekStart ? weekStart : a.StartTime;
                DateTime renderEnd = a.EndTime > weekEnd.AddDays(1) ? weekEnd.AddDays(1) : a.EndTime;
                
                for (DateTime d = renderStart.Date; d < renderEnd; d = d.AddDays(1))
                {
                    int c = (int)d.DayOfWeek;
                    
                    float startHour = (d == a.StartTime.Date) ? a.StartTime.Hour + a.StartTime.Minute / 60f : 0f;
                    float endHour = (d == a.EndTime.Date) ? a.EndTime.Hour + a.EndTime.Minute / 60f : 24f;
                    if (endHour <= startHour && d == a.StartTime.Date) endHour = startHour + 0.5f;
                    
                    int y = (int)(startHour * hourHeight) - scrollY;
                    int h = (int)((endHour - startHour) * hourHeight);
                    
                    var rect = new Rectangle(timeWidth + c * colWidth + 2, y + 1, colWidth - 4, h - 2);
                    _hitTests.Add(Tuple.Create(rect, a));
                    
                    Color color = EventColors[a.Id % EventColors.Length];
                    using (var br = new SolidBrush(Color.FromArgb(200, color)))
                    using (var path = RoundedRect(rect, 4)) g.FillPath(br, path);
                    
                    using var textBrush = new SolidBrush(Color.White);
                    using var titleFont = new Font("Segoe UI", 8, FontStyle.Bold);
                    
                    string label = a.Name;
                    g.DrawString(label, titleFont, textBrush, new RectangleF(rect.X + 2, rect.Y + 2, rect.Width - 4, rect.Height - 4));
                }
            }
        }

        private void DrawYearView(Graphics g)
        {
            int W = panelGrid.Width, H = panelGrid.Height;
            int cw = W / 4, rh = H / 3;
            
            using var sfCenter = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            using var titleFont = new Font("Segoe UI", 10, FontStyle.Bold);
            using var dayFont = new Font("Segoe UI", 8);
            using var todayFont = new Font("Segoe UI", 8, FontStyle.Bold);
            
            for (int m = 1; m <= 12; m++)
            {
                int col = (m - 1) % 4;
                int row = (m - 1) / 4;
                var rect = new Rectangle(col * cw, row * rh, cw, rh);
                
                g.DrawString(MonthNames[m - 1], titleFont, Brushes.Black, new Rectangle(rect.X, rect.Y + 5, rect.Width, 20), sfCenter);
                
                DateTime firstDay = new DateTime(_currentMonth.Year, m, 1);
                int startDOW = (int)firstDay.DayOfWeek;
                int daysInMonth = DateTime.DaysInMonth(_currentMonth.Year, m);
                
                int cellW = (rect.Width - 20) / 7;
                int cellH = (rect.Height - 35) / 6;
                int startX = rect.X + 10;
                int startY = rect.Y + 30;
                
                for (int d = 1; d <= daysInMonth; d++)
                {
                    int r = (startDOW + d - 1) / 7;
                    int c = (startDOW + d - 1) % 7;
                    var cell = new Rectangle(startX + c * cellW, startY + r * cellH, cellW, cellH);
                    
                    DateTime curDate = new DateTime(_currentMonth.Year, m, d);
                    bool isToday = curDate == DateTime.Today;
                    
                    if (isToday)
                    {
                        using var todayBr = new SolidBrush(Color.FromArgb(26, 115, 232));
                        g.FillEllipse(todayBr, cell.X + cell.Width/2 - 9, cell.Y + cell.Height/2 - 9, 18, 18);
                        g.DrawString(d.ToString(), todayFont, Brushes.White, cell, sfCenter);
                    }
                    else
                    {
                        Color fg = (c == 0) ? Color.FromArgb(192,0,0) : Color.FromArgb(60,64,67);
                        using var br = new SolidBrush(fg);
                        g.DrawString(d.ToString(), dayFont, br, cell, sfCenter);
                        
                        if (CalendarData.Appointments.Any(a => a.StartTime.Date <= curDate && a.EndTime >= curDate && ((chkMyCal.Checked && !a.IsGroupMeeting) || (chkGroupMeeting.Checked && a.IsGroupMeeting))))
                        {
                            using var dotBr = new SolidBrush(Color.FromArgb(26, 115, 232));
                            g.FillEllipse(dotBr, cell.X + cell.Width/2 - 2, cell.Bottom - 4, 4, 4);
                        }
                    }
                }
            }
        }

        private void DrawEventChips(Graphics g, DateTime day, Rectangle area)
        {
            var appts = CalendarData.Appointments
                .Where(a => a.StartTime.Date <= day.Date && a.EndTime >= day.Date)
                .Where(a => (chkMyCal.Checked && !a.IsGroupMeeting) || (chkGroupMeeting.Checked && a.IsGroupMeeting))
                .OrderBy(a => a.StartTime)
                .ToList();

            int chipH = 18, spacing = 2, y = area.Top;
            int maxChips = Math.Max(0, (area.Height + spacing) / (chipH + spacing));
            int shown = Math.Min(appts.Count, maxChips > 0 ? maxChips - (appts.Count > maxChips ? 1 : 0) : 0);

            for (int i = 0; i < shown; i++)
            {
                var a     = appts[i];
                Color c   = EventColors[a.Id % EventColors.Length];
                var chip  = new Rectangle(area.Left, y, area.Width, chipH);
                using var path = RoundedRect(chip, 4);
                using var br   = new SolidBrush(c);
                g.FillPath(br, path);
                
                _hitTests.Add(Tuple.Create(chip, a));
                
                string label = a.Name.Length > 18 ? a.Name[..15] + "…" : a.Name;
                using var sf  = new StringFormat { LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.NoWrap, Trimming = StringTrimming.EllipsisCharacter };
                g.DrawString(label, new Font("Segoe UI", 7.5f), Brushes.White, new RectangleF(chip.Left + 4, chip.Top, chip.Width - 5, chip.Height), sf);
                y += chipH + spacing;
            }

            if (appts.Count > shown && shown >= 0)
            {
                int rem = appts.Count - shown;
                using var sf  = new StringFormat { LineAlignment = StringAlignment.Center };
                using var br  = new SolidBrush(Color.FromArgb(95,99,104));
                g.DrawString($"+{rem} thêm", new Font("Segoe UI", 7.5f), br, new RectangleF(area.Left + 4, y, area.Width, chipH), sf);
            }
        }


        private void panelMiniCal_Paint(object sender, PaintEventArgs e)
        {
            var g    = e.Graphics;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            var bounds = panelMiniCal.ClientRectangle;
            g.Clear(Color.FromArgb(248,249,250));

            string hdr = $"{MonthNames[_currentMonth.Month-1]} {_currentMonth.Year}";
            using var hf = new Font("Segoe UI", 9, FontStyle.Bold);
            using var hb = new SolidBrush(Color.FromArgb(60,64,67));
            using var sf = new StringFormat { Alignment = StringAlignment.Center };
            g.DrawString(hdr, hf, hb, new RectangleF(30, 2, bounds.Width - 60, 22), sf);

            using var abr = new SolidBrush(Color.FromArgb(95,99,104));
            g.DrawString("‹", new Font("Segoe UI",12), abr, new PointF(6, 0));
            g.DrawString("›", new Font("Segoe UI",12), abr, new PointF(bounds.Width - 22, 0));

            int cw = bounds.Width / 7, ch = 22;
            for (int d = 0; d < 7; d++)
            {
                var r  = new Rectangle(d * cw, 24, cw, ch);
                Color fg = d == 0 ? Color.FromArgb(192,0,0) : d == 6 ? Color.FromArgb(70,70,200) : Color.FromArgb(120,125,130);
                using var dfont = new Font("Segoe UI", 7.5f, FontStyle.Bold);
                using var dbr   = new SolidBrush(fg);
                using var dsf   = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(DayNames[d], dfont, dbr, r, dsf);
            }

            DateTime start = GridStart();
            for (int row = 0; row < 6; row++)
            for (int col = 0; col < 7; col++)
            {
                DateTime day = start.AddDays(row * 7 + col);
                var cell = new Rectangle(col * cw, 48 + row * ch, cw, ch);
                bool curM   = day.Month == _currentMonth.Month;
                bool isToday = day.Date == DateTime.Today;
                bool isSel  = day.Date == _selectedDate.Date;

                if (isToday || isSel)
                {
                    Color circleC = isToday ? Color.FromArgb(26,115,232) : Color.FromArgb(230,230,230);
                    using var cbr = new SolidBrush(circleC);
                    g.FillEllipse(cbr, cell.Left + cw/2 - 10, cell.Top + 1, 20, 20);
                }

                Color numC = !curM ? Color.FromArgb(190,192,195) : isToday || isSel ? Color.White : Color.FromArgb(60,64,67);
                using var nf  = new Font("Segoe UI", 8.5f, isToday ? FontStyle.Bold : FontStyle.Regular);
                using var nbr = new SolidBrush(numC);
                using var nsf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(day.Day.ToString(), nf, nbr, cell, nsf);

                if (day.Month == _currentMonth.Month && CalendarData.Appointments.Any(a => a.StartTime.Date <= day.Date && a.EndTime >= day.Date && ((chkMyCal.Checked && !a.IsGroupMeeting) || (chkGroupMeeting.Checked && a.IsGroupMeeting))))
                {
                    using var dotBr = new SolidBrush(isToday ? Color.White : Color.FromArgb(26,115,232));
                    g.FillEllipse(dotBr, cell.Left + cw/2 - 2, cell.Bottom - 5, 4, 4);
                }
            }
        }

        private void panelMiniCal_MouseClick(object sender, MouseEventArgs e)
        {
            int cw = panelMiniCal.Width / 7, ch = 22;
            if (e.X < 26 && e.Y < 24)  { _currentMonth = _currentMonth.AddMonths(-1); RefreshAll(); return; }
            if (e.X > panelMiniCal.Width - 26 && e.Y < 24) { _currentMonth = _currentMonth.AddMonths(1); RefreshAll(); return; }
            if (e.Y < 48) return;
            
            int col = e.X / cw, row = (e.Y - 48) / ch;
            if (row < 0 || row > 5 || col < 0 || col > 6) return;
            
            _selectedDate = GridStart().AddDays(row * 7 + col);
            _currentMonth = new DateTime(_selectedDate.Year, _selectedDate.Month, 1);
            
            _viewMode = ViewMode.Day; 
            btnViewMode.Text = "Ngày ▼";
            RefreshAll();
        }


        private void panelGrid_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = _hitTests.Count - 1; i >= 0; i--)
            {
                if (_hitTests[i].Item1.Contains(e.Location))
                {
                    using var viewForm = new ViewAppointmentForm(_hitTests[i].Item2);
                    if (viewForm.ShowDialog(this) == DialogResult.OK) RefreshAll();
                    return;
                }
            }

            if (_viewMode == ViewMode.Year)
            {
                int cw = panelGrid.Width / 4, rh = panelGrid.Height / 3;
                int col = e.X / cw, row = e.Y / rh;
                int m = row * 4 + col + 1;
                if (m >= 1 && m <= 12)
                {
                    int startDOW = (int)(new DateTime(_currentMonth.Year, m, 1).DayOfWeek);
                    int cellW = (cw - 20) / 7, cellH = (rh - 35) / 6;
                    int startX = col * cw + 10, startY = row * rh + 30;
                    
                    int r = (e.Y - startY) / cellH, c = (e.X - startX) / cellW;
                    if (r >= 0 && r < 6 && c >= 0 && c < 7)
                    {
                        int dayOffset = r * 7 + c - startDOW;
                        if (dayOffset >= 0 && dayOffset < DateTime.DaysInMonth(_currentMonth.Year, m))
                        {
                            _selectedDate = new DateTime(_currentMonth.Year, m, dayOffset + 1);
                            _viewMode = ViewMode.Day;
                            btnViewMode.Text = "Ngày ▼";
                            RefreshAll();
                        }
                    }
                }
                return;
            }

            DateTime clicked = _selectedDate;
            if (_viewMode == ViewMode.Month)
            {
                int cw = panelGrid.Width / 7, rh = panelGrid.Height / 6;
                int col = e.X / cw, row = e.Y / rh;
                if (col > 6 || row > 5) return;
                clicked = GridStart().AddDays(row * 7 + col);
            }
            else if (_viewMode == ViewMode.Week)
            {
                int timeWidth = 60;
                if (e.X > timeWidth)
                {
                    int colWidth = (panelGrid.Width - timeWidth) / 7;
                    int c = (e.X - timeWidth) / colWidth;
                    DateTime weekStart = _selectedDate.Date.AddDays(-(int)_selectedDate.DayOfWeek);
                    clicked = weekStart.AddDays(c);
                    
                    float hour = (e.Y + vScrollBar.Value) / 60f;
                    int h = Math.Clamp((int)hour, 0, 23);
                    int min = (int)((hour - h) * 60);
                    clicked = clicked.AddHours(h).AddMinutes(min);
                }
            }
            else if (_viewMode == ViewMode.Day)
            {
                float hour = (e.Y + vScrollBar.Value) / 60f;
                int h = Math.Clamp((int)hour, 0, 23);
                int min = (int)((hour - h) * 60);
                clicked = _selectedDate.Date.AddHours(h).AddMinutes(min);
            }

            _selectedDate = clicked.Date;
            _currentMonth = new DateTime(_selectedDate.Year, _selectedDate.Month, 1);

            if (e.Clicks >= 2 || _viewMode == ViewMode.Day || _viewMode == ViewMode.Week)
            {
                using var form = new AddAppointmentForm(clicked);
                if (form.ShowDialog(this) == DialogResult.OK) RefreshAll();
            }
            
            RefreshAll();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            using var form = new AddAppointmentForm(_selectedDate.Date.AddHours(8));
            if (form.ShowDialog(this) == DialogResult.OK) RefreshAll();
        }

        private DateTime GridStart()
        {
            var first = new DateTime(_currentMonth.Year, _currentMonth.Month, 1);
            return first.AddDays(-(int)first.DayOfWeek);
        }

        private static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(r.Left, r.Top, radius*2, radius*2, 180, 90);
            path.AddArc(r.Right - radius*2, r.Top, radius*2, radius*2, 270, 90);
            path.AddArc(r.Right - radius*2, r.Bottom - radius*2, radius*2, radius*2, 0, 90);
            path.AddArc(r.Left, r.Bottom - radius*2, radius*2, radius*2, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}