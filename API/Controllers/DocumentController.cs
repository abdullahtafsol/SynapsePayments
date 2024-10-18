using DocuSignHelper;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocuSign _docuSign;
        private readonly ILogger<DocumentController> _logger;
        private readonly IConfiguration _configuration;

        public DocumentController(IDocuSign docuSign, ILogger<DocumentController> logger, IConfiguration configuration)
        {
            _docuSign = docuSign;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("synapse-document")]
        public async Task<IActionResult> AddSynapseDocumentAsync(IFormFile pdfFile, [FromForm] string signerName, [FromForm] int documentType, [FromForm] string email)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await pdfFile.CopyToAsync(memoryStream);
                    byte[] pdfBytes = memoryStream.ToArray();

                    var dict = new Dictionary<string, (byte[], string)>
                    {
                            { "Document", (pdfBytes, pdfFile.FileName) }
                    };

                    string envelopeId = _docuSign.CreateSynapseEnvelope(email, signerName, documentType, ref dict);
                    string signingUrl = _docuSign.GetSigningUrl(email, signerName, envelopeId);
                    string status = _docuSign.GetEnvelopeStatus(envelopeId);

                    var result = new List<string> { envelopeId, signingUrl, status };

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("redirect")]
        public IActionResult RedirectAsync([FromQuery(Name = "event")] string eventName)
        {
            try
            {
                bool isCompleted = eventName == "signing_complete";

                string returnUrl = _configuration["SuccessRedirectUrl"];

                if (isCompleted)
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    returnUrl = _configuration["FailureRedirectUrl"];
                    return Redirect(returnUrl);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
