using CrossExchange.Controller;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrossExchange.Tests
{
    public class TradeControllerTests
    {
        private readonly Mock<IShareRepository> _shareRepositoryMock = new Mock<IShareRepository>();

        private readonly TradeController _tradeController;
        private ITradeRepository _tradeRepository { get; set; }
        private IPortfolioRepository _portfolioRepository { get; set; }
        public TradeControllerTests()
        {
            _tradeController = new TradeController(_shareRepositoryMock.Object,_tradeRepository,_portfolioRepository);
        }

        [Test]
        public async Task Post_BuyShare()
        {
            var trade = new TradeModel
            {
                Symbol = "REL",
	                NoOfShares = 25,
	                PortfolioId = 1,
	                Action = "BUY",
	                ShareId = 3,
	                Price = 190.0M
            };


            var result = await _tradeController.BuyShare(trade);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(true, createdResult.StatusCode);
        }

        [Test]
        public async Task Post_BuyShareWithCELLAction()
        {
            var trade = new TradeModel
            {
                Symbol = "REL",
                NoOfShares = 25,
                PortfolioId = 1,
                Action = "CELL",
                ShareId = 3,
                Price = 190.0M
            };


            var result = await _tradeController.BuyShare(trade);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(false, createdResult.StatusCode);
        }

        [Test]
        public async Task Post_SellShare()
        {
            var trade = new TradeModel
            {
                Symbol = "REL",
                NoOfShares = 25,
                PortfolioId = 1,
                Action = "SELL",
                ShareId = 3,
                Price = 190.0M
            };


            var result = await _tradeController.BuyShare(trade);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(true, createdResult.StatusCode);
        }

        [Test]
        public async Task Post_SellShareWithBUYAction()
        {
            var trade = new TradeModel
            {
                Symbol = "REL",
                NoOfShares = 25,
                PortfolioId = 1,
                Action = "BUY",
                ShareId = 3,
                Price = 190.0M
            };


            var result = await _tradeController.SellShare(trade);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(false, createdResult.StatusCode);
        }

        [Test]
        public async Task Post_TradeSuccess()
        {
            var trade = new TradeModel
            {
                Symbol = "CBI",
                Action = "BUY",
                NoOfShares = 25,
                PortfolioId = 1,
                Price = 95.0M,
                ShareId = 16
            };

            // Arrange

            // Act
            var result = await _tradeController.BuyShare(trade);

            // Assert
            //Assert.NotNull(result);

            //var createdResult = result as CreatedResult;
            //Assert.NotNull(createdResult);
            Assert.AreEqual(true, result);
        }
    }
}
