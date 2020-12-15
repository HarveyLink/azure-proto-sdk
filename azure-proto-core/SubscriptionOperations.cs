﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using Azure;
using Azure.ResourceManager.Resources;
using azure_proto_core.Adapters;
using System.Threading.Tasks;

namespace azure_proto_core
{
    /// <summary>
    ///     Subscription operations
    /// </summary>
    public class SubscriptionOperations : ResourceOperationsBase<Subscription, SubscriptionData>
    {
        public static readonly string AzureResourceType = "Microsoft.Resources/subscriptions";

        internal SubscriptionOperations(ArmClientContext context, string defaultSubscription, ArmClientOptions clientOptions)
            : base(context, $"/subscriptions/{defaultSubscription}", clientOptions)
        {
        }

        internal SubscriptionOperations(ArmClientContext context, ResourceIdentifier id, ArmClientOptions clientOptions)
            : base(context, id, clientOptions)
        {
        }

        internal SubscriptionOperations(ArmClientContext context, Resource subscription, ArmClientOptions clientOptions)
            : base(context, subscription, clientOptions)
        {
        }

        public override ResourceType ResourceType => AzureResourceType;

        internal SubscriptionsOperations SubscriptionsClient => GetClient<ResourcesManagementClient>((uri, cred) =>
            new ResourcesManagementClient(uri, Guid.NewGuid().ToString(), cred, ArmClientOptions.Convert<ResourcesManagementClientOptions>(ClientOptions))).Subscriptions;

        internal ResourceGroupsOperations RgOperations => GetClient<ResourcesManagementClient>((uri, cred) =>
            new ResourcesManagementClient(uri, Id.Subscription, cred, ArmClientOptions.Convert<ResourcesManagementClientOptions>(ClientOptions))).ResourceGroups;

        public Pageable<ResourceGroupOperations> ListResourceGroups(CancellationToken cancellationToken = default)
        {
            return new PhWrappingPageable<Azure.ResourceManager.Resources.Models.ResourceGroup, ResourceGroupOperations>(
                RgOperations.List(null, null, cancellationToken),
                s => new ResourceGroupOperations(ClientContext, new ResourceGroupData(s), ClientOptions));
        }

        public AsyncPageable<ResourceGroupOperations> ListResourceGroupsAsync(
            CancellationToken cancellationToken = default)
        {
            return new PhWrappingAsyncPageable<Azure.ResourceManager.Resources.Models.ResourceGroup, ResourceGroupOperations>(
                RgOperations.ListAsync(null, null, cancellationToken),
                s => new ResourceGroupOperations(ClientContext, new ResourceGroupData(s), ClientOptions));
        }

        public ResourceGroupOperations ResourceGroup(ResourceGroupData resourceGroup)
        {
            return new ResourceGroupOperations(ClientContext, resourceGroup, ClientOptions);
        }

        public ResourceGroupOperations ResourceGroup(ResourceIdentifier resourceGroup)
        {
            return new ResourceGroupOperations(ClientContext, resourceGroup, ClientOptions);
        }

        public ResourceGroupOperations ResourceGroup(string resourceGroup)
        {
            return new ResourceGroupOperations(ClientContext, $"{Id}/resourceGroups/{resourceGroup}", ClientOptions);
        }

        public override ArmResponse<Subscription> Get()
        {
            return new PhArmResponse<azure_proto_core.Subscription, Azure.ResourceManager.Resources.Models.Subscription>(
                SubscriptionsClient.Get(Id.Name),
                Converter());
        }
        
        public async override Task<ArmResponse<Subscription>> GetAsync(CancellationToken cancellationToken = default)
        {
            return new PhArmResponse<azure_proto_core.Subscription, Azure.ResourceManager.Resources.Models.Subscription>(
                await SubscriptionsClient.GetAsync(Id.Name, cancellationToken),
                Converter());
        }

        public ResourceGroupContainerOperations ResourceGroups()
        {
            return new ResourceGroupContainerOperations(ClientContext, this, ClientOptions);
        }
        private Func< Azure.ResourceManager.Resources.Models.Subscription, azure_proto_core.Subscription> Converter()
        {
            return s => new azure_proto_core.Subscription(ClientContext, new SubscriptionData(s), ClientOptions);
        }
    }
}
