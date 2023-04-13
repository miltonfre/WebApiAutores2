using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiAutores.Controllers;
using WebApiAutores.Test.Mocks;

namespace WebApiAutores.Test.UnitTests
{
    [TestClass]
    public class RouteControllerTests
    {
        [TestMethod]
        public async Task IfUserIsAdmin_Got4Links()
        {
            //preparation
            var authorizationService= new AuthorizationServiceMock();
            authorizationService.Result = AuthorizationResult.Success();
            var rootController=new RouteController(authorizationService);
            rootController.Url = new URLHelperMock();
            //Execution
            var result= await rootController.GetRoute();

            //Verification
            Assert.AreEqual(4, result.Value.Count());
        }

        [TestMethod]
        public async Task IfUserIsNotAdmin_Got2Links()
        {
            //preparation
            var authorizationService = new AuthorizationServiceMock();
            authorizationService.Result = AuthorizationResult.Failed();
            var rootController = new RouteController(authorizationService);
            rootController.Url = new URLHelperMock();
            //Execution
            var result = await rootController.GetRoute();

            //Verification
            Assert.AreEqual(2, result.Value.Count());
        }

        [TestMethod]
        public async Task IfUserIsNotAdmin_Got2Links_UsingMoq()
        {
            //preparation
            var mockAuthorizationServices = new Mock<IAuthorizationService>();
            mockAuthorizationServices.Setup(x =>
                  x.AuthorizeAsync
                  (
                        It.IsAny<ClaimsPrincipal>()
                        , It.IsAny<object>()
                        , It.IsAny<IEnumerable<IAuthorizationRequirement>>()
                    )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            mockAuthorizationServices.Setup(x =>
                 x.AuthorizeAsync
                 (
                       It.IsAny<ClaimsPrincipal>()
                       , It.IsAny<object>()
                       , It.IsAny<string>()
                   )).Returns(Task.FromResult(AuthorizationResult.Failed()));


            var rootController = new RouteController(mockAuthorizationServices.Object);

            var mockUrlHelper=new Mock<IUrlHelper>();
            mockUrlHelper.Setup(x =>
                                x.Link(
                                    It.IsAny<string>()
                                    , It.IsAny<object>()
                                    )).Returns(string.Empty);
            rootController.Url = mockUrlHelper.Object;
            //Execution
            var result = await rootController.GetRoute();

            //Verification
            Assert.AreEqual(2, result.Value.Count());
        }
    }
}
