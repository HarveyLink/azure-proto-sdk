﻿namespace azure_proto_core
{
    public interface IResource : IModel
    {
        ClientFactory Clients { get; }
    }
}
