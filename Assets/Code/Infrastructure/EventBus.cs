using System;

namespace PMT
{
    internal static class EventBus<T> where T : struct, IEventData
    {
        private static event Action<T> OnEvent;

        public static void Publish(T eventData) => OnEvent?.Invoke(eventData);
        public static void Subscribe(Action<T> handler) => OnEvent += handler;
        public static void Unsubscribe(Action<T> handler) => OnEvent -= handler;
    }
}