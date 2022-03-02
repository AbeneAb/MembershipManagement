using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RabbitMQEventbus
{
    public record Event
    {
        public Event()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }
        [JsonConstructor]
        public Event(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }
        [JsonInclude]
        public Guid Id { get; private init; }
        [JsonInclude]
        public DateTime CreationDate { get; private init; }
    }
}
