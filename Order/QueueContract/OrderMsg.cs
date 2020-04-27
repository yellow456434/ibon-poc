using System;
namespace QueueContract
{
    public interface OrderMsg
    {
        int id { get; }
        string name { get; }
        DateTime dateTime { get; }
    }

    public interface PriceMsg
    {
        int uuid { get; }
        string price { get; }
    }
}
