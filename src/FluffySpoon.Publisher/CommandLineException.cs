﻿using System;
using System.Runtime.Serialization;

namespace FluffySpoon.Publisher;

[Serializable]
internal class CommandLineException : Exception
{
	public CommandLineException()
	{
	}

	public CommandLineException(string message) : base(message)
	{
	}

	public CommandLineException(string message, Exception innerException) : base(message, innerException)
	{
	}

	protected CommandLineException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}