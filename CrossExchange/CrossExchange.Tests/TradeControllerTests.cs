using System;
using System.Threading.Tasks;
using CrossExchange.Controller;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;

namespace CrossExchange.Tests
{
    public class TradeControllerTests
    {
        private readonly Mock<IShareRepository> _shareRepositoryMock = new Mock<IShareRepository>();
        private readonly Mock<ITradeRepository> _tradeRepositoryMock = new Mock<ITradeRepository>();
        private readonly Mock<IPortfolioRepository> _portfolioRepositoryMock = new Mock<IPortfolioRepository>();
        
        private readonly TradeController _tradeController;

        public TradeControllerTests()
        {
            _tradeController = new TradeController(_shareRepositoryMock.Object, _tradeRepositoryMock.Object, _portfolioRepositoryMock.Object);
        }

        [Test]
        public async Task Post_ShouldNotAllowUnregisteredUser()
        {
            var trade = new TradeModel
            {
                Symbol = "REL",
                NoOfShares = 30,
                PortfolioId = 100,
                Action = "BUY"
            };

            // Arrange

            // Act
            var result = await _tradeController.Post(trade);

            // Assert
            Assert.NotNull(result);

            var badRequestResult = result as BadRequestResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        //[Test]
        //public async Task Post_ShouldAllowBuy()
        //{
        //    var trade = new TradeModel
        //    {
        //        Symbol = "REL",
        //        NoOfShares = 30,
        //        PortfolioId = 1,
        //        Action = "BUY"
        //    };

        //    // Arrange

        //    // Act
        //    var result = await _tradeController.Post(trade);

        //    // Assert
        //    Assert.NotNull(result);

        //    var createdResult = result as CreatedResult;
        //    Assert.NotNull(createdResult);
        //    Assert.AreEqual(201, createdResult.StatusCode);
        //}

        //[Test]
        //public async Task Post_ShouldAllowSell()
        //{
        //    var trade = new TradeModel
        //    {
        //        Symbol = "REL",
        //        NoOfShares = 30,
        //        PortfolioId = 1,
        //        Action = "SELL"
        //    };

        //    // Arrange

        //    // Act
        //    var result = await _tradeController.Post(trade);

        //    // Assert
        //    Assert.NotNull(result);

        //    var createdResult = result as CreatedResult;
        //    Assert.NotNull(createdResult);
        //    Assert.AreEqual(201, createdResult.StatusCode);
        //}

        [Test]
        public async Task Post_ShouldNotAllowInvalidSymbol()
        {
            var trade = new TradeModel
            {
                Symbol = "ABC",
                NoOfShares = 30,
                PortfolioId = 1,
                Action = "SELL"
            };

            // Arrange

            // Act
            var result = await _tradeController.Post(trade);

            // Assert
            Assert.NotNull(result);

            var badRequestResult = result as BadRequestResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [Test]
        public async Task Post_ShouldNotAllowInsufficientShare()
        {
            var trade = new TradeModel
            {
                Symbol = "CBI",
                NoOfShares = 1000,
                PortfolioId = 1,
                Action = "SELL"
            };

            // Arrange

            // Act
            var result = await _tradeController.Post(trade);

            // Assert
            Assert.NotNull(result);

            var badRequestResult = result as BadRequestResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

    }
}
