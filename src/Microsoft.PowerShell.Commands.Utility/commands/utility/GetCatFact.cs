// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace Microsoft.PowerShell.Commands
{
    internal class CatFact
    {
        public string Fact { get; set; }
        public int Length { get; set; }

        public CatFact (string Fact, int Length)
        {
            this.Fact = Fact;
            this.Length = Length;
        }
    }
    
    /// <summary>
    /// This class implements Get-CatFact.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "CatFact", HelpUri = "https://catfact.ninja/")]
    [OutputType(typeof(CatFact))]
    public class GetCatFactCommand : PSCmdlet
    {
        private static async Task<List<CatFact>> GetCatFact()
        {
            string factURL = "https://catfact.ninja/fact";
            HttpClient factClient = new HttpClient();
            factClient.DefaultRequestHeaders.Accept.Clear();
            factClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            factClient.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var serializer = new DataContractJsonSerializer(typeof(List<CatFact>));
            var streamTask = factClient.GetStreamAsync(factURL);
            var catFactList = serializer.ReadObject(await streamTask) as List<CatFact>;

            return catFactList;
        }

        /// <summary>
        /// Get a cat fact.
        /// </summary>
        protected override void ProcessRecord() // aka the process{} block
        {
            var catFactList = GetCatFact().Result;

            WriteObject(catFactList, true);
        }
    }
}
