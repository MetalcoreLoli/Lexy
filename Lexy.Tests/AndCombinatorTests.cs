using System;
using FluentAssertions;
using Lexy.Exceptions;
using Moq;
using Xunit;

namespace Lexy.Tests
{
    public class AndCombinatorTests
    {
        [Fact]
        public void And_WithValidRulesResult_ReturnsResults()
        {
            var ruleOne = new Mock<Rule>();
            var ruleTwo = new Mock<Rule>();

            ruleOne  .Setup(x => x.ExecuteOn("")).Returns(new Rule.ExecutionResult("Number"));
            ruleTwo  .Setup(x => x.ExecuteOn("")).Returns(new Rule.ExecutionResult("Whitespace"));

            var result = (ruleOne.Object & ruleTwo.Object & ruleOne.Object).ExecuteOn("");

            result.ToString().Should().BeEquivalentTo("`Number` `Whitespace` `Number`");
        }
        
        [Fact]
        public void And_WithInvalidLeftRuleResult_ThrowsRuleExecutionException()
        {
            var ruleOne = new Mock<Rule>();
            var invalid = new Mock<Rule>();

            ruleOne  .Setup(x => x.ExecuteOn("")).Returns(new Rule.ExecutionResult("Number"));
            invalid  .Setup(x => x.ExecuteOn("")).Returns((Rule.ExecutionResult) null);

            Action act = () => (invalid.Object & ruleOne.Object).ExecuteOn("");

            act.Should().Throw<RuleExecutionException>();
        }
        
        [Fact]
        public void And_WithInvalidRightRuleResult_ThrowsRuleExecutionException()
        {
            var ruleOne = new Mock<Rule>();
            var invalid = new Mock<Rule>();

            ruleOne  .Setup(x => x.ExecuteOn("")).Returns(new Rule.ExecutionResult("Number"));
            invalid  .Setup(x => x.ExecuteOn("")).Returns((Rule.ExecutionResult) null);

            Action act = () => (ruleOne.Object & invalid.Object).ExecuteOn("");
            act.Should().Throw<RuleExecutionException>();
        }
    }
}