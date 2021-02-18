using TechTalk.SpecFlow;

namespace PaymentGateway.Tests.Endpoints.RetrievePayment
{
    [Binding]
    [Scope(Feature = "Retrieve Payment")]
    public class RetrivePaymentSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public RetrivePaymentSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
        [Given(@"PaymentId as (.*) is provided")]
        public void GivenPaymentIdAsIsProvided(string cardNumber)
        {

        }

        [Given(@"record with PaymentId (.*) in data store")]
        public void GivenRecordWithPaymentIdInDataStore(string isExist)
        {

        }

        [Given(@"request header has (.*) Authorization token")]
        public void GivenRequestHeaderHasAuthorizationToken(string isValid)
        {

        }

        [When(@"a GET is called on (.*)")]
        public void WhenAGetIsCalledOnPath(string path)
        {

        }

        [Then(@"it returns response with status code (.*)")]
        public void ThenItReturnsResponseWithStatusCodeBadRequest(string code)
        {

        }

        [Then(@"response body indicate Authorization is invalid")]
        public void ThenResponseBodyIndicateAuthorizationIsInvalid()
        {

        }

        [Then(@"response body indicate record with (.*) not found")]
        public void ThenResponseBodyIndicateRecordWithGuidNotFound(string guid)
        {

        }

        [Then(@"response body (.*) (.*) Value")]
        public void ThenResponseBodyContainsPaymentFieldAndItsNotNull(string isConatins, string field)
        {

        }

        [Then(@"response body (.*) Id as (.*)")]
        public void ThenResponseBodyContainsIdAsDefd_Add(string isContains, string paymentId)
        {
        }
    }
}