namespace TownApplication.IntegrationTests
{
    public class TownControllerIntegrationTests
    {
        private readonly TownController _controller;


        public TownControllerIntegrationTests()
        {
            _controller = new TownController();
            _controller.ResetDatabase();

        }

        [Fact]
        public void AddTown_ValidInput_ShouldAddTown()
        {
            _controller.AddTown("sofia", 200);
            Assert.NotNull(_controller.GetTownByName("sofia"));
            Assert.StrictEqual(200, _controller.GetTownByName("sofia").Population);

        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("AB")]
        public void AddTown_InvalidName_ShouldThrowArgumentException(string invalidName)
        {

            var expectedMessage = Assert.Throws<ArgumentException>(() => _controller.AddTown(invalidName, 100));
            Assert.Equal("Invalid town name.", expectedMessage.Message);

        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void AddTown_InvalidPopulation_ShouldThrowArgumentException(int invalidPopulation)
        {
            var expectedMessage = Assert.Throws<ArgumentException>(() => _controller.AddTown("sofia", invalidPopulation));
            Assert.Equal("Population must be a positive number.", expectedMessage.Message);
        }

        [Fact]
        public void AddTown_DuplicateTownName_DoesNotAddDuplicateTown()
        {
            _controller.AddTown("Sofia", 100);
            _controller.AddTown("Sofia", 200);
            Assert.Equal(100, _controller.GetTownByName("Sofia").Population);
            Assert.Single(_controller.ListTowns());
        }

        [Fact]
        public void UpdateTown_ShouldUpdatePopulation()
        {
            _controller.AddTown("sofia1", 100);
            _controller.UpdateTown(_controller.GetTownByName("sofia1").Id, 200);
            Assert.Single(_controller.ListTowns());
            Assert.NotNull(_controller.GetTownByName("sofia1"));
            Assert.Equal(200, _controller.GetTownByName("sofia1").Population);
        }

        [Fact]
        public void DeleteTown_ShouldDeleteTown()
        {

            _controller.AddTown("sofia1", 100);
            _controller.DeleteTown(_controller.GetTownByName("sofia1").Id);
            Assert.Empty(_controller.ListTowns());
        }

        [Fact]
        public void ListTowns_ShouldReturnTowns()
        {
            var townsToAdd = new List<string> { "Sofia", "Varna", "Plovdiv", "Lom" };

            foreach (var town in townsToAdd)
            {
                _controller.AddTown(town, town.Length * 10);
            }

            var allTowns = _controller.ListTowns();

            Assert.Equal(allTowns.Count, townsToAdd.Count);

            foreach (var town in townsToAdd)
            {
            
                var townsExists = allTowns.FirstOrDefault(x => x.Name == town);
                Assert.NotNull(townsExists);
            }

        }
    }
}
