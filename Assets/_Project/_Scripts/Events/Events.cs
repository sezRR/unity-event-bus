public interface IEvent
{
}

public class TestEvent : IEvent { }

public struct PlayerEvent : IEvent
{
    public PlayerEvent(int health, int mana)
    {
        Health = health;
        Mana = mana;
    }

    public int Health { get; }
    public int Mana { get; }
}