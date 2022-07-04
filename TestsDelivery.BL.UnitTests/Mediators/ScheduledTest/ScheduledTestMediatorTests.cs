using AutoMapper;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestsDelivery.BL.Mappings;
using TestsDelivery.BL.Mediators.ScheduledTest;
using TestsDelivery.BL.Services.Communication;
using TestsDelivery.BL.Services.ScheduledTest;
using TestsDelivery.BL.Shared.Providers.Communication;
using TestsDelivery.Domain.Lists;
using TestsDelivery.Domain.ScheduledTest;
using TestsDelivery.UserModels.ListFilters;
using TestsDelivery.UserModels.ScheduledTest;
using Xunit;

namespace TestsDelivery.BL.UnitTests.Mediators.ScheduledTest
{
    public class ScheduledTestMediatorTests
    {
        private Mock<ICommunicationServiceProvider> _communicationServiceProviderMock;
        private Mock<IScheduledTestService> _scheduledTestServiceMock;
        private Mapper _mapper;
        private ScheduledTestMediator _mediator;

        public ScheduledTestMediatorTests()
        {
            _communicationServiceProviderMock = new Mock<ICommunicationServiceProvider>();
            _scheduledTestServiceMock = new Mock<IScheduledTestService>();
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ScheduledTestMappings>();
                cfg.AddProfile<TestMappings>();
                cfg.AddProfile<SubjectMappings>();
                cfg.AddProfile<QuestionMappings>();
                cfg.AddProfile<AnswerOptionMappings>();
                cfg.AddProfile<CandidateMappings>();
                cfg.AddProfile<FiltersMapping>();
            }));
            _mediator = new ScheduledTestMediator(_communicationServiceProviderMock.Object, _scheduledTestServiceMock.Object, _mapper);
        }

        [Fact]
        public void GetTest_IdProvided_TestReturned()
        {
            // Arrange
            const long scheduledTestId = 1234;
            const long testId = 4321;
            var domainModel = new Domain.ScheduledTest.ScheduledTest
            {
                Id = scheduledTestId,
                Duration = 60,
                Candidates = new[] { new Domain.Candidate.Candidate() },
                ExpirationDateTime = DateTime.MaxValue,
                StartDateTime = DateTime.MaxValue,
                Test = new Domain.Test.Test
                {
                    Id = testId,
                    Name = "Test",
                    Questions = new[] { new Domain.Questions.EssayQuestion() }
                }
            };

            _scheduledTestServiceMock.Setup(x => x.GetTest(scheduledTestId)).Returns(domainModel);

            // Act
            var resultModel = _mediator.GetTest(scheduledTestId);

            // Assert
            Assert.Equal(resultModel.Id, scheduledTestId);
            Assert.Equal(resultModel.Duration, domainModel.Duration);
            Assert.Equal(resultModel.Candidates.Count(), domainModel.Candidates.Count());
            Assert.Equal(resultModel.ExpirationDateTime, domainModel.ExpirationDateTime);
            Assert.Equal(resultModel.StartDateTime, domainModel.StartDateTime);

            _scheduledTestServiceMock.Verify(x => x.GetTest(scheduledTestId), Times.Once);
        }

        [Fact]
        public void GetList_FilterProvided_ListReturned()
        {
            // Arrange
            var filterModel = new ListFilterModel()
            {
                Take = 1,
                Skip = 0
            };

            var domainList = new ScheduledTestsList()
            {
                ScheduledTests = new []
                {
                    new ScheduledTestInListDto()
                },
                TotalCount = 1,
            };

            _scheduledTestServiceMock.Setup(x => x.GetList(It.IsAny<ListFilter>()))
                .Returns(domainList);

            // Act
            var resultModel = _mediator.GetList(filterModel);

            // Assert
            Assert.Equal(domainList.ScheduledTests.Count(), resultModel.ScheduledTests.Count());
            Assert.Equal(domainList.TotalCount, resultModel.TotalCount);

            _scheduledTestServiceMock.Verify(x => x.GetList(It.IsAny<ListFilter>()), Times.Once);
        }

        [Fact]
        public async void ScheduleTest_ModelProvided_TestScheduled()
        {
            // Arrange
            var createModel = new ScheduledTestCreateModel()
            {
                CandidateIds = new long[] { 1, 2 },
                DestinationInstance = "InstanceId",
                Duration = 60,
                ExpirationDateTime = DateTime.UtcNow,
                StartDateTime = DateTime.UtcNow,
                TestId = 12
            };

            var scheduledTestDomain = new Domain.ScheduledTest.ScheduledTest()
            {
                Id = 2
            };

            var communicationServiceMock = new Mock<IAdminPanelCommunicationService>();
            communicationServiceMock.Setup(x => x.ScheduleTest(It.IsAny<ScheduledTestDetailedModel>()))
                .Returns(Task.CompletedTask);
            _communicationServiceProviderMock.Setup(x => x.Get<IAdminPanelCommunicationService>(createModel.DestinationInstance))
                .Returns(communicationServiceMock.Object);

            // Act
            var scheduledTest = await _mediator.ScheduleTest(createModel);

            // Assert
            _scheduledTestServiceMock.Verify(x => x.ScheduleTest(It.IsAny<Domain.ScheduledTest.ScheduledTest>()), Times.Once);
            _communicationServiceProviderMock.Verify(x => x.Get<IAdminPanelCommunicationService>(createModel.DestinationInstance), Times.Once);
            communicationServiceMock.Verify(x => x.ScheduleTest(It.IsAny<ScheduledTestDetailedModel>()), Times.Once);
        }
    }
}
