using System.Runtime.Serialization;

namespace SalesOrder.Common.Exceptions;

[Serializable]
public class OrderNotFoundException : Exception
{
    public OrderNotFoundException(string message = "Order is not found") : base(message)
    {
    }

    protected OrderNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}

[Serializable]
public class InvalidOrderIdException : Exception
{
    public InvalidOrderIdException(string message = "Invalid Order Id") : base(message)
    {
    }

    protected InvalidOrderIdException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}