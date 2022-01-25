using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Penrose.Extensions.Extensions
{
  public static class ObjectExtensions
  {
    public static T DeepCopy<T>(this T self)
    {
      MemoryStream memoryStream = new MemoryStream();
      BinaryFormatter formatter = new BinaryFormatter();
      formatter.Serialize(memoryStream, self);
      memoryStream.Seek(0L, SeekOrigin.Begin);

      return (T)formatter.Deserialize(memoryStream);
    }
  }
}