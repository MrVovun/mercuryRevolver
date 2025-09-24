using UnityEngine;

namespace MR.Core
{
    public enum GameState { Boot, MainMenu, InHQ, InCase, Countdown, Repose, Pause }

    public class Game : MonoBehaviour
    {
        public static Game Instance { get; private set; }
        [SerializeField] private GameState state = GameState.Boot;

        void Awake()
        {
            if (Instance != null) { Destroy(this.gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        public GameState State => state;

        public void SetState(GameState next)
        {
            state = next;
            Debug.Log($"Game State changed to {state}");
        }
    }


}
