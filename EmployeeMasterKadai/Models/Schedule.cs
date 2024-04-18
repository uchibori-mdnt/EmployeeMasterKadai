using EmployeeMasterKadai.Validations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeMasterKadai.Models
{
    public class Schedule
    {
        [Key]
        [ScaffoldColumn(false)]
        public Guid Id { get; set; } = Guid.NewGuid();
        [DisplayName("担当者")]
        [Required(ErrorMessage = "{0}は必須です。")]
        public string Organizer { get; set; } = string.Empty;
        [DisplayName("件名")]
        [Required(ErrorMessage = "{0}は必須です。")]
        public string Title { get; set; } = string.Empty;
        [DisplayName("種別")]
        public string? TypeToDo { get; set; }
        [DisplayName("終日")]
        public bool AllDay { get; set; }
        [DisplayName("開始日時")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public DateTime? StartDay { get; set; }



        [DisplayName("終了日時")]
        [SameDay(ErrorMessage = "開始時刻と終了時刻が同じになっています。")]
        [ChangeDateField(ErrorMessage = "開始時刻と終了時刻を入力してください。")]
        [CheckReverseTime(ErrorMessage = "開始時刻が終了時刻を超えることはできません。")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public DateTime? EndDay { get; set; }
        [DisplayName("参加候補者")]
        public string[] JoinPeople { get; set; } = string.Empty.Split(',');

        [ScaffoldColumn(false)]
        public DateTime CreateDate { get; set; }
        [ScaffoldColumn(false)]
        public DateTime UpdatedDate { get; set; }
    }
}
