using CommertialPortal_WebAPI.Infrastructure.Data;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommertialPortal_WebAPI.Features.Analitics.IncrementPromo;

public class IncrementPromoCopiedCommandHandler : IRequestHandler<IncrementPromoCopiedCommand, Result>
{
    private readonly DataContext _context;

    public IncrementPromoCopiedCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(IncrementPromoCopiedCommand request, CancellationToken cancellationToken)
    {
        var analytics = await _context.PostAnalitics
            .FirstOrDefaultAsync(a => a.PostId == request.PostId, cancellationToken);

        if (analytics is null)
            return Result.Failure("Analytics for post not found.");

        analytics.PromosCopied = (analytics.PromosCopied ?? 0) + 1;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}

