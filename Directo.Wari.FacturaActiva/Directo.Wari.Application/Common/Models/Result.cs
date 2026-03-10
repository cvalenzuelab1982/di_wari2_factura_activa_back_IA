namespace Directo.Wari.Application.Common.Models
{
    /// <summary>
    /// Result Pattern - Railway Oriented Programming.
    /// Encapsula el resultado de una operación que puede tener éxito o fallar.
    /// </summary>
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error? Error { get; }

        protected Result(bool isSuccess, Error? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, null);
        public static Result Failure(Error error) => new(false, error);
        public static Result<T> Success<T>(T value) => new(value, true, null);
        public static Result<T> Failure<T>(Error error) => new(default, false, error);
    }

    /// <summary>
    /// Result Pattern genérico con valor de retorno.
    /// </summary>
    public class Result<T> : Result
    {
        public T? Value { get; }

        internal Result(T? value, bool isSuccess, Error? error) : base(isSuccess, error)
        {
            Value = value;
        }

        public static implicit operator Result<T>(T value) => Success(value);
    }

    /// <summary>
    /// Modelo de error estandarizado.
    /// </summary>
    public record Error(string Code, string Message)
    {
        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error NullValue = new("Error.NullValue", "El valor proporcionado es null.");

        public static Error NotFound(string entityName, object key) =>
            new($"{entityName}.NotFound", $"La entidad '{entityName}' con clave ({key}) no fue encontrada.");

        public static Error Validation(string message) =>
            new("Error.Validation", message);

        public static Error Conflict(string message) =>
            new("Error.Conflict", message);

        public static Error Unauthorized(string message = "Credenciales incorrectas.") =>
            new("Error.Unauthorized", message);
    }
}
