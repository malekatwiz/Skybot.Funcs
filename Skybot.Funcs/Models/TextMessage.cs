namespace Skybot.Funcs.Models
{
    public class TextMessage
    {
        public string From { get; set; }
        public string Body { get; set; }
        public UserAccount Account { get; set; }
    }
}
