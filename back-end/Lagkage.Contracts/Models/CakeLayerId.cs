namespace Lagkage.Contracts.Models;

public class CakeLayerId
{
    public CakeLayerId(Guid value)
    {
        Value = value;
    }

    //Creates a CakeLayerId deterministically from the CakelayerName to help with idempotency
    public CakeLayerId(CakeLayerName nameValue)
    {
        Value = GuidGenerator.CreateGuidFromString(nameValue.Value);
    }

    public Guid Value { get;  }
}