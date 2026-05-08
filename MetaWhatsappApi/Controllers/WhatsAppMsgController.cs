using MetaWhatsappApi.Data;
using MetaWhatsappApi.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MetaWhatsappApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsAppMsgController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WhatsAppMsgController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendWhatsAppMessage([FromBody] WhatsAppRequestModel request)
        {
            try
            {
                // 🔐 Step 1: Headers read
                var apiKey = Request.Headers["x-api-key"].FirstOrDefault();
                //   var secretKey = Request.Headers["x-secret-key"].FirstOrDefault();

                if (string.IsNullOrEmpty(apiKey)) // || string.IsNullOrEmpty(secretKey)
                    return Unauthorized("API Key or Secret missing");

                // 🔍 Step 2: Validate API Key
                var apiUser = await _context.ApiCredentialsTbl
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.ApiKey == apiKey && x.IsActive);

                if (apiUser == null)
                    return Unauthorized("Invalid API Key");

                // 🔐 Step 3: Verify Secret Key
                //if (!VerifySecret(secretKey, apiUser.SecretKeyHash))
                //    return Unauthorized("Invalid Secret Key");

                // 📦 Step 4: Get WhatsApp Config
                var config = await _context.WhatsAppConfigs.FirstOrDefaultAsync(x => x.UserId == apiUser.UserId && x.IsActive);

                if (config == null)
                    return BadRequest("WhatsApp configuration not found");

                // 🌐 Step 5: Prepare URL
                var url = $"https://graph.facebook.com/v25.0/{config.PhoneNumberId}/messages";

                // 🧠 Step 6: Payload
                var payload = new
                {
                    messaging_product = "whatsapp",
                    to = request.Mobile,
                    type = "template",
                    template = new
                    {
                        name = "bookingpdf",
                        language = new { code = "en" }
                        ,
                        components = new object[]
                        {
                        new {
                            type = "header",
                            parameters = new object[]
                            {
                                new {
                                    type = "document",
                                    document = new {
                                        link = $"https://vcargosoft.com/Doc/BookingPDF/LuggageBooking1846028.pdf"
                                    }
                                }
                            }
                        },
                        new {
                            type = "body",
                            parameters = new object[]
                            {
                                new { type = "text", text = "document"  },
                                new { type = "text", text = "document"  },
                                new { type = "text", text = "document"  },
                                new { type = "text", text = "document"  },

                                new { type = "text", text = "document"  },
                                new { type = "text", text = "document"  },
                                new { type = "text", text = "document"  },
                                new { type = "text", text = "document"  },

                                new { type = "text", text = "document"  },
                                new { type = "text", text = "document"  },
                                new { type = "text", text = "document"  },
                                new { type = "text", text = "document"  },

                                new { type = "text", text = "document"  }


                            }
                        }
                        }
                    }
                };

                // 🚀 Step 7: Call Meta API
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer" , config.AccessToken);

                    var response = await client.PostAsJsonAsync(url, payload);
                    var result = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return Ok(new
                        {
                            success = true,
                            message = "WhatsApp message sent successfully",
                            data = result
                        });
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            success = false,
                            message = "Meta API error",
                            error = result
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal Server Error",
                    error = ex.Message
                });
            }
        }




        // 🔐 Secret Hash Verify
        private bool VerifySecret(string input, string hash)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                var hashedInput = Convert.ToBase64String(
                    sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input)));

                return hashedInput == hash;
            }
        }
    }
}
