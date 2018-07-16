namespace NotifyPropertyChangedMixin
{
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using Castle.DynamicProxy;

    public class NotifyPropertyChangedMixin : INotifyPropertyChanged
    {
        protected NotifyPropertyChangedMixin(Interceptor interceptor)
        {
            interceptor.Mixin = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Tworzy mixina
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">Instancja obiektu, do której ma zostać wstrzyknieta funcjonalność</param>
        /// <param name="interceptors">Dodatkowe, opcjonalne, interceptory</param>
        /// <returns>Objekt proxy (mixin)</returns>
        public static T Create<T>(T instance, params IInterceptor[] interceptors)
        {
            var options = new ProxyGenerationOptions();
            options.AddMixinInstance(instance);
            var interceptor = new Interceptor();
            var generator = new ProxyGenerator();
            var proxy = generator.CreateClassProxy(
                typeof(NotifyPropertyChangedMixin),
                options,
                new object[] { interceptor },
                new IInterceptor[] { interceptor }.Concat(interceptors).ToArray());
            return (T)proxy;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected class Interceptor : IInterceptor
        {
            private NotifyPropertyChangedMixin _mixin;

            public NotifyPropertyChangedMixin Mixin
            {
                set => _mixin = value;
            }

            public void Intercept(IInvocation invocation)
            {
                invocation.Proceed();
                var type = invocation.Method.DeclaringType;
                var thereIsSetProperty = type != null && type.GetProperties().Any(p => p.GetSetMethod() == invocation.Method);
                if (thereIsSetProperty)
                {
                    var name = invocation.Method.Name.Substring(4);
                    _mixin.OnPropertyChanged(name);
                }
            }
        }
    }
}
