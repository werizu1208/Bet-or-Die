using UnityEngine;
using System;

public enum RouletteBetType { SingleNumber, Color, OddEven, HalfRange }
public enum RouletteColor { Red, Black }
public enum RouletteHalf { Low, High }

public class RouletteGame : MonoBehaviour
{
    public static RouletteGame Instance { get; private set; }

    private RouletteData data;
    private RouletteBetType betType;
    private int betNumber;
    private RouletteColor betColor;
    private bool betOdd;
    private RouletteHalf betHalf;

    public event Action<RouletteData> OnAwaitingBet;
    public event Action<int> OnWheelResult;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Begin(RouletteData d)
    {
        data = d;
        OnAwaitingBet?.Invoke(d);
    }

    public void PlaceBetSingleNumber(int number) { betType = RouletteBetType.SingleNumber; betNumber = number; Spin(); }
    public void PlaceBetColor(RouletteColor color) { betType = RouletteBetType.Color; betColor = color; Spin(); }
    public void PlaceBetOddEven(bool odd) { betType = RouletteBetType.OddEven; betOdd = odd; Spin(); }
    public void PlaceBetHalf(RouletteHalf half) { betType = RouletteBetType.HalfRange; betHalf = half; Spin(); }

    private void Spin()
    {
        int result = UnityEngine.Random.Range(0, data.numberCount + 1);
        OnWheelResult?.Invoke(result);

        if (result == 0)
        {
            GamblingManager.Instance.ResolveResult(false, 0f);
            return;
        }

        bool won = false;
        float multiplier = 1f;

        switch (betType)
        {
            case RouletteBetType.SingleNumber:
                won = result == betNumber;
                multiplier = data.singleNumberMultiplier;
                break;
            case RouletteBetType.Color:
                bool isRed = result % 2 == 1;
                won = (betColor == RouletteColor.Red) == isRed;
                multiplier = data.colorMultiplier;
                break;
            case RouletteBetType.OddEven:
                won = (result % 2 == 1) == betOdd;
                multiplier = data.oddEvenMultiplier;
                break;
            case RouletteBetType.HalfRange:
                bool isLow = result <= data.numberCount / 2;
                won = (betHalf == RouletteHalf.Low) == isLow;
                multiplier = data.halfRangeMultiplier;
                break;
        }

        GamblingManager.Instance.ResolveResult(won, multiplier);
    }
}
