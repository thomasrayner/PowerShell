// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Management.Automation;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Microsoft.PowerShell.Commands
{
    [Serializable]
    internal class CatFact
    {
        public string Fact { get; set; }
        public int Length { get; set; }

        public CatFact (string Fact, int Length)
        {
            if (Fact.Length != Length)
            {
                throw new ArgumentNullException("Fact wasn't the length specified");
            }
            else
            {
                this.Fact = Fact;
                this.Length = Length;
            }
        }
    }
    
    /// <summary>
    /// This class implements Get-CatFact.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "CatFact", HelpUri = "https://catfact.ninja/")]
    [OutputType(typeof(CatFact))]
    public class GetCatFactCommand : PSCmdlet
    {
        private static CatFact GetCatFact()
        {
            string factURL = "https://catfact.ninja/fact";
            HttpClient factClient = new HttpClient();
            factClient.DefaultRequestHeaders.Accept.Clear();
            factClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            factClient.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            var result = factClient.GetStringAsync(new Uri(factURL)).Result;

            CatFact catFact = JsonConvert.DeserializeObject<CatFact>(result);

            return catFact;
        }

        /// <summary>
        /// Get a cat fact.
        /// </summary>
        protected override void ProcessRecord() // aka the process{} block
        {
            var catFactList = GetCatFact().Fact;

            WriteObject(catFactList, true);
        }
    }
}
