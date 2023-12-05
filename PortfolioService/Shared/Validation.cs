namespace ProtfolioService.Shared;

public record Validation<T>(bool Isvalid, T Value, string Message );