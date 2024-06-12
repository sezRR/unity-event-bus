﻿using System;
using System.Collections.Generic;
using System.Reflection;

public static class PredefinedAssemblyUtil
{
    enum AssemblyType
    {
        AssemblyCSharp,
        AssemblyCSharpEditor,
        AssemblyCSharpEditorFirstPass,
        AssemblyCSharpFirstPass
    }

    static AssemblyType? GetAssemblyType(string assemblyName)
    {
        return assemblyName switch
        {
            "Assembly-CSharp" => AssemblyType.AssemblyCSharp,
            "Assembly-CSharp-Editor" => AssemblyType.AssemblyCSharpEditor,
            "Assembly-CSharp-Editor-firstpass" => AssemblyType.AssemblyCSharpEditorFirstPass,
            "Assembly-CSharp-firstpass" => AssemblyType.AssemblyCSharpFirstPass,
            _ => null
        };
    }

    static void AddTypesFromAssembly(Type[] assembly, ICollection<Type> types, Type interfaceType)
    {
        if (assembly == null) return;

        for (int i = 0; i < assembly.Length; i++)
        {
            Type type = assembly[i];
            if (type != interfaceType && interfaceType.IsAssignableFrom(type))
                types.Add(type);
        }
    }

    public static List<Type> GetTypes(Type interfaceType)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        Dictionary<AssemblyType, Type[]> assemblyTypes = new();
        List<Type> types = new();

        for (int i = 0; i < assemblies.Length; i++)
        {
            AssemblyType? assemblyType = GetAssemblyType(assemblies[i].GetName().Name);

            if (assemblyType != null)
                assemblyTypes.Add((AssemblyType)assemblyType, assemblies[i].GetTypes());
        }

        assemblyTypes.TryGetValue(AssemblyType.AssemblyCSharp, out var assemblyCSharpTypes);
        AddTypesFromAssembly(assemblyCSharpTypes, types, interfaceType);
        
        assemblyTypes.TryGetValue(AssemblyType.AssemblyCSharpFirstPass, out var assemblyCSharpFirstPassTypes);
        AddTypesFromAssembly(assemblyCSharpFirstPassTypes, types, interfaceType);

        return types;
    }
}