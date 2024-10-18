using System.Text.Json.Serialization;

namespace Webhook
{

    public class EnvelopeEvent
    {
        [JsonPropertyName("event")]
        public string _event { get; set; }
        public string apiVersion { get; set; }
        public string uri { get; set; }
        public int retryCount { get; set; }
        public int configurationId { get; set; }
        public DateTime generatedDateTime { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string accountId { get; set; }
        public string userId { get; set; }
        public string envelopeId { get; set; }
        public Envelopesummary envelopeSummary { get; set; }
    }

    public class Envelopesummary
    {
        public string status { get; set; }
        public string documentsUri { get; set; }
        public string recipientsUri { get; set; }
        public string attachmentsUri { get; set; }
        public string envelopeUri { get; set; }
        public string emailSubject { get; set; }
        public string envelopeId { get; set; }
        public string signingLocation { get; set; }
        public string customFieldsUri { get; set; }
        public string notificationUri { get; set; }
        public string enableWetSign { get; set; }
        public string allowMarkup { get; set; }
        public string allowReassign { get; set; }
        public DateTime createdDateTime { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
        public DateTime deliveredDateTime { get; set; }
        public DateTime initialSentDateTime { get; set; }
        public DateTime sentDateTime { get; set; }
        public DateTime completedDateTime { get; set; }
        public DateTime statusChangedDateTime { get; set; }
        public string documentsCombinedUri { get; set; }
        public string certificateUri { get; set; }
        public string templatesUri { get; set; }
        public string expireEnabled { get; set; }
        public DateTime expireDateTime { get; set; }
        public string expireAfter { get; set; }
        public Sender sender { get; set; }
        public Recipients recipients { get; set; }
        public Envelopedocument[] envelopeDocuments { get; set; }
        public string purgeState { get; set; }
        public string envelopeIdStamping { get; set; }
        public string is21CFRPart11 { get; set; }
        public string signerCanSignOnMobile { get; set; }
        public string autoNavigation { get; set; }
        public string isSignatureProviderEnvelope { get; set; }
        public string hasFormDataChanged { get; set; }
        public string allowComments { get; set; }
        public string hasComments { get; set; }
        public string allowViewHistory { get; set; }
        public Envelopemetadata envelopeMetadata { get; set; }
        public object anySigner { get; set; }
        public string envelopeLocation { get; set; }
        public string isDynamicEnvelope { get; set; }
        public string burnDefaultTabData { get; set; }
    }

    public class Sender
    {
        public string userName { get; set; }
        public string userId { get; set; }
        public string accountId { get; set; }
        public string email { get; set; }
        public string ipAddress { get; set; }
    }

    public class Recipients
    {
        public Signer[] signers { get; set; }
        public object[] agents { get; set; }
        public object[] editors { get; set; }
        public object[] intermediaries { get; set; }
        public object[] carbonCopies { get; set; }
        public object[] certifiedDeliveries { get; set; }
        public object[] inPersonSigners { get; set; }
        public object[] seals { get; set; }
        public object[] witnesses { get; set; }
        public object[] notaries { get; set; }
        public string recipientCount { get; set; }
        public string currentRoutingOrder { get; set; }
    }

    public class Signer
    {
        public Signatureinfo signatureInfo { get; set; }
        public string creationReason { get; set; }
        public string isBulkRecipient { get; set; }
        public string requireUploadSignature { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string recipientId { get; set; }
        public string recipientIdGuid { get; set; }
        public string requireIdLookup { get; set; }
        public string userId { get; set; }
        public string clientUserId { get; set; }
        public string routingOrder { get; set; }
        public string status { get; set; }
        public string completedCount { get; set; }
        public DateTime signedDateTime { get; set; }
        public DateTime deliveredDateTime { get; set; }
        public DateTime sentDateTime { get; set; }
        public string deliveryMethod { get; set; }
        public string recipientType { get; set; }
    }

    public class Signatureinfo
    {
        public string signatureName { get; set; }
        public string signatureInitials { get; set; }
        public string fontStyle { get; set; }
    }

    public class Envelopemetadata
    {
        public string allowAdvancedCorrect { get; set; }
        public string enableSignWithNotary { get; set; }
        public string allowCorrect { get; set; }
    }

    public class Envelopedocument
    {
        public string documentId { get; set; }
        public string documentIdGuid { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
        public string order { get; set; }
        public Page[] pages { get; set; }
        public string display { get; set; }
        public string includeInDownload { get; set; }
        public string signerMustAcknowledge { get; set; }
        public string templateRequired { get; set; }
        public string authoritativeCopy { get; set; }
        public string PDFBytes { get; set; }
    }

    public class Page
    {
        public string pageId { get; set; }
        public string sequence { get; set; }
        public string height { get; set; }
        public string width { get; set; }
        public string dpi { get; set; }
    }
}
