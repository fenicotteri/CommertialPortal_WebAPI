using Xunit;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CommertialPortal_WebAPI.Infrastructure.Data;
using CommertialPortal_WebAPI.Features.Posts.AddFavouritePost;
using CommertialPortal_WebAPI.Domain.Entities;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace TestCommertialPortal;

public class AddFavouritePostCommandHandlerTests
{
    private DataContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{System.Guid.NewGuid()}")
            .Options;
        return new DataContext(options);
    }

    private IHttpContextAccessor CreateHttpContextAccessor(string email)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email)
            };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);

        var context = new DefaultHttpContext { User = user };
        var mock = new Mock<IHttpContextAccessor>();
        mock.Setup(x => x.HttpContext).Returns(context);

        return mock.Object;
    }

    [Fact]
    public async Task Handle_ShouldAddPostToFavourites_WhenPostIsNotYetFavourite()
    {
        // Arrange
        var context = CreateInMemoryDbContext();
        var email = "test@example.com";

        var user = new User
        {
            Email = email,
            ClientProfile = new ClientProfile
            {
                UserId = "hyft6uigu8h",
                FavouritePosts = new List<Post>()
            }
        };

        var post = new Post
        {
            Id = 1,
            Title = "Test Post"
        };

        context.Users.Add(user);
        context.Posts.Add(post);
        await context.SaveChangesAsync();

        var handler = new AddFavouritePostCommandHandler(context, CreateHttpContextAccessor(email));
        var command = new AddFavouritePostCommand(post.Id);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(user.ClientProfile.FavouritePosts);
        Assert.Equal(post.Id, user.ClientProfile.FavouritePosts.First().Id);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenPostAlreadyFavourite()
    {
        // Arrange
        var context = CreateInMemoryDbContext();
        var email = "test@example.com";

        var post = new Post
        {
            Id = 1,
            Title = "Test Post"
        };

        var user = new User
        {
            Email = email,
            ClientProfile = new ClientProfile
            {
                UserId = "hyft6uigu8h",
                FavouritePosts = new List<Post> { post }
            }
        };

        context.Users.Add(user);
        context.Posts.Add(post);
        await context.SaveChangesAsync();

        var handler = new AddFavouritePostCommandHandler(context, CreateHttpContextAccessor(email));
        var command = new AddFavouritePostCommand(post.Id);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("This post is already favourite", result.Error);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserNotFound()
    {
        // Arrange
        var context = CreateInMemoryDbContext();
        var email = "missing@example.com";

        var handler = new AddFavouritePostCommandHandler(context, CreateHttpContextAccessor(email));
        var command = new AddFavouritePostCommand(1);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenPostNotFound()
    {
        // Arrange
        var context = CreateInMemoryDbContext();
        var email = "test@example.com";

        var user = new User
        {
            Email = email,
            ClientProfile = new ClientProfile
            {
                UserId = "hyft6uigu8h",
                FavouritePosts = new List<Post>()
            }
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        var handler = new AddFavouritePostCommandHandler(context, CreateHttpContextAccessor(email));
        var command = new AddFavouritePostCommand(999); // Несуществующий пост

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
    }
}