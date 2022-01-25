using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;


namespace Penrose.Core.Common
{
  public abstract class AuditableEntity : ICloneable
  {
    public Guid Id { get; set; }
    public Guid Version { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }

    public string ToJson()
    {
      return JsonSerializer.Serialize(this);
    }

    public async Task<string> ToJsonAsync(CancellationToken cancellationToken)
    {
      await Task.Yield();
      await using MemoryStream memoryStream = new MemoryStream();
      await JsonSerializer.SerializeAsync<AuditableEntity>(memoryStream, this, null, cancellationToken);

      byte[] strBuffer = memoryStream.ToArray();
      return Encoding.UTF8.GetString(strBuffer);
    }

    public object Clone()
    {
      return this.MemberwiseClone();
    }
  }

}