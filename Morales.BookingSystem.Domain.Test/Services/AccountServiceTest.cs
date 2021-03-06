using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Core.IServices;
using Core.Models;
using Moq;
using Morales.BookingSystem.Domain.IRepositories;
using Morales.BookingSystem.Domain.Services;
using Xunit;

namespace Morales.BookingSystem.Domain.Test.Services
{
    public class AccountServiceTest
    {
        #region AccountService Initialization Tests
        
        [Fact]
        public void AccountService_IsIAccountService()
        {
            var mockRepo = new Mock<IAccountRepository>();
            var accountService = new AccountService(mockRepo.Object);
            Assert.IsAssignableFrom<IAccountService>(accountService);
        }

        [Fact]
        public void AccountService_WithNullRepository_ThrowInvalidDataException()
        {
            Assert.Throws<InvalidDataException>(() => new AccountService(null));
        }

        [Fact]
        public void AccountService_WithNullRepository_ThrowsExceptionWithMessage()
        {
            var e = Assert.Throws<InvalidDataException>(() => new AccountService(null));
            Assert.Equal("AccountRepository cannot be Null!", e.Message);
        }
        #endregion

        #region AccountService GetAllAccounts Tests

        [Fact]
        public void GetAll_NoParams_CallsAccountRepositoryOnce()
        {
            var mockRepo = new Mock<IAccountRepository>();
            var accountService = new AccountService(mockRepo.Object);

            accountService.GetAll();
            
            mockRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void GetAll_NoParams_ReturnsAllAccountsAsList()
        {
            var expected = new List<Account> {new Account {Id = 1, Name = "Ostekage"}};
            var mockRepo = new Mock<IAccountRepository>();
            mockRepo
                .Setup(s => s.GetAll())
                .Returns(expected);
            var accountService = new AccountService(mockRepo.Object);

            accountService.GetAll();
            
            Assert.Equal(expected, accountService.GetAll(), new AccountComparer());
        }

        #endregion

        #region AccountService GetAccount Tests

        [Fact]
        public void GetAccount_WithParams_CallsAccountRepositoryOnce()
        {
            var mockRepo = new Mock<IAccountRepository>();
            var accountService = new AccountService(mockRepo.Object);
            var accountId = 1;

            accountService.GetAccount(accountId);
            
            mockRepo.Verify(r => r.GetAccount(accountId), Times.Once);
        }

        [Fact]
        public void GetAccount_WithParams_ReturnsSingleProduct()
        {
            var expected = new Account {Id = 1, Name = "Brie"};
            var mockRepo = new Mock<IAccountRepository>();
            mockRepo
                .Setup(s => s.GetAccount(expected.Id))
                .Returns(expected);
            var accountService = new AccountService(mockRepo.Object);

            accountService.GetAccount(expected.Id);
            
            Assert.Equal(expected, accountService.GetAccount(expected.Id), new AccountComparer());
        }
        
        

        #endregion

        #region AccountService Create Account Test

        [Fact]
        public void CreateAccount_WithParams_CallAccountRepositoryOnce()
        {
            var mockRepo = new Mock<IAccountRepository>();
            var accountService = new AccountService(mockRepo.Object);
            var account = new Account();

            accountService.CreateAccount(account);
            
            mockRepo.Verify(s => s.CreateAccount(account), Times.Once);
        }

        [Fact]
        public void CreateAccount_WithParams_ReturnSingleAccount()
        {
            var expected = new Account();
            var mockRepo = new Mock<IAccountRepository>();
            mockRepo
                .Setup(s => s.CreateAccount(expected))
                .Returns(expected);
            
            var accountService = new AccountService(mockRepo.Object);
            
            Assert.Equal(expected, accountService.CreateAccount(expected), new AccountComparer());
        }

        #endregion

        #region AccountService DeleteAccount Test

        [Fact]
        public void DeleteAccount_WithParams_CallsAccountRepositoryOnce()
        {
            var mockRepo = new Mock<IAccountRepository>();
            var accountService = new AccountService(mockRepo.Object);
            var accountId = 1;

            accountService.DeleteAccount(accountId);
            
            mockRepo.Verify(s => s.DeleteAccount(accountId), Times.Once);
        }

        [Fact]
        public void DeleteAccount_WithParams_ReturnsSingleAccount()
        {
            var expected = new Account {Id = 1, Name = "Brie"};
            var mockRepo = new Mock<IAccountRepository>();
                mockRepo
                .Setup(s => s.DeleteAccount(expected.Id))
                .Returns(expected);
                
            var accountService = new AccountService(mockRepo.Object);
            
            Assert.Equal(expected,accountService.DeleteAccount(expected.Id), new AccountComparer());
        }
        #endregion

        #region AccountService UpdateAccount Test
        
        [Fact]
        public void UpdateAccount_WithParams_ReturnsSingleAccount()
        {
            var expected = new Account
            {
                Id = 1,
                Type = "Ost",
                Name = "Port Salut",
                PhoneNumber = "12345678",
                Sex = "yes",
                Email = "Ost@Ost.dk"
            };
            var mockRepo = new Mock<IAccountRepository>();
            mockRepo
                .Setup(s => s.UpdateAccount(expected))
                .Returns(expected);
            var accountService = new AccountService(mockRepo.Object);
            Assert.Equal(expected, accountService.UpdateAccount(expected), new AccountComparer());
        }

        #endregion

        #region AccountService GetAccountFromPhoneNumber Test

        [Fact]
        public void GetAccountFromPhone_WithParams_CallsAccountRepositoryOnce()
        {
            var mockRepo = new Mock<IAccountRepository>();
            var accountService = new AccountService(mockRepo.Object);
            var phoneNumber = "11111111";

            accountService.GetAccountFromPhoneNumber(phoneNumber);
            
            mockRepo.Verify(r => r.GetAccountFromPhoneNumber(phoneNumber), Times.Once);
        }

        [Fact]
        public void GetAccountFromPhone_WithParams_ReturnsSingleAccount()
        {
            var expected = new Account {Id = 1, Name = "Brie", PhoneNumber = "11111111"};
            var mockRepo = new Mock<IAccountRepository>();
            mockRepo
                .Setup(s => s.GetAccountFromPhoneNumber(expected.PhoneNumber))
                .Returns(expected);
            var accountService = new AccountService(mockRepo.Object);

            accountService.GetAccountFromPhoneNumber(expected.PhoneNumber);
            
            Assert.Equal(expected, accountService.GetAccountFromPhoneNumber(expected.PhoneNumber), new AccountComparer());
        }

        #endregion

        #region AccountService GetAccountFromType Test

        [Fact]
        public void GetAccountsFromType_WithParams_CallsAccountRepositoryOnce()
        {
            var mockRepo = new Mock<IAccountRepository>();
            var accountService = new AccountService(mockRepo.Object);
            var accountType = "Employee";

            accountService.GetAccountsFromType(accountType);
            
            mockRepo.Verify(r => r.GetAccountFromType(accountType), Times.Once);
        }

        [Fact]
        public void GetAccountsFromType_WithParams_ReturnsSingleAccount()
        {
            var expected = new Account {Id = 1, Name = "Brie", Type = "Employee"};
            var expectedList = new List<Account>();
            expectedList.Add(expected);
            var mockRepo = new Mock<IAccountRepository>();
            mockRepo.Setup(s => s.GetAccountFromType(expected.Type))
                .Returns(expectedList);
            var accountService = new AccountService(mockRepo.Object);

            accountService.GetAccountsFromType(expected.Type);
            
            Assert.Equal(expectedList, accountService.GetAccountsFromType(expected.Type), new AccountComparer());
        }
        
        #endregion
    }

    #region Account Comparer
    public class AccountComparer : IEqualityComparer<Account>
    {
        public bool Equals(Account x, Account y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Id == y.Id && x.Type == y.Type && x.Name == y.Name && x.PhoneNumber == y.PhoneNumber && x.Sex == y.Sex && x.Email == y.Email;
        }

        public int GetHashCode(Account obj)
        {
            return HashCode.Combine(obj.Id, obj.Type, obj.Name, obj.PhoneNumber, obj.Sex, obj.Email);
        }
    }
    #endregion
}