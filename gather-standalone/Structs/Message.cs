using System;

namespace gather_standalone.Structs
{
    internal class Message
    {
        public DateTime timestamp { get; set; }
        public string sender { get; set; }
        public string content { get; set; }
    }
}
