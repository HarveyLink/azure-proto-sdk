﻿using System.Collections.Generic;

namespace azure_proto_core
{
    //TODO: Remove and use Resource / TrackedResource to accomplish the same thing
    public interface ITaggable
    {
        public IDictionary<string, string> Tags { get; }
    }
}