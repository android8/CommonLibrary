using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Models
{
    public class ErrorLogger
    {
        [Key]
        public int LoggerId { get; set; }
        [Required]
        public string ErrorDetails { get; set; } = String.Empty;
        public DateTime LogDate { get; set; } = DateTime.Now;
    }
}
