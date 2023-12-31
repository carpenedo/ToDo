using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Controllers;

[Route("api/[controller]")] // We define the routing that our controller going to use
[ApiController] // We need to specify the type of the controller to let .Net core know

public class TodoController : ControllerBase
{

    private readonly ApiDbContext _context;

    public TodoController(ApiDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult GetItems()
    {
        var items = _context.Items.ToList();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetItem(int id)
    {
        var item = await _context.Items.FirstOrDefaultAsync(z => z.Id == id);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> CreateItem(ItemData data)
    {
        if (ModelState.IsValid)
        {
            await _context.Items.AddAsync(data);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { data.Id }, data);
        }

        return new JsonResult("Somethign Went wrong") { StatusCode = 500 };
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(int id, ItemData item)
    {
        if (id != item.Id)
            return BadRequest();

        var existItem = await _context.Items.FirstOrDefaultAsync(z => z.Id == id);

        if (existItem == null)
            return NotFound();

        existItem.Title = item.Title;
        existItem.Details = item.Details;
        //existItem.Completed = item.Completed;

        await _context.SaveChangesAsync();

        // Following up the REST standart on update we need to return NoContent
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        var existItem = await _context.Items.FirstOrDefaultAsync(z => z.Id == id);

        if (existItem == null)
            return NotFound();

        _context.Items.Remove(existItem);
        await _context.SaveChangesAsync();

        return Ok(existItem);
    }


}

