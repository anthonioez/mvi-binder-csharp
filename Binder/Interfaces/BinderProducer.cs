using System;

namespace MVI.Binder.Interfaces {

    public interface BinderProducer {

         void Subscribe(BinderObserver observer);
         void Unsubscribe(BinderObserver observer);

    }

}