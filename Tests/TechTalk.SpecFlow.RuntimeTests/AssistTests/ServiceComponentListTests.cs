﻿using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TechTalk.SpecFlow.Assist;

namespace TechTalk.SpecFlow.RuntimeTests.AssistTests
{
    [TestFixture]
    public class ServiceComponentListTests
    {
        [Test]
        public void Should_be_empty_when_created()
        {
            var sut = new ServiceComponentList<ITestComponent>();

            sut.Should().BeEmpty();
        }

        [Test]
        public void Should_allow_the_addition_of_new_components()
        {
            var sut = new ServiceComponentList<ITestComponent>();
            var component = new TestComponentImpl();
            sut.Register(component);

            sut.Should().Equal(component);
        }

        [Test]
        public void Should_reverse_registration_order_of_added_components()
        {
            var sut = new ServiceComponentList<ITestComponent>();

            var components = Enumerable.Range(0, 100).Select(_ => new TestComponentImpl()).ToList();
            components.ForEach(sut.Register);

            components.Reverse();
            sut.Should().Equal(components);
        }

        [Test]
        public void Should_allow_the_addition_of_default_components_at_the_end_of_the_list()
        {
            var sut = new ServiceComponentList<ITestComponent>();
            var registeredFirst = new TestComponentImpl();
            var registeredLast = new TestComponentImpl();
            var @default = new TestComponentImpl();
            sut.Register(registeredFirst);
            sut.Register(registeredLast);
            sut.RegisterDefault(@default);

            sut.Should().Equal(registeredLast, registeredFirst, @default);
        }

        [Test]
        public void Should_allow_unregistration_of_existing_component()
        {
            var sut = new ServiceComponentList<ITestComponent>();
            var component = new TestComponentImpl();
            sut.Register(component);
            sut.Unregister(component);

            sut.Should().BeEmpty();
        }

        [Test]
        public void Should_ignore_unregistration_of_nonexisting_component()
        {
            var sut = new ServiceComponentList<ITestComponent>();
            sut.Register(new TestComponentImpl());
            sut.Unregister(new TestComponentImpl());

            sut.Should().NotBeEmpty();
        }

        private interface ITestComponent
        {
        }

        private class TestComponentImpl : ITestComponent
        {
        }
    }
}