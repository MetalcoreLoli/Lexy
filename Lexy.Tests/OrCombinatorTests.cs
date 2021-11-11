using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Xunit;

namespace Lexy.Tests
{
    public class OrCombinatorTests
    {
        [Fact]
        public void Or_WithValidRules_ReturnsResultsOfBoth()
        {
            var ruleOne = new Mock<Rule>();
            var ruleTwo = new Mock<Rule>();

            ruleOne  .Setup(x => x.ExecuteOn("")).Returns(new Rule.ExecutionResult("Number"));
            ruleTwo  .Setup(x => x.ExecuteOn("")).Returns(new Rule.ExecutionResult("Whitespace"));

            var result = (ruleOne.Object | ruleTwo.Object | ruleOne.Object).ExecuteOn("");

            result.ToString().Should().BeEquivalentTo("`Number` `Whitespace` `Number`");
        }
        
        [Fact]
        public void Or_WithInvalidRuleResult_ReturnsResultsOfBoth()
        {
            var ruleOne = new Mock<Rule>();
            var invalid = new Mock<Rule>();

            ruleOne  .Setup(x => x.ExecuteOn("")).Returns(new Rule.ExecutionResult("Number"));
            invalid  .Setup(x => x.ExecuteOn("")).Returns((Rule.ExecutionResult) null);

            var result = (ruleOne.Object | invalid.Object | ruleOne.Object).ExecuteOn("");

            result.ToString().Should().BeEquivalentTo("`Number` `Number`");
        }
    }
}