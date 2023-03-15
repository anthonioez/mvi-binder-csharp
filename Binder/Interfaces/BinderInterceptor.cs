namespace MVI.Binder.Interfaces {

    public interface BinderInterceptor {

        object Intercept(string name, object newEvent);

    }
}