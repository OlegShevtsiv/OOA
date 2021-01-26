﻿using System;

namespace Library.DataWriters.Exceptions
{
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException()
            : base()
        {
        }

        public ObjectNotFoundException(string message)
        : base(message)
        {
        }

        public ObjectNotFoundException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}