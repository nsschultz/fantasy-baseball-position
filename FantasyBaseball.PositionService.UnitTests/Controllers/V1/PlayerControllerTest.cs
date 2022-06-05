using System.Collections.Generic;
using FantasyBaseball.Common.Models;
using FantasyBaseball.PositionService.Entities;
using FantasyBaseball.PositionService.Services;
using Moq;
using Xunit;

namespace FantasyBaseball.PositionService.Controllers.V1.UnitTests
{
    public class PositionControllerTest
    {
        [Fact] public async void GetPositionsTest()
        {
            var getService = new Mock<IGetPositionsService>();
            getService.Setup(o => o.GetPositions()).ReturnsAsync(new List<PositionEntity> { new PositionEntity() });
            var builderService = new Mock<IBaseballPositionBuilderService>();
            builderService.Setup(o => o.BuildBaseballPosition(It.IsAny<PositionEntity>())).Returns(new BaseballPosition());
            var sortService = new Mock<ISortService>();
            sortService.Setup(o => o.SortPositions(It.IsAny<List<BaseballPosition>>())).Returns((List<BaseballPosition> Positions) => Positions);
            Assert.NotEmpty((await new PositionController(builderService.Object, getService.Object, sortService.Object).GetPositions()));
        }
     }
}