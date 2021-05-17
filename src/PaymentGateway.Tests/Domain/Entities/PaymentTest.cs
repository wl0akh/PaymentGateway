using System;
using NUnit.Framework;
using PaymentGateway.Domain.Entities;
using static PaymentGateway.Domain.Entities.Payment;

namespace PaymentGateway.Tests.Domain.Entities
{
    [TestFixture]
    public class PaymentTest
    {
        [Test]
        public void ApproveTest()
        {
            var payment = new Payment();
            var paymentId = Guid.NewGuid();
            payment.Approve(paymentId);
            Assert.AreEqual(paymentId, payment.PaymentId);
            Assert.AreEqual(Status.APPROVED, payment.PaymentStatus);
        }

        [Test]
        public void DeclineTest()
        {
            var payment = new Payment();
            var paymentId = Guid.NewGuid();
            payment.Decline(paymentId);
            Assert.AreEqual(paymentId, payment.PaymentId);
            Assert.AreEqual(Status.DECLINED, payment.PaymentStatus);
        }
    }
}