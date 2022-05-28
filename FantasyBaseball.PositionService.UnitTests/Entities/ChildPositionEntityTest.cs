using Xunit;

namespace FantasyBaseball.PositionService.Entities.UnitTests
{
    public class ChildPositionEntityTest
    {
        [Fact] public void DefaultsSetTest()
        {
            var obj = new ChildPositionEntity();
            Assert.Null(obj.ChildCode);
            Assert.Null(obj.ParentCode);
            Assert.Null(obj.ChildPosition);
            Assert.Null(obj.ParentPosition);
        }
    }
}