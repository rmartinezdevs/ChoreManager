using AutoMapper;
using ChoreManager.Application.UseCases.ChoreUseCases.Implementations;
using ChoreManager.Domain.Entities;
using ChoreManager.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ChoreManager.Tests.UseCases
{
    public class SelectAllChoresUseCaseTests
    {
        private readonly Mock<IChoreRepository> _repositoryMock;
        private readonly IMapper _mapper;

        public SelectAllChoresUseCaseTests()
        {
            _repositoryMock = new Mock<IChoreRepository>();
            _mapper = MapperTestHelper.CreateMapper();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnAllChores()
        {
            //Arrange
            SelectAllChoresUseCase useCase = new (_repositoryMock.Object, _mapper);

            var chores = new List<Chore>
            {
                new Chore
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Chore 1",
                    Description = "Description 1",
                    DueDate = DateTime.Now.AddDays(1),
                    Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                    AssignedUserId = Guid.NewGuid()
                },
                new Chore
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Chore 2",
                    Description = "Description 2",
                    DueDate = DateTime.Now.AddDays(2),
                    Status = Domain.Enums.ChoreEnums.ChoreStatus.Completed,
                    AssignedUserId = Guid.NewGuid()
                }
            };

            _repositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(chores);


            //Act
            var result = await useCase.ExecuteAsync();

            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Count.Should().Be(chores.Count);
            result.Should().BeEquivalentTo(chores, options => options.ComparingByMembers<Chore>());

            _repositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once());
        }
    }
}
