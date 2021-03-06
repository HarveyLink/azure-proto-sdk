using Azure.ResourceManager.Core;
using System;
using Azure.ResourceManager.Resources.Models;
using azure_proto_network;
using azure_proto_compute;

namespace client
{
    class NullDataValues : Scenario
    {
        public override void Execute()
        {
            var rg = new Azure.ResourceManager.Resources.Models.ResourceGroup("East US");
            var resourceGroupData = new ResourceGroupData(rg);
            var nic = new  Azure.ResourceManager.Network.Models.NetworkInterface();
            var networkInterfaceData = new NetworkInterfaceData(nic);
            var aset = new Azure.ResourceManager.Compute.Models.AvailabilitySet("East US");
            var availabilitySet =  new AvailabilitySetData(aset);

            
        }
    }
}
