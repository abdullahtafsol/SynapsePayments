namespace DocuSignHelper
{
    public interface IDocuSign
    {
        string CreateSynapseEnvelope(string signerEmail, string signerName, int documentType, ref Dictionary<string, (byte[], string)> dict);

        string GetSigningUrl(string signerEmail, string signerName, string envelopeId);

        string GetEnvelopeStatus(string envelopeId);
    }
}
