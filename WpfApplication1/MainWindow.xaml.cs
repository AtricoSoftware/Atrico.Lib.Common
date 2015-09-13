using System.ComponentModel;
using System.Windows;
using Atrico.Lib.Common.PropertyContainer;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly PropertyContainer _properties;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { _properties.PropertyChanged += value; }
            remove { _properties.PropertyChanged -= value; }
        }

        public int Prop1
        {
            get { return _properties.Get<int>(); }
            set { _properties.Set(value); }
        }

        public string Prop2
        {
            get { return _properties.Get<string>(); }
            set { _properties.Set(value); }
        }

        public MainWindow()
        {
            InitializeComponent();
            _properties = new PropertyContainer(this);
            DataContext = this;
            Prop1 = 123;
            Prop2 = "hello";
        }
    }
}
