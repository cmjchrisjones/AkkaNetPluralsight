using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using System;

namespace ReactiveStock.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainWindowViewModel>();
        }

        public MainWindowViewModel Main { get { return ServiceLocator.Current.GetInstance<MainWindowViewModel>(); } }

        public static void Cleanup() { }
    }
}
