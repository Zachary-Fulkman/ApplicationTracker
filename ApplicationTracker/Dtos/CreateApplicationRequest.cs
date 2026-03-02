using System.ComponentModel.DataAnnotations;

namespace ApplicationTracker.Dtos
{
    public class CreateApplicationRequest
    {
        [Required(ErrorMessage = "Company Name is Required")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Date you applied is Required")]
        public DateOnly DateApplied { get; set; }

        [Required(ErrorMessage = "Status of Application is Required")]
        public string? Status { get; set; }

        public string? Notes { get; set; }
    }
}
