using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using Microsoft.Extensions.Configuration;
using Services.Enums;
using System.Web;

namespace DocuSignHelper
{
    public class DocuSign : IDocuSign
    {
        private EnvelopesApi envelopesApi;
        private DocuSignConfiguration _docuSignConfiguration = new();
        private readonly IConfiguration _configuration;
        private readonly IMemoryCacheHelper _memoryCacheHelper;

        public DocuSign(IConfiguration configuration, IMemoryCacheHelper memoryCacheHelper)
        {
            _configuration = configuration;
            _memoryCacheHelper = memoryCacheHelper;
            LoadConfiguration();
            var apiClient = new DocuSignClient(_docuSignConfiguration.BaseUrl);
            var bytes = (byte[])memoryCacheHelper.Get("PrivateKey");
            var stream = new MemoryStream(bytes);

            var accessToken = apiClient.RequestJWTUserToken(_docuSignConfiguration.IntegrationKey, _docuSignConfiguration.UserId, _docuSignConfiguration.BasePath, stream, 1, _docuSignConfiguration.Scopes);
            global::DocuSign.eSign.Client.Auth.OAuth.UserInfo userInfo = apiClient.GetUserInfo(accessToken.access_token);
            envelopesApi = new EnvelopesApi(apiClient);
        }

        public string CreateSynapseEnvelope(string signerEmail, string signerName, int documentType, ref Dictionary<string, (byte[], string)> dict)
        {
            EnvelopeDefinition envelope = MakeSynapseEnvelope(signerEmail, signerName, _docuSignConfiguration.IntegrationKey, documentType, ref dict);
            EnvelopeSummary results = envelopesApi.CreateEnvelope(_docuSignConfiguration.AccountId, envelope);
            string envelopeId = results.EnvelopeId;
            return envelopeId;
        }

        public string GetSigningUrl(string signerEmail, string signerName, string envelopeId)
        {
            RecipientViewRequest viewRequest = MakeRecipientViewRequest(signerEmail, signerName, _docuSignConfiguration.RedirectUri, _docuSignConfiguration.IntegrationKey, envelopeId);
            ViewUrl results = envelopesApi.CreateRecipientView(_docuSignConfiguration.AccountId, envelopeId, viewRequest);
            return results.Url;
        }

        private EnvelopeDefinition MakeSynapseEnvelope(string signerEmail, string signerName, string signerClientId, int documentType, ref Dictionary<string, (byte[], string)> dict)
        {
            EnvelopeDefinition envelopeDefinition = new EnvelopeDefinition();
            envelopeDefinition.EmailSubject = "Please sign this document";

            Document document = new Document();
            string documentBase64String = Convert.ToBase64String(dict["Document"].Item1);
            string documentId = "1";
            document.DocumentBase64 = documentBase64String;
            document.Name = dict["Document"].Item2;
            document.FileExtension = "pdf";
            document.DocumentId = documentId;

            envelopeDefinition.Documents = new List<Document> { document };

            Signer signer = new Signer
            {
                Email = signerEmail,
                Name = signerName,
                ClientUserId = signerClientId,
                RecipientId = "1"
            };

            List<SignHere> signHereTabs = new List<SignHere>();

            if (documentType == (int)DocumentType.Exatouch)
            {
                SignHere signHerePage1 = new SignHere
                {
                    DocumentId = documentId,
                    PageNumber = "1",
                    RecipientId = "1",
                    XPosition = "50",
                    YPosition = "535"
                };

                SignHere signHerePage2 = new SignHere
                {
                    DocumentId = documentId,
                    PageNumber = "2",
                    RecipientId = "1",
                    XPosition = "100",
                    YPosition = "600"
                };

                signHereTabs.Add(signHerePage1);
                signHereTabs.Add(signHerePage2);
            }
            else if (documentType == (int)DocumentType.Merchant)
            {
                SignHere signHerePage1 = new SignHere
                {
                    DocumentId = documentId,
                    PageNumber = "2",
                    RecipientId = "1",
                    XPosition = "200",
                    YPosition = "600"
                };

                signHereTabs.Add(signHerePage1);
            }

            Tabs signerTabs = new Tabs
            {
                SignHereTabs = signHereTabs
            };
            signer.Tabs = signerTabs;

            Recipients recipients = new Recipients
            {
                Signers = new List<Signer> { signer }
            };
            envelopeDefinition.Recipients = recipients;
            envelopeDefinition.Status = "sent";

            return envelopeDefinition;
        }

        private RecipientViewRequest MakeRecipientViewRequest(string signerEmail, string signerName, string returnUrl, string signerClientId, string envelopeId, string pingUrl = null)
        {
            RecipientViewRequest viewRequest = new RecipientViewRequest();

            viewRequest.ReturnUrl = returnUrl + $"?envelopeid={HttpUtility.UrlEncode(envelopeId)}";

            viewRequest.AuthenticationMethod = "none";

            viewRequest.Email = signerEmail;
            viewRequest.UserName = signerName;
            viewRequest.ClientUserId = signerClientId;

            if (pingUrl != null)
            {
                viewRequest.PingFrequency = "600";
                viewRequest.PingUrl = pingUrl;
            }

            return viewRequest;
        }

        public string GetEnvelopeStatus(string envelopeId)
        {
            Envelope envelope = envelopesApi.GetEnvelope(_docuSignConfiguration.AccountId, envelopeId);
            return envelope.Status;
        }

        private void LoadConfiguration()
        {
            _docuSignConfiguration.BaseUrl = _configuration["BaseUrl"];
            _docuSignConfiguration.IntegrationKey = _configuration["IntegrationKey"];
            _docuSignConfiguration.UserId = _configuration["UserId"];
            _docuSignConfiguration.BasePath = _configuration["BasePath"];
            _docuSignConfiguration.Scopes = _configuration["Scopes"].Split(",").ToList();
            _docuSignConfiguration.AccountId = _configuration["AccountId"];
            _docuSignConfiguration.RedirectUri = _configuration["RedirectApiUrl"];
        }
    }
}