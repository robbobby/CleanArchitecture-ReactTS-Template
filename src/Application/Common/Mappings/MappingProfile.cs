using System.Reflection;
using AutoMapper;

namespace Console.Application.Common.Mappings;

public class MappingProfile : Profile {
    public MappingProfile() {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly) {
        var types = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            .ToList();

        foreach (Type type in types) {
            var instance = Activator.CreateInstance(type);

            MethodInfo? methodInfo = type.GetMethod("Mapping")
                ?? type.GetInterface("IMapFrom`1")!.GetMethod("Mapping");

            methodInfo?.Invoke(instance, new object[] {
                this
            });

        }
    }
}
