using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuuto.Model
{
    class DraftModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; internal set; }
        public int InReplyToId { get; internal set; }
        public bool Sensitive { get; internal set; }
        public string SpoilerText { get; internal set; }
        public string Visibility { get; internal set; }
        public List<MediaData> Medias { get; internal set; }
        public string AccessToken { get; internal set; }
        public string Domain { get; internal set; }
        public int AccountId { get; internal set; }
        public string ErrorMessage { get; set; }
    }
}
