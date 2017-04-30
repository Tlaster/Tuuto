using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mastodon.Model;

namespace Tuuto.Model
{
    class ReplyModel
    {
        public ReplyModel()
        {

        }
        public ReplyModel(StatusModel replyStatus)
        {
            UserName = replyStatus.Account.UserName;
            Avatar = replyStatus.Account.Avatar;
            Content = replyStatus.Content;
            InReplyToId = replyStatus.Id;
            Acct = replyStatus.Account.Acct;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public string Content { get; set; }
        public int InReplyToId { get; internal set; }
        public string Acct { get; set; }
    }
    class DraftModel
    {
        public DraftModel() : this(-1, null)
        {

        }
        public DraftModel(int id, StatusModel replyStatus)
        {
            if (replyStatus != null)
            {
                ReplyStatus = new ReplyModel(replyStatus);
            }
            if (id > 0)
            {
                Id = id;
            }
            CreatedAt = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; internal set; }
        public ReplyModel ReplyStatus { get; set; }
        public bool Sensitive { get; internal set; }
        public string SpoilerText { get; internal set; }
        public string Visibility { get; internal set; }
        public List<MediaData> Medias { get; internal set; }
        public string AccessToken { get; internal set; }
        public string Domain { get; internal set; }
        public int AccountId { get; internal set; }
        public string ErrorMessage { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
