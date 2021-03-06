using System.Collections.Generic;
using Core.IServices;
using Core.Models;
using Moq;
using Morales.BookingSystem.Domain.IRepositories;
using Xunit;

namespace Morales.BookingSystem.Domain.Test.IRepositories
{
    public class ITreatmentRepositoryTest
    {
        [Fact]
        public void ITreatmentRepo_Exists()
        {
            var repoMock = new Mock<ITreatmentRepository>();
            Assert.NotNull(repoMock.Object);
        }

        [Fact]
        public void GetAll_NoParams_ReturnsListOfTreatments()
        {
            var repoMock = new Mock<ITreatmentRepository>();
            repoMock
                .Setup(s => s.GetAll())
                .Returns(new List<Treatments>());
            Assert.NotNull(repoMock.Object.GetAll());
        }

        [Fact]
        public void GetTreatment_WithParams_ReturnsSingleTreatment()
        {
            var repoMock = new Mock<ITreatmentRepository>();
            var treatmentId = 1;
            repoMock
                .Setup(s => s.GetTreatment(treatmentId))
                .Returns(new Treatments() {Id = treatmentId});
            Assert.NotNull(repoMock.Object.GetTreatment(treatmentId));
        }

        [Fact]
        public void GetTreatmentBySex_WithParams_ReturnSingleTreatment()
        {
            var repoMock = new Mock<ITreatmentRepository>();
            repoMock
                .Setup(s => s.GetTreatmentBySex())
                .Returns(new List<Treatments>());
            Assert.NotNull(repoMock.Object.GetTreatmentBySex());
        }
        
        [Fact]
        public void DeleteTreatment_WithParams_ReturnsDeletedTreatment()
        {
            var repoMock = new Mock<ITreatmentRepository>();
            var treatmentId = 1;
            repoMock
                .Setup(s => s.DeleteTreatment(treatmentId))
                .Returns(new Treatments() {Id = treatmentId});
            Assert.NotNull(repoMock.Object.DeleteTreatment(treatmentId));
        }
        [Fact]
        public void CreateTreatment_WithParams_ReturnsCreatedTreatment()
        {
            var repoMock = new Mock<ITreatmentRepository>();
            var treatment = new Treatments(){Id = 1};
            repoMock
                .Setup(s => s.CreateTreatment(treatment))
                .Returns(new Treatments() {Id = treatment.Id});
            Assert.NotNull(repoMock.Object.CreateTreatment(treatment));
        }
        [Fact]
        public void UpdateTreatment_WithParams_ReturnsUpdatedTreatment()
        {
            var repoMock = new Mock<ITreatmentRepository>();
            var treatment = new Treatments() {Id = 1};
            repoMock
                .Setup(s => s.UpdateTreatment(treatment))
                .Returns(new Treatments() {Id = treatment.Id});
            Assert.NotNull(repoMock.Object.UpdateTreatment(treatment));
        }
    }
}