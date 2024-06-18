using EmployeeMasterKadai.Validations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeMasterKadai.Models
{
    public class Employee : IValidatableObject
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
        [DataType(DataType.DateTime)]
        [DateInPast(ErrorMessage = "退職日は本日以前の日付を入力してください。")]
        [Column(TypeName = "DATE")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? RetirementDay { get; set; }
        [DisplayName("登録日時")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime CreatedAt { get; set; }
        [DisplayName("更新日時")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime UpdatedAt { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RetirementFlag && RetirementDay == null)
            {
                yield return new ValidationResult("退職日を入力してください。", new[] { nameof(RetirementDay) });
            }
        }
    }
}
