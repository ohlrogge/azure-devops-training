using Xunit;

namespace HelloWorld.UnitTests.Services
{
    public class HelloWorldService_IsHelloWorldShould
    {
        [Fact]
        public void IsHelloWorld_ReturnsHelloWorld()
        {
            var helloWorldService = new HelloWorldService();
            string result = helloWorldService.GetHelloWorld();

            Assert.Equal("Hello, World!", result);
        }
    }
}
