using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AuthGuardCore.Models
{
    public class CreateMessageViewModel
    {
        [Required(ErrorMessage = "Alıcı mail zorunludur")]
        [EmailAddress]
        public string ReceiverEmail { get; set; }

        [Required(ErrorMessage = "Konu zorunludur")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Mesaj içeriği zorunludur")]
        public string MessageDetail { get; set; }

        [Required(ErrorMessage = "Kategori seçiniz")]
        public int CategoryID { get; set; }

        public List<SelectListItem>? Categories { get; set; }
    }

}
