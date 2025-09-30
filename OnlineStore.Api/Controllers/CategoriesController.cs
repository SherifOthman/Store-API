using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.Common;
using OnlineStore.Application.Requests;
using OnlineStore.Application.Responses;
using OnlineStore.Application.Services;

namespace OnlineStore.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();

        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Result<CategoryResponse>>> GetById(int id)
    {
        var result = await _categoryService.GetByIdAsync(id);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Result<CategoryResponse>>> Create(CreateCategoryRequest request)
    {
        var result = await _categoryService.CreateCategoryAsync(request);
        if (!result.Success)
            return BadRequest(result);


        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryRequest request)
    {
        if (id != request.Id)
            return BadRequest(Result.Fail("Id in URL and body do not match."));

        var result = await _categoryService.UpdateAsync(request);
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _categoryService.DeleteAsync(id);

        if (!result.Success)
            return BadRequest(result);

        return NoContent();
    }
}
