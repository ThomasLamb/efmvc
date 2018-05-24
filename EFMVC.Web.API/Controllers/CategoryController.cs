
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using EFMVC.Data.Repositories;
using System.Net;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Domain.Commands;
using EFMVC.Web.API.Models;
using EFMVC.Model;
using System.Collections.Generic;
namespace EFMVC.Web.API.Controllers
{
    //This HTTP service provides the category data with type Category/CategoryWithExpense
    public class CategoryController : ApiController
    {

        private readonly ICommandBus commandBus;
        private readonly ICategoryRepository categoryRepository;
        public CategoryController(ICommandBus commandBus, ICategoryRepository categoryRepository)
        {
            this.commandBus = commandBus;
            this.categoryRepository = categoryRepository;
        }
    public IQueryable<CategoryWithExpense> Get()
    {
        var categories = categoryRepository.GetCategoryWithExpenses().AsQueryable();
        return categories;
    }

    // GET /api/category/5
    public HttpResponseMessage Get(int id)
    {
        var category = categoryRepository.GetCategoryWithExpenses().Where(c => c.CategoryId == id).SingleOrDefault();
        if (category == null)
        {
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
        return Request.CreateResponse(HttpStatusCode.OK, category);
    }

    // POST /api/category
    public HttpResponseMessage Post(CategoryModel category)
    {

        if (ModelState.IsValid)
        {
            var command = new CreateOrUpdateCategoryCommand(category.CategoryId, category.CategoryName, category.Description);
            var result = commandBus.Submit(command);
            if (result.Success)
            {                  
                var response = Request.CreateResponse(HttpStatusCode.Created, category);
                string uri = Url.Link("DefaultApi", new { id = category.CategoryId });
                response.Headers.Location = new Uri(uri);
                return response;
            }
        }
        else
        {
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }
        throw new HttpResponseException(HttpStatusCode.BadRequest);
    }

    // PUT /api/category/5
    public HttpResponseMessage Put(int id, CategoryModel category)
    {
        if (ModelState.IsValid)
        {
            var command = new CreateOrUpdateCategoryCommand(category.CategoryId, category.CategoryName, category.Description);
            var result = commandBus.Submit(command);
            return Request.CreateResponse(HttpStatusCode.OK, category);
        }
        else
        {
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }
        throw new HttpResponseException(HttpStatusCode.BadRequest);
    }

        // DELETE /api/category/5
        public HttpResponseMessage Delete(int id)
        {
            var command = new DeleteCategoryCommand { CategoryId = id };
            var result = commandBus.Submit(command);
            if (result.Success)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
                throw new HttpResponseException(HttpStatusCode.BadRequest);
        }
    }
}

