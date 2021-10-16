
namespace Domain.Model
{
    public class Option
    {
        public Option()
        {

        }

        public Option(string propertyName, string value)
        {
            PropertyName = propertyName;
            Value = value;
        }

        public int Id { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
