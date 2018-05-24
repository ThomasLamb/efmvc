using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;
using System.Web.Http.Routing;
using NUnit.Framework;
using Moq;
using EFMVC.Data.Repositories;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Web.ViewModels;
using EFMVC.CommandProcessor.Command;
using EFMVC.Domain.Commands;
using AutoMapper;
using EFMVC.Web.API.Controllers;
using Newtonsoft.Json;
using EFMVC.Web.API.Models;
using EFMVC.Web.API;

namespace EFMVC.Tests.ApiControllers
{
    [TestFixture]
    public class CategoryApiControllerTest
    {
    private Mock<ICategoryRepository> categoryRepository;
    private Mock<ICommandBus> commandBus;
    [SetUp]
    public void SetUp()
    {
        categoryRepository = new Mock<ICategoryRepository>();
        commandBus = new Mock<ICommandBus>();
    }
    [Test]
    public void Get_All_Returns_AllCategory()
    {
        // Arrange   
        IEnumerable<CategoryWithExpense> fakeCategories = GetCategories();
        categoryRepository.Setup(x => x.GetCategoryWithExpenses()).Returns(fakeCategories);
        CategoryController controller = new CategoryController(commandBus.Object, categoryRepository.Object)
        {
            Request = new HttpRequestMessage()
                    {
                        Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
                    }
        };
        // Act
        var categories = controller.Get();
        // Assert
        Assert.IsNotNull(categories, "Result is null");
        Assert.IsInstanceOf(typeof(IEnumerable<CategoryWithExpense>),categories, "Wrong Model");        
        Assert.AreEqual(3, categories.Count(), "Got wrong number of Categories");
    }       
    [Test]
    public void Get_CorrectCategoryId_Returns_Category()
    {
        // Arrange   
        IEnumerable<CategoryWithExpense> fakeCategories = GetCategories();
        categoryRepository.Setup(x => x.GetCategoryWithExpenses()).Returns(fakeCategories);
        CategoryController controller = new CategoryController(commandBus.Object, categoryRepository.Object)
        {
            Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
            }
        };
        // Act
        var response = controller.Get(1);
        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var category = JsonConvert.DeserializeObject<CategoryWithExpense>(response.Content.ReadAsStringAsync().Result);
        Assert.AreEqual(1, category.CategoryId, "Got wrong number of Categories");
    }
    [Test]
    public void Get_InValidCategoryId_Returns_NotFound()
    {
        // Arrange   
        IEnumerable<CategoryWithExpense> fakeCategories = GetCategories();
        categoryRepository.Setup(x => x.GetCategoryWithExpenses()).Returns(fakeCategories);
        CategoryController controller = new CategoryController(commandBus.Object, categoryRepository.Object)
        {
            Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
            }
        };
        // Act
        var response = controller.Get(5);
        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
          
    }
    [Test]
    public void Post_Category_Returns_CreatedStatusCode()
    {
        // Arrange   
        commandBus.Setup(c => c.Submit(It.IsAny<CreateOrUpdateCategoryCommand>())).Returns(new CommandResult(true));
        Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();     
        var httpConfiguration = new HttpConfiguration();
        WebApiConfig.Register(httpConfiguration);
        var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
            new HttpRouteValueDictionary { { "controller", "category" } });
        var controller = new CategoryController(commandBus.Object, categoryRepository.Object)
        {
            Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/category/")
            {
                Properties = 
                {
                    { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                    { HttpPropertyKeys.HttpRouteDataKey, httpRouteData } 
                }
            }
        };
        // Act
        CategoryModel category = new CategoryModel();
        category.CategoryId = 1;
        category.CategoryName = "Mock Category";
        var response = controller.Post(category);          
        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        var newCategory = JsonConvert.DeserializeObject<CategoryModel>(response.Content.ReadAsStringAsync().Result);
        Assert.AreEqual(string.Format("http://localhost/api/category/{0}", newCategory.CategoryId), response.Headers.Location.ToString());
    }
    [Test]
    public void Post_EmptyCategory_Returns_BadRequestStatusCode()
    {
        // Arrange   
        commandBus.Setup(c => c.Submit(It.IsAny<CreateOrUpdateCategoryCommand>())).Returns(new CommandResult(true));
        Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
        var httpConfiguration = new HttpConfiguration();
        WebApiConfig.Register(httpConfiguration);
        var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
            new HttpRouteValueDictionary { { "controller", "category" } });
        var controller = new CategoryController(commandBus.Object, categoryRepository.Object)
        {
            Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/category/")
            {
                Properties = 
                {
                    { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                    { HttpPropertyKeys.HttpRouteDataKey, httpRouteData } 
                }
            }
        };
        // Act
        CategoryModel category = new CategoryModel();
        category.CategoryId = 0;
        category.CategoryName = "";
        // The ASP.NET pipeline doesn't run, so validation don't run. 
        controller.ModelState.AddModelError("", "mock error message");
        var response = controller.Post(category);
        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

    }
    [Test]
    public void Put_Category_Returns_OKStatusCode()
    {
        // Arrange   
        commandBus.Setup(c => c.Submit(It.IsAny<CreateOrUpdateCategoryCommand>())).Returns(new CommandResult(true));
        Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
        CategoryController controller = new CategoryController(commandBus.Object, categoryRepository.Object)
        {
            Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
            }
        };
        // Act
        CategoryModel category = new CategoryModel();
        category.CategoryId = 1;
        category.CategoryName = "Mock Category";
        var response = controller.Put(category.CategoryId,category);
        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);   
    }
    [Test]
    public void Delete_Category_Returns_NoContentStatusCode()
    {
        // Arrange         
        commandBus.Setup(c => c.Submit(It.IsAny<DeleteCategoryCommand >())).Returns(new CommandResult(true));
        CategoryController controller = new CategoryController(commandBus.Object, categoryRepository.Object)
        {
            Request = new HttpRequestMessage()
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
            }
        };
        // Act          
        var response = controller.Delete(1);
        // Assert
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

    }
    private static IEnumerable<CategoryWithExpense> GetCategories()
    {
        IEnumerable<CategoryWithExpense> fakeCategories = new List<CategoryWithExpense> {
        new CategoryWithExpense {CategoryId=1, CategoryName = "Test1", Description="Test1Desc", TotalExpenses=1000},
        new CategoryWithExpense {CategoryId=2, CategoryName = "Test2", Description="Test2Desc",TotalExpenses=2000},
        new CategoryWithExpense { CategoryId=3, CategoryName = "Test3", Description="Test3Desc",TotalExpenses=3000}  
        }.AsEnumerable();
        return fakeCategories;
    }
    }
}
