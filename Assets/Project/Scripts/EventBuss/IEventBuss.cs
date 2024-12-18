namespace Project.Scripts.EventBuss
{
    public interface IEventBuss
    {
        public void AddListener<T>(IListener<T> listener, int priority = 0) where T : new();
        public void RemoveListener<T>(IListener<T> listener) where T : new();
        public void Execute<T>(T value) where T : new();
    }
}