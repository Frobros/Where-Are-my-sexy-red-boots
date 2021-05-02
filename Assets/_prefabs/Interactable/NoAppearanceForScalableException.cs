using System;
using System.Runtime.Serialization;

[Serializable]
internal class NoAppearanceForScalableException : Exception
{
    public NoAppearanceForScalableException()
    {
    }

    public NoAppearanceForScalableException(string message) : base(message)
    {
    }

    public NoAppearanceForScalableException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected NoAppearanceForScalableException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}