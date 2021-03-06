//// This code has been based from the sample repository "cecil": https://github.com/jbevain/cecil
// Copyright (c) 2020 - 2021 Faber Leonardo. All Rights Reserved. https://github.com/FaberSanZ
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)


using System;


namespace LSharp.IL.PE {

	public sealed class Section 
	{
		public string Name;
		public uint VirtualAddress;
		public uint VirtualSize;
		public uint SizeOfRawData;
		public uint PointerToRawData;
	}
}
