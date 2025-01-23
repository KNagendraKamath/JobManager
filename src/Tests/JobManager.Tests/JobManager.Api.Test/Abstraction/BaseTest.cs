namespace JobManager.Api.Test.Abstraction;
[System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public abstract class BaseTest: IClassFixture<BaseTestWebAppFactory>
{
    protected readonly HttpClient HttpClient;

    private protected BaseTest(BaseTestWebAppFactory factory)
    {
        HttpClient = factory.CreateClient();
    }
}
