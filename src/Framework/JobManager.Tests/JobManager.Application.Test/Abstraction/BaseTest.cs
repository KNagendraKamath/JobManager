

namespace JobManager.Application.Test.Abstraction;
[System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public abstract class BaseTest : IClassFixture<BaseTestWebAppFactory>
{
    private protected BaseTest(BaseTestWebAppFactory factory)
    {
     
    }
}
