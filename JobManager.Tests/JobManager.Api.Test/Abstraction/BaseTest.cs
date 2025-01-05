namespace JobManager.Api.Test.Abstraction;
public abstract class BaseTest: IClassFixture<BaseTestWebAppFactory>
{
    protected readonly HttpClient HttpClient;

    public BaseTest(BaseTestWebAppFactory factory)
    {
        HttpClient = factory.CreateClient();
    }
}
