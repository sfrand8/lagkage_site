namespace Lagkage.MinimalAPI.Features;

public class MedidatRResult<T>
{
    public bool IsSuccess { get; private set; }
    public T? Value { get; private set; }
    public Error Error { get; private set; }

    private MedidatRResult() { }

    public static MedidatRResult<T> Success(T value) => new MedidatRResult<T>() { IsSuccess = true, Value = value };
    public static MedidatRResult<T> Success() => new MedidatRResult<T>() { IsSuccess = true};
    public static MedidatRResult<T> Failure(Error error) => new MedidatRResult<T>() { IsSuccess = false, Error = error };
}