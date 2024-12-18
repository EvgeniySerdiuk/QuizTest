namespace Project.Scripts.EventBuss
{
    public interface IListener<T>
    {
        public void Execute(T value);
    }
}