namespace ZohoDesk.Exceptions;

/// <summary>
/// Базовое исключение библиотеки Zoho Desk.
/// </summary>
public class ZohoException : Exception
{
    /// <summary>
    /// Создает новое исключение.
    /// </summary>
    public ZohoException()
    {
    }

    /// <summary>
    /// Создает новое исключение с сообщением.
    /// </summary>
    public ZohoException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Создает новое исключение с внутренним исключением.
    /// </summary>
    public ZohoException(
        string message,
        Exception innerException)
        : base(message, innerException)
    {
    }
}