﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using Microsoft.Azure.Commands.Billing.Common;
using Microsoft.Azure.Commands.Billing.Properties;
using Microsoft.Azure.Management.Billing;
using Microsoft.Azure.Management.Billing.Models;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using PsInvoice = Microsoft.Azure.Commands.Billing.Models.Invoice;

namespace Microsoft.Azure.Commands.Billing.Cmdlets.Invoices
{
    [Cmdlet(VerbsCommon.Get, "AzureRmBillingInvoice", DefaultParameterSetName = Constants.ParameterSetNames.ListParameterSet), OutputType(typeof(List<PsInvoice>), typeof(PsInvoice))]
    public class GetAzureRmBillingInvoice : AzureBillingCmdletBase
    {
        const string DownloadUrlExpand = "downloadUrl";

        [Parameter(Mandatory = true, HelpMessage = "Get the latest invoice.", ParameterSetName = Constants.ParameterSetNames.LatestItemParameterSet)]
        public SwitchParameter Latest { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Name of a specific invoice to get.", ParameterSetName = Constants.ParameterSetNames.SingleItemParameterSet)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Determine the maximum number of records to return.", ParameterSetName = Constants.ParameterSetNames.ListParameterSet)]
        [ValidateNotNull]
        public int? MaxCount { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Generate the download url of the invoices.", ParameterSetName = Constants.ParameterSetNames.ListParameterSet)]
        public SwitchParameter GenerateDownloadUrl { get; set; }

        public override void ExecuteCmdlet()
        {
            if (ParameterSetName.Equals(Constants.ParameterSetNames.ListParameterSet))
            {
                string expand = this.GenerateDownloadUrl.IsPresent ? DownloadUrlExpand : null;

                if (MaxCount.HasValue && (MaxCount.Value > 100 || MaxCount.Value < 1))
                {
                    throw new PSArgumentException(Resources.MaxCountExceedRangeError);
                }

                WriteObject(BillingManagementClient.Invoices.List(expand, null, null, MaxCount).Select(x => new PsInvoice(x)));
                return;
            }

            Invoice invoice = null;
            if (ParameterSetName.Equals(Constants.ParameterSetNames.LatestItemParameterSet))
            {
                invoice = BillingManagementClient.Invoices.GetLatest();
            }
            else if (ParameterSetName.Equals(Constants.ParameterSetNames.SingleItemParameterSet))
            {
                invoice = BillingManagementClient.Invoices.Get(this.Name);
            }
            if (invoice != null)
            {
                WriteObject(new PsInvoice(invoice));
            }
        }
    }
}
