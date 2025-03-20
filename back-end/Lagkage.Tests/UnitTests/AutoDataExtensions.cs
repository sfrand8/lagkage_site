using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lagkage.UnitTests;

public class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute()
        : this(() => new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true }))
    {
    }

    protected AutoMoqDataAttribute(Func<IFixture> fixtureFactory)
        : base(fixtureFactory)
    {
    }
}

public class AutoMoqControllerDataAttribute : AutoMoqDataAttribute
{
    public AutoMoqControllerDataAttribute()
        : base(() =>
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });

            // Prevent AutoFixture from setting properties on ControllerBase (which can cause issues)
            fixture.Customize<ControllerBase>(c => c.OmitAutoProperties());
            // Specifically ignore ControllerContext to avoid BindingInfo errors
            fixture.Inject(new ControllerContext
            {
                HttpContext = new DefaultHttpContext() // Mock HTTP Context
            });
            return fixture;
        })
    {
    }
}
