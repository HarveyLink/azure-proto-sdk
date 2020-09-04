﻿using Azure;
using Azure.ResourceManager.Network;
using Azure.ResourceManager.Network.Models;
using azure_proto_core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace azure_proto_network
{
    /// <summary>
    /// Virtual Network Operations
    /// </summary>
    public class VnetOperations : ResourceOperations<PhVirtualNetwork>
    {
        public VnetOperations(ArmOperations parent, ResourceIdentifier context) : base(parent, context)
        {
        }

        public VnetOperations(ArmOperations parent, azure_proto_core.Resource context) : base(parent, context)
        {
        }

        protected override ResourceType ResourceType => "Microsoft.Network/virtualNetworks";

        public override ArmOperation<Response> Delete()
        {
            return new ArmVoidOperation(Operations.StartDelete(Context.ResourceGroup, Context.Name));
        }

        public async override Task<ArmOperation<Response>> DeleteAsync(CancellationToken cancellationToken = default)
        {
            return new ArmVoidOperation(await Operations.StartDeleteAsync(Context.ResourceGroup, Context.Name, cancellationToken));
        }

        public override Response<ResourceOperations<PhVirtualNetwork>> Get()
        {
            return new PhArmResponse<ResourceOperations<PhVirtualNetwork>, VirtualNetwork>(Operations.Get(Context.ResourceGroup, Context.Name), 
                n => { Resource = new PhVirtualNetwork(n); return this; });
        }

        public async override Task<Response<ResourceOperations<PhVirtualNetwork>>> GetAsync(CancellationToken cancellationToken = default)
        {
            return new PhArmResponse<ResourceOperations<PhVirtualNetwork>, VirtualNetwork>(await Operations.GetAsync(Context.ResourceGroup, Context.Name, null, cancellationToken),
                n => { Resource = new PhVirtualNetwork(n); return this;});
        }

        public override ArmOperation<ResourceOperations<PhVirtualNetwork>> AddTag(string key, string value)
        {
            var patchable = new TagsObject();
            patchable.Tags[key] = value;
            return new PhArmOperation<ResourceOperations<PhVirtualNetwork>, VirtualNetwork>(Operations.UpdateTags(Context.ResourceGroup, Context.Name, patchable),
                n => { Resource = new PhVirtualNetwork(n); return this; });
        }

        public async override Task<ArmOperation<ResourceOperations<PhVirtualNetwork>>> AddTagAsync(string key, string value, CancellationToken cancellationToken = default)
        {
            var patchable = new TagsObject();
            patchable.Tags[key] = value;
            return new PhArmOperation<ResourceOperations<PhVirtualNetwork>, VirtualNetwork>(await Operations.UpdateTagsAsync(Context.ResourceGroup, Context.Name, patchable, cancellationToken),
                n => { Resource = new PhVirtualNetwork(n); return this; });
        }

        internal VirtualNetworksOperations Operations => GetClient<NetworkManagementClient>((uri, cred) => new NetworkManagementClient(Context.Subscription, uri, cred)).VirtualNetworks;
    }
}
