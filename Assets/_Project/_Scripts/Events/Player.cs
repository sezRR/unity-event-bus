using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int mana;

    private EventBinding<TestEvent> _testEventBinding;
    private EventBinding<PlayerEvent> _playerEventBinding;

    private void OnEnable()
    {
        _testEventBinding = new EventBinding<TestEvent>(HandleTestEvent);
        EventBus<TestEvent>.Register(_testEventBinding);
        
        _playerEventBinding = new EventBinding<PlayerEvent>(HandlePlayerEvent);
        EventBus<PlayerEvent>.Register(_playerEventBinding);
    }

    private void OnDisable()
    {
        EventBus<TestEvent>.DeRegister(_testEventBinding);
        EventBus<PlayerEvent>.DeRegister(_playerEventBinding);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            EventBus<TestEvent>.Raise(new TestEvent());

        if (Input.GetKeyDown(KeyCode.S))
            EventBus<PlayerEvent>.Raise(new PlayerEvent(health, mana));
    }

    void HandleTestEvent()
    {
        Debug.Log("Test event received!");
    }

    void HandlePlayerEvent(PlayerEvent playerEvent)
    {
        Debug.Log($"Player event received! Health: {playerEvent.Health}, Mana: {playerEvent.Mana}");
    }
}