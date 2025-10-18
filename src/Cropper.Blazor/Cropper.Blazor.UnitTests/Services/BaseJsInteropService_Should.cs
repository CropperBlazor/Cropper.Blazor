using Bogus;
using Bunit;

namespace Cropper.Blazor.UnitTests.Services
{
    public abstract class BaseJsInteropService_Should
    {
        protected readonly TestContext _testContext;

        public BaseJsInteropService_Should()
        {
            _testContext = new Faker<TestContext>()
                .Generate();
        }

        protected void VerifyLoadCropperModule(
            string pathToCropperModule)
        {
            _testContext.JSInterop
                .SetupModule(pathToCropperModule);
        }
    }
}
