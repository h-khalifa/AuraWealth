using AuraWealth.MarketData.Domain.TradableAssets.Enums;

namespace AuraWealth.MarketData.Domain.Services;

public sealed class ExchangeClockService : IMarketSessionService
{
    private static readonly TimeZoneInfo EasternTimeZone =
        TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

    public MarketSession GetCurrentSession(DateTime utcNow, AssetClass assetClass)
    {
        // Crypto markets trade 24/7
        if (assetClass == AssetClass.Crypto)
            return MarketSession.Open;

        var easternTime = TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.SpecifyKind(utcNow, DateTimeKind.Utc),
            EasternTimeZone);

        if (easternTime.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
            return MarketSession.Closed;

        var timeOfDay = easternTime.TimeOfDay;

        return timeOfDay switch
        {
            _ when timeOfDay >= new TimeSpan(4, 0, 0) && timeOfDay < new TimeSpan(9, 30, 0) => MarketSession.PreMarket,
            _ when timeOfDay >= new TimeSpan(9, 30, 0) && timeOfDay < new TimeSpan(16, 0, 0) => MarketSession.Open,
            _ when timeOfDay >= new TimeSpan(16, 0, 0) && timeOfDay < new TimeSpan(20, 0, 0) => MarketSession.AfterHours,
            _ => MarketSession.Closed
        };
    }

    public TimeSpan GetMaxQuoteAge(MarketSession session) => session switch
    {
        MarketSession.Open => TimeSpan.FromSeconds(60),
        MarketSession.PreMarket or MarketSession.AfterHours => TimeSpan.FromMinutes(5),
        MarketSession.Closed => TimeSpan.FromHours(48), // Allows after-hours/weekend trading against Friday's close
        _ => TimeSpan.FromSeconds(60)
    };
}