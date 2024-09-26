using PlaceRentalApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlaceRentalApp.Core.ValueObjects;
using System.Collections;
using FluentAssertions;

namespace PlaceRentalApp.UnitTests.Core
{
    public class PlaceTests
    {
        /// <summary>
        /// O padrão "Given-When-Then" é uma abordagem comum em testes unitários que organiza o código de teste em três partes:
        /// Given: Configuração do estado inicial, onde você define as condições necessárias para o teste.
        /// When: Ação que está sendo testada, geralmente a chamada a um método ou função.
        /// Then: Verificação dos resultados esperados, onde você compara a saída do código com o que era esperado.
        /// </summary>
        [Fact]
        public void Update_PlaceAndDataAreOk_Success()
        {
            //arrange
            var place = new Place("title", "description", 100m,
                                  new Address("street", "123", "123456", "district", "City", "state", "Country"),
                                  4, true, 1);

            var newTitle = "New Title";
            var newDescription = "New Description";
            decimal newDailyPrice = 140m;

            // act
            var result = place.Update(newTitle, newDescription, newDailyPrice);

            // assert
            Assert.True(result);
            Assert.Equal(newTitle, place.Title);
            place.Title.Should().BeEquivalentTo(newTitle);
            Assert.Equal(newDescription, place.Description);
            place.Description.Should().BeEquivalentTo(newDescription);
            Assert.Equal(newDailyPrice, place.DailyPrice);
            place.DailyPrice.Should().Be(newDailyPrice);
        }
        
        [Fact]
        public void Update_PlaceIsBlocked_False()
        {
            //arrange
            var place = new Place("title", "description", 100m,
                                  new Address("street", "123", "123456", "district", "City", "state", "Country"),
                                  4, true, 1);

            var newTitle = "New Title";
            var newDescription = "New Description";
            decimal newDailyPrice = 140m;

            place.Block();

            // act
            var result = place.Update(newTitle, newDescription, newDailyPrice);

            // assert
            Assert.False(result);
            Assert.NotEqual(newTitle, place.Title);
            Assert.NotEqual(newDescription, place.Description);
            Assert.NotEqual(newDailyPrice, place.DailyPrice);
        }

        [Fact]
        public void IsBookAllowed_AmounOfPersonAndPetOk_Success()
        {
            //arrange
            var place = new Place("title", "description", 100m,
                                  new Address("street", "123", "123456", "district", "City", "state", "Country"),
                                  4, true, 1);

            place.Block();

            // act
            var isBookAllowed = place.IsBookAllowed(true, 2);

            // assert
            Assert.True(isBookAllowed);
        }

        [Fact]
        public void IsBookAllowed_HasPetButNotAllowed_False()
        {
            //arrange
            var place = new Place("title", "description", 100m,
                                  new Address("street", "123", "123456", "district", "City", "state", "Country"),
                                  4, false, 1);
            place.Block();

            // act
            var isBookAllowed = place.IsBookAllowed(true, 3);

            // assert
            Assert.False(isBookAllowed);
        }

        [Fact]
        public void IsBookAllowed_OverMaximumAllowedPerson_False()
        {
            //arrange
            var place = new Place("title", "description", 100m,
                                  new Address("street", "123", "123456", "district", "City", "state", "Country"),
                                  4, true, 1);

            place.Block();

            // act
            var isBookAllowed = place.IsBookAllowed(true, 5);

            // assert
            Assert.False(isBookAllowed);
        }

        // parametrização, realizando os 3 testes atrás de uma so vez
        [Theory]
        [InlineData(4, true, 2, true, true)]
        [InlineData(4, false, 3, true, false)]
        [InlineData(4, true, 5, true, false)]
        public void IsBookAllowed(int allowedNumberOfPerson, bool acceptPets,
                                  int amountOfPerson, bool hasPet, bool result)
        {
            //arrange
            var place = new Place("title", "description", 100m,
                                  new Address("street", "123", "123456", "district", "City", "state", "Country"),
                                  allowedNumberOfPerson, acceptPets, 1);

            place.Block();

            // act
            var isBookAllowed = place.IsBookAllowed(hasPet, amountOfPerson);

            // assert
            if (result) Assert.True(isBookAllowed);
            else Assert.False(isBookAllowed);
        }

        public static IEnumerable<object[]> GetIsBookAllowedParams() 
        {
            yield return [4, true, 2, true, true];
            yield return [4, false, 3, true, false];
            yield return [4, true, 5, true, false];
            yield return [2, true, 2, false, true];
        }

        [Theory]
        [MemberData(nameof(GetIsBookAllowedParams))]
        public void IsBookAllowedWithMember(int allowedNumberOfPerson, bool acceptPets,
                                  int amountOfPerson, bool hasPet, bool result)
        {
            //arrange
            var place = new Place("title", "description", 100m,
                                  new Address("street", "123", "123456", "district", "City", "state", "Country"),
                                  allowedNumberOfPerson, acceptPets, 1);

            place.Block();

            // act
            var isBookAllowed = place.IsBookAllowed(hasPet, amountOfPerson);

            // assert
            if (result) Assert.True(isBookAllowed);
            else Assert.False(isBookAllowed);
        }
    }
}
