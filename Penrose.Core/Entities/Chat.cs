using System.Collections.Generic;
using Penrose.Core.Common;

namespace Penrose.Core.Entities
{
  public class Chat : AuditableEntity
  {
    public IEnumerable<ChatParticipant> Participants { get; set; }
    public IEnumerable<ChatMessage> Messages { get; set; }
    public ChatProperties Properties { get; set; }
  }
}