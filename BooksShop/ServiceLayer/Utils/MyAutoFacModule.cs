using Autofac;

namespace ServiceLayer.Utils
{
    public class MyAutoFacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly).Where(c => c.Name.EndsWith("Service")).AsImplementedInterfaces();
        }
    }
}