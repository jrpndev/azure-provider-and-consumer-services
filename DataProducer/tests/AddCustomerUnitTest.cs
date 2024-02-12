using NSubstitute;
using FluentAssertions;
using Xunit;
using Microsoft.Azure.Cosmos;
using System;

namespace Database.Test
{
    public class AddCustomerUnityTest
    {
        [Fact]
        public async Task AddCustomerShouldReturnSuccess()
        {
            #region Arrange
            var cosmosClient = Substitute.For<CosmosClient>();
            var container = Substitute.For<Container>();
            cosmosClient.GetDatabase(Arg.Any<string>()).Returns(Substitute.For<DatabaseResponse>());
            cosmosClient.GetDatabase(Arg.Any<string>()).GetContainer(Arg.Any<string>()).Returns(container);

            var customerRepository = new CosmosDBCustomerRepository(cosmosClient, "database", "customers_id");
            var customer = new CustomerDTO
            {
                Email = "teste@gmail.com",
                Name = "JhonDoe",
                PhoneNumber = "9299339981",
            };
            #endregion

            #region Act
            var result = await customerRepository.AddCustomerAsync(customer);
            #endregion

            #region Assert
            result.Should().BeTrue();
            #endregion
        }

        [Fact]
        public async Task AddCustomerShouldReturnFailure()
        {
            #region Arrange
            var cosmosClient = Substitute.For<CosmosClient>();
            var container = Substitute.For<Container>();
            cosmosClient.GetDatabase(Arg.Any<string>()).Returns(Substitute.For<DatabaseResponse>());
            cosmosClient.GetDatabase(Arg.Any<string>()).GetContainer(Arg.Any<string>()).Returns(container);

            container
                .When(c => c.UpsertItemAsync(Arg.Any<CustomerDTO>()))
                .Throw(new Exception("Failed to upsert item"));

            var customerRepository = new CosmosDBCustomerRepository(cosmosClient, "database", "customers_id");
            var customer = new CustomerDTO();
            #endregion

            #region Act
            var result = await customerRepository.AddCustomerAsync(customer);
            #endregion

            #region Assert
            result.Should().BeFalse();
            #endregion
        }
    }
}
