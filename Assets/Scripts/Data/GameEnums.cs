public enum GameState
{
    StartScreen,
    Explanation,
    RoomEntering,
    BetOrDieChoice,
    Betting,
    Gambling,
    DieFlow,
    EventRoom,
    StageEnd,
    GameOver,
    Victory,
    DemonBanishment
}

public enum LimbType
{
    RightArm,
    LeftArm,
    RightLeg,
    LeftLeg,
    Head
}

public enum GamblingGameType
{
    Blackjack,
    Baccarat,
    Chinchiro,
    ChoHan,
    AnimalRace,
    Roulette,
    Slots
}

public enum DemonPersonality
{
    Aggressive,
    Timid,
    Gambler,
    Lazy
}

public enum SkillEffectType
{
    GambleWinRateBoost,
    RerollLowResult,
    BlackjackPeek,
    DieCostReduction,
    LifespanBetMinReduction,
    ConversionRateBoost,
    BetInsurance,
    LimbChoiceOnDie,
    GameOverNullify,
    RoomSelection3Choice,
    CurseGreed,
    CurseTimid,
    CurseProtection
}

public enum EventRoomType
{
    ShopGold,
    ShopLifespan,
    SkillGrant,
    LimbRepair,
    CurseRoom,
    CapturedHuman
}

public enum ResourceType
{
    Gold,
    Lifespan,
    Limb
}

public enum BetCurrency
{
    Gold,
    Lifespan
}
