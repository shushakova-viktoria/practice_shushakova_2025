using Xunit;
using static task04.task04;


namespace task04tests
{
    public class task04tests
    {
        [Fact]
        public void Cruiser_ShouldHaveCorrectStats()
        {
            ISpaceship cruiser = new Cruiser();

            Assert.Equal(50, cruiser.Speed);
            Assert.Equal(100, cruiser.FirePower);
        }

        [Fact]
        public void Fighter_ShouldHaveCorrectStats()
        {
            ISpaceship fighter = new Fighter();

            Assert.Equal(100, fighter.Speed);
            Assert.Equal(20, fighter.FirePower);
        }

        [Fact]
        public void Fighter_ShouldBeFasterThanCruiser()
        {
            var fighter = new Fighter();
            var cruiser = new Cruiser();

            Assert.True(fighter.Speed > cruiser.Speed);
        }

        [Fact]
        public void Fighter_ShouldHaveLessPowerfulFirePowerThanCruiser()
        {
            var fighter = new Fighter();
            var cruiser = new Cruiser();

            Assert.True(fighter.FirePower < cruiser.FirePower);
        }

        [Fact]
        public void Fighter_ShouldHaveBiggerAngleThanCruiser()
        {
            var fighter = new Fighter();
            var cruiser = new Cruiser();

            fighter.Rotate(80); 
            cruiser.Rotate(30);

            Assert.Equal(80, fighter.CurrentAngle); 
            Assert.Equal(30, cruiser.CurrentAngle);

            Assert.True(fighter.CurrentAngle > cruiser.CurrentAngle);

        }

    }
}
