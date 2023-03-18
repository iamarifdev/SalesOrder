using System.Runtime.Serialization;

namespace SalesOrder.Common.Exceptions;

[Serializable]
public class WindowNotFoundException : Exception
{
    public WindowNotFoundException(string message = "Window is not found") : base(message)
    {
    }

    protected WindowNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}

[Serializable]
public class InvalidWindowIdException : Exception
{
    public InvalidWindowIdException(string message = "Invalid Window Id") : base(message)
    {
    }

    protected InvalidWindowIdException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}