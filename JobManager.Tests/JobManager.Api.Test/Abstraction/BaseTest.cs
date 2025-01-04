namespace JobManager.Api.Test.Abstraction;
internal class BaseTest: IClassFixture<BaseTestWebAppFactory>
{
    protected readonly HttpClient HttpClient;

    protected BaseTest(BaseTestWebAppFactory factory)
    {
        HttpClient = factory.CreateClient();
    }
}
