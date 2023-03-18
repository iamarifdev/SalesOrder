using System.Runtime.Serialization;

namespace SalesOrder.Common.Exceptions;

[Serializable]
public class SubElementNotFoundException : Exception
{
    public SubElementNotFoundException(string message = "Window is not found") : base(message)
    {
    }

    protected SubElementNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}

[Serializable]
public class InvalidSubElementIdException : Exception
{
    public InvalidSubElementIdException(string message = "Invalid SubElement Id") : base(message)
    {
    }

    protected InvalidSubElementIdException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}