using System.ComponentModel.DataAnnotations;

namespace EmployeeMasterKadai.Validations;

// ※※※「_triggerPropertyNameが_triggerValueだったら必須」のvalidationを作ってみました
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class RequiredIfAttribute : ValidationAttribute
{
    string _triggerPropertyName { get; }
    object _triggerValue { get; }

    public RequiredIfAttribute(string triggerPropertyName, object triggerValue)
    {
        _triggerPropertyName = triggerPropertyName;
        _triggerValue = triggerValue;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var triggerProperty = validationContext.ObjectType.GetProperty(_triggerPropertyName) ?? throw new ArgumentException("検証設定ミス");

        object? triggerValue = triggerProperty.GetValue(validationContext.ObjectInstance);
        if (triggerValue?.ToString() == _triggerValue.ToString())
        {
            if (value == null)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName),
                    [validationContext.MemberName ?? string.Empty]);
            }
        }

        return ValidationResult.Success;
    }
}
