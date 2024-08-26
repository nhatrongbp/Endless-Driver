using System;

public class GameEvents
{
    public static Action<int> OnTestObserver;

    //ui
    public static Action<int> OnCoinsChanged;

    //ingame
    public static Action<float, float> OnPlayerInput;
    public static Action<float> OnPlayerPositionZChanged;
    public static Action<float> OnSpeedChanged;
    public static Action OnGameOver;

    //fever
    public static Action OnFeverBegin, OnFeverEnd;
}
