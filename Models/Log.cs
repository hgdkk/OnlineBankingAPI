using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBankingAPI.Models
{
    [Table("Logs")]
    public class Log
    {
        [Key]
        public int LogId { get; set; }

        public string Class { get; set; }

        public OperationType Operation { get; set; }

        public LogType LogType { get; set; }

        public string OldVersion { get; set; }

        public string NewVersion { get; set; }

        public DateTime LogDate { get; set; }

        public string Method { get; set; }
    }

    public enum OperationType
    {
        Create,
        Read,
        Update,
        Delete
    }

    public enum LogType
    {
        Info,
        Error,
        Warning
    }
}
