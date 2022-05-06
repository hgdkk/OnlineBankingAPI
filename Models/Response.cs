namespace OnlineBankingAPI.Models
{
    public class Response
    {        
        public string ResponseMessage{ get; set; }
        public bool IsSuccess { get; set; }
        public object Data { get; set; }
    }
}
