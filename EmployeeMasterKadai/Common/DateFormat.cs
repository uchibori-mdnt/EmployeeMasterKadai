namespace EmployeeMasterKadai.Common
{
    public class DateFormat
    {
        private readonly Func<DateTime, string>[] _fmt = {
        date => date.Year.ToString(),
        date => date.Month.ToString("00"),
        date => date.Month.ToString(),
        date => date.Day.ToString("00"),
        date => date.Day.ToString(),
        date => date.Hour.ToString("00"),
        date => date.Hour.ToString(),
        date => date.Minute.ToString("00"),
        date => date.Minute.ToString(),
        date => date.Second.ToString("00"),
        date => date.Second.ToString(),
        date => new string[] {"日", "月", "火", "水", "木", "金", "土"}[(int)date.DayOfWeek]
    };

        private readonly string[] _priority = { "yyyy", "MM", "M", "dd", "d", "hh", "h", "mm", "m", "ss", "s", "w" };

        public string Format(DateTime date, string format)
        {
            foreach (var fmt in _priority)
            {
                format = format.Replace(fmt, _fmt[Array.IndexOf(_priority, fmt)](date));
            }
            return format;
        }
    }
}
