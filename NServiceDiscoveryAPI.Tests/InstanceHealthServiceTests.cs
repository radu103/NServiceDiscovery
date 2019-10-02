using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NServiceDiscovery.Common.ServiceBase;
using NServiceDiscoveryAPI.Controllers;
using NServiceDiscoveryAPI.Services;

namespace NServiceDiscoveryAPI.Tests
{
    [TestClass]
    public class InstanceHealthServiceTests
    {
        [TestMethod]
        public void GetHealthTest()
        {
            var healthServiceMock = new Mock<IInstanceHealthService>();
    
            var expectedServiceHealth = new ServiceHealth() { 
                Status = "UP" 
            };

            healthServiceMock.Setup(s => s.GetHealth()).Returns(expectedServiceHealth);

            InstanceHealthService realhealthService = new InstanceHealthService();
            var realHealth = realhealthService.GetHealth();

            Assert.AreEqual(realHealth.Status, expectedServiceHealth.Status);
        }
    }
}
