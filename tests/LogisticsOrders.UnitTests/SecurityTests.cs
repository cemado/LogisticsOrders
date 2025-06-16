using Xunit;
using LogisticsOrders.API.Pages.Orders;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Reflection;

namespace LogisticsOrders.UnitTests;

public class SecurityTests
{
    [Fact]
    public void EditModel_HasAuthorizeAttribute()
    {
        var type = typeof(EditModel);
        var hasAuthorize = type.GetCustomAttributes(typeof(AuthorizeAttribute), true).Any();
        Assert.True(hasAuthorize);
    }
}