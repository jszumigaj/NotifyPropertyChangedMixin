namespace NotifyPropertyChangedMixin.Tests
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Castle.DynamicProxy;
    using FakeItEasy;
    using global::NotifyPropertyChangedMixin;
    using NUnit.Framework;

    [TestFixture]
    public class MixinTests
    {
        [Test]
        public void MixinIsNotifyPropertyChangedType()
        {
            // Arrange
            var original = A.Fake<ISut>();

            // Act
            object sut = NotifyPropertyChangedMixin.Create(original);

            // Assert
            Assert.That(sut, Is.InstanceOf<ISut>());
            Assert.That(sut, Is.InstanceOf<INotifyPropertyChanged>());
        }

        [Test]
        public void PropertyChangesAreNotified()
        {
            // Arrange
            IList<string> eventInvocations = new List<string>();
            var original = A.Fake<ISut>();

            var sut = NotifyPropertyChangedMixin.Create(original);
            ((INotifyPropertyChanged)sut).PropertyChanged += (sender, args) => eventInvocations.Add(args.PropertyName);

            // Act
            sut.Name = "test";
            sut.Value = 1;

            // Assert
            Assert.That(sut.Name, Is.EqualTo("test"));
            Assert.That(sut.Value, Is.EqualTo(1));
            Assert.That(eventInvocations.Count, Is.EqualTo(2));
            Assert.That(eventInvocations, Is.EquivalentTo(new[] { "Name", "Value" }));
        }

        [Test]
        public void MixinCanBeIntercepted()
        {
            // Arrange
            var original = A.Fake<ISut>();
            var interceptor = A.Fake<IInterceptor>();

            var sut = NotifyPropertyChangedMixin.Create(original, interceptor);

            // Act
            sut.Name = "test";

            // Assert
            A.CallTo(() => interceptor.Intercept(A<IInvocation>.Ignored)).MustHaveHappened();
        }
    }
}
