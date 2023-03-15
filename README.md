# mvi-binder-csharp
 Library for Model View Intent pattern

 
private readonly Binder binder = new Binder();
private readonly BinderEvent eventor = new BinderEvent();

binder.Bind(Binder.Connect.From(eventor).To(model).TransformWith(new SplashTransformer()).Intercept());
binder.Bind(Binder.Connect.From(model).To(this));
