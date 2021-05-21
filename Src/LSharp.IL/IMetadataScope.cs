// This code has been based from the sample repository "cecil": https://github.com/jbevain/cecil
// Copyright (c) 2020 - 2021 Faber Leonardo. All Rights Reserved. https://github.com/FaberSanZ
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)


namespace LSharp.IL
{

    public enum MetadataScopeType
    {
        AssemblyNameReference,
        ModuleReference,
        ModuleDefinition,
    }

    public interface IMetadataScope : IMetadataTokenProvider
    {
        MetadataScopeType MetadataScopeType { get; }
        string Name { get; set; }
    }
}
