using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace ApplicationTracker.Models
{
    /// <summary>
    /// Model that handles the Job application Data creation
    /// </summary>
    public class ApplicationModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Company Name is Required")]
        public string CompanyName { get; set; } = string.Empty;
        [Required(ErrorMessage = "The Date you applied is Required")]
        public DateOnly DateApplied { get; set; }
        [Required(ErrorMessage = "Status of Application is Required")]
        public string? Status { get; set; }
        public string? Notes { get; set; }

    }
}
