namespace JobmanagerTest.Abstract;

public abstract class BaseTest : IClassFixture<BaseTestWebAppFactory>
{
    protected readonly HttpClient HttpClient;

    protected BaseTest(BaseTestWebAppFactory factory)
    {
        HttpClient = factory.CreateClient();
    }
}
