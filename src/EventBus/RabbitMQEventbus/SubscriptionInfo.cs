namespace RabbitMQEventbus;
public partial class EventBusSubscriptionsManager : IEventBusSubscriptionsManager
{
    public class SubscriptionInfo
    {
        public Type HandlerType { get; set; }
        public bool IsDynamic { get; set; }
        public SubscriptionInfo(bool isDynamic, Type handler)
        {
            IsDynamic = isDynamic;
            HandlerType = handler;
        }
        public static SubscriptionInfo Typed(Type hander) => new SubscriptionInfo(false, hander);
    }
}