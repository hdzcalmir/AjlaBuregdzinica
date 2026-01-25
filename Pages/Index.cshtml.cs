using BuregdzinicaAjla.Model;
using BuregdzinicaAjla.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BuregdzinicaAjla.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public EmailForm EmailForm { get; set; } = new();

        public IndexModel(ILogger<IndexModel> logger, IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostContactAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Provjerite unesene podatke.");
                }

                if (!new EmailAddressAttribute().IsValid(EmailForm.Email))
                {
                    return BadRequest("Unesite ispravan email.");
                }

                await _emailSender.SendEmailAsync(EmailForm);

                return Content("OK", "text/plain");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri slanju emaila.");

                return BadRequest("Došlo je do greške prilikom slanja poruke. Pokušajte ponovo.");
            }
        }

    }
}
