using BMT.CMS.Core.Constants;
using BMT.CMS.Core.CMS;
using BMT.CMS.Infrastructure.Data;
using MediatR;


namespace BMT.CMS.Application.Features.CMS.UploadProcessor.Commands;

public record UploadProcessorCommand : IRequest<int>
{
    public string FilePath { get; set; } = "";
    public string FileType { get; set; } = "";
    public string Module { get; set; } = "";
    public string UploadType { get; set; } = "";
}

public class UploadProcessorCommandHandler : IRequestHandler<UploadProcessorCommand, int>
{
    private readonly ApplicationContext _context;
    public UploadProcessorCommandHandler(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(UploadProcessorCommand request, CancellationToken cancellationToken)
    {
        var uploadProcessor = new UploadProcessorState()
        {
            FileType = request.FileType,
            Path = request.FilePath,
            Status = FileUploadStatus.Pending,
            Module = request.Module,
            UploadType = request.UploadType,

        };
        await _context.AddAsync(uploadProcessor, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return 1;
    }
}

