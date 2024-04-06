using EmployeeMasterKadai.Validations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeMasterKadai.Models
{
    public class EmployeeList
    {
        [Key]
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }
        [DisplayName("社員名")]
        [Required(ErrorMessage = "{0}は必須です。")]
        [StringLength(50, ErrorMessage = "{0}は{1}文字以内で入力してください。")]
        public string Name { get; set; } = string.Empty;
        [DisplayName("部署")]
        [Required(ErrorMessage = "{0}は必須です。")]
        public string? Department { get; set; }
        [DisplayName("退職フラグ")]
        public bool RetirementFlag { get; set; }
        [DisplayName("退職日")]
        [DisplayFormat(DataFormatString = "{0:yyyy年MM月dd日}", NullDisplayText = "")] //※※※フォーマットを指定できます
        [DataType(DataType.DateTime)]
        [DateInPast(ErrorMessage = "退職日は本日以前の日付を入力してください。")]
        [RequiredIf(nameof(RetirementFlag), true, ErrorMessage = "退職日を入力してください")]
        public DateTime? RetirementDay { get; set; } //※※※退職フラグ次第でnullでも良いところなので"?"
        [DisplayName("登録日時")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
        [DisplayName("更新日時")]
        [DataType(DataType.DateTime)]
        public DateTime UpdatedAt { get; set; }
    }
}
