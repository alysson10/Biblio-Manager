namespace Bib.Application.Common.Models
{
    public class Error
    {
        public string Code { get; }
        public string Message { get; }

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public static Error None { get; } = new Error(string.Empty, string.Empty);
        public static Error NullValue { get; } = new Error("Error.NullValue", "Null value was provided");
        public static Error Validation { get; } = new Error("Error.Validation", "A validation error occurred");
        public static Error NotFound { get; } = new Error("Error.NotFound", "The requested resource was not found");
        public static Error Unauthorized { get; } = new Error("Error.Unauthorized", "Access denied");
        public static Error Conflict { get; } = new Error("Error.Conflict", "Resource conflict occurred");
        public static Error Database { get; } = new Error("Error.Database", "Database operation failed");
        public static Error InUse { get; } = new Error("Error.InUse", "Record in use");
        public static Error NotChange { get; } = new Error("Error.NotChange", "Record not change");

        public static Error Custom(string code, string message) => new Error(code, message);

        public void Deconstruct(out string code, out string message)
        {
            code = Code;
            message = Message;
        }

        public override string ToString() => $"{Code}: {Message}";
    }
}
