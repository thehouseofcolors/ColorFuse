
using UnityEngine;


namespace GameEvents
{

    #region GameFlow
    public enum GamePauseType { Fail_TimeOver, Fail_NoMoves, Fail_UserQuit }
    public readonly struct GameLoadEvent : IGameEvent
    {
        public readonly LevelConfig Level;

        public GameLoadEvent(LevelConfig level)
        {
            Level = level;
        }
    }
    public struct GameResumeEvent : IGameEvent
    {
        public readonly JokerConfig jokerConfig;

        public GameResumeEvent(JokerConfig joker)
        {
            jokerConfig = joker;
        }
        
    }
    public struct GameWinEvent : IGameEvent { }

    public struct GamePauseEvent : IGameEvent
    {
        public GamePauseType Result;
        public GamePauseEvent(GamePauseType result) => Result = result;
    }
    #endregion

    #region UIEvent

    public enum ScreenType { Menu, Game, Loading, Win, Fail_TimeOver, Fail_NoMoves }
    public struct ScreenChangeEvent : IGameEvent
    {
        public ScreenType Screen;
        public object TransitionData;
        public ScreenChangeEvent(ScreenType screen, object data = null) =>
            (Screen, TransitionData) = (screen, data);
    }
    #endregion

    #region Gameplay
    public struct TileSelectionEvent : IGameEvent
    {
        public Tile Tile;
        public TileSelectionEvent(Tile tile) => Tile = tile;
    }

    public struct TileFuseEvent : IGameEvent
    {
        public Tile Source;
        public Tile Target;
        public TileFuseEvent(Tile source, Tile target) =>
            (Source, Target) = (source, target);
    }

    #endregion


    #region UI Events

    public struct UpdateTimerUIEvent : IGameEvent
    {
        public float RemainingTime;
        public UpdateTimerUIEvent(float time) => RemainingTime = time;
    }
    public struct UpdateMoveCountUIEvent : IGameEvent
    {
        public int MoveCount;
        public UpdateMoveCountUIEvent(int count) => MoveCount = count;
    }

    #endregion






}


