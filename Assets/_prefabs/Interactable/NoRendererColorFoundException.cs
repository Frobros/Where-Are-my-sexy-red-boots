using System;
using System.Runtime.Serialization;

[Serializable]
internal class NoRendererColorFoundException : Exception
{
    public NoRendererColorFoundException()
    {
    }

    public NoRendererColorFoundException(string message) : base(message)
    {
    }

    public NoRendererColorFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected NoRendererColorFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}