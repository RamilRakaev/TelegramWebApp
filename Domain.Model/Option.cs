
namespace Domain.Model
{
    public class Option : BaseEntity
    {
        public Option()
        {

        }

        public Option(string propertyName, string value)
        {
            PropertyName = propertyName;
            Value = value;
        }

        public string PropertyName { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
