﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.7.0.0
//      SpecFlow Generator Version:3.7.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace PaymentGateway.Tests.Endpoints.ProcessPayment
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.7.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("With Valid Input Fields")]
    [NUnit.Framework.CategoryAttribute("ProcessPayment")]
    public partial class WithValidInputFieldsFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "ProcessPayment"};
        
#line 1 "WithValidInputFields.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Endpoints/ProcessPayment", "With Valid Input Fields", "    Payment gateway endpoint to process payment with valid input fields", ProgrammingLanguage.CSharp, new string[] {
                        "ProcessPayment"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Process Payment when a valid request is provided")]
        [NUnit.Framework.TestCaseAttribute("41.00", "", "successful", "GBP", null)]
        [NUnit.Framework.TestCaseAttribute("41.00", "USD", "successful", "USD", null)]
        [NUnit.Framework.TestCaseAttribute("-51.92", "USD", "successful", "USD", null)]
        [NUnit.Framework.TestCaseAttribute("0.00", "GBP", "successful", "GBP", null)]
        [NUnit.Framework.TestCaseAttribute("24.00", "USD", "unsuccessful", "USD", null)]
        [NUnit.Framework.TestCaseAttribute("24.00", "USD", "not working", "USD", null)]
        public virtual void ProcessPaymentWhenAValidRequestIsProvided(string amount, string currency, string status, string currencyInResponse, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            argumentsOfScenario.Add("amount?", amount);
            argumentsOfScenario.Add("currency?", currency);
            argumentsOfScenario.Add("status?", status);
            argumentsOfScenario.Add("currencyInResponse?", currencyInResponse);
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Process Payment when a valid request is provided", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 6
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 7
        testRunner.Given("a request with body as", string.Format("{{\n    \"cardNumber\": \"5500000000000004\",\n    \"expiry\": \"12/2024\",\n    \"amount\": \"" +
                            "{0}\",\n    \"currency\": \"{1}\",\n    \"cvv\": \"123\"\n}}", amount, currency), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 17
        testRunner.When("Bank Service Responds as", string.Format("{{\n    \"paymentId\": \"d3a36044-9996-4f46-a73f-5d82a7b85a85\",\n    \"status\": \"{0}\"\n}" +
                            "}", status), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 24
        testRunner.And("a POST is called on /api/payments", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 25
        testRunner.Then("it returns response with status code Created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 26
        testRunner.And("payment Id is returned in response header", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 27
        testRunner.And("payment is recorded in data store", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 28
        testRunner.And("payment is recorded in data store with paymentId as d3a36044-9996-4f46-a73f-5d82a" +
                        "7b85a85", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 29
        testRunner.And("payment is recorded in data store with cardNumber as 5500000000000004", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 30
        testRunner.And(string.Format("payment is recorded in data store with status as {0}", status), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 31
        testRunner.And("payment is recorded in data store with expiry as 12/2024", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 32
        testRunner.And(string.Format("payment is recorded in data store with amount as {0}", amount), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 33
        testRunner.And(string.Format("payment is recorded in data store with currency as {0}", currencyInResponse), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Process Payment when Bank Service is inaccessible")]
        public virtual void ProcessPaymentWhenBankServiceIsInaccessible()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Process Payment when Bank Service is inaccessible", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 49
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 50
        testRunner.Given("a request with body as", "{\n    \"cardNumber\": \"5500000000000004\",\n    \"expiry\": \"12/2024\",\n    \"amount\": \"2" +
                        "4.56\",\n    \"currency\": \"GBP\",\n    \"cvv\": \"123\"\n}", ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 60
        testRunner.When("Bank Service is inaccessible", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 61
        testRunner.And("a POST is called on /api/payments", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 62
        testRunner.Then("it returns response with status code ServiceUnavailable", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 63
        testRunner.And("Bank Service Unavailable is shown in response body", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 64
        testRunner.And("payment is not recorded in data store", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Process Payment when Bank Service response not valid")]
        public virtual void ProcessPaymentWhenBankServiceResponseNotValid()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Process Payment when Bank Service response not valid", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 66
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 67
        testRunner.Given("a request with body as", "{\n    \"cardNumber\": \"5500000000000004\",\n    \"expiry\": \"12/2024\",\n    \"amount\": \"2" +
                        "4.56\",\n    \"currency\": \"GBP\",\n    \"cvv\": \"123\"\n}", ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 77
        testRunner.When("Bank Service Responds as", "\"<html>page not found</html>\"", ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 81
        testRunner.And("a POST is called on /api/payments", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 82
        testRunner.Then("it returns response with status code ServiceUnavailable", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 83
        testRunner.And("Bank Service Incompatible is shown in response body", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 84
        testRunner.And("payment is not recorded in data store", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Process Payment when currency is not supplyed")]
        public virtual void ProcessPaymentWhenCurrencyIsNotSupplyed()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Process Payment when currency is not supplyed", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 86
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 87
        testRunner.Given("a request with body as", "{\n    \"cardNumber\": \"5500000000000004\",\n    \"expiry\": \"12/2024\",\n    \"amount\": \"2" +
                        "4.56\",\n    \"cvv\": \"123\"\n}", ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 96
        testRunner.When("Bank Service Responds as", "{\n    \"paymentId\": \"d3a36044-9996-4f46-a73f-5d82a7b85a85\",\n    \"status\": \"success" +
                        "ful\"\n}", ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 103
        testRunner.And("a POST is called on /api/payments", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 104
        testRunner.Then("it returns response with status code Created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 105
        testRunner.And("payment Id is returned in response header", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 106
        testRunner.And("payment is recorded in data store", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 107
        testRunner.And("payment is recorded in data store with paymentId as d3a36044-9996-4f46-a73f-5d82a" +
                        "7b85a85", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 108
        testRunner.And("payment is recorded in data store with status as successful", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 109
        testRunner.And("payment is recorded in data store with cardNumber as 5500000000000004", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 110
        testRunner.And("payment is recorded in data store with expiry as 12/2024", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 111
        testRunner.And("payment is recorded in data store with amount as 24.56", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 112
        testRunner.And("payment is recorded in data store with currency as GBP", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion