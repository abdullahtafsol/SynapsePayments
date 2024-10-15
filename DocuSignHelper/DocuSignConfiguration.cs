using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSignHelper
{
    public class DocuSignConfiguration
    {
        public string BaseUrl { get; set; }

        public string IntegrationKey { get; set; }

        public string UserId { get; set; }

        public string BasePath { get; set; }

        public List<string> Scopes { get; set; }

        public string AccountId { get; set; }

        public string RedirectUri { get; set; }
    }
}
