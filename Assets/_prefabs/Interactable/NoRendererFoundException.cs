using System;
using System.Runtime.Serialization;

[Serializable]
internal class NoRendererFoundException : Exception
{
    public NoRendererFoundException()
    {
    }

    public NoRendererFoundException(string message) : base(message)
    {
    }

    public NoRendererFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected NoRendererFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}