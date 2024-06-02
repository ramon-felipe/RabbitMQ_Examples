namespace Core;
public interface IMessageBus {
    void Publish<T> (T message);
}
