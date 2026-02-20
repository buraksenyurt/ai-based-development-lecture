using CvApp.Application.DTOs;
using CvApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CvApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResumesController : ControllerBase
{
    private readonly IResumeService _resumeService;

    public ResumesController(IResumeService resumeService)
    {
        _resumeService = resumeService;
    }

    /// <summary>Tüm CV'leri listeler.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ResumeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var resumes = await _resumeService.GetAllAsync(cancellationToken);
        return Ok(resumes);
    }

    /// <summary>Belirtilen Id'ye sahip CV'yi getirir.</summary>
    [HttpGet("{resumeId:guid}")]
    [ProducesResponseType(typeof(ResumeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid resumeId, CancellationToken cancellationToken)
    {
        var resume = await _resumeService.GetByIdAsync(resumeId, cancellationToken);
        return resume is null ? NotFound() : Ok(resume);
    }

    /// <summary>Yeni bir CV oluşturur.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ResumeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateResumeRequest request, CancellationToken cancellationToken)
    {
        var resume = await _resumeService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { resumeId = resume.ResumeId }, resume);
    }

    /// <summary>CV'yi siler.</summary>
    [HttpDelete("{resumeId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid resumeId, CancellationToken cancellationToken)
    {
        await _resumeService.DeleteAsync(resumeId, cancellationToken);
        return NoContent();
    }

    /// <summary>CV'ye iletişim bilgisi ekler.</summary>
    [HttpPost("{resumeId:guid}/contacts")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddContact(
        Guid resumeId,
        [FromBody] CreateContactRequest request,
        CancellationToken cancellationToken)
    {
        await _resumeService.AddContactAsync(resumeId, request, cancellationToken);
        return NoContent();
    }

    /// <summary>CV'den iletişim bilgisi kaldırır.</summary>
    [HttpDelete("{resumeId:guid}/contacts/{contactId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveContact(
        Guid resumeId,
        Guid contactId,
        CancellationToken cancellationToken)
    {
        await _resumeService.RemoveContactAsync(resumeId, contactId, cancellationToken);
        return NoContent();
    }
}
