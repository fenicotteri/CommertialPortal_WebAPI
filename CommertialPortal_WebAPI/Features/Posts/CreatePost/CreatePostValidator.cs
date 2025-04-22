using FluentValidation;

namespace CommertialPortal_WebAPI.Features.Posts.CreatePost;

public class CreatePostValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(1000).WithMessage("Content must not exceed 1000 characters.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.BranchIds)
            .NotEmpty().WithMessage("At least one branch must be specified.");

        RuleFor(x => x.EndDate)
            .Must((command, endDate) =>
            {
                if (endDate == null) return true;
                return endDate > command.StartDate;
            })
            .WithMessage("End date must be after start date.");
    }
}
