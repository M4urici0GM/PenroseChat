
using System;
using Penrose.Core.Enums;

namespace Penrose.Application.DataTransferObjects
{
  public class ChatPropertiesDto
  {
    public Guid ChatId { get; set; }
    public string Name { get; set; }
    public ChatType Type { get; set; }
    public string PhotoUrl { get; set; }
    public bool IsMuted { get; set; }
    public bool IsPinned { get; set; }
  }
}