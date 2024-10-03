using Bogus;
using FluentAssertions;
using PlaceRentalApp.Core.Entities;
using PlaceRentalApp.Core.Enums;
using PlaceRentalApp.UnitTests.Fakes;

namespace PlaceRentalApp.UnitTests.Core
{
    public class PlaceCancelTests
    {
        // RED: nao compilava >> Compila Mas Teste Falha
        // GREEN: metodo Cancel alterado, teste executado com sucesso
        [Fact]
        public void Cancel_PlaceHasnoActiveBooks_Success()
        {
            // arrange
            var place = new PlaceFake().Generate();

            // act
            // RED: o metodo nao existe, irá gerar o metodo vazio
            place.Cancel();

            // assert
            place.Status.Should().Be(PlaceStatus.Inactive);
        }

        //RED: falha
        //GREEN: Passa
        //REFACTOR:
        [Fact]
        public void Cancel_PlaceHasActiveBook_Error()
        {
            //arrage
            var place = new PlaceFake()
                .RuleFor(p => p.Books, new Faker<PlaceBook>()
                             .RuleFor(pb => pb.EndDate, _ => DateTime.Now.AddDays(4))
                             .RuleFor(pb => pb.StartDate, _ => DateTime.Now.AddDays(-1))
                             .Generate(1))
                .Generate();

            //act
            var action = () => place.Cancel();

            //assert
            action.Should().Throw<InvalidOperationException>()
                .WithMessage("Invalid status");
        }
    }
}
