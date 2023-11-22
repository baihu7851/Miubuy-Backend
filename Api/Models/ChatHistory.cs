using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Api.Models
{
    public class ChatHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "聊天類型")]
        public ChatType ChatType { get; set; }

        [Display(Name = "傳送者Id")]
        public int SenderId { get; set; }

        [Display(Name = "傳送對象Id")]
        public int RecipientId { get; set; }

        [Display(Name = "房間Id")]
        public int RoomId { get; set; }

        [Display(Name = "訊息內容")]
        public string Message { get; set; }

        [Display(Name = "傳送時間")]
        public DateTime MsgTime { get; set; }
    }
}