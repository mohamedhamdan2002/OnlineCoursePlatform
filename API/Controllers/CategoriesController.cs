using API.Requests.Categories;
using Application.Features.Categories.Commands.CreateCategory;
using Application.Features.Categories.Commands.DeleteCategory;
using Application.Features.Categories.Commands.UpdateCategory;
using Application.Features.Categories.Dots;
using Application.Features.Categories.Queries.GetAllCategories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CategoriesController(ISender sender) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllCategoriesQuery();
        var result = await sender.Send(query, cancellationToken);
        return HandleResult(result, () => Ok(result.Data));
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Post(CreateCategoryRequest request, CancellationToken cancellationToken)
    { 
        var command = new CreateCategoryCommand(request.Name);
        var result = await sender.Send(command);
        return HandleResult(result, () => StatusCode(StatusCodes.Status201Created, result.Data));
    }

    [HttpPut("{categoryId:guid}")]
    public async Task<IActionResult> Update(Guid categoryId,CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCategoryCommand(categoryId, request.Name);
        var result = await sender.Send(command);
        return HandleResult(result, () => NoContent());
    }

    [HttpDelete("{categoryId:guid}")]
    public async Task<IActionResult> Delete(Guid categoryId, CancellationToken cancellationToken)
    {
        var command = new DeleteCategoryCommand(categoryId);
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result, () => NoContent());
    }
}
