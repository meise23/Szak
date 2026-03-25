using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class PartnersController : ControllerBase
{
    private readonly AppDbContext _db;

    public PartnersController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetPartners()
    {
        return Ok(await _db.Partners.ToListAsync());
    }
    [HttpPost]
public async Task<IActionResult> CreatePartner([FromBody] Partner partner)
{
    if (string.IsNullOrWhiteSpace(partner.Name))
        return BadRequest("A partner neve kötelező.");

    _db.Partners.Add(partner);
    await _db.SaveChangesAsync();

    return Ok(partner);
}
[HttpPut("{id}")]
public async Task<IActionResult> UpdatePartner(int id, [FromBody] Partner partner)
{
    var existing = await _db.Partners.FindAsync(id);
    if (existing == null)
        return NotFound();

    existing.Name = partner.Name;

    await _db.SaveChangesAsync();
    return Ok(existing);
}

}
