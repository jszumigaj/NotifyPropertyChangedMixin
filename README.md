# NotifyPropertyChangedMixin

Creates a mixin that implements the INotifyPropertyChanged interface from an object of any class with just one line of code.

## Usage

Lets say you have an implementation of:

```C#
    public interface IFoo 
    {
        string Name { get; set; }
        int Value { get; set; }
    }
```

```C#
    public class Foo : IFoo
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
```

With NotifyPropertyChangedMixin you can dynamicaly turn your object into properly implemented INotifyPropertyChanged innstance.
That is, in other words, **instead of** write code like this:

```C#
    public class Foo : IFoo, INotifyPropertyChanged
    {
        private string _name;
        public string Name 
        { 
            get { return _name; }  
            set { _name = value; OnPropertyChanged("Name"); }
        }

        //... similar code here...

        // ... and here...
        
        protected void OnPropertyChanged(string propertyName)
        {
            // ... a few lines of code ...
        }
    }
```

**you can just create mixin** from your simple implementation

```C#
    var foo = new Foo();
    var mixFoo = NotifyPropertyChangedMixin.Create(foo);
```

and use it like normal INotifyPropertyChanged type:

```C#
    ((INotifyPropertyChanged)mixFoo).PropertyChanged += (sender, args) => { ... your handler ...});
```



### More

Mixin creates dynamic proxy interceptor using Castle.Core's proxy generator.
 
Read unit tests for more examples.