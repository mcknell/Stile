#region License info...
// Propter for .NET, Copyright 2011-2012 by Mark Knell
// Licensed under the MIT License found at the top directory of the Stile project on GitHub
#endregion

#region using...
using System;
#endregion

namespace Stile.Prototypes.Specifications.SemanticModel.Evaluations
{
    public interface IError
    {
        Exception Exception { get; }
        bool Handled { get; }
    }

    public class Error : IError
    {
        public Error(Exception exception, bool handled = true)
        {
            Exception = exception;
            Handled = handled;
        }

        public Exception Exception { get; private set; }
        public bool Handled { get; private set; }
    }
}
