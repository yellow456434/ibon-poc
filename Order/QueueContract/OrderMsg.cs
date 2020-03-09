using System;
namespace QueueContract
{
    public interface OrderMsg
    {
        int id { get; }
        string name { get; }
        DateTime dateTime { get; }
    }
}
