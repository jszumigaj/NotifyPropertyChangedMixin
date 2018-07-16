namespace NotifyPropertyChangeMixin.Tests
{
    public interface ISut //: INotifyPropertyChanged
    {
        string Name { get; set; }

        int Value { get; set; }
    }
}