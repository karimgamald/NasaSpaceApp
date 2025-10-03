using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkTracking.Core.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace SharkTracking.InfrastructureData.Data
    {
        public class SharkMedia
        {
            [Key]
            public int Id { get; set; } // معرف الصورة أو الفيديو

            [Required]
            public int SharkId { get; set; } // معرف القرش الذي ينتمي إليه الوسائط

            [Required]
            public DateTime Timestamp { get; set; } // وقت أخذ الصورة أو الفيديو

            [Required]
            [MaxLength(500)]
            public string MediaUrl { get; set; } // رابط الصورة أو الفيديو

            [Required]
            [MaxLength(50)]
            public string MediaType { get; set; } // نوع الوسائط: Image أو Video

            // علاقة مع جدول Sharks (اختياري ولكن مفيد للاستعلام Include)
            [ForeignKey("SharkId")]
            public virtual Shark Shark { get; set; }
        }
    }

}
